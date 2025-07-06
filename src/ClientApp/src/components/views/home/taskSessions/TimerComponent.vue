<script setup lang="ts">
import { ref, useTemplateRef, watchEffect } from 'vue'

const props = defineProps({
  duration: Number,
})

let curDuration = ref(props.duration ?? 0)
let seconds = ref('')
let minutes = ref('')
let hours = ref('')
let hoursElm = useTemplateRef('hours-elm')
let minutesElm = useTemplateRef('minutes-elm')
let secondsElm = useTemplateRef('seconds-elm')

watchEffect(() => {
  curDuration.value = props.duration ?? 0
  let hoursCalc = Math.floor(curDuration.value / 3600)
  let minutesCalc = Math.floor((curDuration.value % 3600) / 60)
  let secondsCalc = curDuration.value % 60
  hours.value = `${hoursCalc.toString().padStart(3, '0')}`
  minutes.value = `${minutesCalc.toString().padStart(2, '0')}`
  seconds.value = `${secondsCalc.toString().padStart(2, '0')}`
})
</script>

<template>
  <div class="flex flex-row text-5xl font-semibold justify-center items-center">
    <span v-if="curDuration > 3600" class="hours" ref="hours-elm">{{ hours }}:</span>
    <span class="minutes" ref="minutes-elm">{{ minutes }}:</span>
    <span class="seconds" ref="seconds-elm">{{ seconds }}</span>
  </div>
</template>
