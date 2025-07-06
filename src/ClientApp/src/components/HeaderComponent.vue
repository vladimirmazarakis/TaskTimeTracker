<script setup lang="ts">
import { clearAuthentication } from '@/lib/auth'
import isMobile from '@/lib/isMobile'
import { getToastGroup } from '@/lib/utils'
import { Button, useToast } from 'primevue'
import { useRouter } from 'vue-router'
import AddTaskItemDialog from './views/home/AddTaskItemDialog.vue'
import { ref } from 'vue'
import TrackTimeDialog from './views/home/taskSessions/TrackTimeDialog.vue'

const toast = useToast()
const router = useRouter()

const signOutClick = () => {
  toast.add({
    severity: 'success',
    summary: 'Successfully signed out',
    detail: 'Very sad, you left',
    life: 5000,
    group: getToastGroup(),
  })

  clearAuthentication()

  router.push('/auth/login')
}

let addTaskDialogVisible = ref(false)
let trackDialogVisible = ref(false)
</script>

<template>
  <div class="w-full flex-col justify-center gap-y-4 md:justify-between flex md:flex-row">
    <div class="flex flex-col justify-center gap-y-4 md:flex-row gap-x-4">
      <AddTaskItemDialog
        :visible="addTaskDialogVisible"
        @dialog-visibility-changed="
          (dialogVisibility: boolean) => (addTaskDialogVisible = dialogVisibility)
        "
      />
      <Button
        severity="success"
        label="New Task"
        @click="addTaskDialogVisible = !addTaskDialogVisible"
      ></Button>
      <TrackTimeDialog
        :visible="trackDialogVisible"
        @dialog-visibility-changed="
          (dialogVisibility: boolean) => (trackDialogVisible = dialogVisibility)
        "
      />
      <Button @click="trackDialogVisible = !trackDialogVisible" label="Track Time"></Button>
    </div>
    <Button severity="warn" @click="signOutClick" label="Sign out"></Button>
  </div>
</template>
