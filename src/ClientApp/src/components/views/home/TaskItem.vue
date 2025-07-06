<script setup lang="ts">
import {
  completeTask,
  deleteTask,
  incompleteTask,
  TaskItemPriority,
  taskItemsInjectObjectDefaultValue,
  type TaskItemDto,
  type TaskItemsInjectObject,
} from '@/lib/tasks'
import { Badge, Button, Dialog, useConfirm, useToast } from 'primevue'
import { inject, ref } from 'vue'
import EditTaskItemDialog from '@/components/views/home/EditTaskItemDialog.vue'
import ConfirmPopup from 'primevue/confirmpopup'
import { getToastGroup } from '@/lib/utils'

const props = defineProps<{ taskItem: TaskItemDto }>()

const deleteConfirm = useConfirm()

const toast = useToast()

let taskItemRef = ref(props.taskItem)
let editDialogOpen = ref(false)

let badgeSeverity = ''
let badgeText = ''

const taskItemsObj: TaskItemsInjectObject = inject(
  'taskItemsObj',
  taskItemsInjectObjectDefaultValue,
)

switch (taskItemRef.value.priority) {
  case TaskItemPriority.none:
    badgeSeverity = 'secondary'
    badgeText = 'No priority'
    break
  case TaskItemPriority.low:
    badgeSeverity = 'success'
    badgeText = 'Low priority'
    break
  case TaskItemPriority.medium:
    badgeSeverity = 'info'
    badgeText = 'Medium priority'
    break
  case TaskItemPriority.high:
    badgeSeverity = 'warn'
    badgeText = 'High priority'
    break
  case TaskItemPriority.urgent:
    badgeSeverity = 'danger'
    badgeText = 'Urgent priority'
    break
}

const completeButton = () => {
  if (!taskItemRef.value.isCompleted) {
    completeTask(taskItemRef.value.id).then((data) => {
      taskItemRef.value = data
      toast.add({
        severity: 'success',
        summary: 'Task completed.',
        life: 3000,
        group: getToastGroup(),
      })
    })
  } else {
    incompleteTask(taskItemRef.value.id).then((data) => {
      taskItemRef.value = data
    })
  }
}

const deleteButton = (event: any) => {
  deleteConfirm.require({
    target: event.currentTarget,
    message: 'Are you sure you want to proceed?',
    rejectProps: {
      label: 'Cancel',
      severity: 'secondary',
      outlined: true,
    },
    acceptProps: {
      label: 'Delete',
    },
    accept: () => {
      deleteTask(taskItemRef.value.id).then(() => {
        toast.add({
          severity: 'error',
          summary: 'Deleted',
          life: 5000,
          group: getToastGroup(),
        })
        taskItemsObj.refreshTaskItems()
      })
    },
  })
}
</script>

<template>
  <div
    class="w-full py-6 px-6 bg-white border-2 flex flex-col md:flex-row justify-start gap-y-4 md:justify-between rounded-lg border-[var(--p-rose-200)]"
  >
    <div class="flex flex-col gap-y-4">
      <span class="text-xl font-semibold">{{ taskItemRef.name }}</span>
      <span class="text-sm text-stone-700">{{ taskItemRef.description }}</span>
      <Badge class="w-max" :severity="badgeSeverity" :value="badgeText"></Badge>
    </div>
    <div
      class="flex flex-col gap-y-4 md:flex-row gap-x-4 justify-start md:justify-center items-center"
    >
      <Button
        @click="completeButton"
        class="md:w-auto w-full h-max"
        :severity="taskItemRef.isCompleted ? 'danger' : 'success'"
        :label="taskItemRef.isCompleted ? 'Mark as incomplete' : 'Mark as complete'"
      ></Button>
      <EditTaskItemDialog
        :visible="editDialogOpen"
        :task-item="taskItem"
        @dialog-visibility-changed="
          (dialogVisibility: boolean) => (editDialogOpen = dialogVisibility)
        "
      />
      <Button
        class="h-max md:w-auto w-full"
        @click="editDialogOpen = !editDialogOpen"
        severity="info"
        label="Edit"
      ></Button>
      <ConfirmPopup></ConfirmPopup>
      <Button
        class="h-max md:w-auto w-full"
        @click="deleteButton"
        severity="danger"
        label="Delete"
      ></Button>
    </div>
  </div>
</template>
