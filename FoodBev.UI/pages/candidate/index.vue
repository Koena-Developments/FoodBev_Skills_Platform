<template>
  <div class="h-screen w-full bg-gray-100 flex flex-col">
    <!-- Header -->
    <header class="flex items-center px-4 py-3 border-b border-gray-200 bg-white shadow-sm">
      <NuxtLink
        to="/"
        class="p-2 rounded-full hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-blue-500"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-700" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
        </svg>
      </NuxtLink>
      <div class="ml-2 flex-1 max-w-md">
        <label for="search" class="sr-only">Search</label>
        <div class="relative">
          <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-gray-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </div>
          <input
            id="search"
            type="text"
            placeholder="Search"
            class="w-full pl-10 pr-4 py-2 bg-white border border-gray-300 rounded-full shadow-sm text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
      </div>
      <NuxtLink
        to="/candidate/profile"
        class="ml-2 p-2 rounded-full hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-blue-500"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-700" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.827.476 1.414 1.27 1.414 2.146 0 1.186-.957 2.143-2.143 2.143H5.5a.5.5 0 01-.5-.5z" />
        </svg>
      </NuxtLink>
    </header>

    <!-- Main Content - Full Height -->
    <main class="flex-1 overflow-y-auto bg-white">
      <div class="p-4 sm:p-6">
        <!-- Profile Header -->
        <div class="flex flex-col sm:flex-row sm:items-start gap-4 mb-6">
          <div class="w-20 h-20 sm:w-24 sm:h-24 rounded-full border-2 border-white shadow bg-gray-200 flex items-center justify-center">
            <span class="text-2xl font-bold text-gray-600">
              {{ profileInitials }}
            </span>
          </div>
          <div class="flex-1">
            <div class="flex justify-between items-start">
              <div>
                <h1 class="text-xl font-bold text-gray-900">
                  {{ profile?.firstName && profile?.lastName 
                    ? `${profile.firstName} ${profile.lastName}` 
                    : 'Complete Your Profile' }}
                </h1>
                <p v-if="profile?.institutionName" class="text-sm text-gray-600 mt-1">
                  {{ profile.institutionName }}
                </p>
                <p v-if="profile?.province" class="text-sm text-gray-600">
                  {{ profile.province }}, South Africa
                </p>
                <p v-if="profile?.highestQualification" class="text-sm text-gray-600">
                  {{ profile.highestQualification }}
                </p>
                <p v-if="profile?.ofO_Code" class="text-sm text-foodbev-blue mt-1">
                  OFO Code: {{ profile.ofO_Code }}
                </p>
              </div>
              <NuxtLink
                to="/candidate/profile"
                class="p-2 rounded-full hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-gray-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
                </svg>
              </NuxtLink>
            </div>
            <div class="mt-3 flex gap-2">
              <NuxtLink
                to="/candidate/jobs"
                class="text-sm text-foodbev-blue hover:text-opacity-80 font-medium"
              >
                View Matching Jobs ({{ jobsCount }})
              </NuxtLink>
              <span class="text-gray-400">•</span>
              <NuxtLink
                to="/candidate/applications"
                class="text-sm text-foodbev-blue hover:text-opacity-80 font-medium"
              >
                My Applications ({{ applicationsCount }})
              </NuxtLink>
            </div>
          </div>
        </div>

        <div class="flex flex-col gap-3 mb-6">
          <NuxtLink
            to="/candidate/jobs"
            class="w-full bg-foodbev-blue hover:bg-opacity-90 text-white font-medium py-2.5 px-4 rounded-lg transition focus:outline-none focus:ring-2 focus:ring-foodbev-blue focus:ring-offset-2 text-center"
          >
            Browse Jobs
          </NuxtLink>
          <NuxtLink
            to="/candidate/profile"
            class="w-full border border-foodbev-blue text-foodbev-blue hover:bg-foodbev-platinum font-medium py-2.5 px-4 rounded-lg transition focus:outline-none focus:ring-2 focus:ring-foodbev-blue focus:ring-offset-2 text-center"
          >
            Complete Profile
          </NuxtLink>
        </div>

        <!-- Profile Completeness -->
        <div v-if="profileCompleteness < 100" class="bg-yellow-50 border border-yellow-200 rounded-lg p-4 mb-6">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm font-medium text-yellow-800">Profile Incomplete</p>
              <p class="text-xs text-yellow-600 mt-1">Complete your profile to see more job matches</p>
            </div>
            <div class="text-right">
              <p class="text-lg font-bold text-yellow-800">{{ profileCompleteness }}%</p>
            </div>
          </div>
          <div class="mt-2 w-full bg-yellow-200 rounded-full h-2">
            <div
              class="bg-yellow-600 h-2 rounded-full transition-all"
              :style="{ width: `${profileCompleteness}%` }"
            ></div>
          </div>
        </div>

        <button
          class="w-full border border-gray-300 text-gray-700 font-medium py-2.5 rounded-lg mb-6 hover:bg-gray-50 transition focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
        >
          Enhance profile
        </button>

        <div class="border border-gray-200 rounded-lg mb-6">
          <div class="px-4 py-3 bg-gray-50 flex justify-between items-center">
            <div class="flex items-center">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-gray-600 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
              <h2 class="font-medium text-gray-900">Upload Documents</h2>
            </div>
          </div>
          <div class="px-4 py-4">
            <div v-if="profile?.iD_Document_Ref" class="flex items-center justify-between">
              <div class="flex items-center">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-green-600 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
                <span class="text-sm text-gray-700">Document uploaded</span>
              </div>
                <a
                :href="`http://localhost:5259${profile.iD_Document_Ref}`"
                target="_blank"
                class="text-sm text-foodbev-blue hover:text-opacity-80"
              >
                View
              </a>
            </div>
            <div v-else>
              <p class="text-sm text-gray-600 mb-3">Upload your ID document to complete your profile</p>
              <input
                ref="fileInput"
                type="file"
                accept=".pdf,.jpg,.jpeg,.png"
                @change="handleFileUpload"
                class="hidden"
              />
              <button
                @click="$refs.fileInput?.click()"
                :disabled="uploading"
                class="w-full px-4 py-2 bg-foodbev-blue text-white rounded-lg hover:bg-opacity-90 disabled:opacity-50 disabled:cursor-not-allowed transition"
              >
                {{ uploading ? 'Uploading...' : 'Upload ID Document' }}
              </button>
            </div>
            <p v-if="uploadError" class="mt-2 text-sm text-red-600">{{ uploadError }}</p>
          </div>
        </div>

        <!-- Quick Stats -->
        <div class="border border-gray-200 rounded-lg">
          <div class="px-4 py-3 bg-gray-50">
            <h2 class="font-medium text-gray-900">Quick Stats</h2>
          </div>
          <div class="px-4 py-4 grid grid-cols-2 gap-4">
            <div>
              <p class="text-2xl font-bold text-foodbev-blue">{{ jobsCount }}</p>
              <p class="text-sm text-gray-600">Matching Jobs</p>
            </div>
            <div>
              <p class="text-2xl font-bold text-foodbev-red">{{ applicationsCount }}</p>
              <p class="text-sm text-gray-600">Applications</p>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup>
