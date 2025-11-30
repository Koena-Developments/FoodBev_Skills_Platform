<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-4xl mx-auto px-4 py-4">
        <h1 class="text-2xl font-bold text-gray-900">Company Profile</h1>
        <p class="text-sm text-gray-600 mt-1">Complete your company profile to start posting jobs</p>
      </div>
    </header>

    <!-- Loading State -->
    <div v-if="loading" class="max-w-4xl mx-auto px-4 py-8">
      <div class="text-center">
        <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <p class="mt-2 text-gray-600">Loading profile...</p>
      </div>
    </div>

    <!-- Form -->
    <div v-else class="max-w-4xl mx-auto px-4 py-6">
      <form @submit.prevent="handleSubmit" class="bg-white rounded-lg shadow-sm border border-gray-200 p-6 space-y-6">
        <!-- Company Details -->
        <div>
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Company Details</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div class="md:col-span-2">
              <label for="companyName" class="block text-sm font-medium text-gray-700">Company Name *</label>
              <input
                id="companyName"
                v-model="form.companyName"
                type="text"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="e.g., ABC Food & Beverage Company"
              />
            </div>
            <div>
              <label for="levyNumber" class="block text-sm font-medium text-gray-700">Levy Number</label>
              <input
                id="levyNumber"
                v-model="form.levyNumber"
                type="text"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="e.g., 12345678"
              />
              <p class="mt-1 text-xs text-gray-500">Company's Skills Development Levy Number (SETA compliance)</p>
            </div>
            <div>
              <label for="lNumber" class="block text-sm font-medium text-gray-700">L Number</label>
              <input
                id="lNumber"
                v-model="form.lNumber"
                type="text"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="e.g., L123456"
              />
            </div>
            <div>
              <label for="tNumber" class="block text-sm font-medium text-gray-700">T Number</label>
              <input
                id="tNumber"
                v-model="form.tNumber"
                type="text"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="e.g., T123456"
              />
            </div>
          </div>
        </div>

        <!-- Skills Development Facilitator (SDF) Details -->
        <div>
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Skills Development Facilitator (SDF) Details</h2>
          <p class="text-sm text-gray-600 mb-4">The person responsible for skills development and training in your company</p>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label for="sdfName" class="block text-sm font-medium text-gray-700">SDF Name</label>
              <input
                id="sdfName"
                v-model="form.sdfName"
                type="text"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="e.g., John Smith"
              />
            </div>
            <div>
              <label for="sdfContactNumber" class="block text-sm font-medium text-gray-700">SDF Contact Number</label>
              <input
                id="sdfContactNumber"
                v-model="form.sdfContactNumber"
                type="tel"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="e.g., 012 345 6789"
              />
            </div>
            <div class="md:col-span-2">
              <label for="sdfEmail" class="block text-sm font-medium text-gray-700">SDF Email</label>
              <input
                id="sdfEmail"
                v-model="form.sdfEmail"
                type="email"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="e.g., sdf@company.com"
              />
            </div>
          </div>
        </div>

        <!-- Company Logo Upload (Optional) -->
        <div>
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Company Logo</h2>
          <div class="flex items-start gap-4">
            <div v-if="logoPreview" class="flex-shrink-0">
              <img
                :src="logoPreview"
                alt="Company logo preview"
                class="w-32 h-32 object-contain border border-gray-300 rounded-lg"
              />
            </div>
            <div class="flex-1">
              <label for="logo" class="block text-sm font-medium text-gray-700 mb-2">Upload Logo</label>
              <input
                id="logo"
                type="file"
                accept="image/*"
                @change="handleLogoChange"
                class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
              />
              <p class="mt-1 text-xs text-gray-500">Recommended: Square image, max 2MB (PNG, JPG)</p>
              <button
                v-if="logoFile"
                type="button"
                @click="uploadLogo"
                :disabled="uploadingLogo"
                class="mt-2 px-4 py-2 bg-blue-600 text-white text-sm rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition"
              >
                {{ uploadingLogo ? 'Uploading...' : 'Upload Logo' }}
              </button>
            </div>
          </div>
        </div>

        <!-- Error Message -->
        <div v-if="error" class="bg-red-50 border border-red-200 rounded-lg p-4">
          <p class="text-red-800 text-sm">{{ error }}</p>
        </div>

        <!-- Success Message -->
        <div v-if="success" class="bg-green-50 border border-green-200 rounded-lg p-4">
          <p class="text-green-800 text-sm">Profile updated successfully!</p>
        </div>

        <!-- Submit Button -->
        <div class="flex justify-end gap-4">
          <NuxtLink
            to="/employer"
            class="px-6 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition"
          >
            Cancel
          </NuxtLink>
          <button
            type="submit"
            :disabled="saving"
            class="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition"
          >
            {{ saving ? 'Saving...' : 'Save Profile' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
definePageMeta({
  layout: 'employer'
})
const { getProfile, updateProfile, uploadLogo: uploadLogoAction } = useEmployer()
const router = useRouter()

const loading = ref(true)
const saving = ref(false)
const error = ref(null)
const success = ref(false)
const logoFile = ref(null)
const logoPreview = ref(null)
const uploadingLogo = ref(false)

const form = ref({
  companyName: '',
  levyNumber: '',
  lNumber: '',
  tNumber: '',
  sdfName: '',
  sdfContactNumber: '',
  sdfEmail: ''
})

const loadProfile = async () => {
  loading.value = true
  error.value = null
  
  try {
    const result = await getProfile()
    if (result.success && result.data) {
      const profile = result.data
      
      form.value = {
        companyName: profile.companyName || '',
        levyNumber: profile.levyNumber || '',
        lNumber: profile.lNumber || '',
        tNumber: profile.tNumber || '',
        sdfName: profile.sdfName || '',
        sdfContactNumber: profile.sdfContactNumber || '',
        sdfEmail: profile.sdfEmail || ''
      }
      
      // If there's a logo URL, set it as preview
      if (profile.logoUrl) {
        logoPreview.value = profile.logoUrl
      }
    } else if (result.notFound) {
      // Profile doesn't exist yet - that's OK, user can create it
      console.log('Profile not found - user can create new profile')
    } else {
      error.value = result.error || 'Failed to load profile'
    }
  } catch (err) {
    console.error('Load profile error:', err)
    error.value = 'Failed to load profile'
  } finally {
    loading.value = false
  }
}

const handleLogoChange = (event) => {
  const file = event.target.files?.[0]
  if (file) {
    // Validate file size (max 2MB)
    if (file.size > 2 * 1024 * 1024) {
      error.value = 'Logo file size must be less than 2MB'
      return
    }
    
    // Validate file type
    if (!file.type.startsWith('image/')) {
      error.value = 'Please select an image file'
      return
    }
    
    logoFile.value = file
    
    // Create preview
    const reader = new FileReader()
    reader.onload = (e) => {
      logoPreview.value = e.target.result
    }
    reader.readAsDataURL(file)
  }
}

const uploadLogo = async () => {
  if (!logoFile.value) return
  
  uploadingLogo.value = true
  error.value = null
  
  try {
    const result = await uploadLogoAction(logoFile.value)
    if (result.success) {
      // Logo uploaded successfully
      logoFile.value = null
      if (result.data?.logoUrl) {
        logoPreview.value = result.data.logoUrl
      }
    } else {
      error.value = result.error || 'Failed to upload logo'
    }
  } catch (err) {
    console.error('Upload logo error:', err)
    error.value = 'Failed to upload logo'
  } finally {
    uploadingLogo.value = false
  }
}

const handleSubmit = async () => {
  saving.value = true
  error.value = null
  success.value = false

  // Prepare data for submission
  const submitData = {
    companyName: form.value.companyName || null,
    levyNumber: form.value.levyNumber || null,
    lNumber: form.value.lNumber || null,
    tNumber: form.value.tNumber || null,
    sdfName: form.value.sdfName || null,
    sdfContactNumber: form.value.sdfContactNumber || null,
    sdfEmail: form.value.sdfEmail || null
  }

  try {
    const result = await updateProfile(submitData)
    if (result.success) {
      success.value = true
      setTimeout(() => {
        router.push('/employer')
      }, 1500)
    } else {
      error.value = result.error || 'Failed to update profile'
    }
  } catch (err) {
    console.error('Update profile error:', err)
    error.value = 'Failed to update profile'
  } finally {
    saving.value = false
  }
}

onMounted(() => {
  loadProfile()
})
</script>

<style scoped>
/* Employer Profile Page Styles */
</style>
