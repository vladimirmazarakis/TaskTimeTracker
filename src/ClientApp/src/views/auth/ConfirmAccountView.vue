<script setup lang="ts">
import isMobile from '@/lib/isMobile'
import { getToastGroup } from '@/lib/utils'
import { useToast } from 'primevue'
import { useRouter } from 'vue-router'

const toast = useToast()
const router = useRouter()

toast.add({
  severity: 'success',
  summary: 'Your email address is now verified.',
  detail: 'You can now sign in.',
  group: getToastGroup(),
})

router.replace('/auth/login')
</script>

<template></template>
