<script setup lang="ts">
import {
  taskCreateUpdateSchema,
  TaskItemPriority,
  taskItemsInjectObjectDefaultValue,
  updateTask,
  type TaskItemDto,
  type TaskItemsInjectObject,
  type UpdateTaskItemDto,
} from '@/lib/tasks'
import { getToastGroup } from '@/lib/utils'
import { Form, type FormResolverOptions, type FormSubmitEvent } from '@primevue/forms'
import { zodResolver } from '@primevue/forms/resolvers/zod'
import { Dialog, InputText, Button, Textarea, Select, useToast } from 'primevue'
import { watchEffect, ref, watch, inject } from 'vue'

let props = defineProps<{ visible: boolean; taskItem: TaskItemDto }>()
let emit = defineEmits(['dialog-visibility-changed'])

let dialogVisible = ref(props.visible)
let taskItemName = ref(props.taskItem.name)
let dialogHeader = ref(taskItemName.value)

watch(taskItemName, () => {
  if (!taskItemName.value) {
    dialogHeader.value = props.taskItem.name
  } else {
    dialogHeader.value = taskItemName.value
  }
})

watchEffect(() => {
  dialogVisible.value = props.visible
})

watch(dialogVisible, () => {
  emit('dialog-visibility-changed', dialogVisible)
})

const priorities = ref([
  { name: 'None', code: 0 },
  { name: 'Low', code: 1 },
  { name: 'Medium', code: 2 },
  { name: 'High', code: 3 },
  { name: 'Urgent', code: 4 },
])

const resolver = zodResolver(taskCreateUpdateSchema)

const toast = useToast()

const taskItemsObj: TaskItemsInjectObject = inject(
  'taskItemsObj',
  taskItemsInjectObjectDefaultValue,
)

const formSubmit = (event: FormSubmitEvent<Record<string, any>>) => {
  if (!event.valid) {
    return
  }

  let updateTaskItemDto: UpdateTaskItemDto = {
    id: props.taskItem.id,
    name: event.values.name,
    description: event.values.description,
    priority: event.values.priority,
  }

  updateTask(updateTaskItemDto).then(() => {
    taskItemsObj.refreshTaskItems()
    dialogVisible.value = false
    toast.add({
      severity: 'success',
      summary: 'Successfully updated task.',
      life: 3000,
      group: getToastGroup(),
    })
  })
}

const customResolve = (e: FormResolverOptions) => {
  if (e.values['priority'] === null) {
    e.values['priority'] = props.taskItem.priority
  }
  if (e.values['name'] === null) {
    e.values['name'] = props.taskItem.name
  }
  if (e.values['description'] === null) {
    e.values['description'] = props.taskItem.description
  }
  return resolver(e)
}
</script>

<template>
  <Dialog modal v-model:visible="dialogVisible" class="min-w-80" :header="dialogHeader">
    <Form
      @submit="formSubmit"
      :resolver="customResolve"
      v-slot="$form"
      class="flex flex-col gap-y-3 w-full"
    >
      <div class="flex w-full flex-col gap-y-1">
        <label for="name">Task name</label>
        <InputText fluid name="name" type="text" v-model="taskItemName" placeholder="Task Name" />
      </div>
      <div class="flex w-full flex-col gap-y-1">
        <label for="description">Task description</label>
        <Textarea
          fluid
          name="description"
          type="text"
          :default-value="taskItem.description"
          placeholder="Task description"
        />
      </div>
      <div class="flex w-full flex-col gap-y-1">
        <label for="priority">Task priority</label>
        <Select
          :options="priorities"
          optionLabel="name"
          placeholder="Select priority"
          option-value="code"
          checkmark
          :highlightOnSelect="false"
          class="w-full"
          :default-value="taskItem.priority"
          name="priority"
        />
      </div>
      <Button type="submit" severity="warn" label="Update" />
    </Form>
  </Dialog>
</template>
