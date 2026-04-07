<script setup lang="ts">
import { useTemperatureStore } from '@/stores/temperatureStore'
import SearchForm from '@/components/SearchForm.vue'
import TemperatureCard from '@/components/TemperatureCard.vue'

const store = useTemperatureStore()
</script>

<template>
  <div class="min-h-screen bg-gray-100 flex flex-col items-center py-12">
    <div class="w-full max-w-lg flex flex-col gap-6 px-4">
      <h1 class="text-2xl font-bold text-gray-700">Registrar Temperatura</h1>
      <SearchForm />
      <TemperatureCard v-if="store.lastTemperature" :record="store.lastTemperature" />
    </div>
  </div>
</template>

<style scoped></style>
