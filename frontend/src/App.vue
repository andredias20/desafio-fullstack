<script setup lang="ts">
import { RouterView, RouterLink } from 'vue-router'
</script>

<template>
  <div>
    <nav class="bg-white shadow">
      <div class="max-w-3xl mx-auto px-4 py-3 flex gap-6">
        <RouterLink
          to="/"
          class="text-gray-600 hover:text-blue-600 font-medium transition-colors"
          active-class="text-blue-600 border-b-2 border-blue-600"
        >
          Home
        </RouterLink>
        <RouterLink
          to="/history"
          class="text-gray-600 hover:text-blue-600 font-medium transition-colors"
          active-class="text-blue-600 border-b-2 border-blue-600"
        >
          Histórico
        </RouterLink>
      </div>
    </nav>
    <RouterView />
  </div>
</template>

<style scoped></style>
