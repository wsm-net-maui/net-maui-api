<template>
  <div class="min-h-screen bg-surface-50 dark:bg-surface-900">
    <header class="bg-surface-0 dark:bg-surface-800 shadow-sm border-b border-surface-200 dark:border-surface-700 px-6 py-4 flex justify-between items-center">
      <div class="flex items-center gap-6">
        <h2 class="text-xl font-bold text-primary mr-4 cursor-pointer" @click="$router.push('/')">Wsm Admin</h2>
        <nav class="hidden md:flex gap-4">
          <router-link to="/" class="text-surface-600 dark:text-surface-300 hover:text-primary transition-colors" active-class="font-semibold text-primary">Dashboard</router-link>
          <router-link to="/servicos" class="text-surface-600 dark:text-surface-300 hover:text-primary transition-colors" active-class="font-semibold text-primary">Serviços</router-link>
          <router-link to="/funcionarios" class="text-surface-600 dark:text-surface-300 hover:text-primary transition-colors" active-class="font-semibold text-primary">Funcionários</router-link>
          <router-link to="/horarios" class="text-surface-600 dark:text-surface-300 hover:text-primary transition-colors" active-class="font-semibold text-primary">Agenda</router-link>
        </nav>
      </div>
      <Button icon="pi pi-sign-out" text rounded @click="logout" aria-label="Sair" />
    </header>

    <main>
      <router-view></router-view>
    </main>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
import Button from 'primevue/button';

const router = useRouter();

const logout = () => {
  localStorage.removeItem('token');
  router.push('/login');
};
</script>
