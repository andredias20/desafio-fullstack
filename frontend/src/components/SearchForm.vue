<script setup lang="ts">
import { ref } from 'vue'
import { useTemperatureStore } from '@/stores/temperatureStore'

const store = useTemperatureStore()
const byCoordinates = ref(false)
const cityName = ref('')
const latitude = ref<number | null>(null)
const longitude = ref<number | null>(null)
const validationError = ref<string | null>(null)

async function handleSubmit() {
  if (!byCoordinates.value) {
    if (cityName.value.trim() === '') {
      validationError.value = 'Informe o nome da cidade.'
      return
    }
    validationError.value = null
    await store.registerByCity(cityName.value)
  } else {
    if (latitude.value === null || longitude.value === null) {
      validationError.value = 'Informe latitude e longitude.'
      return
    }
    validationError.value = null
    await store.registerByCoordinates(latitude.value, longitude.value)
  }
}
</script>

<template>
  <form @submit.prevent="handleSubmit" class="flex flex-col gap-3">
    <div class="flex items-center gap-3">
      <span
        :class="!byCoordinates ? 'text-blue-600 font-semibold' : 'text-gray-400'"
        class="text-sm"
        >Cidade</span
      >
      <button
        type="button"
        @click="byCoordinates = !byCoordinates"
        :class="byCoordinates ? 'bg-blue-600' : 'bg-gray-300'"
        class="relative w-12 h-6 rounded-full transition-colors"
      >
        <span
          :class="byCoordinates ? 'translate-x-7' : 'translate-x-1'"
          class="absolute top-1 w-4 h-4 bg-white rounded-full shadow transition-transform block"
        />
      </button>
      <span :class="byCoordinates ? 'text-blue-600 font-semibold' : 'text-gray-400'" class="text-sm"
        >Coordenadas</span
      >
    </div>

    <div class="flex gap-2">
      <template v-if="!byCoordinates">
        <input
          v-model="cityName"
          type="text"
          placeholder="Nome da cidade"
          class="border rounded-lg px-4 py-2 flex-1 focus:outline-none focus:ring-2 focus:ring-blue-400"
        />
      </template>
      <template v-else>
        <input
          v-model.number="latitude"
          type="number"
          step="any"
          placeholder="Latitude"
          class="border rounded-lg px-4 py-2 flex-1 focus:outline-none focus:ring-2 focus:ring-blue-400"
        />
        <input
          v-model.number="longitude"
          type="number"
          step="any"
          placeholder="Longitude"
          class="border rounded-lg px-4 py-2 flex-1 focus:outline-none focus:ring-2 focus:ring-blue-400"
        />
      </template>
      <button
        type="submit"
        :disabled="store.loading"
        class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        {{ store.loading ? 'Buscando...' : 'Buscar' }}
      </button>
    </div>

    <p
      v-if="validationError"
      class="text-amber-600 text-sm bg-amber-50 border border-amber-200 rounded-lg px-3 py-2"
    >
      {{ validationError }}
    </p>
    <p
      v-if="store.error"
      class="text-red-500 text-sm bg-red-50 border border-red-200 rounded-lg px-3 py-2"
    >
      {{ store.error }}
    </p>
  </form>
</template>
