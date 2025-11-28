export const useCandidate = () => {
  const { api } = useApi()

  // Profile operations
  const getProfile = async () => {
    try {
      const response = await api.get('/candidate/profile')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get profile error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch profile'
      }
    }
  }

  const updateProfile = async (profileData) => {
    try {
      const response = await api.put('/candidate/profile', profileData)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Update profile error:', error)
      console.error('Error response:', error.response?.data)
      console.error('Validation errors:', error.response?.data?.errors)
      
      // Extract validation errors if present
      let errorMessage = error.response?.data?.message || error.message || 'Failed to update profile'
      if (error.response?.data?.errors) {
        const validationErrors = Object.entries(error.response.data.errors)
          .map(([field, messages]) => `${field}: ${Array.isArray(messages) ? messages.join(', ') : messages}`)
          .join('; ')
        errorMessage = validationErrors || errorMessage
      }
      
      return {
        success: false,
        error: errorMessage
      }
    }
  }

  // Document operations
  const uploadDocument = async (file) => {
    try {
      const formData = new FormData()
      formData.append('file', file)

      const response = await api.post('/candidate/documents', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Upload document error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.response?.data?.error || error.message || 'Failed to upload document'
      }
    }
  }

  const getDocuments = async () => {
    try {
      const response = await api.get('/candidate/documents')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get documents error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch documents'
      }
    }
  }

  const deleteDocument = async (docType) => {
    try {
      const response = await api.delete(`/candidate/documents/${docType}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Delete document error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to delete document'
      }
    }
  }

  // Job operations
  const getMatchingJobs = async () => {
    try {
      const response = await api.get('/candidate/jobs')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get matching jobs error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch jobs'
      }
    }
  }

  // Application operations
  const applyToJob = async (jobId) => {
    try {
      const response = await api.post(`/candidate/applications/${jobId}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Apply to job error:', error)
      // Handle 409 Conflict (duplicate application)
      if (error.response?.status === 409) {
        return {
          success: false,
          error: error.response?.data?.message || 'You have already applied to this job.'
        }
      }
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to apply to job'
      }
    }
  }

  const getApplications = async () => {
    try {
      const response = await api.get('/candidate/applications')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get applications error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch applications'
      }
    }
  }

  const respondToInterview = async (applicationId, response) => {
    try {
      // Map string to enum value (0=None, 1=Accepted, 2=Declined)
      // The backend expects InterviewResponse enum: None=0, Accepted=1, Declined=2
      const responseValue = response === 'Accepted' ? 1 : response === 'Declined' ? 2 : 0
      const apiResponse = await api.put(`/applications/${applicationId}/interview-response`, {
        response: responseValue
      })
      return { success: true, data: apiResponse.data }
    } catch (error) {
      console.error('Interview response error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to respond to interview'
      }
    }
  }

  // Saved jobs operations
  const saveJob = async (jobId) => {
    try {
      const response = await api.post(`/candidate/saved-jobs/${jobId}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Save job error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to save job'
      }
    }
  }

  const getSavedJobs = async () => {
    try {
      const response = await api.get('/candidate/saved-jobs')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get saved jobs error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch saved jobs'
      }
    }
  }

  return {
    // Profile
    getProfile,
    updateProfile,
    // Documents
    uploadDocument,
    getDocuments,
    deleteDocument,
    // Jobs
    getMatchingJobs,
    // Applications
    applyToJob,
    getApplications,
    respondToInterview,
    // Saved Jobs
    saveJob,
    getSavedJobs
  }
}

