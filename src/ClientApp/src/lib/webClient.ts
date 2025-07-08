import axios from 'axios'
import { clearAuthentication } from './auth'

if (!import.meta.env.VITE_API_HOST) {
  throw new Error('API Host is not specified.')
}

const baseURL: string = import.meta.env.VITE_API_HOST

const webClient = axios.create({
  baseURL: baseURL,
})

const accessToken = localStorage.getItem('accessToken')

if (accessToken) {
  webClient.defaults.headers['Authorization'] = `Bearer ${accessToken}`
}

webClient.interceptors.response.use(
  (resp) => resp,
  async function (error) {
    const response = error.response
    if (response?.status === 401) {
      const refreshToken = localStorage.getItem('refreshToken')
      if (!refreshToken) {
        clearAuthentication()
        return error
      }

      const { status, data } = await webClient.post('/api/Users/refresh', {
        refreshToken: refreshToken,
      })

      if (status === 200) {
        webClient.defaults.headers['Authorization'] = `Bearer ${data.accessToken}`
        localStorage.setItem('refreshToken', data.refreshToken)
        localStorage.setItem('accessToken', data.accessToken)
        return await axios(error.config)
      }
    }

    return error
  },
)

export default webClient
