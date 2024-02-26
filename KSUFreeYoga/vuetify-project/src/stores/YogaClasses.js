// src/stores/YogaClasses.js
import { defineStore } from 'pinia';
import api from '@/services/api';

export const useYogaClassesStore = defineStore('yogaClasses', {
  state: () => ({
    classes: [],
    classesWithMats: [],
  }),
  getters: {
    // Add any getters here if needed
  },
  actions: {
    async fetchYogaClasses() {
      try {
        const response = await api.get('/YogaClass/GetYogaClassInformation');
        this.classes = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes:', error);
        // Handle error appropriately
      }
    },
    async fetchYogaClassesWithMats() {
      try {
        const response = await api.get('/YogaClass/GetYogaClassInformationWithMats');
        this.classesWithMats = response.data;
      } catch (error) {
        console.error('Error fetching yoga classes with mats:', error);
        // Handle error appropriately
      }
    },
  },
});
