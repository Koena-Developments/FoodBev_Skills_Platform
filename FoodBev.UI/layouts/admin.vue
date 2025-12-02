<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Navigation Sidebar -->
    <aside class="fixed left-0 top-0 h-screen w-64 bg-white border-r border-foodbev-platinum shadow-sm z-50 overflow-y-auto">
      <div class="p-6 flex flex-col h-full">
        <div class="flex-shrink-0 mb-6">
          <Logo :show-image="true" :show-text="true" class="mb-4" />
          <h2 class="text-lg font-semibold text-foodbev-blue mt-4">Admin Panel</h2>
        </div>
        <nav class="space-y-2">
            <NuxtLink
              to="/admin"
              class="flex items-center px-4 py-3 text-gray-700 rounded-lg hover:bg-foodbev-platinum transition"
              active-class="bg-foodbev-blue text-white font-semibold"
            >
              <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
              </svg>
              Dashboard
            </NuxtLink>
            <NuxtLink
              to="/admin/agreements"
              class="flex items-center px-4 py-3 text-gray-700 rounded-lg hover:bg-foodbev-platinum transition"
              active-class="bg-foodbev-blue text-white font-semibold"
            >
              <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
              Tripartite Agreements
            </NuxtLink>
        </nav>
        <div class="mt-auto pt-6 border-t border-gray-200">
          <button
            @click="handleLogout"
            class="w-full flex items-center px-4 py-2 text-foodbev-red rounded-lg hover:bg-red-50 transition"
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
  router.push('/admin/login')
}
</script>

<style scoped>
</style>
