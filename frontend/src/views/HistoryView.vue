<script setup lang="ts">
import { onMounted } from 'vue'
import { useTemperatureStore } from '@/stores/temperatureStore'
import HistoryList from '@/components/HistoryList.vue'
import HistoryChart from '@/components/HistoryChart.vue'

const store = useTemperatureStore()

onMounted(() => {
  if (store.lastSearchedCity) {
    store.fetchHistory(store.lastSearchedCity)
  }
})
</script>

<template>
  <div class="min-h-screen bg-gray-100 flex flex-col items-center py-12">
    <div class="w-full max-w-3xl flex flex-col gap-6 px-4">
      <h1 class="text-2xl font-bold text-gray-700">Historico de Temperaturas</h1>
      <HistoryList />
      <HistoryChart />
    </div>
  </div>
</template>

<style scoped></style>
