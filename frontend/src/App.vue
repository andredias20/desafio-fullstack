<script setup lang="ts">
import { RouterView, RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const auth = useAuthStore()
const router = useRouter()

function handleLogout() {
  auth.logout()
  router.push('/login')
}
</script>

<template>
  <div>
    <nav class="bg-white shadow">
      <div class="max-w-3xl mx-auto px-4 py-3 flex items-center gap-6">
        <template v-if="auth.isAuthenticated">
          <RouterLink
            to="/"
            class="text-gray-600 hover:text-blue-600 font-medium transition-colors"
            active-class="text-blue-600 border-b-2 border-blue-600"
          >
            Home
          </RouterLink>
          <RouterLink
            to="/history"
            class="text-gray-600 hover:text-blue-600 font-medium transition-colors"
            active-class="text-blue-600 border-b-2 border-blue-600"
          >
            Histórico
          </RouterLink>
          <button
            @click="handleLogout"
            class="ml-auto text-sm text-gray-500 hover:text-red-500 transition-colors"
          >
            Sair
          </button>
        </template>
      </div>
    </nav>
    <RouterView />
  </div>
</template>

<style scoped></style>
