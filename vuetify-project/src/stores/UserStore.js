

import { defineStore } from 'pinia';
import api from '@/router/api';

export const useUserStore = defineStore('users', {
  state: () => ({
    users: [], 
    user: null,
  }),
  actions: {
    async getUser() {
        try {
            //const userId = 1;
            //const response = await api.get(`/User?userId=${userId}`);
            const response = await api.get("/User");
            this.user = response.data;
          } catch (error) {
            console.error('There was an error fetching the user data:', error);
            this.user = null;
          }
    },
  },
});