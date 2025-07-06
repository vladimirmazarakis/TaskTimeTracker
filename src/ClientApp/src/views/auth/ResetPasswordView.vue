<script setup lang="ts">
import AuthCard from '@/components/views/auth/AuthCard.vue'
import { IconField, InputIcon, InputText, Button, useToast } from 'primevue'
import { Form, type FormSubmitEvent } from '@primevue/forms'
import { RouterLink, useRoute, useRouter } from 'vue-router'
import { ref, watchEffect } from 'vue'
import { zodResolver } from '@primevue/forms/resolvers/zod'
import { resetPassSchema, handleServerErrorObject, passDoNotMatchText, resetPass } from '@/lib/auth'
import ErrorMessage from '@/components/views/auth/ErrorMessage.vue'
import isMobile from '@/lib/isMobile'
import { getToastGroup } from '@/lib/utils'

const route = useRoute()
const router = useRouter()

if (!route.query.code || !route.query.email) {
  router.push('/')
}

const code = route.query.code?.toString()
const email = route.query.email?.toString()

if (!code || !email) {
  router.push('/')
}

let confirmPassErr = ref('')

const resolver = zodResolver(resetPassSchema)

const toast = useToast()

const submit = (event: FormSubmitEvent<Record<string, any>>) => {
  if (!email || !code) {
    return
  }
  resetPass(email, code, event.values.password).then((d: any) => {
    if (d?.errors) {
      handleServerErrorObject(d, toast)
    } else {
      toast.add({
        severity: 'success',
        summary: 'Password successfully changed!',
        detail: 'You can sign in, with your new password now.',
        life: 5000,
        group: getToastGroup(),
      })
      router.push('/auth/login')
    }
  })
}
</script>

<template>
  <div class="flex flex-col items-center gap-4 w-full">
    <div class="flex flex-col gap-2 w-full">
      <div class="text-center text-3xl font-medium text-white leading-tight">
        Enter your new password
      </div>
    </div>
  </div>
  <Form
    v-slot="$form"
    :resolver="resolver"
    @submit="submit"
    class="flex flex-col items-center gap-8 w-full"
  >
    <div class="flex flex-col gap-6 w-full">
      <div>
        <InputText
          name="password"
          type="password"
          class="!appearance-none !border !border-white/10 !w-full !outline-0 !bg-white/10 !text-white placeholder:!text-white/70 !rounded-3xl !shadow-sm"
          placeholder="Password"
        />
        <ErrorMessage v-if="$form.password?.invalid">{{
          $form.password?.error?.message
        }}</ErrorMessage>
      </div>

      <div>
        <InputText
          name="confirmPassword"
          type="password"
          class="!appearance-none !border !border-white/10 !w-full !outline-0 !bg-white/10 !text-white placeholder:!text-white/70 !rounded-3xl !shadow-sm"
          placeholder="Confirm password"
        />
        <ErrorMessage v-if="confirmPassErr">{{ confirmPassErr }}</ErrorMessage>
      </div>
    </div>
    <Button
      label="Reset password"
      class="!w-full !rounded-3xl !bg-surface-950 !border !border-surface-950 !text-white hover:!bg-surface-950/80"
      type="submit"
    />
  </Form>
  <!-- <RouterLink to="/auth/login" class="text-white/80 text-center cursor-pointer hover:text-white/90"
    >Remember your password?</RouterLink
  > -->
</template>
