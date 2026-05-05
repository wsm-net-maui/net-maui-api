<template>
  <div class="p-6 max-w-7xl mx-auto flex flex-col gap-6">
    <div class="flex items-center justify-between">
      <h2 class="text-2xl font-bold text-surface-900 dark:text-surface-0">Agenda de Horários</h2>
    </div>

    <!-- Filter by Funcionario -->
    <div class="bg-surface-0 dark:bg-surface-800 p-4 rounded-xl shadow-sm border border-surface-200 dark:border-surface-700 flex items-center gap-4">
      <label class="font-medium text-surface-700 dark:text-surface-300">Selecione o Funcionário:</label>
      <Dropdown v-model="selectedFuncionario" :options="funcionarios" optionLabel="nome" optionValue="id" 
                placeholder="Escolha um funcionário..." class="w-full md:w-80" @change="loadHorarios" />
    </div>

    <!-- Grid Layout -->
    <div v-if="selectedFuncionario" class="bg-surface-0 dark:bg-surface-800 rounded-xl shadow-sm border border-surface-200 dark:border-surface-700 overflow-x-auto relative">
      <div v-if="loading" class="absolute inset-0 bg-surface-0/50 dark:bg-surface-800/50 z-10 flex items-center justify-center">
         <i class="pi pi-spinner pi-spin text-3xl text-primary"></i>
      </div>

      <div class="min-w-[800px] grid grid-cols-8 gap-px bg-surface-200 dark:bg-surface-700">
        <!-- Header -->
        <div class="bg-surface-100 dark:bg-surface-900 p-3 text-center font-bold text-surface-600 dark:text-surface-300 flex items-center justify-center">
          Hora
        </div>
        <div v-for="dia in diasSemana" :key="dia.value" class="bg-surface-100 dark:bg-surface-900 p-3 text-center font-bold text-surface-600 dark:text-surface-300">
          {{ dia.label }}
        </div>
        
        <!-- Hours and Cells -->
        <template v-for="hora in horasDisponiveis" :key="hora.start">
          <div class="bg-surface-50 dark:bg-surface-800 p-3 text-center text-sm font-medium flex items-center justify-center text-surface-600 dark:text-surface-400">
            {{ hora.start }}
          </div>
          
          <div v-for="dia in diasSemana" :key="`${dia.value}-${hora.start}`" 
               class="bg-surface-0 dark:bg-surface-800 min-h-[80px] p-1 relative group cursor-pointer hover:bg-surface-50 dark:hover:bg-surface-700 transition-colors"
               @click="openModal(dia.value, hora)">
               
               <template v-if="hasHorario(dia.value, hora.start)">
                 <div class="absolute inset-1 bg-primary-100 dark:bg-primary-900/40 border border-primary-300 dark:border-primary-700 rounded p-2 flex flex-col shadow-sm"
                      @click.stop="confirmDeleteHorario(getHorario(dia.value, hora.start))">
                    <span class="text-xs font-bold text-primary-800 dark:text-primary-300 leading-tight">
                        {{ getHorario(dia.value, hora.start).servicoNome || 'Serviço' }}
                    </span>
                    <i class="pi pi-times absolute top-1 right-1 text-red-500 opacity-0 group-hover:opacity-100 transition-opacity p-1 hover:bg-red-100 dark:hover:bg-red-900/50 rounded-full"></i>
                 </div>
               </template>
               <template v-else>
                  <div class="absolute inset-0 flex items-center justify-center opacity-0 group-hover:opacity-100 text-surface-400">
                      <i class="pi pi-plus-circle text-xl"></i>
                  </div>
               </template>
          </div>
        </template>
      </div>
    </div>
    
    <div v-else class="text-center p-12 bg-surface-50 dark:bg-surface-800 rounded-xl border border-dashed border-surface-300 dark:border-surface-600">
      <i class="pi pi-calendar text-4xl text-surface-400 mb-4"></i>
      <h3 class="text-xl font-medium text-surface-600 dark:text-surface-300">Selecione um funcionário para visualizar a agenda</h3>
    </div>

    <!-- Modal Adicionar Horário -->
    <Dialog v-model:visible="horarioDialog" :style="{ width: '450px' }" header="Adicionar Horário" :modal="true" class="p-fluid">
      <div class="flex flex-col gap-4 py-4" v-if="slotSelecionado">
        <div class="bg-surface-100 dark:bg-surface-800 p-3 rounded-lg flex justify-between items-center text-sm">
          <span><i class="pi pi-calendar mr-2"></i>{{ diasSemana.find(d => d.value === slotSelecionado.diaSemana)?.label }}</span>
          <span class="font-bold">{{ slotSelecionado.start }} às {{ slotSelecionado.end }}</span>
        </div>
        
        <div class="flex flex-col gap-2">
          <label for="servico" class="font-medium">Serviço <span class="text-red-500">*</span></label>
          <Dropdown id="servico" v-model="selectedServico" :options="servicos" optionLabel="nome" optionValue="id" 
                    placeholder="Selecione um serviço" :invalid="submitted && !selectedServico" />
          <small class="text-red-500" v-if="submitted && !selectedServico">Por favor, selecione um serviço.</small>
        </div>
      </div>
      <template #footer>
        <Button label="Cancelar" icon="pi pi-times" text @click="horarioDialog = false" />
        <Button label="Salvar" icon="pi pi-check" @click="saveHorario" :loading="saving" />
      </template>
    </Dialog>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import api from '../services/api';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';

