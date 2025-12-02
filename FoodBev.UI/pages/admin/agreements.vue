<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 py-4">
        <h1 class="text-2xl font-bold text-gray-900">Tripartite Agreements - Admin Review</h1>
        <p class="text-sm text-gray-600 mt-1">Review and approve completed agreements</p>
      </div>
    </header>

    <!-- Loading State -->
    <div v-if="loading" class="max-w-7xl mx-auto px-4 py-8">
      <div class="text-center">
        <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <p class="mt-2 text-gray-600">Loading agreements...</p>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="max-w-7xl mx-auto px-4 py-8">
      <div class="bg-red-50 border border-red-200 rounded-lg p-4">
        <p class="text-red-800">{{ error }}</p>
        <button
          @click="loadAgreements"
          class="mt-3 text-sm text-red-600 hover:text-red-800 underline"
        >
          Try again
        </button>
      </div>
    </div>

    <!-- Agreements List -->
    <div v-else class="max-w-7xl mx-auto px-4 py-6">
      <div v-if="agreements.length === 0" class="text-center py-12">
        <p class="text-gray-600">No agreements pending review.</p>
      </div>

      <div v-else class="space-y-4">
        <div
          v-for="agreement in agreements"
          :key="agreement.agreementID"
          class="bg-white rounded-lg shadow-sm border border-gray-200 p-6"
        >
          <div class="flex justify-between items-start">
            <div class="flex-1">
              <h3 class="text-lg font-semibold text-gray-900">{{ agreement.jobTitle }}</h3>
              <p class="text-sm text-gray-600 mt-1">
                Employer: {{ agreement.employerCompanyName }} | Candidate: {{ agreement.candidateName }}
              </p>
              
              <div class="mt-4 flex flex-wrap gap-2">
                <span
                  :class="getStatusBadgeClass(agreement.status)"
                  class="px-3 py-1 rounded-full text-xs font-medium"
                >
                  {{ formatStatus(agreement.status) }}
                </span>
              </div>

              <div class="mt-4 text-sm text-gray-600 space-y-1">
                <p>Submitted: {{ formatDate(agreement.submittedToAdminDate) }}</p>
                <div class="flex gap-4 mt-2">
                  <span :class="agreement.hasCandidateSignature ? 'text-green-600' : 'text-red-600'">
                    Candidate: {{ agreement.hasCandidateSignature ? '✓ Signed' : '✗ Not Signed' }}
                  </span>
                  <span :class="agreement.hasEmployerSignature ? 'text-green-600' : 'text-red-600'">
                    Employer: {{ agreement.hasEmployerSignature ? '✓ Signed' : '✗ Not Signed' }}
                  </span>
                  <span :class="agreement.hasTrainingProviderSignature ? 'text-green-600' : 'text-red-600'">
                    Training Provider: {{ agreement.hasTrainingProviderSignature ? '✓ Uploaded' : '✗ Missing' }}
                  </span>
                </div>
              </div>
            </div>

            <div class="ml-4 flex gap-2">
              <button
                @click="viewAgreement(agreement)"
                class="px-4 py-2 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition"
              >
                View Details
              </button>
              <button
                @click="downloadPdf(agreement.agreementID)"
                class="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition flex items-center gap-2"
                :disabled="downloadingPdf === agreement.agreementID"
              >
                <svg v-if="downloadingPdf !== agreement.agreementID" class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                </svg>
                <span v-if="downloadingPdf === agreement.agreementID">Downloading...</span>
                <span v-else>Download PDF</span>
              </button>
              <button
                v-if="agreement.status === 'SubmittedToAdmin'"
                @click="openReviewModal(agreement)"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
              >
                Review
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Review Agreement Modal -->
    <div
      v-if="showReviewModal"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4"
      @click.self="closeReviewModal"
    >
      <div class="bg-white rounded-lg max-w-3xl w-full max-h-[90vh] overflow-y-auto">
        <div class="p-6">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-xl font-bold text-gray-900">Review Tripartite Agreement</h2>
            <button
              @click="closeReviewModal"
              class="text-gray-400 hover:text-gray-600"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <div v-if="selectedAgreement" class="space-y-6">
            <div class="bg-gray-50 p-4 rounded-lg">
              <div class="flex justify-between items-start">
                <div>
                  <h3 class="font-semibold text-gray-900">{{ selectedAgreement.jobTitle }}</h3>
                  <p class="text-sm text-gray-600 mt-1">
                    Employer: {{ selectedAgreement.employerCompanyName }} | Candidate: {{ selectedAgreement.candidateName }}
                  </p>
                </div>
                <button
                  @click="downloadPdf(selectedAgreement.agreementID)"
                  class="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition flex items-center gap-2"
                  :disabled="downloadingPdf === selectedAgreement.agreementID"
                >
                  <svg v-if="downloadingPdf !== selectedAgreement.agreementID" class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                  </svg>
                  <span v-if="downloadingPdf === selectedAgreement.agreementID">Downloading...</span>
                  <span v-else>Download PDF</span>
                </button>
              </div>
            </div>

            <!-- Complete Form Details -->
            <div v-if="completeFormDetails" class="space-y-4">
              <!-- Candidate Details Section -->
              <div class="border rounded-lg p-4 bg-white">
                <h3 class="text-lg font-semibold text-gray-900 mb-3">Candidate Details</h3>
                <div class="grid grid-cols-1 md:grid-cols-2 gap-3 text-sm">
                  <div><span class="font-medium">Name:</span> {{ completeFormDetails.candidateFirstName }} {{ completeFormDetails.candidateLastName }}</div>
                  <div><span class="font-medium">ID Number:</span> {{ completeFormDetails.candidateIDNumber || 'N/A' }}</div>
                  <div v-if="completeFormDetails.candidateDateOfBirth"><span class="font-medium">Date of Birth:</span> {{ formatDate(completeFormDetails.candidateDateOfBirth) }}</div>
                  <div><span class="font-medium">Email:</span> {{ completeFormDetails.candidateEmail || 'N/A' }}</div>
                  <div><span class="font-medium">Contact:</span> {{ completeFormDetails.candidateContactNumber || 'N/A' }}</div>
                  <div><span class="font-medium">Address:</span> {{ completeFormDetails.candidatePhysicalAddress || 'N/A' }}</div>
                  <div><span class="font-medium">Postal Code:</span> {{ completeFormDetails.candidatePostalCode || 'N/A' }}</div>
                  <div><span class="font-medium">Province:</span> {{ completeFormDetails.candidateProvince || 'N/A' }}</div>
                  <div><span class="font-medium">Race:</span> {{ completeFormDetails.candidateRace || 'N/A' }}</div>
                  <div><span class="font-medium">Gender:</span> {{ completeFormDetails.candidateGender || 'N/A' }}</div>
                  <div><span class="font-medium">Nationality:</span> {{ completeFormDetails.candidateNationality || 'N/A' }}</div>
                  <div><span class="font-medium">Employment Status:</span> {{ completeFormDetails.candidateEmploymentStatus || 'N/A' }}</div>
                  <div><span class="font-medium">OFO Code:</span> {{ completeFormDetails.candidateOFO_Code || 'N/A' }}</div>
                  <div><span class="font-medium">Highest Qualification:</span> {{ completeFormDetails.candidateHighestQualification || 'N/A' }}</div>
                  <div v-if="completeFormDetails.candidateInstitutionName"><span class="font-medium">Institution:</span> {{ completeFormDetails.candidateInstitutionName }}</div>
                  <div v-if="completeFormDetails.candidateQualificationYear"><span class="font-medium">Qualification Year:</span> {{ completeFormDetails.candidateQualificationYear }}</div>
                  <div v-if="completeFormDetails.candidateIsDisabled"><span class="font-medium">Disability:</span> {{ completeFormDetails.candidateDisabilityDetails || 'Details not provided' }}</div>
                </div>
              </div>

              <!-- Employer Details Section -->
              <div class="border rounded-lg p-4 bg-white">
                <h3 class="text-lg font-semibold text-gray-900 mb-3">Employer Details</h3>
                <div class="grid grid-cols-1 md:grid-cols-2 gap-3 text-sm">
                  <div><span class="font-medium">Company Name:</span> {{ completeFormDetails.employerCompanyName }}</div>
                  <div v-if="completeFormDetails.employerLevyNumber"><span class="font-medium">Levy Number:</span> {{ completeFormDetails.employerLevyNumber }}</div>
                  <div v-if="completeFormDetails.employerLNumber"><span class="font-medium">L Number:</span> {{ completeFormDetails.employerLNumber }}</div>
                  <div v-if="completeFormDetails.employerTNumber"><span class="font-medium">T Number:</span> {{ completeFormDetails.employerTNumber }}</div>
                  <div v-if="completeFormDetails.employerSDFName"><span class="font-medium">SDF Name:</span> {{ completeFormDetails.employerSDFName }}</div>
                  <div v-if="completeFormDetails.employerSDFEmail"><span class="font-medium">SDF Email:</span> {{ completeFormDetails.employerSDFEmail }}</div>
                  <div v-if="completeFormDetails.employerSDFContactNumber"><span class="font-medium">SDF Contact:</span> {{ completeFormDetails.employerSDFContactNumber }}</div>
                </div>
              </div>

              <!-- Job Information Section -->
              <div class="border rounded-lg p-4 bg-white">
                <h3 class="text-lg font-semibold text-gray-900 mb-3">Job Information</h3>
                <div class="grid grid-cols-1 md:grid-cols-2 gap-3 text-sm">
                  <div><span class="font-medium">Job Title:</span> {{ completeFormDetails.jobTitle }}</div>
                  <div v-if="completeFormDetails.OFO_Code_Required"><span class="font-medium">Required OFO Code:</span> {{ completeFormDetails.OFO_Code_Required }}</div>
                  <div v-if="completeFormDetails.jobDescription" class="md:col-span-2"><span class="font-medium">Description:</span> {{ completeFormDetails.jobDescription }}</div>
                </div>
              </div>

              <!-- Application Details Section -->
              <div class="border rounded-lg p-4 bg-white">
                <h3 class="text-lg font-semibold text-gray-900 mb-3">Application Details</h3>
                <div class="grid grid-cols-1 md:grid-cols-2 gap-3 text-sm">
                  <div><span class="font-medium">Date Applied:</span> {{ formatDate(completeFormDetails.applicationDateApplied) }}</div>
                  <div><span class="font-medium">Status:</span> {{ completeFormDetails.applicationStatus }}</div>
                  <div v-if="completeFormDetails.applicationInterviewDate"><span class="font-medium">Interview Date:</span> {{ formatDate(completeFormDetails.applicationInterviewDate) }}</div>
                  <div v-if="completeFormDetails.applicationInterviewVenue"><span class="font-medium">Interview Venue:</span> {{ completeFormDetails.applicationInterviewVenue }}</div>
                </div>
              </div>
            </div>

            <!-- Signatures Review -->
            <div class="border-t pt-4">
              <h3 class="text-lg font-semibold text-gray-900 mb-4">Signatures</h3>
              
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <!-- Candidate Signature -->
                <div v-if="selectedAgreement.hasCandidateSignature" class="border rounded-lg p-4">
                  <h4 class="font-semibold text-sm text-gray-700 mb-2">Candidate Signature</h4>
                  <img
                    v-if="selectedAgreement.candidateSignature"
                    :src="`data:image/png;base64,${selectedAgreement.candidateSignature}`"
                    alt="Candidate Signature"
                    class="w-full border rounded bg-white"
                  />
                  <div v-else class="bg-gray-100 border rounded p-4 text-center">
                    <p class="text-sm text-gray-600">Signature on file</p>
                  </div>
                  <p class="text-xs text-gray-500 mt-2">
                    Signed: {{ formatDate(selectedAgreement.candidateSignedDate) }}
                  </p>
                </div>

                <!-- Employer Signature -->
                <div v-if="selectedAgreement.hasEmployerSignature" class="border rounded-lg p-4">
                  <h4 class="font-semibold text-sm text-gray-700 mb-2">Employer Signature</h4>
                  <img
                    v-if="selectedAgreement.employerSignature"
                    :src="`data:image/png;base64,${selectedAgreement.employerSignature}`"
                    alt="Employer Signature"
                    class="w-full border rounded bg-white"
                  />
                  <div v-else class="bg-gray-100 border rounded p-4 text-center">
                    <p class="text-sm text-gray-600">Signature on file</p>
                  </div>
                  <p class="text-xs text-gray-500 mt-2">
                    Signed: {{ formatDate(selectedAgreement.employerSignedDate) }}
                  </p>
                </div>

                <!-- Training Provider Signature -->
                <div v-if="selectedAgreement.hasTrainingProviderSignature" class="border rounded-lg p-4 md:col-span-2">
                  <h4 class="font-semibold text-sm text-gray-700 mb-2">Training Provider Signature</h4>
                  <div class="bg-gray-100 border rounded p-4">
                    <a
                      :href="`http://localhost:5259/uploads/${selectedAgreement.trainingProviderSignatureFileRef}`"
                      target="_blank"
                      class="inline-flex items-center text-blue-600 hover:text-blue-800 underline"
                    >
                      <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                      </svg>
                      View Training Provider Signature File
                    </a>
                  </div>
                  <p class="text-xs text-gray-500 mt-2">
                    Uploaded: {{ formatDate(selectedAgreement.trainingProviderSignatureUploadDate) }}
                  </p>
                </div>
              </div>
            </div>

            <!-- Review Actions -->
            <div class="border-t pt-4">
              <label class="block text-sm font-medium text-gray-700 mb-2">
                Review Notes (Optional)
              </label>
              <textarea
                v-model="reviewNotes"
                rows="3"
                class="w-full border border-gray-300 rounded-lg p-2 text-sm"
                placeholder="Add any notes about this agreement..."
              ></textarea>

              <div class="mt-6 flex justify-end gap-3">
                <button
                  @click="closeReviewModal"
                  class="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
                >
                  Cancel
                </button>
                <button
                  @click="rejectAgreement"
                  :disabled="reviewing"
                  class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
                >
                  {{ reviewing ? 'Processing...' : 'Reject' }}
                </button>
                <button
                  @click="approveAgreement"
                  :disabled="reviewing"
                  class="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
                >
                  {{ reviewing ? 'Processing...' : 'Approve' }}
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
definePageMeta({
  layout: 'admin'
})

