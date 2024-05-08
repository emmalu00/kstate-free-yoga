<template>
    <v-card >
        <v-card-title class="wrap-content" style="white-space: normal; font-size: 16px;font-size: 16px; line-height: 1rem;">
            Find  Upcoming Classes That Suit Me  <br>
        </v-card-title>
        <v-card-subtitle class="wrap-content" style="font-size: 14px; line-height: 1rem;">
            Custom suggestions based on your needs & preferences
        </v-card-subtitle>        <v-card-actions> 
            <v-btn density="compact" variant="tonal" color="#BF98B2" @click="finderForm = true"> Find </v-btn>
        </v-card-actions>
        <v-dialog v-model="finderForm" persistent max-width="600px">
            <v-card color="#BF98B2" variant="flat">
                <v-form ref="form" class="pa-3">
                    <v-card-item title="Find Upcoming Classes Best Suited For You"
                    style="color: black !important; font-size:20px; margin-bottom: 2%">
                        <template v-slot:append>
                          <span class="close" @click="finderForm = false">&times;</span>
                        </template>
                      </v-card-item>
                    <v-card-text> What is your current yoga experience level? </v-card-text>
                    <v-select :items="experienceOptions" item-title="text" item-value="id"
                    variant="solo-filled" density="compact" v-model="formAnswers.experienceLevel" >
                    </v-select>

                    <v-card-text> What type of yoga classes are you interested in? </v-card-text>
                    <v-select :items="classTypeOptions" item-title="text" multiple
                    variant="solo-filled" density="compact" v-model="formAnswers.classTypes" >
                    </v-select>

                    <v-card-text> What time(s) of day are you available for yoga classes?  </v-card-text>
                    <v-select :items="timesOptions" item-title="text" item-value="id" multiple
                    variant="solo-filled" density="compact" v-model="formAnswers.times">
                    </v-select>

                    <v-card-text> Do you prefer classes with provided yoga mats? </v-card-text>
                    <v-select :items="matsOptions" item-title="text" item-value="id"
                    variant="solo-filled" density="compact" v-model="formAnswers.matOption">
                    </v-select>

                    <v-divider style="margin: 2%"></v-divider>
                    <v-row justify="space-between">
                        <v-col>
                            <v-btn @click="finderForm = false" block variant="tonal"> Cancel </v-btn>        
                        </v-col>
                        <v-col>
                            <v-btn @click="recommendClasses" block color="#ece1e9" :disabled="!enableFindButton"> Find Classes </v-btn>
                        </v-col>
                    </v-row>
                </v-form>
            </v-card>
        </v-dialog>
        <v-dialog v-model="recommendClassesForm" persistent max-width="600px">
            <v-sheet style="overflow-y: auto;">
                <v-card-item title="Upcoming Classes You May Enjoy">
                    <template v-slot:append>
                      <span class="close" @click="recommendClassesForm = false">&times;</span>
                    </template>
                  </v-card-item>
                  <v-card-text class=wrap-content v-if="!recommendedClasses.length">
                     Oh no! There are no upcoming classes that align with your needs and preferences. Please come back later or modify your needs and preferences.
                  </v-card-text>
                  <v-card-text class=wrap-content v-if="recommendedClasses.length === 1">
                    Ahh dang! There is only one upcoming class that aligns with your needs and preferences. 
                 </v-card-text>
                 <v-card v-for="event in recommendedClasses"  variant="flat" color="#bfaec2">
                    <v-card-item :title="event.ClassName">
                        <template v-slot:subtitle>
                                {{ formatDate(event.ClassDate) }} | {{ event.BuildingName }} - {{ event.RoomNumber }}
                            </template>
                        </v-card-item>
                        
                        <v-card-item>
                            <v-btn variant="tonal"  density="compact" block 
                            @click="getSelectedEvent(event)">  
                            View More Details
                            </v-btn>
                        </v-card-item>
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
                      <v-card-item v-if="user">
                        <v-divider></v-divider>
                      </v-card-item >
                      <v-list-item>
                        <v-btn block v-if="!selectedAttendanceID && user" @click="signUp" size="small" color="#769cbc"> 
                          Add this class to my schedule
                        </v-btn>
                        <div class="d-flex align-center justify-center">
                        <v-list-item-title class="text-button" v-if="selectedAttendanceID" 
                        style="color: #769cbc; font-weight: bold"> This class is already in your schedule. </v-list-item-title>
                        </div> 
                        
                    </v-list-item>
                    </v-card>
                  </v-dialog>

                
                <v-list-item>
                    <v-btn variant="tonal" block @click="recommendClassesForm = false" size="small"> Close </v-btn>
                </v-list-item>
            </v-sheet>
        </v-dialog>
    </v-card>
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
import { useUserStore } from '@/stores/UserStore';
import { useClassAttendanceStore } from '@/stores/ClassAttendanceStore';


