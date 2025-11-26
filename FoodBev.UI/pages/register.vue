<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8">
      <div>
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
          Create your account
        </h2>
      </div>
      <form class="mt-8 space-y-6" @submit.prevent="handleRegister">
        <div class="rounded-md shadow-sm space-y-4">
          <div>
            <label for="email" class="block text-sm font-medium text-gray-700">Email address</label>
            <input
              id="email"
              v-model="email"
              name="email"
              type="email"
              autocomplete="email"
              required
              class="mt-1 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              placeholder="Email address"
            />
          </div>
          <div>
            <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
            <input
              id="password"
              v-model="password"
              name="password"
              type="password"
              autocomplete="new-password"
              required
              minlength="8"
              class="mt-1 appearance-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              placeholder="Password (min 8 characters)"
            />
          </div>
          <div>
            <label for="userType" class="block text-sm font-medium text-gray-700">Account Type</label>
            <select
              id="userType"
              v-model="userType"
              name="userType"
              required
              class="mt-1 block w-full px-3 py-2 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
            >
              <option :value="0">Candidate</option>
              <option :value="1">Employer</option>
            </select>
          </div>
        </div>

        <div v-if="error" class="text-red-600 text-sm text-center bg-red-50 p-3 rounded border border-red-200">
          <p class="font-semibold">Error:</p>
          <p>{{ error }}</p>
          <p class="text-xs mt-2 text-gray-600">
            Make sure the backend is running on http://localhost:5259
          </p>
        </div>

        <div>
          <button
            type="submit"
            :disabled="loading"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
          >
            <span v-if="loading">Creating account...</span>
            <span v-else>Register</span>
          </button>
        </div>

        <div class="text-center">
          <NuxtLink to="/login" class="text-indigo-600 hover:text-indigo-500">
            Already have an account? Sign in here
          </NuxtLink>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
const { register } = useAuth()
const router = useRouter()

const email = ref('')
const password = ref('')
const userType = ref(0) // 0 = Candidate, 1 = Employer
const error = ref('')
const loading = ref(false)

const handleRegister = async () => {
  error.value = ''
  loading.value = true
  
  try {
    const result = await register(email.value, password.value, userType.value)
    
    if (result.success) {
      // Redirect based on user type
      const userTypeStr = result.data?.userType?.toLowerCase()
      if (userTypeStr === 'candidate') {
        router.push('/candidate')
      } else if (userTypeStr === 'employer') {
        router.push('/employer')
      } else {
        router.push('/')
      }
    } else {
      error.value = result.error || 'Registration failed'
    }
  } catch (err) {
    error.value = 'An unexpected error occurred'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
</style>