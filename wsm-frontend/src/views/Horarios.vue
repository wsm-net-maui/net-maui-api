<template>
  <div class="p-6 max-w-7xl mx-auto">
    <div class="flex items-center justify-between mb-6">
      <h2 class="text-2xl font-bold text-surface-900 dark:text-surface-0">Agenda de Horários</h2>
      <Button label="Novo Horário" icon="pi pi-plus" @click="openNew" />
    </div>

    <div class="bg-surface-0 dark:bg-surface-800 p-4 rounded-xl shadow-sm border border-surface-200 dark:border-surface-700">
      <DataTable :value="horarios" :loading="loading" dataKey="id" 
        paginator :rows="10" 
        :rowsPerPageOptions="[5, 10, 25]"
        stripedRows responsiveLayout="scroll">
        
        <Column field="funcionarioPerfilId" header="ID Funcionário"></Column>
        <Column field="diaSemana" header="Dia da Semana" :sortable="true">
          <template #body="slotProps">
            {{ formatDiaSemana(slotProps.data.diaSemana) }}
          </template>
        </Column>
        <Column field="horaInicio" header="Início"></Column>
        <Column field="horaFim" header="Fim"></Column>
        <Column field="ativo" header="Status" :sortable="true">
          <template #body="slotProps">
            <Tag :value="slotProps.data.ativo ? 'Ativo' : 'Inativo'" :severity="slotProps.data.ativo ? 'success' : 'danger'" />
          </template>
        </Column>
        <Column :exportable="false" style="min-width: 8rem">
          <template #body="slotProps">
            <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editHorario(slotProps.data)" />
            <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteHorario(slotProps.data)" />
          </template>
        </Column>
      </DataTable>
    </div>

    <!-- Dialog Create/Edit -->
    <Dialog v-model:visible="horarioDialog" :style="{ width: '450px' }" header="Detalhes do Horário" :modal="true" class="p-fluid">
      <div class="flex flex-col gap-4 py-4">
        <div class="flex flex-col gap-2">
          <label for="funcionarioId" class="font-medium">ID do Funcionário (GUID)</label>
          <InputText id="funcionarioId" v-model.trim="horario.funcionarioPerfilId" required autofocus :invalid="submitted && !horario.funcionarioPerfilId" />
          <small class="text-red-500" v-if="submitted && !horario.funcionarioPerfilId">Obrigatório.</small>
        </div>

        <div class="flex flex-col gap-2">
          <label for="diaSemana" class="font-medium">Dia da Semana (0=Dom, 6=Sáb)</label>
          <InputNumber id="diaSemana" v-model="horario.diaSemana" :min="0" :max="6" :invalid="submitted && horario.diaSemana == null" />
        </div>

        <div class="flex flex-col gap-2">
          <label class="font-medium">Selecione os Horários</label>
          <div class="grid grid-cols-2 sm:grid-cols-3 gap-3 max-h-60 overflow-y-auto p-2 border border-surface-200 dark:border-surface-700 rounded-lg">
            <div v-for="slot in timeSlots" :key="slot.label" 
                 @click="toggleSlot(slot)"
                 class="p-3 border rounded-lg cursor-pointer text-center font-medium transition-all"
                 :class="isSelected(slot) ? 'bg-primary text-primary-contrast border-primary shadow-md' : 'bg-surface-0 text-surface-700 dark:bg-surface-800 dark:text-surface-300 hover:border-primary'">
              {{ slot.label }}
            </div>
          </div>
          <small class="text-red-500" v-if="submitted && selectedSlots.length === 0">Selecione ao menos um horário.</small>
        </div>

        <div class="flex flex-col gap-2">
          <label for="intervalo" class="font-medium">Intervalo (Minutos)</label>
          <InputNumber id="intervalo" v-model="horario.intervaloMinutos" />
        </div>

        <div class="flex items-center gap-2 mt-2" v-if="horario.id">
          <Checkbox v-model="horario.ativo" inputId="ativo" :binary="true" />
          <label for="ativo" class="font-medium">Ativo</label>
        </div>
      </div>

      <template #footer>
        <Button label="Cancelar" icon="pi pi-times" text @click="hideDialog" />
        <Button label="Salvar" icon="pi pi-check" @click="saveHorario" :loading="saving" />
      </template>
    </Dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '../services/api';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Checkbox from 'primevue/checkbox';