export default {
    data() {
        return {
            finderForm: false,
            recommendClassesForm: false,
            experienceOptions: [
                {text: 'Beginner', id: 1}, 
                {text: 'Experienced Beginner', id: 2},
                {text: 'Intermediate/Advanced', id: 3} 
            ], 
            classTypeOptions: [
                {text: 'Slow-paced/restorative classes', id: 1}, 
                {text: 'Basic yoga practice', id: 2}, 
                {text: 'Challenging classes', id: 3}, 
                {text: 'I am  not sure', id: 4},
            ], 
            timesOptions: [
                {text: 'Morning (6:00AM - 11:00AM)', id: 1}, 
                {text: 'Lunchtime/Midday (11:00AM - 1:00PM)', id: 2},
                {text: 'Afternoon (1:00PM - 5:00PM)', id: 3}, 
                {text: 'Evening (5:00PM - 9:00PM)', id: 4} 
            ],
            matsOptions: [
                {text: 'I prefer classes that provide yoga mats', id: 1}, 
                {text: 'I can bring my own mats to classes that do not provide them.', id: 2},
            ], 
            formAnswers: { 
                experienceLevel: null, 
                classTypes: [],
                times: [], 
                matOption: null, 
            },
            futureYogaClasses: [], 
            recommendedClasses: [], 
            selectedAttendanceID: 0, 
            user: null, 
            selectedEvent: {}, 
            showClassInfo: false, 
            confirmationDialog: { text: '', show: false},

        }
    }, 
    methods: {
        async getFutureClasses() {
            const token = localStorage.getItem('userToken');
            if (token) {
                await useUserStore().getUser();
                this.user = useUserStore().user[0];
            }
            
            const yogaClassesStore = useYogaClassesStore();
            await yogaClassesStore.getClasses();

            const currentDate = new Date();
            this.futureYogaClasses = yogaClassesStore.filteredClasses.filter(yogaClass => {
                const classTime = new Date(`${yogaClass.ClassDate.substring(0, 10)}T${yogaClass.StartTime}`);
                return classTime > currentDate;
            });

           
        },
        async getSelectedEvent(event) {
            this.selectedEvent = event;
            if (this.user != null)
            {
                await useClassAttendanceStore().fetchAttendanceRecords(null, this.user.UserID, this.selectedEvent.ClassID, null, null);
                if ( useClassAttendanceStore().attendanceRecords[0]) { this.selectedAttendanceID = useClassAttendanceStore().attendanceRecords[0].AttendanceID; }
                else { this.selectedAttendanceID = 0; }
            }
            
            this.showClassInfo = true;
        },
        async signUp()
        {
            await useClassAttendanceStore().addAttendanceRecord(this.user.UserID, this.selectedEvent.ClassID);
            this.showClassInfo = false;
            this.confirmationDialog = { text: 'Class successfully added to schedule', show: true};

        },
        async isRecordExists(classID) {
            console.log(classID);
            console.log(this.user.UserID);
            if (this.user != null)
            {
                await useClassAttendanceStore().fetchAttendanceRecords(null, this.user.UserID, classID, null, null);
                const currentRecord = useClassAttendanceStore().attendanceRecords[0];
                console.log(currentRecord);
                if (currentRecord != null) 
                { 
                    console.log(useClassAttendanceStore().attendanceRecords[0]);
                    this.selectedAttendanceID = useClassAttendanceStore().attendanceRecords[0].AttendanceID;
                    return true; 
                }
                else 
                {   
                    console.log("help");
                    this.selectedAttendanceID = 0; 
                    return false;
                }
            }
        },
        async recommendClasses() {
            const { valid } = await this.$refs.form.validate();

            if (valid) {
                this.recommendedClasses = [];

            // filter by experience level
            if (this.formAnswers.experienceLevel == 1) { this.getEasyYogaClasses(); }
            else if (this.formAnswers.experienceLevel == 3) { this.getHardYogaClasses() }

            // filter by classType
            const classTypeFilters = this.formAnswers.classTypes.map(type => type.toLowerCase());
            if (classTypeFilters.includes('slow-paced/restorative classes')) { this.getEasyYogaClasses(); }
            if (classTypeFilters.includes('basic yoga practice') || classTypeFilters.includes('i am not sure')) {
                this.recommendedClasses.push(
                ...this.futureYogaClasses.filter(yogaClass =>
                    yogaClass.ClassName.toLowerCase().includes('noontime yoga') ||
                    yogaClass.ClassDescription.toLowerCase().includes('noontime yoga') ||
                    yogaClass.ClassName.toLowerCase().includes('fundamentals') ||
                    yogaClass.ClassDescription.toLowerCase().includes('fundamentals') ||
                    yogaClass.ClassName.toLowerCase().includes('basic') ||
                    yogaClass.ClassDescription.toLowerCase().includes('basic') ||
                    yogaClass.ClassName.toLowerCase().includes('simple') ||
                    yogaClass.ClassDescription.toLowerCase().includes('simple')
                ));
            }  
            if (classTypeFilters.includes('challenging classes')) {
                if (this.formAnswers.experienceLevel == 1) {
                    this.recommendedClasses.push(
                        ...this.futureYogaClasses.filter(yogaClass =>
                            yogaClass.ClassName.toLowerCase().includes('noontime yoga') ||
                            yogaClass.ClassDescription.toLowerCase().includes('noontime yoga')
                        ).filter(yogaClass => !this.recommendedClasses.includes(yogaClass))
                    );
                } else {
                    this.getHardYogaClasses();
                }
            }

            // filter by time options
            const timeRanges = {
               1:  { start: 6, end: 11 },
               2:  { start: 11, end: 13 },
               3:  { start: 13, end: 17 },
               4:  { start: 17, end: 21 }
            };
            this.recommendedClasses = this.recommendedClasses.filter(yogaClass => {
                const classStartTime = parseInt(yogaClass.StartTime.substring(0, 2)); 
                return this.formAnswers.times.some(time => {
                    const timeRange = timeRanges[time]; 
                    return classStartTime >= timeRange.start && classStartTime < timeRange.end; 
                });
            });
            console.log(this.recommendedClasses);

            // filter by mat preference
            if (this.formAnswers.matOption == 1) {
                this.recommendedClasses = this.recommendedClasses.filter(yogaClass => 
                {
                    return yogaClass.MatsProvided == true;
                });
            }

            // Remove duplicates based on ClassID
            const uniqueClassesMap = this.recommendedClasses.reduce((map, obj) => {
                map.set(obj.ClassID, obj);
                return map;
            }, new Map());
            this.recommendedClasses = Array.from(uniqueClassesMap.values());

            this.recommendedClasses.sort((a, b) => {
                return a.ClassDate.localeCompare(b.ClassDate);
            });

            if (this.user != null) {
                this.recommendedClasses.forEach(cls => {
                    const exists = this.isRecordExists(cls.ClassID);
                    console.log(exists);
                    cls.exists = exists;
                    });
            }

            this.finderForm = false;
            this.recommendClassesForm = true;
            }
            
        }, 
        getEasyYogaClasses() {
            this.recommendedClasses.push(
            ...this.futureYogaClasses.filter(yogaClass =>
                yogaClass.ClassName.toLowerCase().includes('gentle') ||
                yogaClass.ClassDescription.toLowerCase().includes('gentle') ||
                yogaClass.ClassName.toLowerCase().includes('restorative') ||
                yogaClass.ClassDescription.toLowerCase().includes('restorative') ||
                yogaClass.ClassName.toLowerCase().includes('easy') ||
                yogaClass.ClassDescription.toLowerCase().includes('easy') ||
                yogaClass.ClassName.toLowerCase().includes('slow') ||
                yogaClass.ClassDescription.toLowerCase().includes('slow')
            ));
        }, 
        getHardYogaClasses() {
            this.recommendedClasses.push(
            ...this.futureYogaClasses.filter(yogaClass =>
                yogaClass.ClassName.toLowerCase().includes('intermediate') ||
                yogaClass.ClassDescription.toLowerCase().includes('intermediate') ||
                yogaClass.ClassName.toLowerCase().includes('advanced') ||
                yogaClass.ClassDescription.toLowerCase().includes('advanced') ||
                yogaClass.ClassName.toLowerCase().includes('challenge') ||
                yogaClass.ClassDescription.toLowerCase().includes('challenge') ||
                yogaClass.ClassName.toLowerCase().includes('challenging') ||
                yogaClass.ClassDescription.toLowerCase().includes('challenging')
            ));
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
    computed: {
        enableFindButton() {
            return (
                this.formAnswers.experienceLevel != null &&
                this.formAnswers.classTypes.length > 0 &&
                this.formAnswers.times.length > 0 &&
                this.formAnswers.matOption != null
            );
        }
    }, 
    mounted() {
        this.getFutureClasses();
    }
}
</script>

<style scoped>
.wrap-content {
    white-space: normal !important;
}

.close {
    color: #ece1e9;
    float: right;
    font-size: 28px;
    font-weight: bold;
}
    
.close:hover, .close:focus {
    color: black;
    text-decoration: none;
    cursor: pointer;
}
 .v-card-text {
    font-size: 16px !important;
    font-weight: bold;
 }
</style>