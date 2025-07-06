<script setup lang="ts">
import HeaderComponent from '@/components/HeaderComponent.vue'
import { getAllTaskItems, type TaskItemDto, type TaskItemsInjectObject } from '@/lib/tasks'
import { onMounted, provide, ref, watchEffect } from 'vue'
import { RouterView } from 'vue-router'

let taskItems = ref<TaskItemDto[]>()
let loading = ref(true)

async function refreshTaskItems() {
  try {
    loading.value = true
    const data = await getAllTaskItems()
    taskItems.value = data
  } catch (err) {
    console.error('Error loading task items:', err)
  } finally {
    loading.value = false
  }
}

let taskItemsObject: TaskItemsInjectObject = {
  taskItems,
  loading,
  refreshTaskItems,
}

provide('taskItemsObj', taskItemsObject)

onMounted(() => {
  refreshTaskItems()
})
</script>

<template>
  <div class="app-layout px-4 md:px-24 lg:px-32 w-full py-16 flex flex-col gap-y-16">
    <HeaderComponent />
    <RouterView></RouterView>
  </div>
</template>
