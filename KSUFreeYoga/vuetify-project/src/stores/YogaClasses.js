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
    // async fetchYogaClasses() {
    //   try {
    //     const response = await api.get('/YogaClass');
    //     this.classes = response.data;
    //   } catch (error) {
    //     console.error('Error fetching yoga classes:', error);
    //     // handle errors?
    //   }
    // },
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
    async filterYogaClasses(buildingName, teacherFirstName, teacherLastName, matsAvailable) {
      try {
        let queryParams = new URLSearchParams();

        if (buildingName) {
          queryParams.set('buildingName', buildingName);
        }
        if (teacherFirstName) {
          queryParams.set('teacherFirstName', teacherFirstName);
        }
        if (teacherLastName) {
          queryParams.set('teacherLastName', teacherLastName);
        }
        if (matsAvailable !== undefined && matsAvailable !== null) {
          queryParams.set('matsAvailable', matsAvailable);
        }
    
        const queryString = queryParams.toString();
        const response = await api.get(`/YogaClass${queryString ? `?${queryString}` : ''}`);
      
        this.filteredClasses = response.data;
      } catch (error) {
        console.error('Error fetching filtered yoga classes:', error);
        // handle errors?
      }
    },
    async addClass(newClass)
    {
      try {
        console.log(newClass);
  
        const response = await api.post('/YogaClass/AddClass', newClass);
        
        if (response.data) {
          console.log('Class added successfully:', response.data);
          // You might want to do something with the response data here
        }
      } catch (error) {
        console.error('Error adding yoga class:', error);
        // Handle errors appropriately
      }
    },
    async addYogaClass(className, startTime, endTime, classDate, instructorID, locationID, matsAvailable, classDescription) {
      try {

        console.log({
          className: className, 
          startTime: startTime, 
          endTime: endTime, 
          classDate: classDate, 
          instructorID: instructorID, 
          locationID: locationID,
          matsAvailable: matsAvailable, 
          classDescription: classDescription
        });
  
        const response = await api.post('/YogaClass/AddYogaClassInformation', {
          className, 
          startTime, 
          endTime, 
          classDate, 
          instructorID, 
          locationID, 
          matsAvailable, 
          classDescription
        }).then(async () => {
          await this.hydrate()
        });
        
        if (response.data) {
          console.log('Class added successfully:', response.data);
          // You might want to do something with the response data here
        }
      } catch (error) {
        console.error('Error adding yoga class:', error);
        // Handle errors appropriately
      }
    }
    
    
  },
});
