<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8">
      <div>
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
          Create Admin Account
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          One-time setup to create the initial administrator account
        </p>
      </div>
      <form class="mt-8 space-y-6" @submit.prevent="handleSetup" novalidate>
        <div class="rounded-md shadow-sm space-y-4">
          <div>
            <label for="email" class="block text-sm font-medium text-gray-700 mb-1">
              Admin Email
            </label>
            <input
              id="email"
              v-model="email"
              name="email"
              type="email"
              autocomplete="email"
              class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="admin@foodbev.com"
              @input="error = ''"
            />
          </div>
          <div>
            <label for="password" class="block text-sm font-medium text-gray-700 mb-1">
              Password
            </label>
            <input
              id="password"
              v-model="password"
              name="password"
              type="password"
              autocomplete="new-password"
              class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Minimum 8 characters"
              @input="error = ''"
            />
          </div>
          <div>
            <label for="secretKey" class="block text-sm font-medium text-gray-700 mb-1">
              Secret Key
            </label>
            <input
              id="secretKey"
              v-model="secretKey"
              name="secretKey"
              type="text"
              class="appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Enter the admin setup secret key"
              @input="error = ''"
            />
            <p class="mt-1 text-xs text-gray-500">
              Secret key: <code class="bg-gray-100 px-1 rounded">ADMIN_SETUP_SECRET_KEY_2024_CHANGE_IN_PRODUCTION</code>
            </p>
          </div>
        </div>

        <div v-if="error" class="text-red-600 text-sm text-center bg-red-50 border border-red-200 rounded p-3">
          {{ error }}
        </div>

        <div v-if="success" class="text-green-600 text-sm text-center bg-green-50 border border-green-200 rounded p-3">
          {{ success }}
        </div>

        <div>
          <button
            type="submit"
            :disabled="loading || success || !isFormValid"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <span v-if="loading">Creating admin account...</span>
            <span v-else-if="success">Account Created!</span>
            <span v-else>Create Admin Account</span>
          </button>
        </div>

        <div class="text-center">
          <NuxtLink to="/admin/login" class="text-indigo-600 hover:text-indigo-500 text-sm">
            Already have an admin account? Login here
          </NuxtLink>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
const { createAdminAccount } = useAdminSetup()
const router = useRouter()

const email = ref('')
const password = ref('')
const secretKey = ref('')
const error = ref('')
const success = ref('')
const loading = ref(false)

// Computed property to check if form is valid
const isFormValid = computed(() => {
  return email.value.trim() !== '' && 
         password.value.length >= 8 && 
         secretKey.value.trim() !== ''
})

const handleSetup = async () => {
  // Validate form before submission
  if (!isFormValid.value) {
    error.value = 'Please fill in all fields. Password must be at least 8 characters.'
    return
  }

  error.value = ''
  success.value = ''
  loading.value = true
  
  try {
    const result = await createAdminAccount(email.value, password.value, secretKey.value)
    
    if (result.success) {
      success.value = result.data?.message || 'Admin account created successfully! Redirecting to login...'
      // Redirect to login after 2 seconds
      setTimeout(() => {
        router.push('/admin/login')
      }, 2000)
    } else {
      error.value = result.error || 'Failed to create admin account'
    }
  } catch (err) {
    error.value = 'An unexpected error occurred. Please try again.'
    console.error('Setup error:', err)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
</style>

