<script setup lang="ts">
import AuthCard from '@/components/views/auth/AuthCard.vue'
import { IconField, InputIcon, InputText, Button, useToast, BlockUI } from 'primevue'
import { Form, type FormSubmitEvent } from '@primevue/forms'
import { handleServerErrorObject, signIn, signInSchema } from '@/lib/auth'
import { RouterLink, useRouter } from 'vue-router'
import { z } from 'zod'
import { zodResolver } from '@primevue/forms/resolvers/zod'
import Message from 'primevue/message'
import ErrorMessage from '@/components/views/auth/ErrorMessage.vue'
import isMobile from '@/lib/isMobile'
import { ref } from 'vue'
import { getToastGroup } from '@/lib/utils'

const resolver = zodResolver(signInSchema)

const router = useRouter()

let loading = ref(false)

const toast = useToast()

const submit = (event: FormSubmitEvent<Record<string, any>>) => {
  if (!event.valid) {
    return
  }
  loading.value = true
  const email = event?.values?.email
  const password = event?.values.password

  if (!email || !password) {
    loading.value = false
    return
  }

  signIn(email, password).then((res: any) => {
    if (res?.status === 401) {
      toast.add({
        severity: 'error',
        summary: 'Wrong email or password',
        group: getToastGroup(),
      })
    } else if (res?.errors) {
      handleServerErrorObject(res, toast)
    } else {
      toast.add({
        severity: 'success',
        summary: 'Welcome back',
        detail: 'Successful authorization.',
        life: 5000,
        group: getToastGroup(),
      })
      router.push('/')
    }
    loading.value = false
  })
}
</script>

<template>
  <BlockUI :blocked="loading" fullScreen />
  <div class="flex flex-col items-center gap-4 w-full">
    <div class="flex flex-col gap-2 w-full">
      <div class="text-center text-3xl font-medium text-white leading-tight">Welcome Back</div>
      <div class="text-center">
        <span class="text-white/80">Don't have an account? </span>
        <RouterLink
          to="/auth/register"
          class="text-white/80 cursor-pointer hover:text-white/90 underline"
          >Sign up</RouterLink
        >
      </div>
    </div>
  </div>
  <Form
    v-slot="$form"
    @submit="submit"
    :resolver="resolver"
    class="flex flex-col items-center gap-8 w-full"
  >
    <div class="flex flex-col gap-6 w-full">
      <div>
        <InputText
          name="email"
          type="email"
          class="!appearance-none !border !border-white/10 !w-full !outline-0 !bg-white/10 !text-white placeholder:!text-white/70 !rounded-3xl !shadow-sm"
          placeholder="Email"
        />
        <ErrorMessage v-if="$form.email?.invalid">{{ $form.email.error.message }}</ErrorMessage>
      </div>
      <div>
        <InputText
          name="password"
          type="password"
          class="!appearance-none !border !border-white/10 !w-full !outline-0 !bg-white/10 !text-white placeholder:!text-white/70 !rounded-3xl !shadow-sm"
          placeholder="Password"
        />
        <ErrorMessage v-if="$form.password?.invalid">{{
          $form.password.error.message
        }}</ErrorMessage>
      </div>
    </div>
    <Button
      label="Sign In"
      class="!w-full !rounded-3xl !bg-surface-950 !border !border-surface-950 !text-white hover:!bg-surface-950/80"
      type="submit"
    />
  </Form>
  <RouterLink
    to="/auth/forgot-pass"
    class="text-white/80 text-center cursor-pointer hover:text-white/90"
    >Forgot your password?</RouterLink
  >
</template>