import Tag from 'primevue/tag';

const toast = useToast();
const confirm = useConfirm();

const horarios = ref([]);
const loading = ref(false);
const saving = ref(false);

const horarioDialog = ref(false);
const horario = ref({});
const submitted = ref(false);

const diasSemana = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'];

const generateSlots = () => {
  const slots = [];
  for (let i = 8; i < 20; i++) {
    const start = i.toString().padStart(2, '0') + ':00';
    const end = (i + 1).toString().padStart(2, '0') + ':00';
    slots.push({ label: `${start} - ${end}`, start, end });
  }
  return slots;
};

const timeSlots = generateSlots();
const selectedSlots = ref([]);

const toggleSlot = (slot) => {
  const index = selectedSlots.value.findIndex(s => s.start === slot.start && s.end === slot.end);
  if (index >= 0) {
    selectedSlots.value.splice(index, 1);
  } else {
    // Se for modo edição de um já existente (tem ID), só pode selecionar 1
    if (horario.value.id) {
      selectedSlots.value = [slot];
    } else {
      selectedSlots.value.push(slot);
    }
  }
};

const isSelected = (slot) => {
  return selectedSlots.value.some(s => s.start === slot.start && s.end === slot.end);
};

onMounted(() => {
  loadHorarios();
});

const loadHorarios = async () => {
  loading.value = true;
  try {
    const response = await api.get('/horariosatendimento');
    horarios.value = response.data?.data || response.data;
  } catch (error) {
    console.error('Erro ao buscar horários', error);
    toast.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível carregar os horários.', life: 3000 });
  } finally {
    loading.value = false;
  }
};

const formatDiaSemana = (dia) => {
  return diasSemana[dia] || dia;
};

const openNew = () => {
  horario.value = { ativo: true, diaSemana: 1, intervaloMinutos: 30 };
  selectedSlots.value = [];
  submitted.value = false;
  horarioDialog.value = true;
};

const hideDialog = () => {
  horarioDialog.value = false;
  submitted.value = false;
};

const saveHorario = async () => {
  submitted.value = true;
  if (horario.value.funcionarioPerfilId && selectedSlots.value.length > 0) {
    saving.value = true;
    try {
      if (horario.value.id) {
        // Atualiza o único registro
        const slot = selectedSlots.value[0];
        const payload = { ...horario.value, horaInicio: slot.start + ':00', horaFim: slot.end + ':00' };
        await api.put(`/horariosatendimento/${horario.value.id}`, payload);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Horário atualizado', life: 3000 });
      } else {
        // Cria múltiplos registros (um para cada card selecionado)
        for (const slot of selectedSlots.value) {
          const payload = { ...horario.value, horaInicio: slot.start + ':00', horaFim: slot.end + ':00' };
          await api.post('/horariosatendimento', payload);
        }
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Horário(s) adicionado(s) com sucesso', life: 3000 });
      }
      horarioDialog.value = false;
      horario.value = {};
      selectedSlots.value = [];
      await loadHorarios();
    } catch (error) {
      console.error('Erro ao salvar horário', error);
      toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao salvar o horário', life: 3000 });
    } finally {
      saving.value = false;
    }
  }
};

const editHorario = (data) => {
  horario.value = { ...data };
  
  const start = data.horaInicio?.substring(0, 5) || '';
  const end = data.horaFim?.substring(0, 5) || '';
  selectedSlots.value = [{ label: `${start} - ${end}`, start, end }];
  
  horarioDialog.value = true;
};

const confirmDeleteHorario = (data) => {
  confirm.require({
    message: `Tem certeza que deseja excluir o horário selecionado?`,
    header: 'Confirmação',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: 'Sim',
    rejectLabel: 'Não',
    accept: async () => {
      try {
        await api.delete(`/horariosatendimento/${data.id}`);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Horário excluído', life: 3000 });
        await loadHorarios();
      } catch (error) {
        console.error('Erro ao excluir', error);
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao excluir o horário', life: 3000 });
      }
    }
  });
};
</script>
