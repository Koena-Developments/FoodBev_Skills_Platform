<template>
  <div class="flex items-center space-x-3">
    <!-- Logo Image - FoodBev SETA Official Logo -->
    <div v-if="showImage" class="flex-shrink-0">
      <img
        v-if="logoUrl && !imageError"
        :src="logoUrl"
        :alt="altText"
        :class="logoSizeClasses"
        @error="onImageError"
      />
      <div
        v-else
        :class="[logoSizeClasses, 'bg-foodbev-blue flex items-center justify-center rounded px-3']"
      >
        <span class="text-white font-bold text-sm whitespace-nowrap">FoodBev SETA</span>
      </div>
    </div>
    <!-- Text Logo -->
    <div v-if="showText" class="flex flex-col">
      <span class="text-foodbev-blue font-bold text-lg leading-tight">FoodBev</span>
      <span class="text-foodbev-red font-semibold text-xs leading-tight">SETA</span>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
  showImage: {
    type: Boolean,
    default: true
  },
  showText: {
    type: Boolean,
    default: false
  },
  logoUrl: {
    type: String,
    default: '/foodbev-logo.png' // Official FoodBev SETA logo path
  },
  altText: {
    type: String,
    default: 'FoodBev SETA Logo'
  },
  size: {
    type: String,
    default: 'md' // sm, md, lg, xl
  }
})

const imageError = ref(false)

const logoSizeClasses = computed(() => {
  const sizes = {
    sm: 'h-8 w-auto',
    md: 'h-10 w-auto',
    lg: 'h-12 w-auto',
    xl: 'h-16 w-auto'
  }
  return sizes[props.size] || sizes.md
})

const onImageError = () => {
  // Fallback to text logo if image fails to load
  imageError.value = true
  console.warn('FoodBev SETA logo image failed to load, using text fallback')
}
</script>

<style scoped>
/* Additional styling if needed */
</style>