definePageMeta({
  layout: 'candidate'
})
const { getProfile, uploadDocument, getMatchingJobs, getApplications } = useCandidate()

const profile = ref(null)
const jobsCount = ref(0)
const applicationsCount = ref(0)
const uploading = ref(false)
const uploadError = ref(null)
const loading = ref(true)

const profileInitials = computed(() => {
  if (profile.value?.firstName && profile.value?.lastName) {
    return `${profile.value.firstName[0]}${profile.value.lastName[0]}`.toUpperCase()
  }
  return '??'
})

const profileCompleteness = computed(() => {
  if (!profile.value) return 0
  
  let completed = 0
  let total = 0
  
  // Required fields
  const requiredFields = [
    'firstName', 'lastName', 'idNumber', 'contactNumber', 'email',
    'physicalAddress', 'province', 'highestQualification', 'institutionName',
    'employmentStatus', 'ofO_Code', 'iD_Document_Ref'
  ]
  
  requiredFields.forEach(field => {
    total++
    if (profile.value[field] && profile.value[field] !== '') {
      completed++
    }
  })
  
  return Math.round((completed / total) * 100)
})

const loadData = async () => {
  loading.value = true
  
  // Load profile
  const profileResult = await getProfile()
  if (profileResult.success) {
    profile.value = profileResult.data
  }
  
  // Load jobs count
  const jobsResult = await getMatchingJobs()
  if (jobsResult.success) {
    jobsCount.value = jobsResult.data?.length || 0
  }
  
  // Load applications count
  const appsResult = await getApplications()
  if (appsResult.success) {
    applicationsCount.value = appsResult.data?.length || 0
  }
  
  loading.value = false
}

const handleFileUpload = async (event) => {
  const file = event.target.files?.[0]
  if (!file) return
  
  // Validate file type
  const allowedTypes = ['application/pdf', 'image/jpeg', 'image/jpg', 'image/png']
  if (!allowedTypes.includes(file.type)) {
    uploadError.value = 'Invalid file type. Please upload PDF, JPG, or PNG files.'
    return
  }
  
  // Validate file size (5MB)
  if (file.size > 5 * 1024 * 1024) {
    uploadError.value = 'File size exceeds 5MB limit.'
    return
  }
  
  uploading.value = true
  uploadError.value = null
  
  const result = await uploadDocument(file)
  if (result.success) {
    // Reload profile to get updated document reference
    await loadData()
    alert('Document uploaded successfully!')
  } else {
    uploadError.value = result.error
  }
  
  uploading.value = false
  // Reset file input
  if (event.target) {
    event.target.value = ''
  }
}

onMounted(() => {
  loadData()
})
</script>

<style scoped>
/* Ensure full height */
html, body, #app {
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>