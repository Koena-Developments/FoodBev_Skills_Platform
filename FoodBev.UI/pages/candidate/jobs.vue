<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 py-4">
        <h1 class="text-2xl font-bold text-gray-900">Matching Jobs</h1>
        <p class="text-sm text-gray-600 mt-1">Jobs that match your profile</p>
      </div>
    </header>

    <!-- Loading State -->
    <div v-if="loading" class="max-w-7xl mx-auto px-4 py-8">
      <div class="text-center">
        <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <p class="mt-2 text-gray-600">Loading jobs...</p>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="max-w-7xl mx-auto px-4 py-8">
      <div class="bg-red-50 border border-red-200 rounded-lg p-4">
        <p class="text-red-800">{{ error }}</p>
        <button
          @click="loadJobs"
          class="mt-3 text-sm text-red-600 hover:text-red-800 underline"
        >
          Try again
        </button>
      </div>
    </div>

    <!-- Jobs List -->
    <div v-else class="max-w-7xl mx-auto px-4 py-6">
      <div v-if="jobs.length === 0" class="text-center py-12">
        <p class="text-gray-600">No matching jobs found. Complete your profile to see more opportunities.</p>
      </div>

      <div v-else class="space-y-4">
        <div
          v-for="job in jobs"
          :key="job.jobID"
          class="bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md transition"
        >
          <div class="flex justify-between items-start">
            <div class="flex-1">
              <h3 class="text-xl font-semibold text-gray-900">{{ job.jobTitle }}</h3>
              <p class="text-sm text-gray-600 mt-1">{{ job.companyName }}</p>
              <p class="text-sm text-gray-500 mt-2">{{ job.jobDescription }}</p>
              
              <div class="mt-4 flex flex-wrap gap-2">
                <span class="px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded">
                  OFO: {{ job.ofO_Code_Required }}
                </span>
                <span v-if="job.isBursary" class="px-2 py-1 bg-green-100 text-green-800 text-xs rounded">
                  Bursary
                </span>
                <span class="px-2 py-1 bg-gray-100 text-gray-800 text-xs rounded">
                  Deadline: {{ formatDate(job.applicationDeadline) }}
                </span>
              </div>
            </div>

            <div class="ml-4 flex flex-col gap-2">
              <button
                @click="applyToJob(job.jobID)"
                :disabled="applyingJobs.includes(job.jobID)"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition"
              >
                {{ applyingJobs.includes(job.jobID) ? 'Applying...' : 'Apply' }}
              </button>
              <button
                @click="saveJob(job.jobID)"
                :disabled="savingJobs.includes(job.jobID)"
                class="px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition"
              >
                {{ savingJobs.includes(job.jobID) ? 'Saving...' : 'Save' }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const { getMatchingJobs, applyToJob: applyJob, saveJob: saveJobAction } = useCandidate()

const jobs = ref([])
const loading = ref(true)
const error = ref(null)
const applyingJobs = ref([])
const savingJobs = ref([])

const loadJobs = async () => {
  loading.value = true
  error.value = null
  
  const result = await getMatchingJobs()
  if (result.success) {
    jobs.value = result.data || []
  } else {
    error.value = result.error
  }
  loading.value = false
}

const applyToJob = async (jobId) => {
  applyingJobs.value.push(jobId)
  const result = await applyJob(jobId)
  if (result.success) {
    // Show success message
    alert('Application submitted successfully!')
    // Reload jobs to update status
    await loadJobs()
  } else {
    alert(`Failed to apply: ${result.error}`)
  }
  applyingJobs.value = applyingJobs.value.filter(id => id !== jobId)
}

const saveJob = async (jobId) => {
  savingJobs.value.push(jobId)
  const result = await saveJobAction(jobId)
  if (result.success) {
    alert('Job saved successfully!')
  } else {
    alert(`Failed to save job: ${result.error}`)
  }
  savingJobs.value = savingJobs.value.filter(id => id !== jobId)
}

const formatDate = (dateString) => {
  if (!dateString) return 'N/A'
  const date = new Date(dateString)
  return date.toLocaleDateString()
}

// Load jobs on mount
onMounted(() => {
  loadJobs()
})
</script>

<style scoped>
</style>
