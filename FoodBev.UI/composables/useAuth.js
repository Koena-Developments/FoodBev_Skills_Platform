export const useAuth = () => {
  const { api } = useApi()
  const router = useRouter()
  
  const login = async (email, password) => {
    try {
      console.log('Attempting login:', { email })
      
      const response = await api.post('/auth/login', {
        email,
        password
      })
      
      console.log('Login response:', response.data)
      
      if (response.data?.token) {
        if (process.client) {
          // Use sessionStorage instead of localStorage to isolate tokens per browser tab
          // This prevents one tab's login from overwriting another tab's token
          sessionStorage.setItem('auth_token', response.data.token)
          sessionStorage.setItem('user', JSON.stringify(response.data))
        }
        return { success: true, data: response.data }
      }
      
      return { success: false, error: 'Invalid response from server' }
    } catch (error) {
      console.error('Login error:', error)
      
      // Handle network errors
      if (!error.response) {
        return {
          success: false,
          error: 'Network error: Unable to connect to the server. Please ensure the backend is running on http://localhost:5259'
        }
      }
      
      return {
        success: false,
        error: error.response?.data?.message || error.response?.data?.errors?.[0] || error.message || 'Login failed'
      }
    }
  }
  
  const register = async (email, password, userType = 0) => {
    try {
      // UserType: 0 = Candidate, 1 = Employer
      console.log('Attempting registration:', { email, userType })
      
      const response = await api.post('/auth/register', {
        email,
        password,
        userType
      })
      
      console.log('Registration response:', response.data)
      
      if (response.data?.token) {
        if (process.client) {
          // Use sessionStorage instead of localStorage to isolate tokens per browser tab
          sessionStorage.setItem('auth_token', response.data.token)
          sessionStorage.setItem('user', JSON.stringify(response.data))
        }
        return { success: true, data: response.data }
      }
      
      return { success: false, error: 'Invalid response from server' }
    } catch (error) {
      console.error('Registration error:', error)
      
      // Handle network errors
      if (!error.response) {
        return {
          success: false,
          error: 'Network error: Unable to connect to the server. Please ensure the backend is running on http://localhost:5259'
        }
      }
      
      return {
        success: false,
        error: error.response?.data?.message || error.response?.data?.errors?.[0] || error.message || 'Registration failed'
      }
    }
  }
  
  const logout = () => {
    if (process.client) {
      sessionStorage.removeItem('auth_token')
      sessionStorage.removeItem('user')
    }
    router.push('/login')
  }
  
  const getToken = () => {
    if (process.client) {
      return sessionStorage.getItem('auth_token')
    }
    return null
  }
  
  const getUser = () => {
    if (process.client) {
      const userStr = sessionStorage.getItem('user')
      return userStr ? JSON.parse(userStr) : null
    }
    return null
  }
  
  const isAuthenticated = () => {
    return !!getToken()
  }
  
  return {
    login,
    register,
    logout,
    getToken,
    getUser,
    isAuthenticated
  }
}

