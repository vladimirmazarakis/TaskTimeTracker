<script setup lang="ts">
import AuthCard from '@/components/views/auth/AuthCard.vue'
import { RouterView, useRoute, useRouter } from 'vue-router'

const router = useRouter()
const route = useRoute()

if (route.path === '/auth') {
  router.push('/auth/login')
}
</script>

<template>
  <AuthCard>
    <RouterView />
  </AuthCard>
</template>
