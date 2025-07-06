<script setup lang="ts">
import AuthCard from '@/components/views/auth/AuthCard.vue'
import { IconField, InputIcon, InputText, Button, useToast, BlockUI } from 'primevue'
import { Form, type FormSubmitEvent } from '@primevue/forms'
import { RouterLink, useRoute, useRouter } from 'vue-router'
import { ref, watchEffect } from 'vue'
import { zodResolver } from '@primevue/forms/resolvers/zod'
import {
  forgotPass,
  forgotPassSchema,
  handleServerErrorObject,
  passDoNotMatchText,
  resetPass,
} from '@/lib/auth'
import ErrorMessage from '@/components/views/auth/ErrorMessage.vue'
import isMobile from '@/lib/isMobile'
import { getToastGroup } from '@/lib/utils'

const route = useRoute()
const router = useRouter()

const resolver = zodResolver(forgotPassSchema)

let loading = ref(false)

const toast = useToast()

const submit = (event: FormSubmitEvent<Record<string, any>>) => {
  loading.value = true
  forgotPass(event.values?.email).then(() => {
    toast.add({
      severity: 'warn',
      summary: 'We sent you an email.',
      detail: 'Follow the instructions, in order to recover your password.',
      life: 5000,
      group: getToastGroup(),
    })
    router.push('/')
    loading.value = false
  })
}
</script>

<template>
  <BlockUI :blocked="loading" fullScreen />
  <div class="flex flex-col items-center gap-4 w-full">
    <div class="flex flex-col gap-2 w-full">
      <div class="text-center text-3xl font-medium text-white leading-tight">
        Hmm... Forgot your password?
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
          name="email"
          type="email"
          class="!appearance-none !border !border-white/10 !w-full !outline-0 !bg-white/10 !text-white placeholder:!text-white/70 !rounded-3xl !shadow-sm"
          placeholder="Email"
        />
        <ErrorMessage v-if="$form.email?.invalid">{{ $form.email?.error?.message }}</ErrorMessage>
      </div>
    </div>
    <Button
      label="Recover password"
      class="!w-full !rounded-3xl !bg-surface-950 !border !border-surface-950 !text-white hover:!bg-surface-950/80"
      type="submit"
    />
  </Form>
  <RouterLink to="/auth/login" class="text-white/80 text-center cursor-pointer hover:text-white/90"
    >Remember your password?</RouterLink
  >
</template>
