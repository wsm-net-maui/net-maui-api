<template>
  <div class="p-6 max-w-7xl mx-auto">
    <div class="flex items-center justify-between mb-6">
      <h2 class="text-2xl font-bold text-surface-900 dark:text-surface-0">Gerenciar Serviços</h2>
      <Button label="Novo Serviço" icon="pi pi-plus" @click="openNew" />
    </div>

    <div class="bg-surface-0 dark:bg-surface-800 p-4 rounded-xl shadow-sm border border-surface-200 dark:border-surface-700">
      <DataTable :value="servicos" :loading="loading" dataKey="id" 
        paginator :rows="10" 
        :rowsPerPageOptions="[5, 10, 25]"
        stripedRows responsiveLayout="scroll">
        
        <Column field="nome" header="Nome" :sortable="true"></Column>
        <Column field="preco" header="Preço" :sortable="true">
          <template #body="slotProps">
            {{ formatCurrency(slotProps.data.preco) }}
          </template>
        </Column>
        <Column field="duracaoMinutos" header="Duração (min)" :sortable="true"></Column>
        <Column field="ativo" header="Status" :sortable="true">
          <template #body="slotProps">
            <Tag :value="slotProps.data.ativo ? 'Ativo' : 'Inativo'" :severity="slotProps.data.ativo ? 'success' : 'danger'" />
          </template>
        </Column>
        <Column :exportable="false" style="min-width: 8rem">
          <template #body="slotProps">
            <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editServico(slotProps.data)" />
            <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteServico(slotProps.data)" />
          </template>
        </Column>
      </DataTable>
    </div>

    <!-- Dialog Create/Edit -->
    <Dialog v-model:visible="servicoDialog" :style="{ width: '450px' }" header="Detalhes do Serviço" :modal="true" class="p-fluid">
      <div class="flex flex-col gap-4 py-4">
        <div class="flex flex-col gap-2">
          <label for="nome" class="font-medium">Nome</label>
          <InputText id="nome" v-model.trim="servico.nome" required autofocus :invalid="submitted && !servico.nome" />
          <small class="text-red-500" v-if="submitted && !servico.nome">Nome é obrigatório.</small>
        </div>

        <div class="flex flex-col gap-2">
          <label for="descricao" class="font-medium">Descrição</label>
          <Textarea id="descricao" v-model="servico.descricao" rows="3" cols="20" />
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div class="flex flex-col gap-2">
            <label for="preco" class="font-medium">Preço</label>
            <InputNumber id="preco" v-model="servico.preco" mode="currency" currency="BRL" locale="pt-BR" />
          </div>

          <div class="flex flex-col gap-2">
            <label for="duracao" class="font-medium">Duração (Min)</label>
            <InputNumber id="duracao" v-model="servico.duracaoMinutos" />
          </div>
        </div>

        <div class="flex items-center gap-2 mt-2" v-if="servico.id">
          <Checkbox v-model="servico.ativo" inputId="ativo" :binary="true" />
          <label for="ativo" class="font-medium">Ativo</label>
        </div>
      </div>

      <template #footer>
        <Button label="Cancelar" icon="pi pi-times" text @click="hideDialog" />
        <Button label="Salvar" icon="pi pi-check" @click="saveServico" :loading="saving" />
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
import Textarea from 'primevue/textarea';
import InputNumber from 'primevue/inputnumber';
import Checkbox from 'primevue/checkbox';
import Tag from 'primevue/tag';

const toast = useToast();
const confirm = useConfirm();

const servicos = ref([]);
const loading = ref(false);
const saving = ref(false);

const servicoDialog = ref(false);
const servico = ref({});
const submitted = ref(false);

onMounted(() => {
  loadServicos();
});

const loadServicos = async () => {
  loading.value = true;
  try {
    const response = await api.get('/servicos');
    servicos.value = response.data;
  } catch (error) {
    console.error('Erro ao buscar serviços', error);
    toast.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível carregar os serviços.', life: 3000 });
  } finally {
    loading.value = false;
  }
};

const formatCurrency = (value) => {
  return value?.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }) || 'R$ 0,00';
};

const openNew = () => {
  servico.value = { ativo: true, preco: 0, duracaoMinutos: 30 };
  submitted.value = false;
  servicoDialog.value = true;
};

const hideDialog = () => {
  servicoDialog.value = false;
  submitted.value = false;
};

const saveServico = async () => {
  submitted.value = true;
  if (servico.value.nome?.trim()) {
    saving.value = true;
    try {
      if (servico.value.id) {
        await api.put(`/servicos/${servico.value.id}`, servico.value);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Serviço atualizado', life: 3000 });
      } else {
        await api.post('/servicos', servico.value);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Serviço criado', life: 3000 });
      }
      servicoDialog.value = false;
      servico.value = {};
      await loadServicos();
    } catch (error) {
      console.error('Erro ao salvar serviço', error);
      toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao salvar o serviço', life: 3000 });
    } finally {
      saving.value = false;
    }
  }
};

const editServico = (data) => {
  servico.value = { ...data };
  servicoDialog.value = true;
};

const confirmDeleteServico = (data) => {
  confirm.require({
    message: `Tem certeza que deseja excluir o serviço ${data.nome}?`,
    header: 'Confirmação',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: 'Sim',
    rejectLabel: 'Não',
    accept: async () => {
      try {
        await api.delete(`/servicos/${data.id}`);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Serviço excluído', life: 3000 });
        await loadServicos();
      } catch (error) {
        console.error('Erro ao excluir serviço', error);
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao excluir o serviço', life: 3000 });
      }
    }
  });
};
</script>
