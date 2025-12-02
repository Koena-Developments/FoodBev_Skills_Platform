<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 py-4">
        <h1 class="text-2xl font-bold text-gray-900">Tripartite Agreements</h1>
        <p class="text-sm text-gray-600 mt-1">Agreements requiring your signature and Training Provider upload</p>
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
        <p class="text-gray-600">No agreements found.</p>
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
              <p class="text-sm text-gray-600 mt-1">Candidate: {{ agreement.candidateName }}</p>
              
              <div class="mt-4 flex flex-wrap gap-2">
                <span
                  :class="getStatusBadgeClass(agreement.status)"
                  class="px-3 py-1 rounded-full text-xs font-medium"
                >
                  {{ formatStatus(agreement.status) }}
                </span>
              </div>

              <div class="mt-4 text-sm text-gray-600 space-y-1">
                <p>Created: {{ formatDate(agreement.createdDate) }}</p>
                <p v-if="agreement.candidateSignedDate">
                  Candidate signed: {{ formatDate(agreement.candidateSignedDate) }}
                </p>
                <p v-if="agreement.employerSignedDate">
                  You signed: {{ formatDate(agreement.employerSignedDate) }}
                </p>
                <p v-if="agreement.trainingProviderSignatureUploadDate">
                  TP signature uploaded: {{ formatDate(agreement.trainingProviderSignatureUploadDate) }}
                </p>
              </div>
            </div>

            <div class="ml-4">
              <button
                v-if="agreement.status === 'AwaitingEmployerSignature'"
                @click="openSignModal(agreement)"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
              >
                Sign & Upload
              </button>
              <button
                v-else
                @click="viewAgreement(agreement)"
                class="px-4 py-2 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition"
              >
                View Details
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Sign Agreement Modal -->
    <div
      v-if="showSignModal"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4"
      @click.self="closeSignModal"
    >
      <div class="bg-white rounded-lg max-w-3xl w-full max-h-[90vh] overflow-y-auto">
        <div class="p-6">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-xl font-bold text-gray-900">Sign Agreement & Upload Training Provider Signature</h2>
            <button
              @click="closeSignModal"
              class="text-gray-400 hover:text-gray-600"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <div v-if="selectedAgreement" class="space-y-6">
            <div class="bg-gray-50 p-4 rounded-lg">
              <h3 class="font-semibold text-gray-900">{{ selectedAgreement.jobTitle }}</h3>
              <p class="text-sm text-gray-600 mt-1">Candidate: {{ selectedAgreement.candidateName }}</p>
            </div>

            <!-- Employer Signature Section -->
            <div class="border-t pt-4">
              <h3 class="text-lg font-semibold text-gray-900 mb-2">Your Digital Signature</h3>
              <p class="text-sm text-gray-600 mb-4">
                Please provide your digital signature below.
              </p>

              <SignatureCanvas
                ref="signatureCanvasRef"
                :canvas-width="600"
                :canvas-height="200"
                @signature-change="onSignatureChange"
              />
            </div>

            <!-- Training Provider Signature Upload Section -->
            <div class="border-t pt-4">
              <h3 class="text-lg font-semibold text-gray-900 mb-2">Training Provider Signature</h3>
              <p class="text-sm text-gray-600 mb-4">
                Upload a scanned copy or photo of the Training Provider's handwritten signature.
                Accepted formats: PDF, JPG, JPEG, PNG (max 10MB)
              </p>

              <div class="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center">
                <input
                  type="file"
                  ref="fileInputRef"
                  @change="onFileChange"
                  accept=".pdf,.jpg,.jpeg,.png"
                  class="hidden"
                  id="tp-signature-file"
                />
                <label
                  for="tp-signature-file"
                  class="cursor-pointer"
                >
                  <svg class="mx-auto h-12 w-12 text-gray-400" stroke="currentColor" fill="none" viewBox="0 0 48 48">
                    <path d="M28 8H12a4 4 0 00-4 4v20m32-12v8m0 0v8a4 4 0 01-4 4H12a4 4 0 01-4-4v-4m32-4l-3.172-3.172a4 4 0 00-5.656 0L28 28M8 32l9.172-9.172a4 4 0 015.656 0L28 28m0 0l4 4m4-24h8m-4-4v8m-12 4h.02" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                  </svg>
                  <p class="mt-2 text-sm text-gray-600">
                    <span class="font-medium text-blue-600 hover:text-blue-500">Click to upload</span> or drag and drop
                  </p>
                  <p class="text-xs text-gray-500 mt-1">PDF, JPG, PNG up to 10MB</p>
                </label>
              </div>

              <div v-if="selectedFile" class="mt-4 p-3 bg-green-50 border border-green-200 rounded-lg">
                <div class="flex items-center justify-between">
                  <div class="flex items-center">
                    <svg class="h-5 w-5 text-green-600 mr-2" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"></path>
                    </svg>
                    <span class="text-sm text-green-800">{{ selectedFile.name }}</span>
                  </div>
                  <button
                    @click="removeFile"
                    class="text-red-600 hover:text-red-800 text-sm"
                  >
                    Remove
                  </button>
                </div>
              </div>
            </div>

            <div class="mt-6 flex justify-end gap-3 border-t pt-4">
              <button
                @click="closeSignModal"
                class="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
              >
                Cancel
              </button>
              <button
                @click="submitSignature"
                :disabled="!signatureData || signing"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
              >
                {{ signing ? 'Submitting...' : 'Submit to Admin' }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
definePageMeta({
  layout: 'employer'
})

import { ref, onMounted } from 'vue'
import { useAgreements } from '~/composables/useAgreements'

const { getEmployerAgreements, signAsEmployer } = useAgreements()

const agreements = ref([])
const loading = ref(true)
const error = ref(null)
const showSignModal = ref(false)
const selectedAgreement = ref(null)
const signatureData = ref(null)
const signatureCanvasRef = ref(null)
const selectedFile = ref(null)
const fileInputRef = ref(null)
const signing = ref(false)

const loadAgreements = async () => {
  loading.value = true
  error.value = null
  
  const result = await getEmployerAgreements()
  if (result.success) {
    agreements.value = result.data || []
  } else {
    error.value = result.error
  }
  
  loading.value = false
}

const openSignModal = (agreement) => {
  selectedAgreement.value = agreement
  showSignModal.value = true
  signatureData.value = null
  selectedFile.value = null
  if (signatureCanvasRef.value) {
    signatureCanvasRef.value.clearSignature()
  }
  if (fileInputRef.value) {
    fileInputRef.value.value = ''
  }
}

const closeSignModal = () => {
  showSignModal.value = false
  selectedAgreement.value = null
  signatureData.value = null
  selectedFile.value = null
}

const onSignatureChange = (signature) => {
  signatureData.value = signature
}

const onFileChange = (event) => {
  const file = event.target.files[0]
  if (file) {
    // Validate file size (10MB)
    if (file.size > 10 * 1024 * 1024) {
      alert('File size exceeds 10MB limit')
      event.target.value = ''
      return
    }
    
    // Validate file type
    const allowedTypes = ['application/pdf', 'image/jpeg', 'image/jpg', 'image/png']
    if (!allowedTypes.includes(file.type)) {
      alert('Invalid file type. Please upload PDF, JPG, or PNG files only.')
      event.target.value = ''
      return
    }
    
    selectedFile.value = file
  }
}

const removeFile = () => {
  selectedFile.value = null
  if (fileInputRef.value) {
    fileInputRef.value.value = ''
  }
}

const submitSignature = async () => {
  if (!signatureData.value || !selectedAgreement.value) {
    alert('Please provide your signature and upload the Training Provider signature file.')
    return
  }

  if (!selectedFile.value) {
    alert('Please upload the Training Provider signature file.')
    return
  }

  signing.value = true
  
  // Extract base64 data (remove data:image/png;base64, prefix)
  const base64Data = signatureData.value.split(',')[1]
  
  const result = await signAsEmployer(
    selectedAgreement.value.agreementID,
    base64Data,
    selectedFile.value
  )
  
  if (result.success) {
    closeSignModal()
    await loadAgreements() // Refresh list
  } else {
    alert('Failed to submit agreement: ' + result.error)
  }
  
  signing.value = false
}

const viewAgreement = (agreement) => {
  // TODO: Implement view agreement details
  console.log('View agreement:', agreement)
}

const formatStatus = (status) => {
  const statusMap = {
    'PendingCandidateSignature': 'Awaiting Candidate',
    'AwaitingEmployerSignature': 'Awaiting Your Action',
    'SubmittedToAdmin': 'Under Review',
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

