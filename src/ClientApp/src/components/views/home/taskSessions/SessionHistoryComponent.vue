<script setup lang="ts">
import type { TaskSessionDto } from '@/lib/tasks'
import { formatSecondsToDigitalTimerString } from '@/lib/utils'
import { DataTable, Column, Panel } from 'primevue'
import { ref, watch, watchEffect } from 'vue'

const props = defineProps<{
  taskSessions: TaskSessionDto[]
}>()

let formattedTaskSessions = ref<any[]>([])

watchEffect(() => {
  formattedTaskSessions.value = []
  for (const taskSession of props.taskSessions) {
    let startDate = new Date(taskSession.startDate).toLocaleString()
    let endDate = new Date(taskSession.endDate).toLocaleString()
    formattedTaskSessions.value.push({
      startDate: startDate,
      endDate: endDate,
      duration: formatSecondsToDigitalTimerString(taskSession.totalDuration),
    })
  }
})
</script>

<template>
  <Panel class="w-full" header="Task sessions history" toggleable :collapsed="true">
    <DataTable :value="formattedTaskSessions" class="w-full">
      <Column field="startDate" header="Start"></Column>
      <Column field="endDate" header="End"></Column>
      <Column field="duration" header="Duration"></Column>
    </DataTable>
  </Panel>
</template>