import { ref, onMounted } from 'vue'
import { useAgreements } from '~/composables/useAgreements'

const { getPendingReviewAgreements, reviewAgreement } = useAgreements()

const agreements = ref([])
const loading = ref(true)
const error = ref(null)
const showReviewModal = ref(false)
const selectedAgreement = ref(null)
const completeFormDetails = ref(null)
const reviewNotes = ref('')
const reviewing = ref(false)
const downloadingPdf = ref(null)

const loadAgreements = async () => {
  loading.value = true
  error.value = null
  
  const result = await getPendingReviewAgreements()
  if (result.success) {
    agreements.value = result.data || []
  } else {
    error.value = result.error
  }
  
  loading.value = false
}

const openReviewModal = async (agreement) => {
  selectedAgreement.value = agreement
  showReviewModal.value = true
  reviewNotes.value = ''
  
  // Load complete form details
  try {
    const { api } = useApi()
    const response = await api.get(`/admin/agreements/${agreement.agreementID}/complete-details`)
    if (response.data) {
      completeFormDetails.value = response.data
    }
  } catch (error) {
    console.error('Error loading complete form details:', error)
    completeFormDetails.value = null
  }
}

const closeReviewModal = () => {
  showReviewModal.value = false
  selectedAgreement.value = null
  completeFormDetails.value = null
  reviewNotes.value = ''
}

