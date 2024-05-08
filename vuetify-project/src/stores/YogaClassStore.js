import { defineStore } from 'pinia';
import api from '@/router/api';

export const useYogaClassesStore = defineStore('yogaClasses', {
    state: () => ({
      filteredClasses: [], 
    }),
    actions: {
      async getClasses(filters) 
      {
        try {
          if (filters)
          {
            let queryParams = new URLSearchParams();
          
            if (filters.classID) { queryParams.set('classID', filters.classID); }
            if (filters.buildingName) { queryParams.set('buildingName', filters.buildingName); }
            if (filters.instructorFullName) { queryParams.set('instructorFullName', filters.instructorFullName); }
            if (filters.matsProvided !== undefined && filters.matsProvided !== null) { queryParams.set('matsProvided', filters.matsProvided); }
        
            const queryString = queryParams.toString();
            const response = await api.get(`/yogaclass${queryString ? `?${queryString}` : ''}`);
            this.filteredClasses = response.data;
          }
          else 
          {
            const response = await api.get(`/yogaclass`);
            this.filteredClasses = response.data;
          }
        
        } 
        catch (error) { console.error('Error fetching filtered yoga classes:', error); }
      },
      async addClass(newClass)
      {
        try {
          const response = await api.post('/yogaclass', newClass);
          if (response.data) { console.log('Class added successfully:', response.data); }
        } 
        catch (error) { console.error('Error adding yoga class:', error); }
      },
      async updateClass(id, updatedClass)
      {
        try {
            const response = await api.put(`/yogaclass?classID=${id}`, updatedClass);
            if (response.data) { console.log('Class updated successfully:', response.data); }
        } 
        catch (error) { console.error('Error updating yoga class:', error); }
      },
      async deleteClass(id)
      {
        console.log("In store...trying to delete class")
        try {
          const response = await api.delete(`/yogaclass?classID=${id}`);
          if (response.data) { console.log('Class deleted successfully:', response.data); }
        } 
        catch (error) { console.error('Error deleting yoga class:', error); }
      }
    },
  });
  