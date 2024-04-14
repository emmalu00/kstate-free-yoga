import { defineStore } from 'pinia';
import api from '@/services/api';

export const useLocationsStore = defineStore('locations', {
  state: () => ({
    classLocations: [], 
    classLocationID: 0,
  }),
  actions: {
    async fetchLocations() 
    {
      try {
        const response = await api.get(`/ClassLocation`);
        this.classLocations = response.data;
      } 
      catch (error) { console.error('Error fetching yoga classes:', error); }
    },
    async addClassLocation(newClassLocation)
    {
      try {
        const response = await api.post('/ClassLocation', newClassLocation);
        this.classLocationID = response.data;
        if (response.data) { console.log('Location added successfully:', response.data); }
      } 
      catch (error) { console.error('Error adding yoga class:', error); }
    },
  },
});