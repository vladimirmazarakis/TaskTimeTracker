import type { AxiosError } from 'axios'
import webClient from './webClient'
import z from 'zod'
import type { ToastServiceMethods } from 'primevue'
import isMobile from './isMobile'

function clearAuthentication() {
  delete webClient.defaults.headers['Authorization']
  localStorage.removeItem('refreshToken')
  localStorage.removeItem('accessToken')
}

function signOut() {
  clearAuthentication()
  location.reload()
}

function isAuthenticated() {
  if (!localStorage.getItem('accessToken') || !localStorage.getItem('refreshToken')) {
    clearAuthentication()
    return false
  }
  return true
}

async function signIn(email: string, password: string) {
  const resp = await webClient.post('/api/Users/login', {
    email: email,
    password: password,
  })

  if (resp.status !== 200) {
    return resp
  }
  webClient.defaults.headers['Authorization'] = `Bearer ${resp.data.accessToken}`
  localStorage.setItem('refreshToken', resp.data.refreshToken)
  localStorage.setItem('accessToken', resp.data.accessToken)
  return
}

async function signUp(email: string, password: string): Promise<any> {
  let resp: any = await webClient.post('/api/Users/register', {
    email: email,
    password: password,
  })

  if (resp.status === 200) {
    return
  } else if (resp.status === 400) {
    let err: AxiosError = resp as AxiosError
    return err?.response?.data
  }
}

async function resetPass(email: string, resetCode: string, password: string) {
  let resp: any = await webClient.post('/api/Users/resetPassword', {
    email: email,
    resetCode: resetCode,
    newPassword: password,
  })

  if (resp.status === 200) {
    return
  } else if (resp.status === 400) {
    let err: AxiosError = resp as AxiosError
    return err?.response?.data
  }
}

async function forgotPass(email: string) {
  let resp: any = await webClient.post('/api/Users/forgotPassword', {
    email: email,
  })

  return
}

const passwordMinMessage = 'Password must contain at least 6 characters'
const emailInvalid = 'Please enter a valid email address'
const passDoNotMatchText = 'Passwords do not match'
const fieldIsRequired = 'Field is required'

const signInSchema = z.object({
  email: z.coerce.string().email(emailInvalid),
  password: z.coerce.string().min(6, passwordMinMessage),
})

const signUpSchema = z.object({
  email: z.coerce.string().email(emailInvalid),
  password: z.coerce.string().min(6, passwordMinMessage),
  confirmPassword: z.coerce.string(),
})

const resetPassSchema = z.object({
  password: z.coerce.string().min(6, passwordMinMessage),
  confirmPassword: z.coerce.string(),
})

const forgotPassSchema = z.object({
  email: z.coerce.string().email(),
})

function handleServerErrorObject(data: any, toast: ToastServiceMethods) {
  if (!data || !data.errors) {
    return
  }
  var errors = Object.keys(data?.errors)
  for (const err of errors) {
    let arrErr: any[] = data?.errors[err]
    for (const errStr of arrErr) {
      toast.add({
        severity: 'error',
        summary: errStr,
        life: 5000,
        group: isMobile() ? 'bc' : 'br',
      })
    }
  }
}

export {
  signIn,
  clearAuthentication,
  isAuthenticated,
  signUp,
  resetPass,
  handleServerErrorObject,
  forgotPass,
  signOut,
  signInSchema,
  signUpSchema,
  forgotPassSchema,
  passDoNotMatchText,
  resetPassSchema,
}
