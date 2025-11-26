<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 py-4">
        <div class="flex justify-between items-center">
          <div>
            <h1 class="text-2xl font-bold text-gray-900">Employer Dashboard</h1>
            <p class="text-sm text-gray-600 mt-1">Manage job postings and review applicants</p>
          </div>
          <button
            @click="showCreateJobModal = true"
            class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
          >
            + Post New Job
          </button>
        </div>
      </div>
    </header>

    <!-- Loading State -->
    <div v-if="loading" class="max-w-7xl mx-auto px-4 py-8">
      <div class="text-center">
        <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <p class="mt-2 text-gray-600">Loading...</p>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="max-w-7xl mx-auto px-4 py-8">
      <div class="bg-red-50 border border-red-200 rounded-lg p-4">
        <p class="text-red-800">{{ error }}</p>
      </div>
    </div>

    <!-- Main Content -->
    <div v-else class="max-w-7xl mx-auto px-4 py-6">
      <!-- Quick Stats -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
        <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 class="text-sm font-medium text-gray-600">Total Jobs</h3>
          <p class="text-3xl font-bold text-gray-900 mt-2">{{ jobs.length }}</p>
        </div>
        <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 class="text-sm font-medium text-gray-600">Active Jobs</h3>
          <p class="text-3xl font-bold text-green-600 mt-2">{{ activeJobsCount }}</p>
        </div>
        <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <h3 class="text-sm font-medium text-gray-600">Total Applicants</h3>
          <p class="text-3xl font-bold text-blue-600 mt-2">{{ totalApplicants }}</p>
        </div>
      </div>

      <!-- Profile Summary -->
      <div v-if="profile" class="bg-white rounded-lg shadow-sm border border-gray-200 p-6 mb-6">
        <div class="flex justify-between items-start">
          <div>
            <h2 class="text-lg font-semibold text-gray-900">{{ profile.companyName || 'Company Name' }}</h2>
            <p class="text-sm text-gray-600 mt-1">{{ profile.email }}</p>
            <p class="text-sm text-gray-600">Levy Number: {{ profile.levyNumber || 'N/A' }}</p>
          </div>
          <NuxtLink
            to="/employer/profile"
            class="text-sm text-blue-600 hover:text-blue-800 font-medium"
          >
            Edit Profile
          </NuxtLink>
        </div>
      </div>

      <!-- Profile Not Found Message -->
      <div v-else class="bg-yellow-50 border border-yellow-200 rounded-lg p-4 mb-6">
        <div class="flex justify-between items-start">
          <div>
            <h3 class="text-sm font-medium text-yellow-800">Complete Your Company Profile</h3>
            <p class="text-sm text-yellow-700 mt-1">Set up your company profile to start posting jobs.</p>
          </div>
          <NuxtLink
            to="/employer/profile"
            class="text-sm text-yellow-800 hover:text-yellow-900 font-medium underline"
          >
            Set Up Profile
          </NuxtLink>
        </div>
      </div>

      <!-- Jobs List -->
      <div>
        <h2 class="text-lg font-semibold text-gray-900 mb-4">My Job Postings</h2>
        
        <div v-if="jobs.length === 0" class="bg-white rounded-lg shadow-sm border border-gray-200 p-12 text-center">
          <p class="text-gray-600 mb-4">You haven't posted any jobs yet.</p>
          <button
            @click="showCreateJobModal = true"
            class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
          >
            Post Your First Job
          </button>
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
                <p class="text-sm text-gray-500 mt-2 line-clamp-2">{{ job.jobDescription }}</p>
                
                <div class="mt-4 flex flex-wrap gap-2">
                  <span class="px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded">
                    OFO: {{ job.ofO_Code_Required }}
                  </span>
                  <span v-if="job.preferredProvince" class="px-2 py-1 bg-purple-100 text-purple-800 text-xs rounded">
                    {{ job.preferredProvince }}
                  </span>
                  <span v-if="job.isBursary" class="px-2 py-1 bg-green-100 text-green-800 text-xs rounded">
                    Bursary
                  </span>
                  <span class="px-2 py-1 bg-gray-100 text-gray-800 text-xs rounded">
                    Posted: {{ formatDate(job.datePosted) }}
                  </span>
                  <span class="px-2 py-1 bg-red-100 text-red-800 text-xs rounded">
                    Deadline: {{ formatDate(job.applicationDeadline) }}
                  </span>
                </div>
              </div>

              <div class="ml-4 flex flex-col gap-2">
                <NuxtLink
                  :to="`/employer/jobs/${job.jobID}/applicants`"
                  class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition text-center text-sm"
                >
                  View Applicants
                </NuxtLink>
                <button
                  @click="editJob(job)"
                  class="px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition text-sm"
                >
                  Edit
                </button>
                <button
                  @click="deleteJob(job.jobID)"
                  :disabled="deletingJobs.includes(job.jobID)"
                  class="px-4 py-2 border border-red-300 text-red-700 rounded-lg hover:bg-red-50 disabled:opacity-50 transition text-sm"
                >
                  {{ deletingJobs.includes(job.jobID) ? 'Deleting...' : 'Delete' }}
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Create/Edit Job Modal -->
    <div
      v-if="showCreateJobModal || editingJob"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4"
      @click.self="closeModal"
    >
      <div class="bg-white rounded-lg shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        <div class="p-6">
          <h2 class="text-2xl font-bold text-gray-900 mb-4">
            {{ editingJob ? 'Edit Job' : 'Post New Job' }}
          </h2>

          <form @submit.prevent="handleSubmitJob" class="space-y-4">
            <div>
              <label for="jobTitle" class="block text-sm font-medium text-gray-700">Job Title *</label>
              <input
                id="jobTitle"
                v-model="jobForm.jobTitle"
                type="text"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="e.g., Food & Beverage Manager"
              />
            </div>

            <div>
              <label for="jobDescription" class="block text-sm font-medium text-gray-700">Job Description *</label>
              <textarea
                id="jobDescription"
                v-model="jobForm.jobDescription"
                required
                rows="4"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="Describe the role, responsibilities, and requirements..."
              ></textarea>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label for="ofO_Code_Required" class="block text-sm font-medium text-gray-700">OFO Code Required *</label>
                <input
                  id="ofO_Code_Required"
                  v-model="jobForm.ofO_Code_Required"
                  type="text"
                  required
                  maxlength="10"
                  class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  placeholder="e.g., 123456"
                />
                <p class="mt-1 text-xs text-gray-500">Only candidates with this OFO code will see this job</p>
              </div>

              <div>
                <label for="preferredProvince" class="block text-sm font-medium text-gray-700">Preferred Province</label>
                <select
                  id="preferredProvince"
                  v-model="jobForm.preferredProvince"
                  class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                >
                  <option value="">Any Province</option>
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

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label for="applicationDeadline" class="block text-sm font-medium text-gray-700">Application Deadline *</label>
                <input
                  id="applicationDeadline"
                  v-model="jobForm.applicationDeadline"
                  type="date"
                  required
                  :min="new Date().toISOString().split('T')[0]"
                  class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              <div class="flex items-center">
                <label class="flex items-center">
                  <input
                    v-model="jobForm.isBursary"
                    type="checkbox"
                    class="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                  />
                  <span class="ml-2 text-sm text-gray-700">This is a bursary opportunity</span>
                </label>
              </div>
            </div>

            <div v-if="jobError" class="bg-red-50 border border-red-200 rounded-lg p-3">
              <p class="text-red-800 text-sm">{{ jobError }}</p>
            </div>

            <div class="flex justify-end gap-3 pt-4">
              <button
                type="button"
                @click="closeModal"
                class="px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition"
              >
                Cancel
              </button>
              <button
                type="submit"
                :disabled="savingJob"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition"
              >
                {{ savingJob ? 'Saving...' : (editingJob ? 'Update Job' : 'Post Job') }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const { getProfile, getJobs, createJob, updateJob, deleteJob: deleteJobAction } = useEmployer()

const loading = ref(true)
const error = ref(null)
const profile = ref(null)
const jobs = ref([])
const deletingJobs = ref([])

const showCreateJobModal = ref(false)
const editingJob = ref(null)
const savingJob = ref(false)
const jobError = ref(null)

const jobForm = ref({
  jobTitle: '',
  jobDescription: '',
  ofO_Code_Required: '',
  preferredProvince: '',
  isBursary: false,
  applicationDeadline: ''
})

const loadData = async () => {
  loading.value = true
  error.value = null

  try {
    const [profileResult, jobsResult] = await Promise.all([
      getProfile(),
      getJobs()
    ])

    // Profile might not exist yet (404 is OK)
    if (profileResult.success) {
      profile.value = profileResult.data
    } else if (profileResult.error && !profileResult.error.includes('404')) {
      // Only show error if it's not a 404 (profile not found is expected for new employers)
      console.warn('Profile not found:', profileResult.error)
    }

    // Jobs might return 403 if not authorized, or empty array if no jobs
    if (jobsResult.success) {
      jobs.value = jobsResult.data || []
    } else {
      // Check if it's a 403 (forbidden) - might be role issue
      if (jobsResult.error && jobsResult.error.includes('403')) {
        error.value = 'Access denied. Please ensure you are logged in as an Employer.'
      } else {
        error.value = jobsResult.error || 'Failed to load jobs'
      }
    }
  } catch (err) {
    error.value = 'Failed to load data'
    console.error('Load data error:', err)
  } finally {
    loading.value = false
  }
}

const activeJobsCount = computed(() => {
  const now = new Date()
  return jobs.value.filter(job => {
    const deadline = new Date(job.applicationDeadline)
    return deadline > now
  }).length
})

const totalApplicants = computed(() => {
  // This would need to be calculated from all applications
  // For now, return 0 or fetch from a separate endpoint
  return 0
})

const formatDate = (dateString) => {
  if (!dateString) return 'N/A'
  const date = new Date(dateString)
  return date.toLocaleDateString('en-ZA', { year: 'numeric', month: 'short', day: 'numeric' })
}

const closeModal = () => {
  showCreateJobModal.value = false
  editingJob.value = null
  jobError.value = null
  jobForm.value = {
    jobTitle: '',
    jobDescription: '',
    ofO_Code_Required: '',
    preferredProvince: '',
    isBursary: false,
    applicationDeadline: ''
  }
}

const editJob = (job) => {
  editingJob.value = job
  jobForm.value = {
    jobTitle: job.jobTitle || '',
    jobDescription: job.jobDescription || '',
    ofO_Code_Required: job.ofO_Code_Required || '',
    preferredProvince: job.preferredProvince || '',
    isBursary: job.isBursary || false,
    applicationDeadline: job.applicationDeadline ? new Date(job.applicationDeadline).toISOString().split('T')[0] : ''
  }
}

const handleSubmitJob = async () => {
  savingJob.value = true
  jobError.value = null

  try {
    const jobData = {
      jobTitle: jobForm.value.jobTitle,
      jobDescription: jobForm.value.jobDescription,
      ofO_Code_Required: jobForm.value.ofO_Code_Required,
      preferredProvince: jobForm.value.preferredProvince || null,
      isBursary: jobForm.value.isBursary,
      applicationDeadline: new Date(jobForm.value.applicationDeadline).toISOString()
    }

    let result
    if (editingJob.value) {
      result = await updateJob(editingJob.value.jobID, jobData)
    } else {
      result = await createJob(jobData)
    }

    if (result.success) {
      closeModal()
      await loadData()
    } else {
      jobError.value = result.error || 'Failed to save job'
    }
  } catch (err) {
    jobError.value = 'An error occurred while saving the job'
  } finally {
    savingJob.value = false
  }
}

const deleteJob = async (jobId) => {
  if (!confirm('Are you sure you want to delete this job posting?')) {
    return
  }

  deletingJobs.value.push(jobId)
  const result = await deleteJobAction(jobId)
  
  if (result.success) {
    await loadData()
  } else {
    alert(`Failed to delete job: ${result.error}`)
  }
  
  deletingJobs.value = deletingJobs.value.filter(id => id !== jobId)
}

onMounted(() => {
  loadData()
})
</script>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
