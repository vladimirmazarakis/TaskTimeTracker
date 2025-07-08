<script setup lang="ts">
import { inject, ref, watch, watchEffect } from 'vue'
import { Button, Dialog, Select, useToast, type SelectChangeEvent } from 'primevue'
import {
  getTaskSessionDuration,
  getTotalTaskSessionDuration,
  startTaskSession,
  stopTaskSession,
  taskItemsInjectObjectDefaultValue,
  type TaskItemsInjectObject,
} from '@/lib/tasks'
import { getToastGroup } from '@/lib/utils'
import TimerComponent from '@/components/views/home/taskSessions/TimerComponent.vue'
import TotalTimeSpentComponent from './TotalTimeSpentComponent.vue'

let props = defineProps<{ visible: boolean }>()
let emit = defineEmits(['dialog-visibility-changed'])

const toast = useToast()

let dialogVisible = ref(props.visible)

const taskItemsObj: TaskItemsInjectObject = inject(
  'taskItemsObj',
  taskItemsInjectObjectDefaultValue,
)

watchEffect(() => {
  dialogVisible.value = props.visible
})

let tasks = ref<any>([])

let selectedTask = ref<number | null>(null)

watch(dialogVisible, () => {
  emit('dialog-visibility-changed', dialogVisible)
  if (
    dialogVisible.value == true &&
    (!taskItemsObj?.taskItems?.value || taskItemsObj.taskItems.value.length <= 0)
  ) {
    toast.add({
      severity: 'error',
      summary: 'Add at least one task',
      life: 5000,
      group: getToastGroup(),
    })
    dialogVisible.value = false
  } else if (taskItemsObj.taskItems.value) {
    for (const taskItem of taskItemsObj?.taskItems?.value) {
      tasks.value.push({
        name: taskItem.name,
        id: taskItem.id,
      })
    }
  }

  if (dialogVisible.value == false) {
    tasks.value = []
    curDuration.value = 0
    totalTime.value = 0
    isStarted.value = false
    clearInterval(timer.value)
  }
})

let curDuration = ref(0)
let totalTime = ref(0)
let isStarted = ref(false)
let timer = ref(0)

watch(isStarted, () => {
  if (isStarted.value) {
    clearInterval(timer.value)
    timer.value = setInterval(() => curDuration.value++, 1000)
  } else {
    clearInterval(timer.value)
  }
})

const selectedChange = async (event: SelectChangeEvent) => {
  isStarted.value = false

  if (!selectedTask.value) {
    curDuration.value = 0
    return
  }

  curDuration.value = await getTaskSessionDuration(selectedTask.value)
  totalTime.value = await getTotalTaskSessionDuration(selectedTask.value)
  if (curDuration.value > 0) {
    isStarted.value = true
  }
}

const startSession = async () => {
  if (!selectedTask.value) {
    toast.add({
      severity: 'error',
      summary: 'Select a task first.',
      life: 3000,
      group: getToastGroup(),
    })
    return
  }
  if (isStarted.value) {
    return
  }
  await startTaskSession(selectedTask.value)
  isStarted.value = true
}

const stopSession = async () => {
  if (!selectedTask.value) {
    toast.add({
      severity: 'error',
      summary: 'Select a task first.',
      life: 3000,
      group: getToastGroup(),
    })
    return
  }
  if (!isStarted.value) {
    return
  }
  await stopTaskSession(selectedTask.value)
  totalTime.value = await getTotalTaskSessionDuration(selectedTask.value)
  isStarted.value = false
}
</script>

<template>
  <Dialog modal v-model:visible="dialogVisible" class="min-w-96" header="Time tracking">
    <div class="flex flex-col gap-y-4 w-full">
      <TimerComponent :duration="curDuration" />
      <TotalTimeSpentComponent :duration="totalTime" />
      <Select
        v-if="tasks.length > 0"
        :options="tasks"
        option-label="name"
        option-value="id"
        default-value="0"
        placeholder="Select task"
        class="w-full"
        v-model="selectedTask"
        @change="selectedChange"
      ></Select>
      <div class="flex flex-row w-full gap-x-4">
        <Button
          :disabled="isStarted"
          @click="startSession"
          class="w-full"
          label="Start Session"
        ></Button>
        <Button
          :disabled="!isStarted"
          @click="stopSession"
          class="w-full"
          label="Stop Session"
        ></Button>
      </div>
    </div>
  </Dialog>
</template>
