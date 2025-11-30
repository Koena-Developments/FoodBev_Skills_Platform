export const useAdmin = () => {
  const { api } = useApi()

  // Get dashboard statistics
  const getDashboardStats = async () => {
    try {
      const response = await api.get('/admin/stats')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get dashboard stats error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch dashboard statistics'
      }
    }
  }

  // Get demographics by province
  const getDemographics = async () => {
    try {
      const response = await api.get('/admin/demographics')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get demographics error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch demographics'
      }
    }
  }

  // Get recent activity
  const getRecentActivity = async (limit = 50) => {
    try {
      const response = await api.get(`/admin/recent-activity?limit=${limit}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get recent activity error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch recent activity'
      }
    }
  }

  return {
    getDashboardStats,
    getDemographics,
    getRecentActivity
  }
}

