<script setup lang="ts">
import {
  createTask,
  taskCreateUpdateSchema,
  taskItemsInjectObjectDefaultValue,
  type CreateTaskItemDto,
  type TaskItemDto,
  type TaskItemsInjectObject,
} from '@/lib/tasks'
import { Form, type FormResolverOptions, type FormSubmitEvent } from '@primevue/forms'
import { zodResolver } from '@primevue/forms/resolvers/zod'
import { Dialog, InputText, Button, Textarea, Select, Message } from 'primevue'
import { watchEffect, ref, watch, inject } from 'vue'

let props = defineProps<{ visible: boolean }>()
let emit = defineEmits(['dialog-visibility-changed'])

let dialogVisible = ref(props.visible)
let taskItemName = ref('New Task')
let dialogHeader = ref(taskItemName.value)

let selected = ref(0)

watch(taskItemName, () => {
  if (!taskItemName.value) {
    dialogHeader.value = 'New Task'
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

const taskItemsObj: TaskItemsInjectObject = inject(
  'taskItemsObj',
  taskItemsInjectObjectDefaultValue,
)

const formSubmit = (event: FormSubmitEvent<Record<string, any>>) => {
  if (!event.valid) {
    return
  }

  let taskItem: CreateTaskItemDto = {
    name: event.values.name,
    description: event.values.description,
    priority: event.values.priority,
  }

  createTask(taskItem).then(() => {
    taskItemsObj.refreshTaskItems()
    dialogVisible.value = false
  })
}

const customResolve = (e: FormResolverOptions) => {
  if (e.values['priority'] === null) {
    e.values['priority'] = selected.value
  }
  if (e.values['name'] === null && taskItemName.value) {
    e.values['name'] = taskItemName.value
  }
  if (e.values['description'] === null) {
    e.values['description'] = ''
  }
  return resolver(e)
}
</script>

<template>
  <Dialog modal v-model:visible="dialogVisible" :header="dialogHeader">
    <Form
      :resolver="customResolve"
      @submit="formSubmit"
      v-slot="$form"
      class="flex flex-col gap-y-3 w-80"
    >
      <div class="flex w-full flex-col gap-y-1">
        <label for="name">Task name</label>
        <InputText fluid name="name" type="text" v-model="taskItemName" placeholder="Task Name" />
        <Message v-if="$form.name?.invalid" severity="error" variant="simple">{{
          $form.name.error.message
        }}</Message>
      </div>
      <div class="flex w-full flex-col gap-y-1">
        <label for="description">Task description</label>
        <Textarea fluid name="description" type="text" placeholder="Task description" />
        <Message v-if="$form.description?.invalid" severity="error" variant="simple">{{
          $form.description.error.message
        }}</Message>
      </div>
      <div class="flex w-full flex-col gap-y-1">
        <label for="taskPriority">Task priority</label>
        <Select
          :options="priorities"
          optionLabel="name"
          placeholder="Select priority"
          option-value="code"
          checkmark
          :highlightOnSelect="false"
          class="w-full"
          name="priority"
          v-model="selected"
        />
        <Message v-if="$form.priority?.invalid" severity="error" variant="simple">{{
          $form.priority.error.message
        }}</Message>
      </div>
      <Button type="submit" severity="success" label="Create" />
    </Form>
  </Dialog>
</template>
