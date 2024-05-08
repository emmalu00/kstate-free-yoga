<template>
    <v-card color="#bfaec2">
        <v-card-item title="Recommended Classes">
            <v-card-subtitle> Based on your class history. </v-card-subtitle>
         </v-card-item>
         <v-sheet color="#bfaec2" style="height: 200px; overflow-y: auto !important">
            <v-list-item v-if="!recommendedClasses.length"> 
                Sorry, no current recommendations.
            </v-list-item>
            <v-card v-for="event in recommendedClasses" variant="flat">
                <v-list-item :title="event.ClassName">
               <template v-slot:subtitle>
                        <v-icon class="me-1 pb-1" color="black" icon="$calendar" size="18"></v-icon>
                        {{ formatDate(event.ClassDate) }} 
                      </template>
                </v-list-item>
                
                <v-list-item>
                    <v-btn variant="tonal" size="small" density="compact" block color="#927396"
                    @click="getSelectedEvent(event)">  
                    View More Details
                    </v-btn>
                </v-list-item>
             </v-card>
         </v-sheet>
    </v-card>

    <v-dialog v-model="showClassInfo" persistent max-width="600px">
        <v-card>
          <v-card-item :title="selectedEvent.ClassName">
            <template v-slot:append>
              <span class="close" @click="showClassInfo = false">&times;</span>
            </template>
            <p style="color: grey; line-height: 1.0; font-size: 15px;" > {{selectedEvent.ClassDescription }}</p>
          </v-card-item>
          <v-card-item>
            <template v-slot:prepend>
              <v-icon class="me-1 pb-1" color="black" icon="$calendar"></v-icon>
              {{ formatDate(selectedEvent.ClassDate) }}
            </template>
          </v-card-item>
          <v-card-item>
            <template v-slot:prepend>
              <v-icon class="me-1 pb-1" color="black" icon="$clock"></v-icon>
              {{ formatTime(selectedEvent.StartTime) }} - {{ formatTime(selectedEvent.EndTime) }}
            </template>
          </v-card-item>
          <v-card-item>
            <v-divider></v-divider>
          </v-card-item>
          <v-card-item>
            <template v-slot:prepend>
              <v-icon class="me-1 pb-1" color="black" icon="$location"></v-icon>
              {{ selectedEvent.BuildingName }} - {{ selectedEvent.RoomNumber }}
            </template>
          </v-card-item>
          <v-card-item>
            <v-tooltip text="Open In Google Maps" location="bottom">
              <template v-slot:activator="{ props }">
                <a :href="`https://maps.google.com/?q=${encodeURIComponent(selectedEvent.BuildingName)}+${encodeURIComponent(selectedEvent.LocationAddress)}`" 
                target="_blank" style="color: inherit; text-decoration: none;" v-bind="props">
                  {{ selectedEvent.LocationAddress }}
                </a>
              </template>
            </v-tooltip>
          </v-card-item>
          <v-card-item>
            <v-divider></v-divider>
          </v-card-item>
          <v-card-item>
            <template v-slot:prepend>
              <v-icon v-if="selectedEvent.MatsProvided" class="me-1 pb-1" color="#87ab87" icon="$yes"></v-icon>
              <v-icon v-else class="me-1 pb-1" color="red" icon="$no"></v-icon>
              Mats Provided
            </template>
          </v-card-item>
          <v-card-item>
            <template v-slot:prepend>
              <v-icon class="me-1 pb-1" color="black" icon="$instructor"></v-icon>
              <strong>Instructor: </strong>{{ selectedEvent.FirstName }}  {{ selectedEvent.LastName }}
            </template>
          </v-card-item>
          <v-card-item>
            <v-divider></v-divider>
          </v-card-item >
          <v-list-item>
            <v-btn block v-if="!selectedAttendanceID" @click="signUp" size="small" color="#769cbc"> 
              Add this class to my schedule
            </v-btn>
            <div class="d-flex align-center justify-center">
            <v-list-item-title class="text-button" v-if="selectedAttendanceID" 
            style="color: #769cbc; font-weight: bold"> This class is already in your schedule. </v-list-item-title>
            </div> 
            <!-- <v-btn block v-if="selectedAttendanceID" @click="confirmRemove = true" variant="tonal" size="small"
          color="#769cbc"> 
            Remove From My Schedule
          </v-btn> -->
        </v-list-item>
        </v-card>
      </v-dialog>
      <v-dialog v-model="confirmRemove" persistent max-width="400px">
        <v-card>
          <v-card-title>Confirm Delete</v-card-title>
          <v-card-text>Are you sure you want to remove this class from your schedule? </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="#769cbc" variant="tonal"  text @click="confirmRemove = false">Cancel</v-btn>
            <v-btn color="#df2525" variant="tonal" text @click="removeFromSchedule">Confirm</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-snackbar v-model="confirmationDialog.show" timeout="4000">
        {{ confirmationDialog.text }}
        <template v-slot:actions>
          <v-btn color="#bfc5e8" variant="text" @click="confirmationDialog.show = false">
            Close
          </v-btn>
          
        </template>
      </v-snackbar>
