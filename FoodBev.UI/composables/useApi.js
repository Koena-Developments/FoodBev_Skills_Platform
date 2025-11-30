export const useApi = () => {
  const nuxtApp = useNuxtApp()
  
  // Access axios from the plugin's provide
  const api = nuxtApp.$axios || nuxtApp.provide?.axios
  
  if (!api) {
    // If axios is not available (e.g., during SSR or plugin not loaded), return a safe mock
    if (process.server) {
      return {
        api: null,
        get: () => Promise.reject(new Error('API calls are not available during SSR')),
        post: () => Promise.reject(new Error('API calls are not available during SSR')),
        put: () => Promise.reject(new Error('API calls are not available during SSR')),
        delete: () => Promise.reject(new Error('API calls are not available during SSR')),
        patch: () => Promise.reject(new Error('API calls are not available during SSR'))
      }
    }
    throw new Error('Axios plugin is not available. Make sure the axios plugin is registered.')
  }
  
  return {
    api: api,
    get: (url, config = {}) => api.get(url, config),
    post: (url, data, config = {}) => api.post(url, data, config),
    put: (url, data, config = {}) => api.put(url, data, config),
    delete: (url, config = {}) => api.delete(url, config),
    patch: (url, data, config = {}) => api.patch(url, data, config)
  }
}

