import axios from 'axios'

export default defineNuxtPlugin(() => {
  const config = useRuntimeConfig()
  
  // Force the correct API base URL (override any .env settings)
  const apiBaseUrl = 'http://localhost:5259/api/v1'
  
  console.log('Axios plugin initialized with baseURL:', apiBaseUrl)
  console.log('Runtime config apiBase:', config.public?.apiBase)

  const api = axios.create({
    baseURL: apiBaseUrl,
    headers: {
      'Content-Type': 'application/json'
    },
    timeout: 10000, // 10 second timeout
    withCredentials: false
  })

  // Add request interceptor for authentication tokens
  api.interceptors.request.use(
    (config) => {
      // Get token from sessionStorage if available (isolated per browser tab)
      if (process.client) {
        const token = sessionStorage.getItem('auth_token')
        if (token) {
          config.headers.Authorization = `Bearer ${token}`
        }
      }
      return config
    },
    (error) => {
      return Promise.reject(error)
    }
  )

  // Add response interceptor for error handling
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      // Better error logging for debugging
      if (process.client) {
        console.error('API Error:', {
          message: error.message,
          response: error.response?.data,
          status: error.response?.status,
          url: error.config?.url
        })
      }
      
      if (error.response?.status === 401) {
        console.log("Unauthorized error - redirecting to login")
        if (process.client) {
          sessionStorage.removeItem('auth_token')
          sessionStorage.removeItem('user')
          if (window.location.pathname !== '/login') {
            window.location.href = '/login'
          }
        }
      }
      
      if (error.response?.status === 403) {
        console.error("Forbidden error - user may not have required role")
        // Don't redirect on 403, let the component handle it
      }
      return Promise.reject(error)
    }
  )

  return {
    provide: {
      axios: api
    }
  }
})