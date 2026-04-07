import axios from 'axios'
import type {
  RegisterByCityRequest,
  RegisterByCoordinatesRequest,
  RegisterTemperatureResponse,
  TemperatureRecord,
} from '@/types/temperature'

const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

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

