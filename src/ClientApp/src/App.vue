<script setup lang="ts">
import { RouterView } from 'vue-router'
import { Toast } from 'primevue'
</script>

<template>
  <header></header>

  <main>
    <Toast position="bottom-right" group="br" />
    <Toast position="bottom-center" class="w-full px-4" group="bc" />

    <RouterView />
  </main>
</template>
