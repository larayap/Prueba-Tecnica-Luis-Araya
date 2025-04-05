<template>
  <div class="roulette-grid">
    <!-- Celda del 0 a la izquierda -->
    <div class="cell green zero" @click="handleClick({ type: 'Número', value: 0 })">0</div>

    <!-- Filas de números del 1 al 36 -->
    <div
      v-for="(row, rowIndex) in grid"
      :key="rowIndex"
      class="row"
    >
      <div
        v-for="cell in row"
        :key="cell.number"
        class="cell"
        :class="getColorClass(cell.number)"
        @click="handleClick({ type: 'Número', value: cell.number })"
      >
        {{ cell.number }}
      </div>
    </div>
    <div class="cell option impar-option" @click="handleClick({ type: 'Tipo', value: 'Impar' })">IMPAR</div>
    <div class="cell option black-option" @click="handleClick({ type: 'Color', value: 'Negro' })">N</div>
    <div class="cell option red-option" @click="handleClick({ type: 'Color', value: 'Rojo' })">R</div>
    <div class="cell option par-option" @click="handleClick({ type: 'Tipo', value: 'Par' })">PAR</div>
    <p v-if="selectedBet" class="selected-info">
      Apostando por: <strong>{{ selectedBet.type }} → {{ selectedBet.value }}</strong>
    </p>
    <p v-else class="selected-info">¡Escoge tu apuesta!</p>
    <div v-if="selectedBet" class="bet-content">
      <input class="amount-input" type="number" v-model.number="amount" min="1" placeholder="Monto a apostar">
      <button class="bet-button" @click="placeBet"  :disabled="!amount || amount <= 0 || amount > userStore.balance">
        ¡Apostar!
      </button>
      <p v-if="amount > userStore.balance" style="color: #ff8080; font-weight: bold;">
        Saldo insuficiente para esta apuesta
      </p>
    </div>
    
  </div>

</template>

<script setup>
import { ref } from 'vue'
import { defineEmits } from 'vue'
import { useUserStore } from '@/stores/user'

const userStore = useUserStore()
const selectedBet = ref(null)
const amount = ref(null)
const emit = defineEmits(['betting'])

const grid = [
  [ { number: 3 }, { number: 6 }, { number: 9 }, { number: 12 }, { number: 15 }, { number: 18 }, { number: 21 }, { number: 24 }, { number: 27 }, { number: 30 }, { number: 33 }, { number: 36 } ],
  [ { number: 2 }, { number: 5 }, { number: 8 }, { number: 11 }, { number: 14 }, { number: 17 }, { number: 20 }, { number: 23 }, { number: 26 }, { number: 29 }, { number: 32 }, { number: 35 } ],
  [ { number: 1 }, { number: 4 }, { number: 7 }, { number: 10 }, { number: 13 }, { number: 16 }, { number: 19 }, { number: 22 }, { number: 25 }, { number: 28 }, { number: 31 }, { number: 34 } ],
]

const redNumbers = [1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36]
const blackNumbers = [2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35]

function getColorClass(num) {
  if (num === 0) return 'green'
  if (redNumbers.includes(num)) return 'red'
  if (blackNumbers.includes(num)) return 'black'
  return ''
}

function handleClick(bet) {
  selectedBet.value = bet
}

function placeBet() {
  emit('betting', {
    type: selectedBet.value.type,
    value: selectedBet.value.value,
    amount: amount.value
  })
}
</script>

<style scoped>
.roulette-grid {
  display: grid;
  grid-template-columns: 40px repeat(12, 40px);
  grid-template-rows: repeat(3, 40px);
  gap: 4px;
  background: #006400;
  padding: 10px;
  border: 4px solid #fff;
  width: max-content;
  margin: 20px 30px;
  margin-top: 70px;
}

.row {
  display: contents; /* permite que los elementos hijos usen el grid del contenedor */
}

.cell {
  width: 40px;
  height: 40px;
  font-size: 16px;
  font-weight: bold;
  color: white;
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 4px;
  cursor: pointer;
  border: 1px solid #fff;
}

.red {
  background-color: #d32f2f;
}

.black {
  background-color: #212121;
}

.green {
  background-color: #388e3c;
}

.zero {
  grid-row: span 3;
  writing-mode: vertical-rl;
  text-align: center;
  font-size: 24px;
  height: calc(40px * 3 + 8px);
  align-items: center;
  justify-content: center;
}

.option {
  font-size: 14px;
  font-weight: bold;
  border-radius: 4px;
  display: flex;
  justify-content: center;
  align-items: center;
  color: white;
  width: calc(40px * 2 + 4px);
  grid-column: span 2;
}

.impar-option {
  grid-column: 1 / span 2;
  background-color: #525151;
  width: calc(40px * 2 + 4px)
}

.par-option {
  grid-column:  12 / span 2;
  background-color: #525151;
  width: calc(40px * 2 + 4px)
}

.black-option {
  grid-column: 5 / span 2;
  background-color: #000;
  width: calc(40px * 2 + 4px)
}

.red-option {
  grid-column: 8 / span 2;
  background-color: #d32f2f;
  width: calc(40px * 2 + 4px)
}

.selected-info {
  text-align: center;
  font-size: 18px;
  font-weight: bold;
  color: #fff;
  grid-column: 1 / span 13
}

.bet-button {
  margin: 10px auto 20px;
  display: block;
  text-align: center;
  padding: 10px 20px;
  font-size: 16px;
  font-weight: bold;
  color: rgb(22, 22, 22);

  border: none;
  border-radius: 6px;
  cursor: pointer;
  grid-column: 1 / span 13;
  transition: background-color 0.3s ease;
}

.bet-content {
  margin: 10px auto 20px;
  display: block;
  text-align: center;
  padding: 10px 20px;
  font-size: 16px;
  font-weight: bold;
  color: rgb(22, 22, 22);

  border: none;
  border-radius: 6px;
  cursor: pointer;
  grid-column: 1 / span 13;
  transition: background-color 0.3s ease;
}
.bet-button:hover {
  background-color: #b9b9b9;
}

.bet-button:disabled {
  background-color: #999;
  color: #444;
  cursor: not-allowed;
  pointer-events: none;
}

.bet-button:not(:disabled):hover {
  background-color: #b9b9b9;
}

.amount-input {
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
  transition: border-color 0.3s ease, box-shadow 0.3s ease;
  margin-bottom: 12px;
}

.amount-input::placeholder {
  color: #ccc;
  opacity: 0.8;
}

.amount-input:focus {
  box-shadow: 0 0 5px #ffffff;
}

</style>
