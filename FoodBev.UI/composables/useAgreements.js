export const useAgreements = () => {
  const { api } = useApi()

  // Get all agreements for candidate
  const getCandidateAgreements = async () => {
    try {
      const response = await api.get('/candidate/agreements')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get candidate agreements error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch agreements'
      }
    }
  }

  // Get single agreement for candidate
  const getCandidateAgreement = async (agreementId) => {
    try {
      const response = await api.get(`/candidate/agreements/${agreementId}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get candidate agreement error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch agreement'
      }
    }
  }

  // Candidate signs agreement
  const signAsCandidate = async (agreementId, signatureBase64) => {
    try {
      const response = await api.post(`/candidate/agreements/${agreementId}/sign`, {
        agreementID: agreementId,
        signatureBase64: signatureBase64
      })
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Sign as candidate error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to sign agreement'
      }
    }
  }

  // Get all agreements for employer
  const getEmployerAgreements = async () => {
    try {
      const response = await api.get('/employer/agreements')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get employer agreements error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch agreements'
      }
    }
  }

  // Get single agreement for employer
  const getEmployerAgreement = async (agreementId) => {
    try {
      const response = await api.get(`/employer/agreements/${agreementId}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get employer agreement error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch agreement'
      }
    }
  }

  // Employer signs agreement and uploads TP signature
  const signAsEmployer = async (agreementId, employerSignatureBase64, trainingProviderFile) => {
    try {
      const formData = new FormData()
      formData.append('agreementID', agreementId)
      formData.append('employerSignatureBase64', employerSignatureBase64)
      if (trainingProviderFile) {
        formData.append('trainingProviderSignatureFile', trainingProviderFile)
      }

      const response = await api.post(`/employer/agreements/${agreementId}/sign`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Sign as employer error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to sign agreement'
      }
    }
  }

  // Admin: Get pending review agreements
  const getPendingReviewAgreements = async () => {
    try {
      const response = await api.get('/admin/agreements/pending-review')
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get pending review agreements error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch agreements'
      }
    }
  }

  // Admin: Get single agreement
  const getAdminAgreement = async (agreementId) => {
    try {
      const response = await api.get(`/admin/agreements/${agreementId}`)
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Get admin agreement error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch agreement'
      }
    }
  }

  // Admin: Review agreement
  const reviewAgreement = async (agreementId, approved, notes) => {
    try {
      const response = await api.post(`/admin/agreements/${agreementId}/review`, {
        agreementID: agreementId,
        approved: approved,
        notes: notes || null
      })
      return { success: true, data: response.data }
    } catch (error) {
      console.error('Review agreement error:', error)
      return {
        success: false,
        error: error.response?.data?.message || error.message || 'Failed to review agreement'
      }
    }
  }

  return {
    // Candidate methods
    getCandidateAgreements,
    getCandidateAgreement,
    signAsCandidate,
    // Employer methods
    getEmployerAgreements,
    getEmployerAgreement,
    signAsEmployer,
    // Admin methods
    getPendingReviewAgreements,
    getAdminAgreement,
    reviewAgreement
  }
}

