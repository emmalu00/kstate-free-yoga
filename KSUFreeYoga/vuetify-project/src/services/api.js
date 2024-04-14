// From Office Hourse Queue
// https://www.bezkoder.com/vue-refresh-token/ 

import axios from 'axios'

const instance = axios.create({
  baseURL: 'http://localhost:37559/api',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
  },
})
export default instance