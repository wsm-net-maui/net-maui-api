<template>
  <Toast />
  <ConfirmDialog />
  <router-view></router-view>
</template>

<script setup>
import Toast from 'primevue/toast';
import ConfirmDialog from 'primevue/confirmdialog';
</script>
