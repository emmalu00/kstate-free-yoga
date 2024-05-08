import { defineStore } from 'pinia';
import api from '@/router/api';

export const useClassAttendanceStore = defineStore('attendanceRecords', {
  state: () => ({
    attendanceRecords: [], 
    attendanceID: 0,
  }),
  actions: {
    async fetchAttendanceRecords(attendanceID, userID, classID, attendanceStatus, favorited) 
    {
      try {
        let queryParams = new URLSearchParams();
  
        if (attendanceID) { queryParams.set('attendanceID', attendanceID); }
        if (userID) { queryParams.set('userID', userID); }
        if (classID) { queryParams.set('classID', classID); }
        if (attendanceStatus) { queryParams.set('attendanceStatus', attendanceStatus); }
        if (favorited !== undefined && favorited !== null) { queryParams.set('favorited', favorited); }
      
          const queryString = queryParams.toString();
        const response = await api.get(`/classattendance${queryString ? `?${queryString}` : ''}`);
        this.attendanceRecords = response.data;
      } 
      catch (error) { console.error('Error fetching attendance records:', error); }
    },
    async addAttendanceRecord(userID, classID)
    {
      try {
        let queryParams = new URLSearchParams();
        queryParams.set('userID', userID);
        queryParams.set('classID', classID);
        const queryString = queryParams.toString();

        const response = await api.post(`/classattendance${queryString ? `?${queryString}` : ''}`);
       // this.attendanceID = response.data;
        if (response.data) { console.log('Attendance added successfully:', response.data); }
      } 
      catch (error) { console.error('Error adding class attendance:', error); }
    },
    async updateAttendanceRecord(attendanceID, favorited)
    {
      try {
        let queryParams = new URLSearchParams();
        queryParams.set('attendanceID', attendanceID);
        queryParams.set('favorited', favorited);
        const queryString = queryParams.toString();
        const response = await api.put(`/classattendance${queryString ? `?${queryString}` : ''}`);
      }
      catch (error) { console.error('Error updating attendance record:', error); }
    },
    async deleteAttendanceRecord(id)
      {
        try {
          const response = await api.delete(`/classattendance?attendanceID=${id}`);
          if (response.data) { console.log('Record deleted successfully:', response.data); }
        } 
        catch (error) { console.error('Error deleting attendance record:', error); }
      }
  },
});