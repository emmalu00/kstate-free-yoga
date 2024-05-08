import { defineStore } from 'pinia';
import api from '@/router/api';

export const useInstructorsStore = defineStore('instructors', {
  state: () => ({
    instructors: [], 
    instructorID: 0,
  }),
  actions: {
    async fetchInstructors(id) {
      try {
        let queryParams = new URLSearchParams();

        if (id) { queryParams.set('instructorID', id); }
   
        const queryString = queryParams.toString();
        const response = await api.get(`/instructor${queryString ? `?${queryString}` : ''}`);
        this.instructors = response.data;
      } 
      catch (error) { console.error('Error fetching yoga classes:', error); }
    },
    async addInstructor(newInstructor)
    {
      try {
        const response = await api.post('/instructor', newInstructor);
        this.instructorID = response.data;
        if (response.data) { console.log('Instructor added successfully:', response.data); }
      } 
      catch (error) {console.error('Error adding yoga class:', error);
      }
    },
  },
});