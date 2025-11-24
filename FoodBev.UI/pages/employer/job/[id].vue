<template>
  <section class="page">
    <div class="page__content">
      <h1>Job Details</h1>
      <p>Currently viewing posting <strong>#{{ jobId }}</strong>. Use this area to manage applicants and update the listing.</p>
    </div>
  </section>
</template>

<script setup>
import { computed } from 'vue'

const route = useRoute()
const jobId = computed(() => route.params.id)
</script>

<style scoped>
.page {
  min-height: 70vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 2rem;
}

.page__content {
  text-align: center;
  max-width: 520px;
}

h1 {
  font-size: 2rem;
  margin-bottom: 0.75rem;
}

p {
  color: #4b5563;
}

strong {
  color: #111827;
}
</style>
