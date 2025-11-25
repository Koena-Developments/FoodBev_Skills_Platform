<template>
  <div class="bg-white font-sans p-6">
    <!-- Header -->
    <header class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-gray-800">Dashboard</h1>
      <div class="flex space-x-4">
        <button class="p-2 bg-gray-100 rounded-full hover:bg-gray-200">
          <i class="fas fa-bell text-gray-600"></i>
        </button>
        <button class="p-2 bg-gray-100 rounded-full hover:bg-gray-200">
          <i class="fas fa-cog text-gray-600"></i>
        </button>
      </div>
    </header>

    <!-- Stats Cards -->
    <div class="grid grid-cols-1 md:grid-cols-4 gap-6 mb-6">
      <!-- Total Students -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <div class="flex justify-between items-start">
          <div>
            <p class="text-sm text-gray-500">Total Students</p>
            <p class="text-2xl font-bold text-gray-800">3,241</p>
            <p class="text-xs text-green-600 mt-1">+12% from last month</p>
          </div>
          <div class="p-3 bg-blue-100 rounded-full">
            <i class="fas fa-users text-blue-600"></i>
          </div>
        </div>
      </div>

      <!-- Total Applications -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <div class="flex justify-between items-start">
          <div>
            <p class="text-sm text-gray-500">Total Applications</p>
            <p class="text-2xl font-bold text-gray-800">4,923</p>
            <p class="text-xs text-green-600 mt-1">+8% from last month</p>
          </div>
          <div class="p-3 bg-green-100 rounded-full">
            <i class="fas fa-file-alt text-green-600"></i>
          </div>
        </div>
      </div>

      <!-- Active Students -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <div class="flex justify-between items-start">
          <div>
            <p class="text-sm text-gray-500">Active Students (24h)</p>
            <p class="text-2xl font-bold text-gray-800">1,876</p>
            <p class="text-xs text-green-600 mt-1">+5% from yesterday</p>
          </div>
          <div class="p-3 bg-yellow-100 rounded-full">
            <i class="fas fa-user-clock text-yellow-600"></i>
          </div>
        </div>
      </div>

      <!-- Funded Companies -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <div class="flex justify-between items-start">
          <div>
            <p class="text-sm text-gray-500">Funded Companies</p>
            <p class="text-2xl font-bold text-gray-800">87</p>
            <p class="text-xs text-green-600 mt-1">+3 new this week</p>
          </div>
          <div class="p-3 bg-purple-100 rounded-full">
            <i class="fas fa-building text-purple-600"></i>
          </div>
        </div>
      </div>
    </div>

    <!-- Charts Row -->
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6 mb-6">
      <!-- Application Trends -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <h2 class="text-lg font-semibold mb-4">Application Trends</h2>
        <div class="h-40 flex items-center justify-center bg-gray-50 rounded">
          <p class="text-gray-500">Line chart placeholder</p>
        </div>
      </div>

      <!-- Employer Engagement -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <h2 class="text-lg font-semibold mb-4">Employer Engagement</h2>
        <div class="h-40 flex items-center justify-center bg-gray-50 rounded">
          <p class="text-gray-500">Bar chart placeholder</p>
        </div>
      </div>

      <!-- Revenue & Funding -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <h2 class="text-lg font-semibold mb-4">Funding Overview</h2>
        <div class="h-40 flex items-center justify-center bg-gray-50 rounded">
          <p class="text-gray-500">Area chart placeholder</p>
        </div>
      </div>
    </div>

    <!-- World Map Section -->
    <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200 mb-6">
      <h2 class="text-lg font-semibold mb-4">Student Demographics by Region</h2>
      <div class="h-64 bg-gray-50 rounded flex items-center justify-center">
        <div class="text-center">
          <i class="fas fa-globe-americas text-4xl text-gray-400 mb-2"></i>
          <p class="text-gray-500">World map placeholder — will show student density by country/region</p>
        </div>
      </div>
    </div>

    <!-- Recent Activity Table -->
    <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
      <h2 class="text-lg font-semibold mb-4">Recent Activity</h2>
      <div class="overflow-x-auto">
        <table class="w-full text-sm">
          <thead>
            <tr class="border-b">
              <th class="py-2 text-left">User</th>
              <th class="py-2 text-left">Action</th>
              <th class="py-2 text-left">Time</th>
            </tr>
          </thead>
          <tbody>
            <tr class="border-b">
              <td class="py-2">John Doe</td>
              <td class="py-2">Applied to Job #123</td>
              <td class="py-2">2 mins ago</td>
            </tr>
            <tr class="border-b">
              <td class="py-2">Jane Smith</td>
              <td class="py-2">Uploaded CV</td>
              <td class="py-2">5 mins ago</td>
            </tr>
            <tr class="border-b">
              <td class="py-2">ABC Corp</td>
              <td class="py-2">Posted New Job</td>
              <td class="py-2">10 mins ago</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
// No script needed for static page
</script>

<style scoped>
/* Optional: Add any component-specific styles here */
</style>