<template>
  <div class="min-h-screen bg-gray-50 p-6">
    <!-- Header -->
    <header class="mb-6">
      <h1 class="text-2xl font-bold text-foodbev-blue">Admin Dashboard</h1>
      <p class="text-sm text-gray-600 mt-1">Overview of platform statistics and activity</p>
    </header>

    <!-- Loading State -->
    <div v-if="loading" class="text-center py-12">
      <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-foodbev-blue"></div>
      <p class="mt-2 text-gray-600">Loading dashboard data...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="bg-red-50 border border-red-200 rounded-lg p-4 mb-6">
      <p class="text-red-800">{{ error }}</p>
      <button
        @click="loadDashboardData"
        class="mt-3 text-sm text-red-600 hover:text-red-800 underline"
      >
        Try again
      </button>
    </div>

    <!-- Stats Cards -->
    <div v-else class="grid grid-cols-1 md:grid-cols-4 gap-6 mb-6">
      <!-- Total Students -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <div class="flex justify-between items-start">
          <div>
            <p class="text-sm text-gray-500">Total Candidates</p>
            <p class="text-2xl font-bold text-gray-800">{{ stats.totalStudents || 0 }}</p>
          </div>
          <div class="p-3 bg-foodbev-platinum rounded-full">
            <svg class="w-6 h-6 text-foodbev-blue" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Total Applications -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <div class="flex justify-between items-start">
          <div>
            <p class="text-sm text-gray-500">Total Applications</p>
            <p class="text-2xl font-bold text-gray-800">{{ stats.totalApplications || 0 }}</p>
          </div>
          <div class="p-3 bg-foodbev-platinum rounded-full">
            <svg class="w-6 h-6 text-foodbev-red" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Active Students -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <div class="flex justify-between items-start">
          <div>
            <p class="text-sm text-gray-500">Active Candidates (24h)</p>
            <p class="text-2xl font-bold text-gray-800">{{ stats.activeStudents24h || 0 }}</p>
          </div>
          <div class="p-3 bg-foodbev-platinum rounded-full">
            <svg class="w-6 h-6 text-foodbev-yellow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Funded Companies -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <div class="flex justify-between items-start">
          <div>
            <p class="text-sm text-gray-500">Total Employers</p>
            <p class="text-2xl font-bold text-gray-800">{{ stats.fundedCompanies || 0 }}</p>
          </div>
          <div class="p-3 bg-foodbev-platinum rounded-full">
            <svg class="w-6 h-6 text-foodbev-blue" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
            </svg>
          </div>
        </div>
      </div>
    </div>

    <!-- Charts Row -->
    <div v-if="!loading && !error" class="grid grid-cols-1 lg:grid-cols-3 gap-6 mb-6">
      <!-- Application Trends -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <h2 class="text-lg font-semibold mb-4">Application Trends (Last 30 Days)</h2>
        <div class="h-64">
          <ClientOnly>
            <v-chart v-if="trendsChartOptions" :option="trendsChartOptions" autoresize />
            <div v-else class="flex items-center justify-center h-full">
              <p class="text-gray-500">{{ loading ? 'Loading trends...' : 'No data available' }}</p>
            </div>
            <template #fallback>
              <div class="flex items-center justify-center h-full">
                <p class="text-gray-500">Initializing chart...</p>
              </div>
            </template>
          </ClientOnly>
        </div>
      </div>

      <!-- Employer Engagement -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <h2 class="text-lg font-semibold mb-4">Employer Engagement</h2>
        <div class="h-40 flex items-center justify-center bg-gray-50 rounded">
          <p class="text-gray-500">Chart coming soon</p>
        </div>
      </div>

      <!-- Revenue & Funding -->
      <div class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <h2 class="text-lg font-semibold mb-4">Funding Overview</h2>
        <div class="h-40 flex items-center justify-center bg-gray-50 rounded">
          <p class="text-gray-500">Chart coming soon</p>
        </div>
      </div>
    </div>

    <!-- ✅ South African Map Section (Interactive with ECharts) -->
    <div v-if="!loading && !error" class="bg-white p-6 rounded-xl shadow-sm border border-gray-200 mb-6">
      <h2 class="text-lg font-semibold mb-4">Candidate Demographics by Province</h2>
      <div class="h-64 bg-gray-50 rounded">
        <ClientOnly>
          <v-chart v-if="chartOptions" :option="chartOptions" autoresize />
          <template #fallback>
            <div class="flex items-center justify-center h-full">
              <p class="text-gray-500">Loading map data...</p>
            </div>
          </template>
        </ClientOnly>
      </div>
      <p class="text-xs text-gray-500 mt-2 text-center">Hover over provinces to view candidate counts</p>
    </div>

    <!-- Recent Activity Table -->
    <div v-if="!loading && !error" class="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
      <h2 class="text-lg font-semibold mb-4">Recent Activity</h2>
      <div v-if="recentActivity.length === 0" class="text-center py-8 text-gray-500">
        <p>No recent activity</p>
      </div>
      <div v-else class="overflow-x-auto">
        <table class="w-full text-sm">
          <thead>
            <tr class="border-b">
              <th class="py-2 text-left">Type</th>
              <th class="py-2 text-left">Description</th>
              <th class="py-2 text-left">Date</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(activity, index) in recentActivity" :key="index" class="border-b">
              <td class="py-2">
                <span class="px-2 py-1 text-xs rounded bg-foodbev-platinum text-foodbev-blue font-medium">{{ activity.type }}</span>
              </td>
              <td class="py-2">{{ activity.description }}</td>
              <td class="py-2">{{ formatDate(activity.date) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
// Page meta must be at the top
definePageMeta({
  layout: 'admin'
})

// Imports
import { ref, onMounted, computed } from 'vue'
import VChart from 'vue-echarts'
import * as echarts from 'echarts'

const loading = ref(true)
const error = ref(null)
const stats = ref({})
const demographics = ref([])
const recentActivity = ref([])
const applicationTrends = ref([])

// Initialize composable functions (will be set in onMounted for client-side only)
let getDashboardStats = null
let getDemographics = null
let getRecentActivity = null
let getApplicationTrends = null

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

// Register the map with ECharts (client-side only)
if (process.client) {
  echarts.registerMap('SouthAfrica', saGeo)
}

// Application Trends Chart Options
const trendsChartOptions = computed(() => {
  // Always return chart options, even with empty data
  if (!applicationTrends.value || applicationTrends.value.length === 0) {
    // Return empty chart configuration
    return {
      title: {
        text: 'No Data Available',
        left: 'center',
        top: 'middle',
        textStyle: {
          color: '#999',
          fontSize: 14
        }
      },
      xAxis: { type: 'category', data: [] },
      yAxis: { type: 'value', name: 'Applications' },
      series: [{ type: 'line', data: [] }]
    }
  }

  const dates = applicationTrends.value.map(t => {
    const dateStr = t.date || t.Date
    if (!dateStr) return ''
    try {
      const date = new Date(dateStr)
      if (isNaN(date.getTime())) return dateStr
      return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' })
    } catch {
      return dateStr
    }
  }).filter(d => d !== '')

  const counts = applicationTrends.value.map(t => {
    const count = t.count ?? t.Count ?? 0
    return typeof count === 'number' ? count : parseInt(count) || 0
  })

  if (dates.length === 0 || counts.length === 0) {
    return {
      title: {
        text: 'No Data Available',
        left: 'center',
        top: 'middle',
        textStyle: {
          color: '#999',
          fontSize: 14
        }
      },
      xAxis: { type: 'category', data: [] },
      yAxis: { type: 'value', name: 'Applications' },
      series: [{ type: 'line', data: [] }]
    }
  }

  return {
    tooltip: {
      trigger: 'axis',
      backgroundColor: 'rgba(0,0,0,0.7)',
      textStyle: { color: '#fff' },
      formatter: (params) => {
        const param = params[0]
        return `${param.name}<br/>Applications: ${param.value}`
      }
    },
    grid: {
      left: '3%',
      right: '4%',
      bottom: '3%',
      top: '10%',
      containLabel: true
    },
    xAxis: {
      type: 'category',
      boundaryGap: false,
      data: dates,
      axisLine: {
        lineStyle: {
          color: '#221F72' // FoodBev Blue
        }
      }
    },
    yAxis: {
      type: 'value',
      name: 'Applications',
      axisLine: {
        lineStyle: {
          color: '#221F72' // FoodBev Blue
        }
      },
      splitLine: {
        lineStyle: {
          color: '#D3D6D8' // FoodBev Platinum
        }
      }
    },
    series: [
      {
        name: 'Applications',
        type: 'line',
        smooth: true,
        data: counts,
        itemStyle: {
          color: '#221F72' // FoodBev Blue
        },
        areaStyle: {
          color: {
            type: 'linear',
            x: 0,
            y: 0,
            x2: 0,
            y2: 1,
            colorStops: [
              { offset: 0, color: 'rgba(34, 31, 114, 0.3)' }, // FoodBev Blue with opacity
              { offset: 1, color: 'rgba(34, 31, 114, 0.05)' }
            ]
          }
        },
        lineStyle: {
          width: 3,
          color: '#221F72' // FoodBev Blue
        }
      }
    ]
  }
})

// Chart options (computed from demographics data)
const chartOptions = computed(() => {
  const maxValue = demographics.value.length > 0 
    ? Math.max(...demographics.value.map(d => d.count))
    : 1000

  return {
    tooltip: {
      trigger: 'item',
      formatter: '{b}: {c} candidates',
      backgroundColor: 'rgba(0,0,0,0.7)',
      textStyle: { color: '#fff' },
    },
    visualMap: {
      min: 0,
      max: maxValue || 1000,
      text: ['High', 'Low'],
      realtime: false,
      calculable: true,
      inRange: {
        color: ['#D3D6D8', '#221F72'], // FoodBev Platinum to FoodBev Blue
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
            areaColor: '#D3D6D8', // FoodBev Platinum
            borderColor: '#221F72', // FoodBev Blue
            borderWidth: 1,
          },
          emphasis: {
            label: {
              show: true,
              fontSize: 12,
              fontWeight: 'bold',
            },
            itemStyle: {
              areaColor: '#221F72', // FoodBev Blue
            },
          },
        data: demographics.value.map(d => ({
          name: d.province,
          value: d.count
        })),
      },
    ],
  }
})

const loadDashboardData = async () => {
  if (!getDashboardStats || !getDemographics || !getRecentActivity || !getApplicationTrends) {
    return // Wait for composables to be initialized
  }

  loading.value = true
  error.value = null

  try {
    // Load stats
    const statsResult = await getDashboardStats()
    if (statsResult.success) {
      stats.value = statsResult.data
    } else {
      error.value = statsResult.error
    }

    // Load demographics
    const demographicsResult = await getDemographics()
    if (demographicsResult.success) {
      demographics.value = demographicsResult.data || []
    }

    // Load recent activity
    const activityResult = await getRecentActivity(10)
    if (activityResult.success) {
      recentActivity.value = activityResult.data || []
    }

    // Load application trends (last 30 days)
    const trendsResult = await getApplicationTrends(30)
    if (trendsResult.success) {
      applicationTrends.value = trendsResult.data || []
      console.log('✅ Loaded application trends:', applicationTrends.value)
      console.log('✅ Trends count:', applicationTrends.value.length)
    } else {
      console.error('❌ Failed to load application trends:', trendsResult.error)
      // Set empty array so chart still renders
      applicationTrends.value = []
    }
  } catch (err) {
    error.value = 'Failed to load dashboard data'
    console.error('Dashboard load error:', err)
  } finally {
    loading.value = false
  }
}

const formatDate = (dateString) => {
  if (!dateString) return 'N/A'
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

onMounted(() => {
  // Initialize composables on client side only
  const adminComposables = useAdmin()
  getDashboardStats = adminComposables.getDashboardStats
  getDemographics = adminComposables.getDemographics
  getRecentActivity = adminComposables.getRecentActivity
  getApplicationTrends = adminComposables.getApplicationTrends
  
  loadDashboardData()
})
</script>

<style scoped>
/* Ensure ECharts chart fills container */
.v-chart {
  width: 100%;
  height: 100%;
}
</style>