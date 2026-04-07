import axios from 'axios'
import type {
  RegisterByCityRequest,
  RegisterByCoordinatesRequest,
  RegisterTemperatureResponse,
  TemperatureRecord,
} from '@/types/temperature'

const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
})

httpClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

httpClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  },
)

async function registerByCityApi(cityName: string) : Promise<RegisterTemperatureResponse>
{
  const body : RegisterByCityRequest = {cityName};
  const response = await httpClient.post<RegisterTemperatureResponse>('/api/temperature/city', body);
  return response.data;
}

async function registerByCoordinatesApi(latitude: number, longitude: number) : Promise<RegisterTemperatureResponse>
{
  const body: RegisterByCoordinatesRequest = { latitude, longitude };
  const response = await httpClient.post<RegisterTemperatureResponse>('/api/temperature/coordinates', body);
  return response.data;
}

async function getHistoryByCityApi(cityName: string) : Promise<TemperatureRecord[]>
{
  const config = { params: { cityName } }
  const response = await httpClient.get<TemperatureRecord[]>('/api/temperature/history', config)
  return response.data;
}

export { registerByCityApi, registerByCoordinatesApi, getHistoryByCityApi }

