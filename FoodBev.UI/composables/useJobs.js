export const useJobs = () => {
  const { api } = useApi()

  const searchJobs = async (searchParams = {}) => {
    try {
      const response = await api.get('/jobs/search', { params: searchParams })
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Search jobs error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to search jobs'
      }
    }
  }

  const getJob = async (jobId) => {
    try {
      const response = await api.get(`/jobs/${jobId}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get job error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch job'
      }
    }
  }

  return {
    searchJobs,
    getJob
  }
}

