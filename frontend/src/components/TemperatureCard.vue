<script setup lang="ts">
import type { RegisterTemperatureResponse } from '@/types/temperature'

const props = defineProps<{
  record: RegisterTemperatureResponse
}>()
</script>

<template>
  <div class="rounded-xl shadow p-6 bg-white max-w-sm">
    <p class="text-sm text-gray-500 uppercase tracking-wide">Temperatura registrada</p>
    <p class="text-4xl font-semibold text-blue-600 mt-2">{{ record.temperatureCelsius }}°C</p>
  </div>
</template>
