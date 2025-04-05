<template>
  <UserSesion @login="isLoggedIn = true" @logout="isLoggedIn = false"/>

  <div v-if="isLoggedIn" style="display: flex; justify-content: center;">
    <RouletteSpin ref="rouletteRef" @winner="handleWinner"/>
    <BettingTable @betting="handleBet"/>
  </div>
  <!-- Modal Winner -->
  <div v-if="showWinner && isLoggedIn" class="modal-overlay">
    <div class="modal-content">
      <div v-if="winner">
        <p class="modal-title">¡Ganaste!</p>
        <p>Tu saldo se ha actualizado</p>
        <p>Haz ganado ${{ amountWL }}</p>
        <p>Tu saldo ahora es de ${{ userStore.balance }}</p>
      </div>
      <div v-else>
        <p class="modal-title">¡Perdiste!</p>
        <p>Haz perdido ${{ amountWL }}</p>
        <p>Tu saldo ahora es de ${{ userStore.balance }}</p>
      </div>
      <div class="modal-actions">
        <button @click="showWinner = false">Volver a jugar</button>
        <button @click="saveGame">Guardar partida</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

import RouletteSpin from './components/RouletteSpin.vue'
import UserSesion from './components/UserSesion.vue'
import BettingTable from './components/BettingTable.vue'
import { useUserStore } from '@/stores/user'

const userStore = useUserStore()
const isLoggedIn = ref(false)
const showWinner = ref(false)
const winner = ref(false)
const amountWL = ref(0)

const rouletteRef = ref(null)

const handleBet = (values) => {
  rouletteRef.value?.spin(values)
}
const handleWinner = (value) => {
  showWinner.value = true
  winner.value = value.win
  amountWL.value = value.amountWL
}
const saveGame = async () => {
  try {
    await userStore.saveGame(userStore.name, userStore.balance)
    showWinner.value = false
    alert('La partida se guardo correctamente')
  } catch (err) {
    alert('Ocurrió un error al guardar la partida')
  }
}

</script>

<style>
body {
  background-color: #2c3e50;
}
button {
  margin: 10px 5px 20px;
  text-align: center;
  padding: 10px 20px;
  font-size: 16px;
  font-weight: bold;
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #f3f1f1;

}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.75);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}

.modal-content {
  background-color: #2c3e50;
  padding: 30px 40px;
  border-radius: 12px;
  box-shadow: 0 0 20px rgba(255, 255, 255, 0.2);
  text-align: center;
  color: #fff;
  max-width: 400px;
  width: 90%;
}

.modal-title {
  font-size: 3em;
  margin-bottom: 10px;
  margin-top: 0;
  font-weight: bold;
}

.modal-actions {
  margin-top: 20px;
}

.modal-actions button {
  margin: 0 10px;
  padding: 10px 20px;
  font-weight: bold;
  font-size: 16px;
  cursor: pointer;
  border-radius: 6px;
  border: none;
  background-color: #fff;
  color: #2c3e50;
  transition: background-color 0.3s ease;
}

.modal-actions button:hover {
  background-color: #ddd;
}

</style>
