<template>
  <div class="login-container">
    <div v-if="!userStore.isLoggedIn" class="login-form">
      <input v-model="username" placeholder="Ingresa tu nombre" />

      <!-- Login -->
      <button v-if="!showRegister" @click="login" :disabled="loading">
        {{ loading ? 'Cargando...' : 'Iniciar sesión' }}
      </button>

      <!-- Register user -->
      <div v-if="showRegister" class="register-box">
        <input
          v-model.number="initialBalance"
          type="number"
          placeholder="Saldo inicial"
        />
        <button @click="register" :disabled="loading || !initialBalance">
          Confirmar Registro
        </button>
      </div>
      <button @click="toggleRegister" :disabled="loading">
        {{ showRegister ? 'Cancelar' : 'Deseo registrarse' }}
      </button>
      <p v-if="error" class="error">{{ error }}</p>
    </div>

    <div v-else class="welcome">
      <p>Bienvenido, {{ userStore.name }}.</p>
      <p>Tu saldo es: ${{ userStore.balance }}</p>
      <button @click="logout" :disabled="loading">Cerrar sesión</button>
    </div>
  </div>
</template>

<script setup>
import { ref, defineEmits } from 'vue'
import { useUserStore } from '@/stores/user'

const emit = defineEmits(['login', 'logout'])
const userStore = useUserStore()

const username = ref('')
const initialBalance = ref(null)
const loading = ref(false)
const error = ref('')
const showRegister = ref(false)

function toggleRegister() {
  error.value = ''
  showRegister.value = !showRegister.value
  initialBalance.value = null
}

async function login() {
  error.value = ''
  loading.value = true
  try {
    await userStore.login(username.value)
    emit('login')
  } catch (err) {
    if (err.response?.data?.error === 'NoUser') {
      error.value = 'Usuario no encontrado'
    } else {
      error.value = 'Error al iniciar sesión'
    }
  } finally {
    loading.value = false
  }
}

async function register() {
  error.value = ''
  loading.value = true
  try {
    await userStore.register(username.value, initialBalance.value)
    emit('login')
  } catch (err) {
    if (err.response?.data?.error === 'UserExists') {
      error.value = 'El usuario ya existe'
    } else {
      error.value = 'Error al registrar'
    }
  } finally {
    loading.value = false
    showRegister.value = false
  }
}

function logout() {
  userStore.logout()
  emit('logout')
}
</script>

<style scoped>
.login-container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.login-form input {
  width: 200px;
  padding: 10px 14px;
  font-size: 16px;
  font-weight: bold;
  border-radius: 8px;
  border: 2px solid #ffffff;
  background-color: #333;
  color: #fff;
  text-align: center;
  outline: none;
  margin-bottom: 12px;
}

.register-box {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.register-box input {
  margin-bottom: 8px;
  width: 200px;
  padding: 10px;
  font-size: 16px;
  border-radius: 6px;
  border: 1px solid #ccc;
}

.login-form button {
  padding: 10px 20px;
  font-size: 16px;
  cursor: pointer;
  margin-bottom: 10px;
}

.error {
  color: #ff4d4f;
  margin-top: 10px;
}

.welcome {
  text-align: center;
  font-size: 18px;
}
</style>
