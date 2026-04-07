import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { loginApi, registerApi } from '@/services/authApi'
import type { AuthRequest } from '@/types/auth'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'))
  const loading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => token.value !== null)

  function getToken(): string | null {
    return token.value
  }

  async function login(payload: AuthRequest) {
    loading.value = true
    error.value = null
    try {
      const response = await loginApi(payload)
      token.value = response.token
      localStorage.setItem('token', response.token)
    } catch {
      error.value = 'Email ou senha inválidos.'
    } finally {
      loading.value = false
    }
  }

  async function register(payload: AuthRequest) {
    loading.value = true
    error.value = null
    try {
      await registerApi(payload)
    } catch (e: any) {
      const data = e?.response?.data
      if (data?.message) {
        error.value = data.message
      } else if (data?.errors) {
        const messages = Object.values(data.errors as Record<string, string[]>).flat()
        error.value = messages[0] ?? 'Erro ao criar conta. Tente novamente.'
      } else {
        error.value = 'Erro ao criar conta. Tente novamente.'
      }
    } finally {
      loading.value = false
    }
  }

  function logout() {
    token.value = null
    localStorage.removeItem('token')
  }

  return { isAuthenticated, loading, error, getToken, login, register, logout }
})
