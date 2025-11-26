<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-4xl mx-auto px-4 py-4">
        <h1 class="text-2xl font-bold text-gray-900">Edit Profile</h1>
        <p class="text-sm text-gray-600 mt-1">Complete your profile to see more job matches</p>
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
        <!-- Personal Details -->
        <div>
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Personal Details</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label for="firstName" class="block text-sm font-medium text-gray-700">First Name *</label>
              <input
                id="firstName"
                v-model="form.firstName"
                type="text"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="lastName" class="block text-sm font-medium text-gray-700">Last Name *</label>
              <input
                id="lastName"
                v-model="form.lastName"
                type="text"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="idNumber" class="block text-sm font-medium text-gray-700">ID Number *</label>
              <input
                id="idNumber"
                v-model="form.idNumber"
                type="text"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="dateOfBirth" class="block text-sm font-medium text-gray-700">Date of Birth</label>
              <input
                id="dateOfBirth"
                v-model="form.dateOfBirth"
                type="date"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="gender" class="block text-sm font-medium text-gray-700">Gender</label>
              <select
                id="gender"
                v-model="form.gender"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              >
                <option value="">Select</option>
                <option value="Male">Male</option>
                <option value="Female">Female</option>
                <option value="Other">Other</option>
              </select>
            </div>
            <div>
              <label for="race" class="block text-sm font-medium text-gray-700">Race</label>
              <input
                id="race"
                v-model="form.race"
                type="text"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="nationality" class="block text-sm font-medium text-gray-700">Nationality</label>
              <input
                id="nationality"
                v-model="form.nationality"
                type="text"
                placeholder="e.g., South African"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label class="flex items-center">
                <input
                  v-model="form.isDisabled"
                  type="checkbox"
                  class="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                />
                <span class="ml-2 text-sm text-gray-700">I have a disability</span>
              </label>
            </div>
            <div v-if="form.isDisabled" class="md:col-span-2">
              <label for="disabilityDetails" class="block text-sm font-medium text-gray-700">Disability Details</label>
              <textarea
                id="disabilityDetails"
                v-model="form.disabilityDetails"
                rows="3"
                placeholder="Please provide details about your disability"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              ></textarea>
            </div>
          </div>
        </div>

        <!-- Contact Details -->
        <div>
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Contact Details</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label for="email" class="block text-sm font-medium text-gray-700">Email *</label>
              <input
                id="email"
                v-model="form.email"
                type="email"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="contactNumber" class="block text-sm font-medium text-gray-700">Contact Number *</label>
              <input
                id="contactNumber"
                v-model="form.contactNumber"
                type="tel"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div class="md:col-span-2">
              <label for="physicalAddress" class="block text-sm font-medium text-gray-700">Physical Address *</label>
              <input
                id="physicalAddress"
                v-model="form.physicalAddress"
                type="text"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="postalCode" class="block text-sm font-medium text-gray-700">Postal Code *</label>
              <input
                id="postalCode"
                v-model="form.postalCode"
                type="text"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="province" class="block text-sm font-medium text-gray-700">Province *</label>
              <select
                id="province"
                v-model="form.province"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              >
                <option value="">Select Province</option>
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
        </div>

        <!-- Education & Employment -->
        <div>
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Education & Employment</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label for="highestQualification" class="block text-sm font-medium text-gray-700">Highest Qualification *</label>
              <input
                id="highestQualification"
                v-model="form.highestQualification"
                type="text"
                required
                placeholder="e.g., Matric, Diploma, Degree"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="institutionName" class="block text-sm font-medium text-gray-700">Institution Name *</label>
              <input
                id="institutionName"
                v-model="form.institutionName"
                type="text"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="qualificationYear" class="block text-sm font-medium text-gray-700">Qualification Year</label>
              <input
                id="qualificationYear"
                v-model.number="form.qualificationYear"
                type="number"
                min="1950"
                :max="new Date().getFullYear()"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
            </div>
            <div>
              <label for="employmentStatus" class="block text-sm font-medium text-gray-700">Employment Status *</label>
              <select
                id="employmentStatus"
                v-model="form.employmentStatus"
                required
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              >
                <option value="">Select</option>
                <option value="Employed">Employed</option>
                <option value="Unemployed">Unemployed</option>
                <option value="Student">Student</option>
              </select>
            </div>
            <div class="md:col-span-2">
              <label for="ofO_Code" class="block text-sm font-medium text-gray-700">OFO Code *</label>
              <input
                id="ofO_Code"
                v-model="form.ofO_Code"
                type="text"
                required
                placeholder="e.g., 123456"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              />
              <p class="mt-1 text-xs text-gray-500">Occupational Field of Study code</p>
            </div>
          </div>
        </div>

        <!-- POPI Consent -->
        <div>
          <label class="flex items-center">
            <input
              v-model="form.acceptsPOPI"
              type="checkbox"
              required
              class="rounded border-gray-300 text-blue-600 focus:ring-blue-500"
            />
            <span class="ml-2 text-sm text-gray-700">
              I accept the POPI Act consent and privacy policy *
            </span>
          </label>
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
            to="/candidate"
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
const { getProfile, updateProfile } = useCandidate()
const router = useRouter()

