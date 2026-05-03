<template>
  <div class="min-h-screen flex items-center justify-center bg-surface-50 dark:bg-surface-900 p-4">
    <div class="bg-surface-0 dark:bg-surface-800 p-8 rounded-xl shadow-lg w-full max-w-md">
      <div class="text-center mb-8">
        <h1 class="text-3xl font-bold text-primary">Wsm Admin</h1>
        <p class="text-surface-500 dark:text-surface-400 mt-2">Faça login para continuar</p>
      </div>

      <form @submit.prevent="handleLogin" class="flex flex-col gap-4">
        <div class="flex flex-col gap-2">
          <label for="email" class="text-surface-700 dark:text-surface-300 font-medium">E-mail</label>
          <InputText id="email" v-model="email" type="email" placeholder="admin@exemplo.com" required class="w-full" />
        </div>
        
        <div class="flex flex-col gap-2">
          <label for="password" class="text-surface-700 dark:text-surface-300 font-medium">Senha</label>
          <InputText id="password" v-model="password" type="password" placeholder="••••••••" required class="w-full" />
        </div>

        <Button type="submit" label="Entrar" :loading="loading" class="w-full mt-4" />
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import api from '../services/api';

const router = useRouter();
const email = ref('');
const password = ref('');
const loading = ref(false);

const handleLogin = async () => {
  loading.value = true;
  try {
    // Exemplo de login real (descomente e ajuste conforme a API):
    // const response = await api.post('/auth/login', { email: email.value, password: password.value });
    // localStorage.setItem('token', response.data.token);
    
    // Mock de login para MVP
    setTimeout(() => {
      localStorage.setItem('token', 'fake-jwt-token-para-mvp');
      router.push('/');
    }, 1000);
  } catch (error) {
    console.error('Erro de login:', error);
  } finally {
    loading.value = false;
  }
};
</script>
