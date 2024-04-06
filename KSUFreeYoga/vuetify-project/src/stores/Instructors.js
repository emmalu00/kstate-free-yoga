import { defineStore } from 'pinia';
import api from '@/services/api';

export const useInstructorsStore = defineStore('instructors', {
  state: () => ({
    instructors: [], 
  }),
  getters: {
    // getters??
  },
  actions: {
    async fetchInstructors() {
      try {
        
        const response = await api.get(`/Instructor`);
        this.instructors = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes:', error);
      }
    },
  },
});