<script setup lang="ts">
import { ref, watchEffect } from 'vue'

const props = defineProps({
  duration: Number,
})

let curDuration = ref(props.duration ?? 0)
let durationString = ref('')

watchEffect(() => {
  curDuration.value = props.duration ?? 0
  let hoursCalc = Math.floor(curDuration.value / 3600)
  let minutesCalc = Math.floor((curDuration.value % 3600) / 60)
  let secondsCalc = curDuration.value % 60
  durationString.value = `${hoursCalc.toString().padStart(3, '0')}:${minutesCalc.toString().padStart(2, '0')}:${secondsCalc.toString().padStart(2, '0')}`
})
</script>

<template>
  <span class="font-semibold text-sm text-center"
    >Total time spent on this task: {{ durationString }}</span
  >
</template>
