<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 py-4">
        <div class="flex items-center justify-between">
          <div>
            <NuxtLink
              to="/employer"
              class="text-sm text-blue-600 hover:text-blue-800 mb-2 inline-block"
            >
              ← Back to Jobs
            </NuxtLink>
            <h1 class="text-2xl font-bold text-gray-900">Job Applicants</h1>
            <p class="text-sm text-gray-600 mt-1">{{ jobTitle || 'Loading...' }}</p>
          </div>
        </div>
      </div>
    </header>

    <!-- Loading State -->
    <div v-if="loading" class="max-w-7xl mx-auto px-4 py-8">
      <div class="text-center">
        <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <p class="mt-2 text-gray-600">Loading applicants...</p>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="max-w-7xl mx-auto px-4 py-8">
      <div class="bg-red-50 border border-red-200 rounded-lg p-4">
        <p class="text-red-800">{{ error }}</p>
        <button
          @click="loadApplicants"
          class="mt-3 text-sm text-red-600 hover:text-red-800 underline"
        >
          Try again
        </button>
      </div>
    </div>

    <!-- Applicants List -->
    <div v-else class="max-w-7xl mx-auto px-4 py-6">
      <!-- Filters -->
      <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 mb-6">
        <h3 class="text-sm font-medium text-gray-700 mb-3">Filter Applicants</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div>
            <label class="block text-xs text-gray-600 mb-1">OFO Code</label>
            <input
              v-model="filters.ofoCode"
              type="text"
              placeholder="Filter by OFO code"
              class="w-full px-3 py-2 border border-gray-300 rounded-md text-sm"
            />
          </div>
          <div>
            <label class="block text-xs text-gray-600 mb-1">Employment Status</label>
            <select
              v-model="filters.employmentStatus"
              class="w-full px-3 py-2 border border-gray-300 rounded-md text-sm"
            >
              <option value="">All</option>
              <option value="Employed">Employed</option>
              <option value="Unemployed">Unemployed</option>
            </select>
          </div>
          <div>
            <label class="block text-xs text-gray-600 mb-1">Province</label>
            <select
              v-model="filters.province"
              class="w-full px-3 py-2 border border-gray-300 rounded-md text-sm"
            >
              <option value="">All Provinces</option>
              <option value="Gauteng">Gauteng</option>
              <option value="Western Cape">Western Cape</option>
              <option value="KwaZulu-Natal">KwaZulu-Natal</option>
              <option value="Eastern Cape">Eastern Cape</option>
              <option value="Limpopo">Limpopo</option>
              <option value="Mpumalanga">Mpumalanga</option>
              <option value="North West">North West</option>
              <option value="Free State">Free State</option>
              <option value="Northern Cape">Northern Cape</option>
            </select>
          </div>
        </div>
        <div class="mt-3 flex gap-2">
          <button
            @click="applyFilters"
            class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition text-sm"
          >
            Apply Filters
          </button>
          <button
            @click="clearFilters"
            class="px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition text-sm"
          >
            Clear
          </button>
        </div>
      </div>

      <!-- Applicants Count -->
      <div class="mb-4">
        <p class="text-sm text-gray-600">
          <strong>{{ filteredApplicants.length }}</strong> applicant(s) found
        </p>
      </div>

      <!-- No Applicants -->
      <div v-if="filteredApplicants.length === 0" class="bg-white rounded-lg shadow-sm border border-gray-200 p-12 text-center">
        <p class="text-gray-600">No applicants found for this job.</p>
      </div>

      <!-- Applicants Cards -->
      <div v-else class="space-y-4">
        <div
          v-for="applicant in filteredApplicants"
          :key="applicant.applicationID"
          class="bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md transition"
        >
          <div class="flex justify-between items-start">
            <div class="flex-1">
              <h3 class="text-lg font-semibold text-gray-900">
                {{ getCandidateName(applicant) }}
              </h3>
              <p class="text-sm text-gray-600 mt-1">
                {{ applicant.candidateEmail || 'No email' }} | {{ applicant.candidateContactNumber || 'No contact' }}
              </p>
              <p class="text-sm text-gray-500 mt-1">
                Applied: {{ formatDate(applicant.dateApplied) }}
              </p>
              
              <div class="mt-4 flex flex-wrap gap-2">
                <span :class="getStatusClass(applicant.status)">
                  {{ formatStatus(applicant.status) }}
                </span>
                <span v-if="applicant.interviewDate" class="px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded">
                  Interview: {{ formatDate(applicant.interviewDate) }}
                </span>
                <span v-if="applicant.hasSkillsForm" class="px-2 py-1 bg-green-100 text-green-800 text-xs rounded">
                  Skills Form
                </span>
              </div>

              <!-- CV Document Link -->
              <div v-if="applicant.cV_File_Ref || applicant.cvFileRef" class="mt-4 p-3 bg-gray-50 rounded-lg">
                <p class="text-sm font-medium text-gray-700 mb-2">CV Document</p>
                <a
                  :href="`http://localhost:5259/api/v1/files/${applicant.cV_File_Ref || applicant.cvFileRef}`"
                  target="_blank"
                  class="text-sm text-blue-600 hover:text-blue-800 underline"
                >
                  View CV
                </a>
              </div>
            </div>

            <div class="ml-4 flex flex-col gap-2">
              <button
                @click="viewApplicantProfile(applicant.candidateID)"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition text-sm"
              >
                View Profile
              </button>
              <select
                :value="applicant.status"
                @change="updateStatus(applicant.applicationID, $event.target.value)"
                class="px-3 py-2 border border-gray-300 rounded-lg text-sm"
              >
                <option value="Applied">Applied</option>
                <option value="Shortlisted">Shortlisted</option>
                <option value="InterviewScheduled">Interview Scheduled</option>
                <option value="InterviewAccepted">Interview Accepted</option>
                <option value="InterviewDeclined">Interview Declined</option>
                <option value="Hired">Hired</option>
                <option value="Rejected">Rejected</option>
              </select>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- CV View Modal -->
    <div
      v-if="selectedCandidate"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4"
      @click.self="closeCVModal"
    >
      <div class="bg-white rounded-lg shadow-xl max-w-4xl w-full max-h-[90vh] overflow-y-auto">
        <div class="p-6">
          <div class="flex justify-between items-start mb-4">
            <h2 class="text-2xl font-bold text-gray-900">Candidate CV</h2>
            <button
              @click="closeCVModal"
              class="text-gray-400 hover:text-gray-600 text-2xl"
            >
              ×
            </button>
          </div>

          <!-- Candidate Information -->
          <div class="space-y-6">
            <!-- Personal Details -->
            <div>
              <h3 class="text-lg font-semibold text-gray-900 mb-3 border-b pb-2">Personal Information</h3>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <p class="text-sm text-gray-600">Full Name</p>
                  <p class="font-medium">{{ getCandidateName(selectedCandidate) }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Email</p>
                  <p class="font-medium">{{ selectedCandidate.candidate?.email || selectedCandidate.candidateEmail || 'N/A' }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Contact Number</p>
                  <p class="font-medium">{{ selectedCandidate.candidate?.contactNumber || selectedCandidate.candidateContactNumber || 'N/A' }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Province</p>
                  <p class="font-medium">{{ selectedCandidate.candidate?.province || selectedCandidate.candidateProvince || 'N/A' }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Employment Status</p>
                  <p class="font-medium">{{ selectedCandidate.candidate?.employmentStatus || selectedCandidate.candidateEmploymentStatus || 'N/A' }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">OFO Code</p>
                  <p class="font-medium">{{ selectedCandidate.candidate?.ofO_Code || selectedCandidate.candidateOFO_Code || 'N/A' }}</p>
                </div>
              </div>
            </div>

            <!-- Education -->
            <div>
              <h3 class="text-lg font-semibold text-gray-900 mb-3 border-b pb-2">Education</h3>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <p class="text-sm text-gray-600">Highest Qualification</p>
                  <p class="font-medium">{{ selectedCandidate.candidate?.highestQualification || selectedCandidate.candidateHighestQualification || 'N/A' }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Institution</p>
                  <p class="font-medium">{{ selectedCandidate.candidate?.institutionName || selectedCandidate.candidateInstitutionName || 'N/A' }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Year Obtained</p>
                  <p class="font-medium">{{ selectedCandidate.candidate?.qualificationYear || selectedCandidate.candidateQualificationYear || 'N/A' }}</p>
                </div>
              </div>
            </div>

            <!-- Documents -->
            <div>
              <h3 class="text-lg font-semibold text-gray-900 mb-3 border-b pb-2">Documents</h3>
              <div class="space-y-2">
                <div v-if="selectedCandidate.candidate?.id_Document_Ref || selectedCandidate.candidateID_Document_Ref">
                  <p class="text-sm text-gray-600 mb-1">Identity Document</p>
                  <a
                    :href="`http://localhost:5259/api/v1/files/${selectedCandidate.candidate?.id_Document_Ref || selectedCandidate.candidateID_Document_Ref}`"
                    target="_blank"
                    class="text-blue-600 hover:text-blue-800 underline"
                  >
                    View ID Document
                  </a>
                </div>
                <div v-if="selectedCandidate.cV_File_Ref || selectedCandidate.cvFileRef">
                  <p class="text-sm text-gray-600 mb-1">CV Document</p>
                  <a
                    :href="`http://localhost:5259/api/v1/files/${selectedCandidate.cV_File_Ref || selectedCandidate.cvFileRef}`"
                    target="_blank"
                    class="text-blue-600 hover:text-blue-800 underline"
                  >
                    View CV Document
                  </a>
                </div>
                <p v-if="!selectedCandidate.candidate?.id_Document_Ref && !selectedCandidate.candidateID_Document_Ref && !selectedCandidate.cV_File_Ref && !selectedCandidate.cvFileRef" class="text-gray-500 text-sm">No documents available</p>
              </div>
            </div>

            <!-- Application Details -->
            <div>
              <h3 class="text-lg font-semibold text-gray-900 mb-3 border-b pb-2">Application Details</h3>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <p class="text-sm text-gray-600">Date Applied</p>
                  <p class="font-medium">{{ formatDate(selectedCandidate.dateApplied) }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Status</p>
                  <span :class="getStatusClass(selectedCandidate.status)">
                    {{ formatStatus(selectedCandidate.status) }}
                  </span>
                </div>
              </div>
            </div>
          </div>

          <div class="mt-6 flex justify-end">
            <button
              @click="closeCVModal"
              class="px-4 py-2 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition"
            >
              Close
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const route = useRoute()
const jobId = computed(() => parseInt(route.params.id))
const { getApplicants, updateApplicationStatus } = useEmployer()

const loading = ref(true)
const error = ref(null)
const applicants = ref([])
const jobTitle = ref('')
const selectedCandidate = ref(null)
const filters = ref({
  ofoCode: '',
  employmentStatus: '',
  province: ''
})

const filteredApplicants = computed(() => {
  if (!applicants.value || applicants.value.length === 0) {
    return []
  }

  let filtered = [...applicants.value]

  // Filter by OFO Code
  if (filters.value.ofoCode && filters.value.ofoCode.trim() !== '') {
    const ofoCodeFilter = filters.value.ofoCode.trim().toLowerCase()
    filtered = filtered.filter(applicant => {
      const candidateOFO = (applicant.candidateOFO_Code || applicant.candidateOFO || '').toLowerCase()
      return candidateOFO.includes(ofoCodeFilter)
    })
  }

  // Filter by Employment Status
  if (filters.value.employmentStatus && filters.value.employmentStatus.trim() !== '') {
    filtered = filtered.filter(applicant => {
      const employmentStatus = (applicant.candidateEmploymentStatus || '').trim()
      return employmentStatus === filters.value.employmentStatus
    })
  }

  // Filter by Province
  if (filters.value.province && filters.value.province.trim() !== '') {
    filtered = filtered.filter(applicant => {
      const province = (applicant.candidateProvince || '').trim()
      return province === filters.value.province
    })
  }

  return filtered
})

const loadApplicants = async () => {
  loading.value = true
  error.value = null

  try {
    // Load all applicants without filters for client-side filtering
    const result = await getApplicants(jobId.value, {})
    if (result.success) {
      applicants.value = result.data || []
      if (applicants.value.length > 0) {
        jobTitle.value = applicants.value[0].jobTitle || 'Job Applicants'
      }
    } else {
      error.value = result.error || 'Failed to load applicants'
    }
  } catch (err) {
    error.value = 'Failed to load applicants'
    console.error('Load applicants error:', err)
  } finally {
    loading.value = false
  }
}

const applyFilters = () => {
  // Client-side filtering is now reactive via computed property
  // Filters are applied automatically as user types/changes values
  // This function can be used to reload from server if needed
  // For now, filtering happens reactively, so no action needed
}

const clearFilters = () => {
  filters.value = {
    ofoCode: '',
    employmentStatus: '',
    province: ''
  }
  loadApplicants()
}

const updateStatus = async (applicationId, newStatus) => {
  try {
    const result = await updateApplicationStatus(applicationId, newStatus)
    if (result.success) {
      // Update local state
      const applicant = applicants.value.find(a => a.applicationID === applicationId)
      if (applicant) {
        applicant.status = newStatus
      }
    } else {
      alert(`Failed to update status: ${result.error}`)
      // Reload to get correct status
      await loadApplicants()
    }
  } catch (err) {
    console.error('Update status error:', err)
    alert('Failed to update status')
    await loadApplicants()
  }
}

const viewApplicantProfile = async (candidateId) => {
  // Fetch and show candidate profile in CV modal
  try {
    const { api } = useApi()
    const response = await api.get(`/jobs/${jobId.value}/applicants/${candidateId}`)
    if (response.data) {
      selectedCandidate.value = response.data
    }
  } catch (error) {
    console.error('Error fetching candidate profile:', error)
    alert('Failed to load candidate profile')
  }
}

const closeCVModal = () => {
  selectedCandidate.value = null
}

const getCandidateName = (applicant) => {
  const firstName = applicant.candidateFirstName || applicant.candidate?.firstName || ''
  const lastName = applicant.candidateLastName || applicant.candidate?.lastName || ''
  if (firstName || lastName) {
    return `${firstName} ${lastName}`.trim()
  }
  return `Candidate #${applicant.candidateID}`
}

const formatDate = (dateString) => {
  if (!dateString) return 'N/A'
  const date = new Date(dateString)
  return date.toLocaleDateString('en-ZA', { year: 'numeric', month: 'short', day: 'numeric' })
}

const formatStatus = (status) => {
  if (!status) return 'Unknown'
  return status.replace(/([A-Z])/g, ' $1').trim()
}

const getStatusClass = (status) => {
  const baseClass = 'px-2 py-1 text-xs rounded'
  switch (status) {
    case 'Shortlisted':
      return `${baseClass} bg-green-100 text-green-800`
    case 'InterviewScheduled':
      return `${baseClass} bg-blue-100 text-blue-800`
    case 'Hired':
      return `${baseClass} bg-purple-100 text-purple-800`
    case 'Rejected':
      return `${baseClass} bg-red-100 text-red-800`
    default:
      return `${baseClass} bg-gray-100 text-gray-800`
  }
}


onMounted(() => {
  loadApplicants()
})
</script>

<style scoped>
</style>

