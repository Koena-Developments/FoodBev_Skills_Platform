<template>
  <section class="page">
    <div class="page__content">
      <h1>{{ pageTitle }}</h1>
      <p>Track the progress of each job application and follow up with employers.</p>
    </div>
  </section>
</template>

<script setup>
const pageTitle = 'Candidate Applications'
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
</style>
