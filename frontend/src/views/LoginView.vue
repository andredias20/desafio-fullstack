<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const router = useRouter()
const route = useRoute()
const store = useAuthStore()

const email = ref('')
const password = ref('')

async function handleSubmit() {
  await store.login({ email: email.value, password: password.value })
  if (store.isAuthenticated) {
    const redirect = route.query.redirect as string | undefined
    router.push(redirect ?? '/')
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-100 flex items-center justify-center px-4">
    <div class="w-full max-w-sm bg-white rounded-xl shadow p-8 flex flex-col gap-6">
      <h1 class="text-2xl font-bold text-gray-700 text-center">Entrar</h1>

      <form @submit.prevent="handleSubmit" class="flex flex-col gap-4">
        <div class="flex flex-col gap-1">
          <label class="text-sm text-gray-600">Email</label>
          <input
            v-model="email"
            type="email"
            placeholder="seu@email.com"
            required
            class="border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
          />
        </div>

        <div class="flex flex-col gap-1">
          <label class="text-sm text-gray-600">Senha</label>
          <input
            v-model="password"
            type="password"
            placeholder="••••••••"
            required
            class="border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
          />
        </div>

        <p v-if="store.error" class="text-red-500 text-sm bg-red-50 border border-red-200 rounded-lg px-3 py-2">
          {{ store.error }}
        </p>

        <button
          type="submit"
          :disabled="store.loading"
          class="bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed font-medium"
        >
          {{ store.loading ? 'Entrando...' : 'Entrar' }}
        </button>
      </form>

      <p class="text-center text-sm text-gray-500">
        Não tem conta?
        <RouterLink to="/register" class="text-blue-600 hover:underline">Criar conta</RouterLink>
      </p>
    </div>
  </div>
</template>
