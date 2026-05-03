<template>
  <div class="p-6 max-w-7xl mx-auto">
    <div class="flex items-center justify-between mb-6">
      <h2 class="text-2xl font-bold text-surface-900 dark:text-surface-0">Gerenciar Funcionários</h2>
      <Button label="Novo Funcionário" icon="pi pi-plus" @click="openNew" />
    </div>

    <div class="bg-surface-0 dark:bg-surface-800 p-4 rounded-xl shadow-sm border border-surface-200 dark:border-surface-700">
      <DataTable :value="funcionarios" :loading="loading" dataKey="id" 
        paginator :rows="10" 
        :rowsPerPageOptions="[5, 10, 25]"
        stripedRows responsiveLayout="scroll">
        
        <Column field="usuarioId" header="ID Usuário"></Column>
        <Column field="especialidade" header="Especialidade" :sortable="true"></Column>
        <Column field="descricao" header="Descrição"></Column>
        <Column field="ativo" header="Status" :sortable="true">
          <template #body="slotProps">
            <Tag :value="slotProps.data.ativo ? 'Ativo' : 'Inativo'" :severity="slotProps.data.ativo ? 'success' : 'danger'" />
          </template>
        </Column>
        <Column :exportable="false" style="min-width: 8rem">
          <template #body="slotProps">
            <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editFuncionario(slotProps.data)" />
            <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteFuncionario(slotProps.data)" />
          </template>
        </Column>
      </DataTable>
    </div>

    <!-- Dialog Create/Edit -->
    <Dialog v-model:visible="funcionarioDialog" :style="{ width: '450px' }" header="Detalhes do Funcionário" :modal="true" class="p-fluid">
      <div class="flex flex-col gap-4 py-4">
        <div class="flex flex-col gap-2">
          <label for="usuarioId" class="font-medium">ID do Usuário (GUID)</label>
          <InputText id="usuarioId" v-model.trim="funcionario.usuarioId" required autofocus :invalid="submitted && !funcionario.usuarioId" placeholder="ex: 3fa85f64-5717-4562-b3fc-2c963f66afa6" />
          <small class="text-red-500" v-if="submitted && !funcionario.usuarioId">ID do Usuário é obrigatório.</small>
        </div>

        <div class="flex flex-col gap-2">
          <label for="especialidade" class="font-medium">Especialidade</label>
          <InputText id="especialidade" v-model.trim="funcionario.especialidade" required :invalid="submitted && !funcionario.especialidade" />
          <small class="text-red-500" v-if="submitted && !funcionario.especialidade">Especialidade é obrigatória.</small>
        </div>

        <div class="flex flex-col gap-2">
          <label for="descricao" class="font-medium">Descrição</label>
          <Textarea id="descricao" v-model="funcionario.descricao" rows="3" cols="20" />
        </div>

        <div class="flex items-center gap-2 mt-2" v-if="funcionario.id">
          <Checkbox v-model="funcionario.ativo" inputId="ativo" :binary="true" />
          <label for="ativo" class="font-medium">Ativo</label>
        </div>
      </div>

      <template #footer>
        <Button label="Cancelar" icon="pi pi-times" text @click="hideDialog" />
        <Button label="Salvar" icon="pi pi-check" @click="saveFuncionario" :loading="saving" />
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
import Checkbox from 'primevue/checkbox';
import Tag from 'primevue/tag';

const toast = useToast();
const confirm = useConfirm();

const funcionarios = ref([]);
const loading = ref(false);
const saving = ref(false);

const funcionarioDialog = ref(false);
const funcionario = ref({});
const submitted = ref(false);

onMounted(() => {
  loadFuncionarios();
});

const loadFuncionarios = async () => {
  loading.value = true;
  try {
    const response = await api.get('/funcionarios');
    // Ajuste conforme o formato do BaseController: Success(data)
    funcionarios.value = response.data?.data || response.data;
  } catch (error) {
    console.error('Erro ao buscar funcionários', error);
    toast.add({ severity: 'error', summary: 'Erro', detail: 'Não foi possível carregar os funcionários.', life: 3000 });
  } finally {
    loading.value = false;
  }
};

const openNew = () => {
  funcionario.value = { ativo: true };
  submitted.value = false;
  funcionarioDialog.value = true;
};

const hideDialog = () => {
  funcionarioDialog.value = false;
  submitted.value = false;
};

const saveFuncionario = async () => {
  submitted.value = true;
  if (funcionario.value.especialidade?.trim() && funcionario.value.usuarioId?.trim()) {
    saving.value = true;
    try {
      if (funcionario.value.id) {
        await api.put(`/funcionarios/${funcionario.value.id}`, funcionario.value);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Funcionário atualizado', life: 3000 });
      } else {
        await api.post('/funcionarios', funcionario.value);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Funcionário criado', life: 3000 });
      }
      funcionarioDialog.value = false;
      funcionario.value = {};
      await loadFuncionarios();
    } catch (error) {
      console.error('Erro ao salvar funcionário', error);
      toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao salvar o funcionário', life: 3000 });
    } finally {
      saving.value = false;
    }
  }
};

const editFuncionario = (data) => {
  funcionario.value = { ...data };
  funcionarioDialog.value = true;
};

const confirmDeleteFuncionario = (data) => {
  confirm.require({
    message: `Tem certeza que deseja excluir o funcionário?`,
    header: 'Confirmação',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: 'Sim',
    rejectLabel: 'Não',
    accept: async () => {
      try {
        await api.delete(`/funcionarios/${data.id}`);
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Funcionário excluído', life: 3000 });
        await loadFuncionarios();
      } catch (error) {
        console.error('Erro ao excluir funcionário', error);
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Falha ao excluir o funcionário', life: 3000 });
      }
    }
  });
};
</script>
