<template>
  <div class="landing">
    <p>Redirecting to candidates…</p>
  </div>
</template>

<script setup>
definePageMeta({
  redirect: '/candidate'
})
</script>

<style scoped>
.landing {
  min-height: 60vh;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 1.125rem;
  color: #1a1a1a;
}
</style>
