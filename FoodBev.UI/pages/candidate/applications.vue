<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 py-4">
        <div class="flex items-center justify-between">
          <div>
            <h1 class="text-2xl font-bold text-gray-900">My Applications</h1>
            <p class="text-sm text-gray-600 mt-1">Track your job applications</p>
          </div>
          <div class="flex gap-2">
            <button
              @click="loadApplications"
              :disabled="loading"
              class="px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2"
              title="Refresh applications"
            >
              <svg 
                :class="{ 'animate-spin': loading }"
                class="w-4 h-4" 
                fill="none" 
                stroke="currentColor" 
                viewBox="0 0 24 24"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
              <span v-if="!loading">Refresh</span>
              <span v-else>Refreshing...</span>
            </button>
            <NuxtLink
              to="/candidate/jobs"
              class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
            >
              Browse Jobs
            </NuxtLink>
          </div>
        </div>
      </div>
    </header>

    <!-- Loading State -->
    <div v-if="loading" class="max-w-7xl mx-auto px-4 py-8">
      <div class="text-center">
        <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <p class="mt-2 text-gray-600">Loading applications...</p>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="max-w-7xl mx-auto px-4 py-8">
      <div class="bg-red-50 border border-red-200 rounded-lg p-4">
        <p class="text-red-800">{{ error }}</p>
        <button
          @click="loadApplications"
          class="mt-3 text-sm text-red-600 hover:text-red-800 underline"
        >
          Try again
        </button>
      </div>
    </div>

    <!-- Applications List -->
    <div v-else class="max-w-7xl mx-auto px-4 py-6">
      <div v-if="applications.length === 0" class="text-center py-12">
        <p class="text-gray-600">You haven't applied to any jobs yet.</p>
        <NuxtLink
          to="/candidate/jobs"
          class="mt-4 inline-block px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
        >
          Browse Jobs
        </NuxtLink>
      </div>

      <div v-else class="space-y-4">
        <div
          v-for="app in applications"
          :key="app.applicationID"
          class="bg-white rounded-lg shadow-sm border border-gray-200 p-6"
        >
          <div class="flex justify-between items-start">
            <div class="flex-1">
              <h3 class="text-xl font-semibold text-gray-900">{{ app.jobTitle }}</h3>
              <p class="text-sm text-gray-600 mt-1">{{ app.companyName }}</p>
              
              <div class="mt-4 flex flex-wrap gap-2">
                <span :class="getStatusClass(app.status)">
                  {{ formatStatus(app.status) }}
                </span>
                <span class="px-2 py-1 bg-gray-100 text-gray-800 text-xs rounded">
                  Applied: {{ formatDate(app.dateApplied) }}
                </span>
              </div>

              <!-- Interview Details -->
              <div v-if="app.interviewDate" class="mt-4 p-3 bg-blue-50 rounded-lg">
                <p class="text-sm font-medium text-blue-900">Interview Scheduled</p>
                <p class="text-sm text-blue-700 mt-1">
                  Date: {{ formatDate(app.interviewDate) }}
                </p>
                <p class="text-sm text-blue-700">
                  Venue: {{ app.interviewVenue }}
                </p>
                
                <!-- Interview Response Buttons -->
                <div v-if="app.status === 'InterviewScheduled'" class="mt-3 flex gap-2">
                  <button
                    @click="respondToInterview(app.applicationID, 'Accepted')"
                    :disabled="responding"
                    class="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 disabled:opacity-50 text-sm"
                  >
                    Accept Interview
                  </button>
                  <button
                    @click="respondToInterview(app.applicationID, 'Declined')"
                    :disabled="responding"
                    class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 disabled:opacity-50 text-sm"
                  >
                    Decline Interview
                  </button>
                </div>
                
                <div v-else-if="app.interviewResponse === 'Accepted'" class="mt-2">
                  <span class="text-sm text-green-700 font-medium">✓ Interview Accepted</span>
                </div>
                <div v-else-if="app.interviewResponse === 'Declined'" class="mt-2">
                  <span class="text-sm text-red-700 font-medium">✗ Interview Declined</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const { getApplications, respondToInterview: respondInterview } = useCandidate()

const applications = ref([])
const loading = ref(true)
const error = ref(null)
const responding = ref(false)

const loadApplications = async () => {
  loading.value = true
  error.value = null
  
  const result = await getApplications()
  if (result.success) {
    applications.value = result.data || []
  } else {
    error.value = result.error
  }
  loading.value = false
}

const respondToInterview = async (applicationId, response) => {
  responding.value = true
  const result = await respondInterview(applicationId, response)
  if (result.success) {
    alert(`Interview ${response.toLowerCase()} successfully!`)
    await loadApplications()
  } else {
    alert(`Failed to respond: ${result.error}`)
  }
  responding.value = false
}

const formatStatus = (status) => {
  const statusMap = {
    'Applied': 'Applied',
    'Shortlisted': 'Shortlisted',
    'InterviewScheduled': 'Interview Scheduled',
    'InterviewAccepted': 'Interview Accepted',
    'InterviewDeclined': 'Interview Declined',
    'Hired': 'Hired',
    'Rejected': 'Rejected'
  }
  return statusMap[status] || status
}

const getStatusClass = (status) => {
  const classes = {
    'Applied': 'px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded',
    'Shortlisted': 'px-2 py-1 bg-yellow-100 text-yellow-800 text-xs rounded',
    'InterviewScheduled': 'px-2 py-1 bg-purple-100 text-purple-800 text-xs rounded',
    'InterviewAccepted': 'px-2 py-1 bg-green-100 text-green-800 text-xs rounded',
    'InterviewDeclined': 'px-2 py-1 bg-red-100 text-red-800 text-xs rounded',
    'Hired': 'px-2 py-1 bg-green-100 text-green-800 text-xs rounded font-bold',
    'Rejected': 'px-2 py-1 bg-red-100 text-red-800 text-xs rounded'
  }
  return classes[status] || 'px-2 py-1 bg-gray-100 text-gray-800 text-xs rounded'
}

const formatDate = (dateString) => {
  if (!dateString) return 'N/A'
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' })
}

// Auto-refresh when window gains focus (user switches back to tab)
const handleVisibilityChange = () => {
  if (!document.hidden && !loading.value) {
    // Only refresh if page is visible and not already loading
    loadApplications()
  }
}

onMounted(() => {
  loadApplications()
  // Listen for visibility changes (tab switch)
  document.addEventListener('visibilitychange', handleVisibilityChange)
  // Also listen for window focus (alternative method)
  window.addEventListener('focus', handleVisibilityChange)
})

onUnmounted(() => {
  // Clean up event listeners
  document.removeEventListener('visibilitychange', handleVisibilityChange)
  window.removeEventListener('focus', handleVisibilityChange)
})
</script>

<style scoped>
</style>
