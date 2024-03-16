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
    async fetchYogaClasses() {
      try {
        const response = await api.get('/YogaClass/GetYogaClassInformation');
        this.classes = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes:', error);
        // handle errors?
      }
    },
    async fetchTeacherNames() {
      try {
        const response = await api.get('/YogaClass/GetTeacherNames');
        this.teacherNames = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes:', error);
        // handle errors?
      }
    },
    async fetchLocations() {
      try {
        const response = await api.get('/YogaClass/GetLocations');
        this.locations = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes:', error);
        // handle errors?
      }
    },
    async fetchYogaClassesWithMats() {
      try {
        const response = await api.get('/YogaClass/GetYogaClassInformationWithMats');
        this.classesWithMats = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes with mats:', error);
        // handle errors?
      }
    },
    async filterYogaClasses({matsAvailable, teacherName, locationID}) {
      try {
        const params = new URLSearchParams();
        if (matsAvailable !== undefined) params.append('matsAvailable', matsAvailable);
        if (locationId !== undefined) params.append('locationId', locationId);
        if (teacherName !== undefined) params.append('instructor', teacherName);
        const response = await api.get('/YogaClass/FilterYogaClassInformation?${params}');
        this.filteredClasses = response.data;
      } catch (error) {
        console.error('Error fetching filtered yoga classes:', error);
        // handle errors?
      }
    },
    async addYogaClassInformation({ classID, className, startTime, duration, classDate, teacherName, locationID, matsAvailable, classDescription }) {
      try {
        const response = await api.post('/YogaClass/AddYogaClassInformation', {
          classID,
          className,
          startTime,
          duration,
          classDate,
          teacherName,
          locationID,
          matsAvailable,
          classDescription
        });
        console.log('Class added successfully:', response.data);
        // Optionally, you could fetch the updated list of classes here or update the state directly
        // this.fetchYogaClasses();
      } catch (error) {
        console.error('Error adding yoga class information:', error);
        // Implement error handling logic as needed
      }
    },
  },
});
