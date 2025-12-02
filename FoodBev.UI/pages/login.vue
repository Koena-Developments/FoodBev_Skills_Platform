<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8">
      <div class="text-center">
        <Logo :show-image="true" :show-text="true" class="justify-center mb-6" />
        <h2 class="mt-6 text-center text-3xl font-extrabold text-foodbev-blue">
          Sign in to your account
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          FoodBev SETA Skills Platform
        </p>
      </div>
      <form class="mt-8 space-y-6" @submit.prevent="handleLogin">
        <div class="rounded-md shadow-sm -space-y-px">
          <div>
            <label for="email" class="sr-only">Email address</label>
            <input
              id="email"
              v-model="email"
              name="email"
              type="email"
              autocomplete="email"
              required
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-foodbev-blue focus:border-foodbev-blue focus:z-10 sm:text-sm"
              placeholder="Email address"
            />
          </div>
          <div>
            <label for="password" class="sr-only">Password</label>
            <input
              id="password"
              v-model="password"
              name="password"
              type="password"
              autocomplete="current-password"
              required
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-foodbev-blue focus:border-foodbev-blue focus:z-10 sm:text-sm"
              placeholder="Password"
            />
          </div>
        </div>

        <div v-if="error" class="text-foodbev-red text-sm text-center bg-red-50 p-3 rounded border border-red-200">
          {{ error }}
        </div>

        <div>
          <button
            type="submit"
            :disabled="loading"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-foodbev-blue hover:bg-opacity-90 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-foodbev-blue disabled:opacity-50 transition"
          >
            <span v-if="loading">Signing in...</span>
            <span v-else>Sign in</span>
          </button>
        </div>

        <div class="text-center">
          <NuxtLink to="/register" class="text-foodbev-blue hover:text-opacity-80 font-medium">
            Don't have an account? Register here
          </NuxtLink>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
const { login } = useAuth()
const router = useRouter()

const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

const handleLogin = async () => {
  error.value = ''
  loading.value = true
  
  try {
    const result = await login(email.value, password.value)
    
    if (result.success) {
      // Redirect based on user type
      const userType = result.data?.userType?.toLowerCase()
      if (userType === 'candidate') {
        router.push('/candidate')
      } else if (userType === 'employer') {
        router.push('/employer')
      } else if (userType === 'admin') {
        router.push('/admin')
      } else {
        router.push('/')
      }
    } else {
      error.value = result.error || 'Login failed'
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