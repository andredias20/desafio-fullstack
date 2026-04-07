<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { Chart } from 'chart.js/auto'
import { useTemperatureStore } from '@/stores/temperatureStore'
const store = useTemperatureStore()
const chartCanvas = ref<HTMLCanvasElement | null>(null)
let chartInstance: Chart | null = null

onMounted(() => {
  chartInstance = new Chart(chartCanvas.value!, {
    type: 'line',
    data: {
      labels: store.history.map((r) => new Date(r.recordedAt).toLocaleDateString('pt-BR')),
      datasets: [
        {
          label: 'Temperatura (°C)',
          data: store.history.map((r) => r.temperatureCelsius),
          borderColor: '#2563eb',
          tension: 0.3,
        },
      ],
    },
    options: { responsive: true },
  })
})

watch(
  () => store.history,
  (newHistory) => {
    if (!chartInstance) return
    chartInstance.data.labels = newHistory.map((r) =>
      new Date(r.recordedAt).toLocaleDateString('pt-BR'),
    )
    if (chartInstance.data.datasets[0]) {
      chartInstance.data.datasets[0].data = newHistory.map((r) => r.temperatureCelsius)
    }
    chartInstance.update()
  },
)
</script>

<template>
  <div class="bg-white rounded-xl shadow p-6">
    <canvas ref="chartCanvas"></canvas>
  </div>
</template>
