<template>
    <v-sheet class="pa-2 ma-3">
      
      <v-row>
        <v-col> <h2> Account Information </h2>
         </v-col>
      </v-row>
      <v-row> 
      
    </v-row>
      <v-row>
        <v-col cols="4">
          <v-card v-if="user" variant="flat" color="#927396">
            <v-card-title> 
              <v-icon icon="$yoga" variant="text"></v-icon> 
              {{user.FirstName}} {{user.LastName}}
            </v-card-title>
            <v-card-text> {{user.Email}} </v-card-text>
          </v-card>
          <RecommendedResources v-if="resourcesCard" :favorites="favorites"> </RecommendedResources>
          <RecommendedClasses v-if="recommendationCard" :classHistory="previousClasses" :userSchedule="upcomingClasses"> </RecommendedClasses>
        </v-col>
        <v-col cols="8">
          <h2> Schedule </h2>
          <v-tabs v-model="tab" bg-color="#bfaec2">
              <v-tab value="upcoming" > Upcoming</v-tab>
              <v-tab value="previous"> Previous </v-tab>
              <v-tab value="favorites"> Favorites</v-tab>
          </v-tabs>
          <v-sheet>
            <v-window v-model="tab" >
              <v-window-item value="upcoming">
                <AttendanceCard :eventList="upcomingClasses" :listInfo="'upcoming'"
                @removeClass="deleteAttendanceRecord" @updateFavorited="updateAttendanceRecord">
              </AttendanceCard>
                <v-list-item v-if="!upcomingClasses.length"> You have no upcoming classes. </v-list-item>
              </v-window-item>
      
              <v-window-item value="previous">
                <AttendanceCard :eventList="previousClasses" :listInfo="'previous'"
                @removeClass="deleteAttendanceRecord" @updateFavorited="updateAttendanceRecord">
              </AttendanceCard>
                <v-list-item v-if="!previousClasses.length"> You have no class history. </v-list-item>
              </v-window-item>
      
              <v-window-item value="favorites">
                <AttendanceCard :eventList="favorites" :listInfo="'favorites'"
                @updateFavorited="updateAttendanceRecord">
                </AttendanceCard>
                <v-list-item v-if="!favorites.length"> You have no favorites. </v-list-item>
              </v-window-item>
            </v-window>
          </v-sheet>
        </v-col>
      </v-row>
    </v-sheet>
    <v-snackbar v-model="confirmationDialog.show" timeout="4000">
      {{ confirmationDialog.text }}
      <template v-slot:actions>
        <v-btn color="#bfc5e8" variant="text" @click="confirmationDialog.show = false">
          Close
        </v-btn>
      </template>
    </v-snackbar>
    <v-overlay :model-value="overlayItem" class="align-center justify-center">
      <v-progress-circular indeterminate color="#bfaec2"></v-progress-circular>
     </v-overlay>
  </template>
  
  <script>
  import { useUserStore } from '@/stores/UserStore';
  import { useClassAttendanceStore } from '@/stores/ClassAttendanceStore';
  import { useYogaClassesStore } from '@/stores/YogaClassStore.js'; 
  import AttendanceCard from '../components/AttendanceCard.vue';
  import RecommendedResources from '../components/RecommendedResources.vue';
  import RecommendedClasses from '@/components/RecommendedClasses.vue';
  
  export default {
    data() {
      return {
        tab: null, 
        overlayItem: true,
        resourcesCard: false,
        recommendationCard: false,
        records: [],
        upcomingClasses: [], 
        previousClasses: [], 
        favorites: [], 
        user: {UserID: 0, GoogleID: '', FirstName: '', LastName: '', Email: '', UserRole: ''}, 
        confirmationDialog: { text: '', show: false}, 
        filters: { classID: 0, buildingName: null, instructorFullName: null, matsProvided: null },
      }
    },
    methods: {
      async userInfo()
      {
        await useUserStore().getUser();
        this.user = useUserStore().user[0];    
        await useClassAttendanceStore().fetchAttendanceRecords(null, this.user.UserID, null, null, null);
        this.records = useClassAttendanceStore().attendanceRecords;
  
        const yogaClassStore = useYogaClassesStore();
        const currentDate = new Date();
  
        for (const record of this.records) {
          this.filters.classID = record.ClassID;
          await yogaClassStore.getClasses(this.filters);
          const classInfo = yogaClassStore.filteredClasses[0];
  
          const classRecord = {...classInfo, ...record};
          if (new Date(classRecord.ClassDate) > currentDate) { this.upcomingClasses.push(classRecord); } 
          else { this.previousClasses.push(classRecord); }
          if (classRecord.Favorited) { this.favorites.push(classRecord); }
        }
        this.upcomingClasses = this.upcomingClasses.reverse();
        this.recommendationCard = true;
        this.resourcesCard = true;
        this.overlayItem = false;
      
      },
      async deleteAttendanceRecord(id) {
          await useClassAttendanceStore().deleteAttendanceRecord(id);
          this.confirmationDialog = { text: 'Class successfully removed from schedule.', show: true};
      },
      async updateAttendanceRecord(classToUpdate) {
        if (classToUpdate.Favorited) { 
          this.favorites.push(classToUpdate); 
          this.confirmationDialog = { text: 'Class added to FAVORITES.', show: true};
        }
        else {
          const favIndex = this.favorites.findIndex(fav => fav.AttendanceID === classToUpdate.AttendanceID);
          this.favorites.splice(favIndex, 1);
          this.confirmationDialog = { text: 'Class removed from FAVORITES.', show: true};
  
        }
          const attendanceStore = useClassAttendanceStore();
          attendanceStore.updateAttendanceRecord(classToUpdate.AttendanceID, classToUpdate.Favorited);
      },
    }, 
    mounted() {
        this.userInfo();
    }
  }
  </script>
  
  <style>
  .v-card {
    margin: 4% 2% !important;
    border-radius: .4rem;
  }
  
  .v-window {
    background-color: #ece1e9;
    height: 600px; 
    overflow-y: auto !important; 
  }
  </style>