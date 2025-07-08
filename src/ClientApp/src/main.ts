import { createApp } from 'vue'
import PrimeVue from 'primevue/config'
import App from './App.vue'
import Aura from '@primeuix/themes/aura'
import { definePreset } from '@primeuix/themes'
import LoginView from './views/auth/LoginView.vue'
import { createRouter, createWebHistory } from 'vue-router'
import HomeView from './views/HomeView.vue'
import { isAuthenticated } from './lib/auth'
import RegisterView from './views/auth/RegisterView.vue'
import ForgotPasswordView from './views/auth/ForgotPasswordView.vue'
import ToastService from 'primevue/toastservice'
import AuthLayoutView from './views/auth/AuthLayoutView.vue'
import ResetPasswordView from './views/auth/ResetPasswordView.vue'
import ConfirmAccountView from './views/auth/ConfirmAccountView.vue'
import AppLayoutView from './views/AppLayoutView.vue'
import { ConfirmationService } from 'primevue'
import styles from '@/assets/main.css'

const app = createApp(App)

const routes = [
  { path: '/', component: AppLayoutView, children: [{ path: '', component: HomeView }] },
  {
    path: '/auth',
    component: AuthLayoutView,
    children: [
      { path: 'login', component: LoginView },
      { path: 'register', component: RegisterView },
      {
        path: 'reset-pass',
        component: ResetPasswordView,
      },
      {
        path: 'forgot-pass',
        component: ForgotPasswordView,
      },
      {
        path: 'confirm-account',
        component: ConfirmAccountView,
      },
    ],
  },
]

const router = createRouter({
  history: createWebHistory('/'),
  routes,
})

router.beforeEach((to, from, next) => {
  if (!isAuthenticated() && !to.path.startsWith('/auth')) {
    next({ path: '/auth/login' })
  } else if (isAuthenticated() && to.path.startsWith('/auth')) {
    next({ path: '/' })
  } else {
    next()
  }
})

const Preset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{rose.50}',
      100: '{rose.100}',
      200: '{rose.200}',
      300: '{rose.300}',
      400: '{rose.400}',
      500: '{rose.500}',
      600: '{rose.600}',
      700: '{rose.700}',
      800: '{rose.800}',
      900: '{rose.900}',
      950: '{rose.950}',
    },
  },
})

app.use(PrimeVue, {
  theme: {
    preset: Preset,
    options: {
      darkModeSelector: 'light',
    },
  },
  ripple: true,
})

app.use(ToastService)

app.use(ConfirmationService)

app.use(router)

app.mount('#app')
