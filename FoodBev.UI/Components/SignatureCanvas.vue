<template>
  <div class="signature-canvas-container">
    <div class="bg-white border-2 border-gray-300 rounded-lg p-4">
      <div class="flex justify-between items-center mb-4">
        <h3 class="text-lg font-semibold text-gray-900">Digital Signature</h3>
        <button
          v-if="signatureData"
          @click="clearSignature"
          class="text-sm text-red-600 hover:text-red-800 underline"
        >
          Clear
        </button>
      </div>
      
      <canvas
        ref="canvasRef"
        :width="canvasWidth"
        :height="canvasHeight"
        class="border border-gray-300 rounded cursor-crosshair bg-white"
        @mousedown="startDrawing"
        @mousemove="draw"
        @mouseup="stopDrawing"
        @mouseleave="stopDrawing"
        @touchstart="startDrawingTouch"
        @touchmove="drawTouch"
        @touchend="stopDrawing"
      ></canvas>
      
      <p class="text-sm text-gray-600 mt-2 text-center">
        {{ signatureData ? 'Signature captured' : 'Please sign above' }}
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'

const props = defineProps({
  canvasWidth: {
    type: Number,
    default: 600
  },
  canvasHeight: {
    type: Number,
    default: 200
  }
})

const emit = defineEmits(['signature-change'])

const canvasRef = ref(null)
const signatureData = ref(null)
const isDrawing = ref(false)
const lastX = ref(0)
const lastY = ref(0)

let ctx = null

onMounted(() => {
  if (canvasRef.value) {
    ctx = canvasRef.value.getContext('2d')
    ctx.strokeStyle = '#000000'
    ctx.lineWidth = 2
    ctx.lineCap = 'round'
    ctx.lineJoin = 'round'
  }
})

const startDrawing = (e) => {
  isDrawing.value = true
  const rect = canvasRef.value.getBoundingClientRect()
  lastX.value = e.clientX - rect.left
  lastY.value = e.clientY - rect.top
}

const startDrawingTouch = (e) => {
  e.preventDefault()
  isDrawing.value = true
  const rect = canvasRef.value.getBoundingClientRect()
  const touch = e.touches[0]
  lastX.value = touch.clientX - rect.left
  lastY.value = touch.clientY - rect.top
}

const draw = (e) => {
  if (!isDrawing.value || !ctx) return
  
  const rect = canvasRef.value.getBoundingClientRect()
  const currentX = e.clientX - rect.left
  const currentY = e.clientY - rect.top
  
  ctx.beginPath()
  ctx.moveTo(lastX.value, lastY.value)
  ctx.lineTo(currentX, currentY)
  ctx.stroke()
  
  lastX.value = currentX
  lastY.value = currentY
  
  // Update signature data
  updateSignatureData()
}

const drawTouch = (e) => {
  if (!isDrawing.value || !ctx) return
  e.preventDefault()
  
  const rect = canvasRef.value.getBoundingClientRect()
  const touch = e.touches[0]
  const currentX = touch.clientX - rect.left
  const currentY = touch.clientY - rect.top
  
  ctx.beginPath()
  ctx.moveTo(lastX.value, lastY.value)
  ctx.lineTo(currentX, currentY)
  ctx.stroke()
  
  lastX.value = currentX
  lastY.value = currentY
  
  // Update signature data
  updateSignatureData()
}

const stopDrawing = () => {
  isDrawing.value = false
  updateSignatureData()
}

const updateSignatureData = () => {
  if (canvasRef.value) {
    signatureData.value = canvasRef.value.toDataURL('image/png')
    emit('signature-change', signatureData.value)
  }
}

const clearSignature = () => {
  if (ctx && canvasRef.value) {
    ctx.clearRect(0, 0, props.canvasWidth, props.canvasHeight)
    signatureData.value = null
    emit('signature-change', null)
  }
}

// Expose methods for parent component
defineExpose({
  clearSignature,
  getSignature: () => signatureData.value,
  hasSignature: () => !!signatureData.value
})
</script>

<style scoped>
.signature-canvas-container {
  width: 100%;
}

canvas {
  display: block;
  width: 100%;
  max-width: 100%;
  height: auto;
}
</style>

