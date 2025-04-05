import axios from 'axios'

const baseUrl = process.env.VUE_APP_API_URL + '/user'

export const getBalance = (name) => {
  return axios.post(`${baseUrl}/balance`, { name })
}

export const addUser = (name, balance) => {
  return axios.post(`${baseUrl}/add`, { name, balance })
}

export const updateBalance = (name, balance) => {
  return axios.post(`${baseUrl}/update`, { name, balance })
}