const loading = ref(true)
const saving = ref(false)
const error = ref(null)
const success = ref(false)

const form = ref({
  firstName: '',
  lastName: '',
  idNumber: '',
  dateOfBirth: '',
  gender: '',
  race: '',
  nationality: '',
  isDisabled: false,
  disabilityDetails: '',
  email: '',
  contactNumber: '',
  physicalAddress: '',
  postalCode: '',
  province: '',
  highestQualification: '',
  institutionName: '',
  qualificationYear: null,
  employmentStatus: '',
  ofO_Code: '',
  id_Document_Ref: '',
  acceptsPOPI: false
})

const loadProfile = async () => {
  loading.value = true
  const result = await getProfile()
  if (result.success && result.data) {
    const profile = result.data
    
    // Helper to safely format date
    const formatDate = (dateValue) => {
      if (!dateValue) return ''
      try {
        const date = new Date(dateValue)
        if (isNaN(date.getTime())) return ''
        // Check if date is valid (not year 0 or before 1900)
        if (date.getFullYear() < 1900 || date.getFullYear() > 2100) return ''
        return date.toISOString().split('T')[0]
      } catch {
        return ''
      }
    }
    
    form.value = {
      firstName: profile.firstName || '',
      lastName: profile.lastName || '',
      idNumber: profile.idNumber || '',
      dateOfBirth: formatDate(profile.dateOfBirth),
      gender: profile.gender || '',
      race: profile.race || '',
      nationality: profile.nationality || '',
      isDisabled: profile.isDisabled || false,
      disabilityDetails: profile.disabilityDetails || '',
      email: profile.email || '',
      contactNumber: profile.contactNumber || '',
      physicalAddress: profile.physicalAddress || '',
      postalCode: profile.postalCode || '',
      province: profile.province || '',
      highestQualification: profile.highestQualification || '',
      institutionName: profile.institutionName || '',
      qualificationYear: profile.qualificationYear || null,
      employmentStatus: profile.employmentStatus || '',
      ofO_Code: profile.ofO_Code || '',
      id_Document_Ref: profile.id_Document_Ref || '',
      acceptsPOPI: profile.acceptsPOPI || false
    }
  } else {
    error.value = result.error || 'Failed to load profile'
  }
  loading.value = false
}

const handleSubmit = async () => {
  saving.value = true
  error.value = null
  success.value = false

  // Prepare data for submission - ensure property names match backend DTO
  const submitData = {
    firstName: form.value.firstName || null,
    lastName: form.value.lastName || null,
    idNumber: form.value.idNumber || null,
    dateOfBirth: null,
    gender: form.value.gender || null,
    race: form.value.race || null,
    nationality: form.value.nationality || null,
    isDisabled: form.value.isDisabled || null,
    disabilityDetails: form.value.disabilityDetails || null,
    email: form.value.email || null,
    contactNumber: form.value.contactNumber || null,
    physicalAddress: form.value.physicalAddress || null,
    postalCode: form.value.postalCode || null,
    province: form.value.province || null,
    highestQualification: form.value.highestQualification || null,
    institutionName: form.value.institutionName || null,
    qualificationYear: form.value.qualificationYear || null,
    employmentStatus: form.value.employmentStatus || null,
    ofO_Code: form.value.ofO_Code || null, // Maps to OFO_Code via JsonPropertyName
    id_Document_Ref: form.value.id_Document_Ref || null, // Maps to ID_Document_Ref via JsonPropertyName
    acceptsPOPI: form.value.acceptsPOPI || false
  }
  
  // Validate and clean dateOfBirth
  if (form.value.dateOfBirth && form.value.dateOfBirth.trim() !== '') {
    // Check if date is invalid (starts with 0000)
    if (!form.value.dateOfBirth.startsWith('0000-')) {
      // Validate the date format
      const date = new Date(form.value.dateOfBirth)
      if (!isNaN(date.getTime()) && date.getFullYear() >= 1900 && date.getFullYear() <= 2100) {
        submitData.dateOfBirth = form.value.dateOfBirth
      }
    }
  }

  const result = await updateProfile(submitData)
  if (result.success) {
    success.value = true
    setTimeout(() => {
      router.push('/candidate')
    }, 1500)
  } else {
    error.value = result.error || 'Failed to update profile'
  }

  saving.value = false
}

onMounted(() => {
  loadProfile()
})
</script>

<style scoped>
</style>

