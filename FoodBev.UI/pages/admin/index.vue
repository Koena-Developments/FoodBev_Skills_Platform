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

    <!-- ✅ South African Map Section (Interactive with ECharts) -->
    <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200 mb-6">
      <h2 class="text-lg font-semibold mb-4">Student Demographics by Province</h2>
      <div class="h-64 bg-gray-50 rounded">
        <v-chart :option="chartOptions" autoresize />
      </div>
      <p class="text-xs text-gray-500 mt-2 text-center">Hover over provinces to view student counts</p>
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
import { ref } from 'vue'
import VChart from 'vue-echarts'
import * as echarts from 'echarts'

// ===== South Africa GeoJSON (simplified for demo) =====
const saGeo = {
  type: 'FeatureCollection',
  features: [
    {
      type: 'Feature',
      properties: { name: 'Gauteng' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[27.8, -25.8], [28.5, -25.8], [28.5, -26.5], [27.8, -26.5], [27.8, -25.8]]],
      },
    },
    {
      type: 'Feature',
      properties: { name: 'Western Cape' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[18.5, -33.5], [19.5, -33.5], [19.5, -34.5], [18.5, -34.5], [18.5, -33.5]]],
      },
    },
    {
      type: 'Feature',
      properties: { name: 'KwaZulu-Natal' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[30.5, -28.5], [31.5, -28.5], [31.5, -29.5], [30.5, -29.5], [30.5, -28.5]]],
      },
    },
    {
      type: 'Feature',
      properties: { name: 'Eastern Cape' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[25.5, -32.5], [26.5, -32.5], [26.5, -33.5], [25.5, -33.5], [25.5, -32.5]]],
      },
    },
    {
      type: 'Feature',
      properties: { name: 'Free State' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[27.5, -28.5], [28.5, -28.5], [28.5, -29.5], [27.5, -29.5], [27.5, -28.5]]],
      },
    },
    {
      type: 'Feature',
      properties: { name: 'Northern Cape' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[20.5, -28.5], [21.5, -28.5], [21.5, -29.5], [20.5, -29.5], [20.5, -28.5]]],
      },
    },
    {
      type: 'Feature',
      properties: { name: 'Limpopo' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[28.5, -23.5], [29.5, -23.5], [29.5, -24.5], [28.5, -24.5], [28.5, -23.5]]],
      },
    },
    {
      type: 'Feature',
      properties: { name: 'Mpumalanga' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[29.5, -25.5], [30.5, -25.5], [30.5, -26.5], [29.5, -26.5], [29.5, -25.5]]],
      },
    },
    {
      type: 'Feature',
      properties: { name: 'North West' },
      geometry: {
        type: 'Polygon',
        coordinates: [[[26.5, -26.5], [27.5, -26.5], [27.5, -27.5], [26.5, -27.5], [26.5, -26.5]]],
      },
    },
  ],
}

// Register the map with ECharts
echarts.registerMap('SouthAfrica', saGeo)

// Mock student data (replace with API later)
const studentData = [
  { name: 'Gauteng', value: 850 },
  { name: 'Western Cape', value: 620 },
  { name: 'KwaZulu-Natal', value: 580 },
  { name: 'Eastern Cape', value: 320 },
  { name: 'Free State', value: 180 },
  { name: 'Northern Cape', value: 90 },
  { name: 'Limpopo', value: 210 },
  { name: 'Mpumalanga', value: 290 },
  { name: 'North West', value: 240 },
]

// Chart options
const chartOptions = ref({
  tooltip: {
    trigger: 'item',
    formatter: '{b}: {c} students',
    backgroundColor: 'rgba(0,0,0,0.7)',
    textStyle: { color: '#fff' },
  },
  visualMap: {
    min: 0,
    max: 1000,
    text: ['High', 'Low'],
    realtime: false,
    calculable: true,
    inRange: {
      color: ['#e6f7ff', '#0050b3'],
    },
    orient: 'horizontal',
    left: 'center',
    bottom: '5px',
  },
  series: [
    {
      type: 'map',
      map: 'SouthAfrica',
      roam: false,
      label: {
        show: true,
        fontSize: 10,
        color: '#333',
      },
      itemStyle: {
        areaColor: '#f0f0f0',
        borderColor: '#666',
        borderWidth: 1,
      },
      emphasis: {
        label: {
          show: true,
          fontSize: 12,
          fontWeight: 'bold',
        },
        itemStyle: {
          areaColor: '#0050b3',
        },
      },
       studentData,
    },
  ],
})
</script>

<style scoped>
/* Ensure ECharts chart fills container */
.v-chart {
  width: 100%;
  height: 100%;
}
</style>