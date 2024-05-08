// From Office Hourse Queue
// https://www.bezkoder.com/vue-refresh-token/ 

import axios from 'axios'

const instance = axios.create({
  baseURL: 'https://localhost:44374/api', //CHANGE THIS LATER
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
  },
})

// Request interceptor
instance.interceptors.request.use(config => {
  // Read token from localStorage
  const token = localStorage.getItem('userToken');
  if (token) {
    config.headers['Authorization'] = 'Bearer ' + token;  // Append token to any existing headers
  }
  return config;
}, error => {
  return Promise.reject(error);
});

export default instance