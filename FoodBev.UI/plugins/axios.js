import axios from 'axios'

export default defineNuxtPlugin(() => {
  const config = useRuntimeConfig()

  const api = axios.create({
    baseURL: config.public?.apiBase || 'https://example.com/api'
  })

  return {
    provide: {
      axios: api
    }
  }
})