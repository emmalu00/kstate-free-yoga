import { defineStore } from 'pinia';
import api from '@/services/api';

export const useYogaClassesStore = defineStore('yogaClasses', {
  state: () => ({
    classes: [],
    classesWithMats: [],
    filteredClasses: [], 
    teacherNames: [], 
    locations: [], 
  }),
  getters: {
    // getters??
  },
  actions: {
    async fetchTeacherNames() {
      try {
        const response = await api.get('/YogaClass/GetTeacherNames');
        this.teacherNames = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes:', error);
      }
    },
    async fetchLocations() {
      try {
        const response = await api.get('/YogaClass/GetLocations');
        this.locations = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes:', error);
      }
    },
    async filterYogaClasses(buildingName, instructorFirst, instructorLast, matsProvided) {
      try {
        let queryParams = new URLSearchParams();

        if (buildingName) {
          queryParams.set('buildingName', buildingName);
        }
        if (instructorFirst) {
          queryParams.set('instructorFirst', instructorFirst);
        }
        if (instructorLast) {
          queryParams.set('instructorLast', instructorLast);
        }
        if (matsProvided !== undefined && matsProvided !== null) {
          queryParams.set('matsProvided', matsProvided);
        }
    
        const queryString = queryParams.toString();
        const response = await api.get(`/YogaClass${queryString ? `?${queryString}` : ''}`);
        this.filteredClasses = response.data;
      } catch (error) {
        console.error('Error fetching filtered yoga classes:', error);
      }
    },
    async addClass(newClass)
    {
      try {
  
        const response = await api.post('/YogaClass', newClass);
        if (response.data) {
          console.log('Class added successfully:', response.data);
          // You might want to do something with the response data here
        }
      } catch (error) {
        console.error('Error adding yoga class:', error);
        // Handle errors appropriately
      }
    },
    async deleteClass(id)
    {
      console.log(id);
      try {
        const response = await api.delete(`/YogaClass?id=${id}`);
        if (response.data) {
          console.log('Class deleted successfully:', response.data);
          // You might want to do something with the confirmation response here
        }
      } catch (error) {
        console.error('Error deleting yoga class:', error);
        // Handle errors appropriately
      }
    }
  
    
    
  },
});
