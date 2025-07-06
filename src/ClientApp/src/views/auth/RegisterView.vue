<script setup lang="ts">
import AuthCard from '@/components/views/auth/AuthCard.vue'
import { IconField, InputIcon, InputText, Button, BlockUI } from 'primevue'
import { Form } from '@primevue/forms'
import { type FormSubmitEvent } from '@primevue/forms'
import { RouterLink, useRouter } from 'vue-router'
import { zodResolver } from '@primevue/forms/resolvers/zod'
import { handleServerErrorObject, passDoNotMatchText, signUp, signUpSchema } from '@/lib/auth'
import ErrorMessage from '@/components/views/auth/ErrorMessage.vue'
import { useForm } from '@primevue/forms/useform'
import { ref } from 'vue'
import { useToast } from 'primevue/usetoast'
import isMobile from '@/lib/isMobile'
import { getToastGroup } from '@/lib/utils'

const toast = useToast()

const resolver = zodResolver(signUpSchema)

let confirmPasswordErr = ref('')
let loading = ref(false)

const router = useRouter()

const submit = (event: FormSubmitEvent<Record<string, any>>) => {
  loading.value = true
  if (!event.valid) {
    loading.value = false
    return
  }

  let email = event.values?.email
  let password = event.values?.password
  let confirmPassword = event.values?.confirmPassword

  if (password !== confirmPassword) {
    confirmPasswordErr.value = passDoNotMatchText
    loading.value = false
    return
  }

  signUp(email, password).then((d: any) => {
    if (d?.errors) {
      handleServerErrorObject(d, toast)
    } else {
      toast.add({
        severity: 'success',
        summary: 'Thank you!',
        detail:
          'Your registration is almost completed. Verify your account by clicking on the link we sent to your email.',
        life: 5000,
        group: getToastGroup(),
      })
      router.push('/auth/login')
    }
    loading.value = false
  })
}
</script>

<template>
  <BlockUI :blocked="loading" fullScreen></BlockUI>
  <div class="flex flex-col items-center gap-4 w-full">
    <div class="flex flex-col gap-2 w-full">
      <div class="text-center text-3xl font-medium text-white leading-tight">
        Welcome to Task Time Tracker
      </div>
      <div class="text-center">
        <span class="text-white/80">Have an account? </span>
        <RouterLink
          to="/auth/login"
          class="text-white/80 cursor-pointer hover:text-white/90 underline"
          >Sign in</RouterLink
        >
      </div>
    </div>
  </div>
  <Form
    @submit="submit"
    :resolver="resolver"
    v-slot="$form"
    class="flex flex-col items-center gap-8 w-full"
  >
    <div class="flex flex-col gap-6 w-full">
      <div>
        <InputText
          name="email"
          type="text"
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

      <div>
        <InputText
          name="confirmPassword"
          @change="
            () => {
              confirmPasswordErr = ''
            }
          "
          type="password"
          class="!appearance-none !border !border-white/10 !w-full !outline-0 !bg-white/10 !text-white placeholder:!text-white/70 !rounded-3xl !shadow-sm"
          placeholder="Confirm password"
        />
        <ErrorMessage v-if="confirmPasswordErr">{{ confirmPasswordErr }}</ErrorMessage>
      </div>
    </div>
    <Button
      label="Sign Up"
      class="!w-full !rounded-3xl !bg-surface-950 !border !border-surface-950 !text-white hover:!bg-surface-950/80"
      type="submit"
    />
  </Form>
  <RouterLink
    to="/auth/forgot-pass"
    class="text-white/80 cursor-pointer text-center hover:text-white/90"
    >Have an account you want to recover?</RouterLink
  >
</template>
