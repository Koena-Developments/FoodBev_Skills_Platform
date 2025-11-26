<template>
  <div class="min-h-screen bg-gray-50 p-8">
    <div class="max-w-6xl mx-auto">
      <h1 class="text-3xl font-bold mb-6">API Endpoints Test Page</h1>
      
      <!-- Auth Section -->
      <div class="bg-white rounded-lg shadow p-6 mb-6">
        <h2 class="text-xl font-semibold mb-4">Authentication</h2>
        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium mb-2">Email</label>
            <input v-model="auth.email" type="email" class="w-full px-3 py-2 border rounded" />
          </div>
          <div>
            <label class="block text-sm font-medium mb-2">Password</label>
            <input v-model="auth.password" type="password" class="w-full px-3 py-2 border rounded" />
          </div>
          <div>
            <label class="block text-sm font-medium mb-2">User Type</label>
            <select v-model.number="auth.userType" class="w-full px-3 py-2 border rounded">
              <option :value="0">Candidate</option>
              <option :value="1">Employer</option>
              <option :value="2">Admin</option>
            </select>
          </div>
          <div class="flex gap-2">
            <button @click="testRegister" class="px-4 py-2 bg-blue-600 text-white rounded">Register</button>
            <button @click="testLogin" class="px-4 py-2 bg-green-600 text-white rounded">Login</button>
          </div>
          <div v-if="authResult" class="p-3 bg-gray-100 rounded text-sm">
            <pre>{{ JSON.stringify(authResult, null, 2) }}</pre>
          </div>
        </div>
      </div>

      <!-- Candidate Endpoints -->
      <div class="bg-white rounded-lg shadow p-6 mb-6">
        <h2 class="text-xl font-semibold mb-4">Candidate Endpoints</h2>
        <div class="grid grid-cols-2 gap-4">
          <button @click="testGetProfile" class="px-4 py-2 bg-blue-500 text-white rounded">Get Profile</button>
          <button @click="testGetJobs" class="px-4 py-2 bg-blue-500 text-white rounded">Get Matching Jobs</button>
          <button @click="testGetApplications" class="px-4 py-2 bg-blue-500 text-white rounded">Get Applications</button>
          <button @click="testGetSavedJobs" class="px-4 py-2 bg-blue-500 text-white rounded">Get Saved Jobs</button>
        </div>
        <div v-if="candidateResult" class="mt-4 p-3 bg-gray-100 rounded text-sm max-h-96 overflow-auto">
          <pre>{{ JSON.stringify(candidateResult, null, 2) }}</pre>
        </div>
      </div>

      <!-- Test Results -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold mb-4">Test Results</h2>
        <div class="space-y-2">
          <div v-for="(test, index) in testResults" :key="index" class="p-3 rounded" :class="test.success ? 'bg-green-50' : 'bg-red-50'">
            <p class="font-medium">{{ test.endpoint }}</p>
            <p class="text-sm" :class="test.success ? 'text-green-700' : 'text-red-700'">
              {{ test.success ? '✓ Success' : '✗ Failed' }}: {{ test.message }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const { login, register } = useAuth()
const { getProfile, getMatchingJobs, getApplications, getSavedJobs } = useCandidate()

const auth = ref({
  email: 'test@example.com',
  password: 'Test1234!',
  userType: 0
})

const authResult = ref(null)
const candidateResult = ref(null)
const testResults = ref([])

const addTestResult = (endpoint, success, message) => {
  testResults.value.unshift({
    endpoint,
    success,
    message,
    timestamp: new Date().toLocaleTimeString()
  })
}

const testRegister = async () => {
  const result = await register(auth.value.email, auth.value.password, auth.value.userType)
  authResult.value = result
  addTestResult('POST /auth/register', result.success, result.success ? 'Registration successful' : result.error)
}

const testLogin = async () => {
  const result = await login(auth.value.email, auth.value.password)
  authResult.value = result
  addTestResult('POST /auth/login', result.success, result.success ? 'Login successful' : result.error)
}

const testGetProfile = async () => {
  const { getProfile: getProfileFn } = useCandidate()
  const result = await getProfileFn()
  candidateResult.value = result
  addTestResult('GET /candidate/profile', result.success, result.success ? 'Profile loaded' : result.error)
}

const testGetJobs = async () => {
  const { getMatchingJobs: getJobsFn } = useCandidate()
  const result = await getJobsFn()
  candidateResult.value = result
  addTestResult('GET /candidate/jobs', result.success, result.success ? `${result.data?.length || 0} jobs found` : result.error)
}

const testGetApplications = async () => {
  const { getApplications: getAppsFn } = useCandidate()
  const result = await getAppsFn()
  candidateResult.value = result
  addTestResult('GET /candidate/applications', result.success, result.success ? `${result.data?.length || 0} applications found` : result.error)
}

const testGetSavedJobs = async () => {
  const { getSavedJobs: getSavedFn } = useCandidate()
  const result = await getSavedFn()
  candidateResult.value = result
  addTestResult('GET /candidate/saved-jobs', result.success, result.success ? `${result.data?.length || 0} saved jobs found` : result.error)
}
</script>

<style scoped>
</style>

