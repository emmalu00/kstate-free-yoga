/* https://www.bezkoder.com/vue-refresh-token/ */
// Imports
import axios from 'axios'

const instance = axios.create({
  baseURL: 'http://localhost:16255/api',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
  },
})

export default instance