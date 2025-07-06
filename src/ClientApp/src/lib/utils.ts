import isMobile from './isMobile'

function getToastGroup() {
  return isMobile() ? 'bc' : 'br'
}

export { getToastGroup }
