<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 py-4">
        <h1 class="text-2xl font-bold text-gray-900">Tripartite Agreements</h1>
        <p class="text-sm text-gray-600 mt-1">Agreements requiring your signature</p>
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
              <p class="text-sm text-gray-600 mt-1">{{ agreement.employerCompanyName }}</p>
              
              <div class="mt-4 flex flex-wrap gap-2">
                <span
                  :class="getStatusBadgeClass(agreement.status)"
                  class="px-3 py-1 rounded-full text-xs font-medium"
                >
                  {{ formatStatus(agreement.status) }}
                </span>
              </div>

              <div class="mt-4 text-sm text-gray-600">
                <p>Created: {{ formatDate(agreement.createdDate) }}</p>
                <p v-if="agreement.candidateSignedDate">
                  You signed: {{ formatDate(agreement.candidateSignedDate) }}
                </p>
              </div>
            </div>

            <div class="ml-4">
              <button
                v-if="agreement.status === 'PendingCandidateSignature'"
                @click="openSignModal(agreement)"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
              >
                Sign Agreement
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
      <div class="bg-white rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        <div class="p-6">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-xl font-bold text-gray-900">Sign Tripartite Agreement</h2>
            <button
              @click="closeSignModal"
              class="text-gray-400 hover:text-gray-600"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <div v-if="selectedAgreement" class="space-y-4">
            <div class="bg-gray-50 p-4 rounded-lg">
              <h3 class="font-semibold text-gray-900">{{ selectedAgreement.jobTitle }}</h3>
              <p class="text-sm text-gray-600 mt-1">{{ selectedAgreement.employerCompanyName }}</p>
            </div>

            <div class="border-t pt-4">
              <p class="text-sm text-gray-700 mb-4">
                Please review the agreement and provide your digital signature below.
              </p>

              <SignatureCanvas
                ref="signatureCanvasRef"
                :canvas-width="600"
                :canvas-height="200"
                @signature-change="onSignatureChange"
              />

              <div class="mt-6 flex justify-end gap-3">
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
                  {{ signing ? 'Signing...' : 'Submit Signature' }}
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
  layout: 'candidate'
})

import { ref, onMounted } from 'vue'
import { useAgreements } from '~/composables/useAgreements'

const { getCandidateAgreements, signAsCandidate } = useAgreements()

const agreements = ref([])
const loading = ref(true)
const error = ref(null)
const showSignModal = ref(false)
const selectedAgreement = ref(null)
const signatureData = ref(null)
const signatureCanvasRef = ref(null)
const signing = ref(false)

const loadAgreements = async () => {
  loading.value = true
  error.value = null
  
  const result = await getCandidateAgreements()
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
  if (signatureCanvasRef.value) {
    signatureCanvasRef.value.clearSignature()
  }
}

const closeSignModal = () => {
  showSignModal.value = false
  selectedAgreement.value = null
  signatureData.value = null
}

const onSignatureChange = (signature) => {
  signatureData.value = signature
}

const submitSignature = async () => {
  if (!signatureData.value || !selectedAgreement.value) return

  signing.value = true
  
  // Extract base64 data (remove data:image/png;base64, prefix)
  const base64Data = signatureData.value.split(',')[1]
  
  const result = await signAsCandidate(selectedAgreement.value.agreementID, base64Data)
  
  if (result.success) {
    closeSignModal()
    await loadAgreements() // Refresh list
  } else {
    alert('Failed to sign agreement: ' + result.error)
  }
  
  signing.value = false
}

const viewAgreement = (agreement) => {
  // TODO: Implement view agreement details
  console.log('View agreement:', agreement)
}

const formatStatus = (status) => {
  const statusMap = {
    'PendingCandidateSignature': 'Awaiting Your Signature',
    'AwaitingEmployerSignature': 'Awaiting Employer',
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

