<script setup lang="ts">
import { ref } from 'vue'
import { useTemperatureStore } from '@/stores/temperatureStore'
const store = useTemperatureStore()
const cityName = ref<string>('')
async function handleSearch() {
  await store.fetchHistory(cityName.value)
}
</script>

<template>
  <div class="flex flex-col gap-4">
    <form @submit.prevent="handleSearch">
      <div class="flex gap-2">
        <input
          v-model="cityName"
          type="text"
          placeholder="Nome da cidade"
          class="border rounded-lg px-4 py-2 flex-1 focus:outline-none focus:ring-2 focus:ring-blue-400"
        />
        <button
          type="submit"
          :disabled="store.loading"
          class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {{ store.loading ? 'Buscando...' : 'Buscar' }}
        </button>
      </div>
      <p v-if="store.error" class="text-red-500 text-sm mt-2">{{ store.error }}</p>
    </form>

    <table v-if="store.history.length > 0" class="w-full text-left border-collapse">
      <thead>
        <tr class="border-b bg-gray-50">
          <th class="px-4 py-2 text-gray-600">Cidade / Coordenadas</th>
          <th class="px-4 py-2 text-gray-600">Temperatura</th>
          <th class="px-4 py-2 text-gray-600">Data</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="record in store.history" :key="record.id" class="border-b hover:bg-gray-50">
          <td class="px-4 py-2">
            <span v-if="record.cityName">{{ record.cityName }}</span>
            <span v-else class="text-sm font-mono text-gray-600">{{ record.latitude.toFixed(4) }}, {{ record.longitude.toFixed(4) }}</span>
          </td>
          <td class="px-4 py-2">{{ record.temperatureCelsius }}°C</td>
          <td class="px-4 py-2">{{ new Date(record.recordedAt).toLocaleDateString('pt-BR') }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
