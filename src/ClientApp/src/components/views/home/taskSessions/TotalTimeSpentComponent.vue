<script setup lang="ts">
import { formatSecondsToDigitalTimerString } from '@/lib/utils'
import { ref, watchEffect } from 'vue'

const props = defineProps({
  duration: Number,
})

let curDuration = ref(props.duration ?? 0)
let durationString = ref('')

watchEffect(() => {
  curDuration.value = props.duration ?? 0
  durationString.value = formatSecondsToDigitalTimerString(curDuration.value)
})
</script>

<template>
  <span class="font-semibold text-sm text-center"
    >Total time spent on this task: {{ durationString }}</span
  >
</template>
