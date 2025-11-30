<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Navigation Sidebar -->
    <aside class="fixed left-0 top-0 h-screen w-64 bg-white border-r border-gray-200 shadow-sm z-50 overflow-y-auto">
      <div class="p-6 flex flex-col h-full">
        <div class="flex-shrink-0">
          <h2 class="text-xl font-bold text-gray-900 mb-6">Candidate Portal</h2>
          <nav class="space-y-2">
            <NuxtLink
              to="/candidate"
              class="flex items-center px-4 py-3 text-gray-700 rounded-lg hover:bg-gray-100 transition"
              active-class="bg-blue-50 text-blue-700 font-semibold"
            >
              <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
              </svg>
              Dashboard
            </NuxtLink>
            <NuxtLink
              to="/candidate/profile"
              class="flex items-center px-4 py-3 text-gray-700 rounded-lg hover:bg-gray-100 transition"
              active-class="bg-blue-50 text-blue-700 font-semibold"
            >
              <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
              </svg>
              My Profile
            </NuxtLink>
            <NuxtLink
              to="/candidate/jobs"
              class="flex items-center px-4 py-3 text-gray-700 rounded-lg hover:bg-gray-100 transition"
              active-class="bg-blue-50 text-blue-700 font-semibold"
            >
              <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 13.255A23.931 23.931 0 0112 15c-3.183 0-6.22-.62-9-1.745M16 6V4a2 2 0 00-2-2h-4a2 2 0 00-2 2v2m4 6h.01M5 20h14a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
              </svg>
              Browse Jobs
            </NuxtLink>
            <NuxtLink
              to="/candidate/applications"
              class="flex items-center px-4 py-3 text-gray-700 rounded-lg hover:bg-gray-100 transition"
              active-class="bg-blue-50 text-blue-700 font-semibold"
            >
              <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
              My Applications
            </NuxtLink>
            <NuxtLink
              to="/candidate/agreements"
              class="flex items-center px-4 py-3 text-gray-700 rounded-lg hover:bg-gray-100 transition"
              active-class="bg-blue-50 text-blue-700 font-semibold"
            >
              <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              Agreements
            </NuxtLink>
          </nav>
        </div>
        <div class="mt-auto pt-6 border-t border-gray-200">
          <button
            @click="handleLogout"
            class="w-full flex items-center px-4 py-2 text-red-600 rounded-lg hover:bg-red-50 transition"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
            </svg>
            Logout
          </button>
        </div>
      </div>
    </aside>

    <!-- Main Content -->
    <div class="ml-64">
      <slot />
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'

const router = useRouter()
let logoutFn = null

onMounted(() => {
  // Only access composables on client side
  const { logout } = useAuth()
  logoutFn = logout
})

const handleLogout = async () => {
  if (logoutFn) {
    await logoutFn()
  }
  router.push('/login')
}
</script>

<style scoped>
</style>
