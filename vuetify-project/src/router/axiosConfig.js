// Function to get the stored token
function getToken() {
    return localStorage.getItem('userToken');
  }
  
  // Request interceptor to add the auth token header to requests
  axios.interceptors.request.use(
    (config) => {
      const token = getToken();
      if (token) {
        config.headers['Authorization'] = 'Bearer ' + token;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );
  
  export default axios;