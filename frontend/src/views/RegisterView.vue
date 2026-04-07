<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const router = useRouter()
const store = useAuthStore()

const email = ref('')
const password = ref('')

const passwordTooShort = computed(() => password.value.length > 0 && password.value.length < 6)

async function handleSubmit() {
  await store.register({ email: email.value, password: password.value })
  if (!store.error) {
    router.push('/login')
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-100 flex items-center justify-center px-4">
    <div class="w-full max-w-sm bg-white rounded-xl shadow p-8 flex flex-col gap-6">
      <h1 class="text-2xl font-bold text-gray-700 text-center">Criar conta</h1>

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
            minlength="6"
            required
            class="border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            :class="passwordTooShort ? 'border-amber-400' : ''"
          />
          <p v-if="passwordTooShort" class="text-amber-600 text-xs mt-1">
            A senha deve ter no mínimo 6 caracteres.
          </p>
          <p v-else class="text-gray-400 text-xs mt-1">Mínimo 6 caracteres.</p>
        </div>

        <p v-if="store.error" class="text-red-500 text-sm bg-red-50 border border-red-200 rounded-lg px-3 py-2">
          {{ store.error }}
        </p>

        <button
          type="submit"
          :disabled="store.loading || passwordTooShort"
          class="bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed font-medium"
        >
          {{ store.loading ? 'Criando...' : 'Criar conta' }}
        </button>
      </form>

      <p class="text-center text-sm text-gray-500">
        Já tem conta?
        <RouterLink to="/login" class="text-blue-600 hover:underline">Entrar</RouterLink>
      </p>
    </div>
  </div>
</template>
