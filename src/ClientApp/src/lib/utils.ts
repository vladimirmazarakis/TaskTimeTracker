import isMobile from './isMobile'

function getToastGroup() {
  return isMobile() ? 'bc' : 'br'
}

function formatSecondsToDigitalTimerString(duration: number) {
  let hoursCalc = Math.floor(duration / 3600)
  let minutesCalc = Math.floor((duration % 3600) / 60)
  let secondsCalc = duration % 60
  return `${hoursCalc.toString().padStart(3, '0')}:${minutesCalc.toString().padStart(2, '0')}:${secondsCalc.toString().padStart(2, '0')}`
}

export { getToastGroup, formatSecondsToDigitalTimerString }
