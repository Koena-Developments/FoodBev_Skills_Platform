export const useEmployer = () => {
  const { api } = useApi()

  const getProfile = async () => {
    try {
      const response = await api.get('/employer/profile')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get employer profile error:', error)
      // 404 is OK - profile might not exist yet
      if (error.response?.status === 404) {
        return {
          success: false,
          error: 'Profile not found',
          notFound: true
        }
      }
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch profile'
      }
    }
  }

  const updateProfile = async (profileData) => {
    try {
      const response = await api.put('/employer/profile', profileData)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Update employer profile error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to update profile'
      }
    }
  }

  const getJobs = async () => {
    try {
      const response = await api.get('/employer/jobs')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get employer jobs error:', error)

      if (error.response?.status === 403) {
        return {
          success: false,
          error: 'Access denied. Please ensure you are logged in as an Employer.',
          forbidden: true
        }
      }
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch jobs'
      }
    }
  }

  const createJob = async (jobData) => {
    try {
      const response = await api.post('/jobs', jobData)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Create job error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to create job'
      }
    }
  }

  const updateJob = async (jobId, jobData) => {
    try {
      const response = await api.put(`/jobs/${jobId}`, jobData)
      console.log(response.data);
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Update job error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to update job'
      }
    }
  }

  const deleteJob = async (jobId) => {
    try {
      const response = await api.delete(`/jobs/${jobId}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Delete job error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to delete job'
      }
    }
  }

  const getApplicants = async (jobId, filters = {}) => {
    try {
      const response = await api.get(`/jobs/${jobId}/applicants`, { params: filters })
      console.log(response.data);
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get applicants error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch applicants'
      }
    }
  }

  const updateApplicationStatus = async (applicationId, status) => {
    try {
      // Convert string status to enum format (capitalize first letter, handle camelCase)
      const statusEnum = status.charAt(0).toUpperCase() + status.slice(1)
      const response = await api.put(`/applications/${applicationId}/status`, { Status: statusEnum })
      return { success: true, data: response.data }
    } catch (error) {
      return {
        success: false,
        error: error.response?.data?.message || error.response?.data?.title || error.message || 'Failed to update status'
      }
    }
  }

  const scheduleInterview = async (applicationId, interviewDate, interviewVenue) => {
    try {
      const response = await api.put(`/applications/${applicationId}/schedule-interview`, {
        interviewDate,
        interviewVenue
      })
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Schedule interview error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to schedule interview'
      }
    }
  }

  const uploadLogo = async (file) => {
    try {
      const formData = new FormData()
      formData.append('file', file)

      const response = await api.post('/employer/logo', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Upload logo error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to upload logo'
      }
    }
  }

  return {
    getProfile,
    updateProfile,
    getJobs,
    createJob,
    updateJob,
    deleteJob,
    getApplicants,
    updateApplicationStatus,
    scheduleInterview,
    uploadLogo
  }
}

