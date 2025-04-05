import { defineStore } from 'pinia'
import { ref } from 'vue'
import { getBalance, addUser, updateBalance as updateBalanceApi } from '@/api/userApi'

export const useUserStore = defineStore('user', () => {
  const name = ref('')
  const balance = ref(0)
  const isLoggedIn = ref(false)

  async function register(username, balanceAmount) {
    name.value = username
    const response = await addUser(username, balanceAmount)
    balance.value = response.data.amount
    isLoggedIn.value = true
  }

  async function login(username) {
    name.value = username
    try {
      const response = await getBalance(username)
      balance.value = response.data.amount
      isLoggedIn.value = true
      return true
    } catch (err) {
      logout()
      throw err
    }
  }

  function logout() {
    name.value = ''
    balance.value = 0
    isLoggedIn.value = false
  }

  function updateBalance(amount) {
    balance.value += amount
  }

  async function saveGame(username, balanceAmount) {
    try {
      await updateBalanceApi(username, balanceAmount)
      return true
    } catch (err) {
      logout()
      throw err
    }
  }
  return {
    name,
    balance,
    isLoggedIn,
    login,
    logout,
    updateBalance,
    register,
    saveGame,
  }
})
