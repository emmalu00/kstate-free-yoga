import { defineStore } from 'pinia';
import api from '@/services/api';

export const useInstructorsStore = defineStore('instructors', {
  state: () => ({
    instructors: [], 
    instructorID: 0,
  }),
  actions: {
    async fetchInstructors() 
    {
      try {
        const response = await api.get(`/Instructor`);
        this.instructors = response.data;
      } 
      catch (error) { console.error('Error fetching yoga classes:', error); }
    },
    async addInstructor(newInstructor)
    {
      try {
        const response = await api.post('/Instructor', newInstructor);
        this.instructorID = response.data;
        if (response.data) { console.log('Instructor added successfully:', response.data); }
      } 
      catch (error) { console.error('Error adding yoga class:', error); }
    },
  },
});