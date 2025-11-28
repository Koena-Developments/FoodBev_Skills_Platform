export const useApi = () => {
  const { $axios } = useNuxtApp()
  
  return {
    api: $axios,
    get: (url, config = {}) => $axios.get(url, config),
    post: (url, data, config = {}) => $axios.post(url, data, config),
    put: (url, data, config = {}) => $axios.put(url, data, config),
    delete: (url, config = {}) => $axios.delete(url, config),
    patch: (url, data, config = {}) => $axios.patch(url, data, config)
  }
}

