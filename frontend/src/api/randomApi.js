import axios from 'axios'

const baseUrl = process.env.VUE_APP_API_URL + '/game'

export function getRandom() {
  return axios.get(`${baseUrl}/random`)
}