</template>

<script>
  import { useYogaClassesStore } from '@/stores/YogaClassStore.js'; 
  import { useClassAttendanceStore } from '@/stores/ClassAttendanceStore';
  import { useUserStore } from '@/stores/UserStore';


export default {
    props: ['classHistory', 'userSchedule'],
    data() {
        return {
            recommendedClasses: [],
            upcomingClasses: [], 
            showClassInfo: false,
            selectedEvent: {}, 
            selectedAttendanceID: 0,
            user: null, 
            confirmationDialog: { text: '', show: false},
            confirmRemove: false,
        }
    },
    methods: {
        async getUpcomingClasses() {
            await useUserStore().getUser();
            this.user = useUserStore().user[0];
            const yogaClassStore = useYogaClassesStore();
            await yogaClassStore.getClasses();
            const yogaClasses = yogaClassStore.filteredClasses;
            const currentDate = new Date();
            console.log(yogaClasses);
            yogaClasses.forEach(item => { 
                if (new Date(item.ClassDate) > currentDate) {
                    this.upcomingClasses.push(item);
                }
            });
            console.log(this.upcomingClasses);
            console.log(this.classHistory);
            this.getRecommendedClasses();
        },
        async signUp() {
            this.userSchedule.push(this.selectedEvent);
            this.userSchedule.sort((a, b) => new Date(a.ClassDate) - new Date(b.ClassDate));

            await useClassAttendanceStore().addAttendanceRecord(this.user.UserID, this.selectedEvent.ClassID);
            this.showClassInfo = false;
            this.confirmationDialog = { text: 'Class successfully added to schedule', show: true};
        },
        async removeFromSchedule() {
            const index = this.userSchedule.findIndex(e => e.ClassID === this.selectedEvent.ClassID);
            this.userSchedule.splice(index, 1);
            await useClassAttendanceStore().deleteAttendanceRecord(this.selectedAttendanceID);
            this.showClassInfo = false;
            this.confirmRemove = false;
            this.confirmationDialog = { text: 'Class successfully removed from schedule', show: true};
        },
        async getSelectedEvent(event) {
            this.selectedEvent = event;
            await useClassAttendanceStore().fetchAttendanceRecords(null, this.user.UserID, this.selectedEvent.ClassID, null, null);
            if ( useClassAttendanceStore().attendanceRecords[0]) { this.selectedAttendanceID = useClassAttendanceStore().attendanceRecords[0].AttendanceID; }
            else { this.selectedAttendanceID = 0; }
            this.showClassInfo = true;
        },
        getRecommendedClasses() {
            const today = new Date();
            const thirtyDaysAgo = new Date(today.setDate(today.getDate() - 30));

            const instructorCount = {};
            const timeRanges = {
                morning: { count: 0, range: [6, 11] },
                midday: { count: 0, range: [11, 13] },
                afternoon: { count: 0, range: [13, 17] },
                evening: { count: 0, range: [17, 21] }
            };

            this.classHistory.forEach(cls => {
                const classDate = new Date(cls.ClassDate);
                const hour = parseInt(cls.StartTime.substring(0, 2));
            
                console.log(hour);
                // Count by instructor
                if (classDate >= thirtyDaysAgo) {
                    if (instructorCount[cls.InstructorID]) {
                        instructorCount[cls.InstructorID].count++;
                        instructorCount[cls.InstructorID].classes.push(cls.ClassID);
                    } 
                    else { instructorCount[cls.InstructorID] = { count: 1, classes: [cls.ClassID] }; }
                }
                // Count by time range
                if (hour >= 6 && hour < 11) timeRanges.morning.count++;
                if (hour >= 11 && hour < 13) timeRanges.midday.count++;
                if (hour >= 13 && hour < 17) timeRanges.afternoon.count++;
                if (hour >= 17 && hour < 21) timeRanges.evening.count++;
            });

            // Recommend by instructor
            const qualifyingInstructors = Object.keys(instructorCount).filter(id => instructorCount[id].count >= 3);
            this.upcomingClasses.forEach(cls => {
                if (qualifyingInstructors.includes(cls.InstructorID.toString())) { this.recommendedClasses.push(cls); }
            });

            Object.entries(timeRanges).forEach(([key, value]) => {
                console.log(value);
                if (value.count >= 4) {
                    this.upcomingClasses.forEach(cls => {
                        const hour = parseInt(cls.StartTime.substring(0, 2));
                        if (hour >= value.range[0] && hour < value.range[1]) { this.recommendedClasses.push(cls); }
                    });
                }});


            const titleKeywords = {};
                this.classHistory.forEach(cls => {
                    const words = cls.ClassName.split(' ');
                    words.forEach(word => {
                        word = word.toLowerCase();
                        if (word !== 'yoga' && word.length > 3) {
                            if (titleKeywords[word]) { titleKeywords[word]++; } 
                            else { titleKeywords[word] = 1; }
                        }
                    });
                });

            const popularWords = Object.keys(titleKeywords).filter(word => titleKeywords[word] >= 2);
            this.upcomingClasses.forEach(cls => {
                const classWords = cls.ClassName.toLowerCase().split(' ');
                if (popularWords.some(pw => classWords.includes(pw))) { this.recommendedClasses.push(cls); }
            });

            const uniqueClassIds = new Set();
            this.recommendedClasses = this.recommendedClasses.filter(cls => {
                const isUnique = !uniqueClassIds.has(cls.ClassID);
                uniqueClassIds.add(cls.ClassID);
                return isUnique;
            });

            // Sort by class date
            this.recommendedClasses.sort((a, b) => new Date(a.ClassDate) - new Date(b.ClassDate));

            this.recommendedClasses = this.recommendedClasses.map(cls => ({
                ...cls,
                expand: false 
            }));

        },
        formatDate(dateString) {
          return new Date(dateString.substr(0, 19)).toLocaleDateString('en-US', {year: 'numeric',  month: 'long',   day: 'numeric' });
        }, 
        formatTime(timeString) {
          const [hours24, minutes] = timeString.split(':');
          const hours24Num = parseInt(hours24, 10);
          return `${hours24Num % 12 || 12}:${minutes.padStart(2, '0')} ${hours24Num >= 12 ? 'PM' : 'AM'}`;
        },
    }, 
    mounted() {
        this.getUpcomingClasses();
    }
}
</script>

<style>
.close {
    color: #aaa;
    float: right;
    font-size: 2rem;
    font-weight: bold;
  }
  
  .close:hover,
  .close:focus {
    color: black;
    text-decoration: none;
    cursor: pointer;
  }
  </style>