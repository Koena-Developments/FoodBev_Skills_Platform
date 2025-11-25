<template>
  <div class="bg-white font-sans">
    <!-- Header -->
    <header class="bg-white p-4 shadow-sm flex items-center justify-between">
      <button @click="$router.back()" class="text-gray-600 text-xl">
        <i class="fas fa-arrow-left"></i>
      </button>
      <div class="flex-grow mx-4">
        <input type="text" placeholder="Search" class="w-full p-2 border rounded-full text-sm">
      </div>
      <button class="text-gray-600 text-xl">
        <i class="fas fa-cog"></i>
      </button>
    </header>

    <!-- Profile Banner -->
    <div class="relative h-28 bg-gradient-to-r from-blue-100 to-indigo-100">
      <div class="absolute bottom-0 left-0 right-0 h-12 bg-white rounded-t-full"></div>
      <div class="absolute bottom-0 left-4 right-4">
        <img src="https://via.placeholder.com/150" alt="Banner" class="w-full h-12 object-cover rounded-t-full">
      </div>
    </div>

    <!-- Profile Picture & Info -->
    <div class="px-4 pt-12 pb-6 relative">
      <div class="absolute top-0 left-4 w-28 h-28">
        <img src="https://via.placeholder.com/150" alt="Profile" class="rounded-full w-28 h-28 object-cover border-4 border-white shadow-md">
      </div>
      <div class="ml-32 mt-4">
        <h1 class="text-2xl font-bold">Kagiso Mojela</h1>
        <p class="text-gray-600">University of Pretoria/Universiteit van Pretoria</p>
        <p class="text-gray-600">Chloorkop Primary School · University of Pretoria</p>
        <p class="text-gray-600">City of Johannesburg, Gauteng, South Africa</p>
        <p class="text-blue-600 mt-2">15 connections</p>
      </div>
      <button class="absolute top-4 right-4 text-gray-600 text-xl">
        <i class="fas fa-pen"></i>
      </button>
    </div>

    <!-- Action Buttons -->
    <div class="px-4 pb-4 flex space-x-4">
      <button class="bg-blue-600 text-white px-6 py-2 rounded-full text-sm font-medium">
        Uploads Docs
      </button>
      <button class="border border-blue-600 text-blue-600 px-6 py-2 rounded-full text-sm font-medium">
        Add section
      </button>
      <button class="border border-gray-300 text-gray-600 px-4 py-2 rounded-full text-sm">
        <i class="fas fa-ellipsis-h"></i>
      </button>
    </div>

    <!-- Enhance Profile Button -->
    <div class="px-4 pb-4">
      <button class="w-full border border-blue-600 text-blue-600 py-2 rounded-full text-sm font-medium">
        Enhance profile
      </button>
    </div>

    <!-- Uploads Docs Section -->
    <div class="px-4 pb-6 bg-gray-50 rounded-lg shadow-sm m-4">
      <div class="flex justify-between items-center mb-2">
        <h2 class="font-semibold">Uploads Docs</h2>
        <button class="text-gray-600 text-xl">
          <i class="fas fa-pen"></i>
        </button>
      </div>
      <div class="space-y-2">
        <div class="bg-white p-3 rounded border border-gray-200">
          <div class="flex items-center space-x-3">
            <i class="far fa-file-pdf text-red-500 text-xl"></i>
            <div>
              <p class="font-medium">Resume.pdf</p>
              <p class="text-xs text-gray-500">Uploaded 2 days ago</p>
            </div>
          </div>
        </div>
        <div class="bg-white p-3 rounded border border-gray-200">
          <div class="flex items-center space-x-3">
            <i class="far fa-file-word text-blue-500 text-xl"></i>
            <div>
              <p class="font-medium">CoverLetter.docx</p>
              <p class="text-xs text-gray-500">Uploaded 1 week ago</p>
            </div>
          </div>
        </div>
        <div class="bg-white p-3 rounded border border-gray-200">
          <div class="flex items-center space-x-3">
            <i class="far fa-file-image text-yellow-500 text-xl"></i>
            <div>
              <p class="font-medium">Certification.jpg</p>
              <p class="text-xs text-gray-500">Uploaded 3 weeks ago</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Suggested for You Section -->
    <div class="px-4 pb-4">
      <h2 class="text-xl font-bold">Suggested for you</h2>
      <p class="text-gray-500 text-sm"><i class="far fa-eye mr-1"></i>Private to you</p>
    </div>

    <!-- Write Summary Card -->
    <div class="px-4 pb-20">
      <div class="bg-white p-4 rounded-lg border border-gray-200">
        <div class="flex items-center space-x-3 mb-2">
          <i class="far fa-sticky-note text-gray-400 text-xl"></i>
          <span class="text-gray-600">Write a summary to highlight</span>
        </div>
        <div class="h-20 bg-gray-100 rounded border border-dashed border-gray-300"></div>
      </div>
    </div>

    <!-- Bottom Navigation -->
    <nav class="fixed bottom-0 left-0 right-0 bg-white border-t border-gray-200 flex justify-around py-2">
      <button class="text-gray-600 text-xl">
        <i class="fas fa-home"></i>
      </button>
      <button class="text-gray-600 text-xl">
        <i class="fas fa-search"></i>
      </button>
      <button class="text-gray-600 text-xl">
        <i class="fas fa-briefcase"></i>
      </button>
      <button class="text-gray-600 text-xl">
        <i class="fas fa-user"></i>
      </button>
    </nav>
  </div>
</template>

<script setup>
// No script needed for static page
</script>

<style scoped>

</style>