import Dropdown from 'primevue/dropdown';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';

const toast = useToast();
const confirm = useConfirm();

const funcionarios = ref([]);
const servicos = ref([]);
const horarios = ref([]);

const selectedFuncionario = ref(null);
const loading = ref(false);
const saving = ref(false);

const horarioDialog = ref(false);
const slotSelecionado = ref(null);
const selectedServico = ref(null);
const submitted = ref(false);

const diasSemana = [
  { label: 'Domingo', value: 0 },
  { label: 'Segunda', value: 1 },
  { label: 'Terça', value: 2 },
  { label: 'Quarta', value: 3 },
  { label: 'Quinta', value: 4 },
  { label: 'Sexta', value: 5 },
  { label: 'Sábado', value: 6 }
];

const horasDisponiveis = computed(() => {
  const slots = [];
  for (let i = 8; i < 20; i++) {
    const start = i.toString().padStart(2, '0') + ':00';
    const end = (i + 1).toString().padStart(2, '0') + ':00';
    slots.push({ start, end });
  }
  return slots;
});

onMounted(async () => {
  try {
    const [resFunc, resServ] = await Promise.all([
      api.get('/funcionarios'),
      api.get('/servicos')
    ]);
    funcionarios.value = resFunc.data?.data || resFunc.data || [];
    servicos.value = resServ.data?.data || resServ.data || [];
  } catch (err) {
    console.error('Erro ao buscar dados iniciais', err);
    toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao carregar funcionários/serviços', life: 3000 });
  }
});

const loadHorarios = async () => {
  if (!selectedFuncionario.value) return;
  
  loading.value = true;
  try {
    const response = await api.get(`/horarios/funcionario/${selectedFuncionario.value}`);
    horarios.value = response.data?.data || response.data || [];
  } catch (error) {
    console.error('Erro ao buscar horários', error);
    toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao carregar os horários.', life: 3000 });
  } finally {
    loading.value = false;
  }
};

const hasHorario = (diaSemana, startHora) => {
  const formatStart = startHora + ':00';
  return horarios.value.some(h => h.diaSemana === diaSemana && h.horaInicio === formatStart);
};

const getHorario = (diaSemana, startHora) => {
  const formatStart = startHora + ':00';
  return horarios.value.find(h => h.diaSemana === diaSemana && h.horaInicio === formatStart);
};

const openModal = (diaSemana, horaObj) => {
  if (hasHorario(diaSemana, horaObj.start)) return; // Se já tem, deleta clicando no card
  
  slotSelecionado.value = { diaSemana, start: horaObj.start, end: horaObj.end };
  selectedServico.value = null;
  submitted.value = false;
  horarioDialog.value = true;
};

const saveHorario = async () => {
  submitted.value = true;
  if (!selectedServico.value || !slotSelecionado.value || !selectedFuncionario.value) return;
  
  saving.value = true;
  try {
    const payload = {
      funcionarioPerfilId: selectedFuncionario.value,
      servicoId: selectedServico.value,
      diaSemana: slotSelecionado.value.diaSemana,
      horaInicio: slotSelecionado.value.start + ':00',
      horaFim: slotSelecionado.value.end + ':00',
      intervaloMinutos: 30
    };
    
    await api.post('/horarios', payload);
    toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Horário cadastrado', life: 3000 });
    horarioDialog.value = false;
    await loadHorarios();
  } catch (error) {
    console.error('Erro ao salvar', error);
    toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao cadastrar o horário', life: 3000 });
  } finally {
    saving.value = false;
  }
};

const confirmDeleteHorario = (horarioObj) => {
  confirm.require({
    message: 'Tem certeza que deseja liberar este horário?',
    header: 'Confirmação',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: 'Sim',
    rejectLabel: 'Não',
    accept: async () => {
      try {
        await api.delete(`/horarios/${horarioObj.id}`);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Horário liberado', life: 3000 });
        await loadHorarios();
      } catch (error) {
        console.error('Erro ao excluir', error);
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao liberar horário', life: 3000 });
      }
    }
  });
};
</script>
