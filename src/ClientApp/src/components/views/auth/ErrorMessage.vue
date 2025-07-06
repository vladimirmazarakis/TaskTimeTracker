<script setup lang="ts">
import Message from 'primevue/message'
</script>

<template>
  <Message class="mx-2 mt-1" severity="error" size="small" variant="simple"><slot></slot></Message>
</template>
