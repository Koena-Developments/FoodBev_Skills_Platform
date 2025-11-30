export const useAdminSetup = () => {
  const { api } = useApi()

  // Create admin account (one-time setup)
  const createAdminAccount = async (email, password, secretKey) => {
    try {
      const response = await api.post('/admin/setup/create', {
        email: email,
        password: password,
        secretKey: secretKey
      })
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Create admin account error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to create admin account'
      }
    }
  }

  return {
    createAdminAccount
  }
}