const downloadPdf = async (agreementId) => {
  try {
    downloadingPdf.value = agreementId
    const { api } = useApi()
    const response = await api.get(`/admin/agreements/${agreementId}/download-pdf`, {
      responseType: 'blob'
    })
    
    // Create a blob URL and trigger download
    const blob = new Blob([response.data], { type: 'application/pdf' })
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `Agreement_${agreementId}_${new Date().toISOString().split('T')[0]}.pdf`
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
  } catch (error) {
    console.error('Error downloading PDF:', error)
    alert('Failed to download PDF. Please try again.')
  } finally {
    downloadingPdf.value = null
  }
}

const approveAgreement = async () => {
  if (!selectedAgreement.value) return
  
  reviewing.value = true
  const result = await reviewAgreement(selectedAgreement.value.agreementID, true, reviewNotes.value)
  
  if (result.success) {
    closeReviewModal()
    await loadAgreements()
  } else {
    alert('Failed to approve agreement: ' + result.error)
  }
  
  reviewing.value = false
}

const rejectAgreement = async () => {
  if (!selectedAgreement.value) return
  
  reviewing.value = true
  const result = await reviewAgreement(selectedAgreement.value.agreementID, false, reviewNotes.value)
  
  if (result.success) {
    closeReviewModal()
    await loadAgreements()
  } else {
    alert('Failed to reject agreement: ' + result.error)
  }
  
  reviewing.value = false
}

const viewAgreement = (agreement) => {
  openReviewModal(agreement)
}

const formatStatus = (status) => {
  const statusMap = {
    'PendingCandidateSignature': 'Awaiting Candidate',
    'AwaitingEmployerSignature': 'Awaiting Employer',
    'SubmittedToAdmin': 'Pending Review',
    'Approved': 'Approved',
    'Rejected': 'Rejected',
    'Archived': 'Archived'
  }
  return statusMap[status] || status
}

const getStatusBadgeClass = (status) => {
  const classMap = {
    'PendingCandidateSignature': 'bg-yellow-100 text-yellow-800',
    'AwaitingEmployerSignature': 'bg-blue-100 text-blue-800',
    'SubmittedToAdmin': 'bg-purple-100 text-purple-800',
    'Approved': 'bg-green-100 text-green-800',
    'Rejected': 'bg-red-100 text-red-800',
    'Archived': 'bg-gray-100 text-gray-800'
  }
  return classMap[status] || 'bg-gray-100 text-gray-800'
}

const formatDate = (dateString) => {
  if (!dateString) return 'N/A'
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

onMounted(() => {
  loadAgreements()
})
</script>

