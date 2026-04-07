import { ref } from 'vue'
import {defineStore} from 'pinia'
import type { TemperatureRecord, RegisterTemperatureResponse } from '@/types/temperature'
import { getHistoryByCityApi, registerByCityApi, registerByCoordinatesApi } from '@/services/api'

export const useTemperatureStore = defineStore('temperature', () => {

  const history = ref<TemperatureRecord[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const lastTemperature = ref<RegisterTemperatureResponse | null>(null)
  const lastSearchedCity = ref<string | null>(null)

  async function registerByCity(cityName: string) {
    loading.value = true
    error.value = null
    lastSearchedCity.value = cityName
    try {
      lastTemperature.value = await registerByCityApi(cityName)
    } catch (e) {
      error.value = 'Erro ao buscar temperatura'
    } finally {
      loading.value = false
    }
  }

  async function registerByCoordinates(latitude: number, longitude: number) {
    loading.value = true
    error.value = null
    try {
      lastTemperature.value = await registerByCoordinatesApi(latitude, longitude)
    } catch (e) {
      error.value = 'Erro ao buscar temperatura'
    } finally {
      loading.value = false
    }
  }

  async function fetchHistory(cityName: string): Promise<void> {
    loading.value = true
    error.value = null
    lastSearchedCity.value = cityName
    try {
      history.value = await getHistoryByCityApi(cityName)
    } catch (e) {
      error.value = 'Erro ao buscar historico'
    } finally {
      loading.value = false
    }
  }

  return {
    lastTemperature,
    lastSearchedCity,
    history,
    loading,
    error,
    registerByCity,
    registerByCoordinates,
    fetchHistory,
  }
})
