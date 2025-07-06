<script setup lang="ts">
import {
  getAllTaskItems,
  taskItemsInjectObjectDefaultValue,
  type TaskItemDto,
  type TaskItemsInjectObject,
} from '@/lib/tasks'
import { inject, provide, ref, watchEffect } from 'vue'
import { Skeleton } from 'primevue'
import TaskItem from '@/components/views/home/TaskItem.vue'

let taskItemsObj: TaskItemsInjectObject = inject('taskItemsObj', taskItemsInjectObjectDefaultValue)
</script>

<template>
  <Skeleton v-if="taskItemsObj.loading.value" class="w-full py-20 px-3 rounded-lg"> </Skeleton>
  <div
    v-if="
      !taskItemsObj.loading.value &&
      taskItemsObj.taskItems.value &&
      taskItemsObj.taskItems.value.length > 0
    "
    class="w-full px-3 py-3 md:py-5 md:px-5 bg-[var(--p-rose-100)] flex flex-col gap-y-4 border-2 rounded-lg border-[var(--p-rose-200)]"
  >
    <TaskItem v-for="taskItem in taskItemsObj.taskItems.value" :taskItem="taskItem" />
  </div>
  <div
    v-if="
      !taskItemsObj.loading.value &&
      taskItemsObj.taskItems.value &&
      taskItemsObj.taskItems.value.length <= 0
    "
    class="w-full py-5 px-3 bg-[var(--p-rose-100)] flex justify-center items-center text-center border-2 rounded-lg border-[var(--p-rose-200)]"
  >
    List is empty. Try adding a new task :)
  </div>
</template>
