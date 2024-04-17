import { defineStore } from 'pinia';
import api from '@/services/api';

export const useInstructorsStore = defineStore('instructors', {
  state: () => ({
    instructors: [], 
    instructor: [], 
    instructorID: 0,
  }),
  actions: {
    async fetchInstructors(certified) {
      try {
        let queryParams = new URLSearchParams();

        if (certified) { queryParams.set('certified', certified); }
   
        const queryString = queryParams.toString();
        const response = await api.get(`/Instructor${queryString ? `?${queryString}` : ''}`);
        this.instructors = response.data;
      } 
      catch (error) { console.error('Error fetching yoga classes:', error); }
    },
    async fetchInstructorByID(instructorID) 
    {
      try {
        const response = await api.get(`/Instructor?instructorID=${instructorID}`);
        this.instructor = response.data;
      } 
      catch (error) { console.error('Error fetching instructor:', error); }
    },
    async addInstructor(newInstructor)
    {
      try {
        const response = await api.post('/Instructor', newInstructor);
        this.instructorID = response.data;
        if (response.data) { console.log('Instructor added successfully:', response.data); }
      } 
      catch (error) {console.error('Error adding yoga class:', error);
      }
    },
  },
});