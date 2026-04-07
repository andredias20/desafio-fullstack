import axios from 'axios'
import type { AuthRequest, AuthResponse } from '@/types/auth'

const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
})

export async function registerApi(payload: AuthRequest): Promise<void> {
  await httpClient.post('/api/auth/register', payload)
}

export async function loginApi(payload: AuthRequest): Promise<AuthResponse> {
  const response = await httpClient.post<AuthResponse>('/api/auth/login', payload)
  return response.data
}
