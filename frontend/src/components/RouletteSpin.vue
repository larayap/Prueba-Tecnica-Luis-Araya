<template>
  <div class="roulette-wrapper">
    <p class="number-display">{{ currentNumber }}</p>
    <div class="roulette-container">
      <img src="@/assets/roulette.png" ref="wheel" class="wheel" />
      <div ref="ball" class="ball" />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import gsap from 'gsap'
import { defineEmits, defineExpose } from 'vue'
import { useUserStore } from '@/stores/user'
import { getRandom } from '@/api/randomApi'

const userStore = useUserStore()
const wheel = ref(null)
const ball = ref(null)
const currentNumber = ref(0)
const emit = defineEmits(['winner'])
const forceNumber = ref(null)
const riggedMode = ref(false)

const rouletteNumbers = [
  0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6,
  27, 13, 36, 11, 30, 8, 23, 10, 5, 24,
  16, 33, 1, 20, 14, 31, 9, 22, 18, 29,
  7, 28, 12, 35, 3, 26
]

async function spin(values) {
  riggedMode.value = true
  try {
    const response = await getRandom()
    forceNumber.value = response.data.number
  } catch (err) {
    riggedMode.value = false
  }
  gsap.set(wheel.value, { rotate: 0 })
  const degreesPerSlot = 360 / rouletteNumbers.length
  const fullRotations = 360 * 5 // 5 vueltas completas

  let finalAngle

  if (riggedMode.value && forceNumber.value !== null) {
    const index = rouletteNumbers.findIndex(n => n === forceNumber.value)
    const angleForNumber = (360 - index * degreesPerSlot) % 360
    finalAngle = fullRotations + angleForNumber
  } else {
    const extra = Math.floor(Math.random() * 360)
    finalAngle = fullRotations + extra
  }

  gsap.to(wheel.value, {
    rotate: finalAngle,
    duration: 4,
    ease: 'power4.out',
    transformOrigin: 'center center',
    onUpdate: () => {
      const angle = (gsap.getProperty(wheel.value, 'rotate') % 360 + 360) % 360
      const index = Math.floor(((360 - angle + degreesPerSlot / 2) % 360) / degreesPerSlot)
      currentNumber.value = rouletteNumbers[index]
    },
    onComplete: () => {
      emit('winner', winner(currentNumber.value, values))
    }
  })
}

const redNumbers = [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36]
const blackNumbers = [2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35]

function winner(winNumber, bet) {
  const { type, value, amount } = bet
  let win = false
  let multiplier = 0
  let winnings = 0

  switch (type) {
    case 'Número':
      win = winNumber === value
      multiplier = 3
      break
    case 'Color':
      if (value === 'Rojo') win = redNumbers.includes(winNumber)
      if (value === 'Negro') win = blackNumbers.includes(winNumber)
      multiplier = 0.5
      break
    case 'Tipo':
      if (value === 'Par') win = winNumber !== 0 && winNumber % 2 === 0
      if (value === 'Impar') win = winNumber % 2 === 1
      multiplier = 1
      break
  }

  if (win) {
    winnings = amount * multiplier
    userStore.updateBalance(winnings) // Gana, se suma lo ganado
  } else {
    userStore.updateBalance(-amount) // Pierde, se descuenta la apuesta
  }

  const amountWL = win ? winnings : amount
  return {
    win,
    amountWL
  }
}

defineExpose({ spin })

</script>

<style scoped>
.roulette-wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.roulette-container {
  position: relative;
  width: 300px;
  height: 300px;
}

.wheel {
  width: 100%;
  height: 100%;
  background-size: cover;
  border-radius: 50%;
}

.ball {
  position: absolute;
  top: 0;
  left: 50%;
  width: 12px;
  height: 12px;
  background: white;
  border-radius: 50%;
  transform: translateX(-50%) translateY(35px);
}

.spin-button {
  margin-top: 20px;
  padding: 10px 20px;
  font-size: 18px;
  cursor: pointer;
}

.number-display {
  font-size: 48px;
  font-weight: bold;
  color: white;
  text-shadow: 0 0 10px black;
  margin-bottom: 20px;
}
</style>
