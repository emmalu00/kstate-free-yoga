<template>
    <v-sheet class="pa-2 ma-2" style="background-color: #f5f5f5">
    
        <v-form ref="form" class="add-form">
            <v-row>
                <v-col class="d-flex justify-space-between align-center">
                  <h2>Add Class</h2>
                  <span class="close" @click="closeForm">&times;</span>
                </v-col>
              </v-row>
    
            <v-row>
                <v-col>
                    <v-text-field
                    label="Class Name" variant="outlined"
                    v-model="className" :rules="rules" >
                    </v-text-field>
                </v-col>
            </v-row> 
         
            <v-row>
                <v-col cols="12" md="4">
                    <v-text-field
                    label="Date in menu" variant="solo-filled" density="compact"
                    :active="helpme" v-model="selectedDate" :rules="rules"
                    readonly>
                    <v-menu
                        v-model="helpme"
                        :close-on-content-click="false"
                        activator="parent"
                        transition="scale-transition"
                    >
                        <v-date-picker
                        v-if="helpme"
                        v-model="selectedDate"
                        :min="minDate"
                        label="Select a date"
                        ></v-date-picker>
                    </v-menu>
                    </v-text-field>
                </v-col>
                <v-spacer></v-spacer>
                <v-col cols="12" md="3">
                    <v-combobox 
                    label="Start Time" variant="solo-filled" density="compact"
                    :items="getTimes"  v-model="selectedStart" :rules="rules"
                    ></v-combobox>
                </v-col>
                
                <v-col cols="auto" >to</v-col>
                
                <v-col cols="12" md="3">
                    <v-combobox
                    density="compact"
                    :items="getTimes"
                    v-model="selectedEnd"
                    :rules="rules"
                    variant="solo-filled"
                    label="End Time"
                    ></v-combobox>                
                </v-col>
            </v-row>

            <v-row style="margin-top: 2%">
                <v-col cols="auto">
                    <v-icon icon="fa-solid fa-user"> </v-icon>
                </v-col>
                <v-col cols="6">
                    <v-combobox
                    :rules="rules"
                    :items="teachers"
                    item-title="text"
                    item-value="id"
                    label="Instructor"
                    variant="outlined"
                    density="compact"
                    v-model="selectedInstructor">
                    </v-combobox>
                    <v-btn block variant="outlined" density="compact"
                    style="background-color: #cbacc1" @click="openInstructorDropdown"> 
                        Add New Instructor
                    </v-btn>
                    <v-row v-if="showDropdown">
                        <v-col cols="12">
                            <v-text-field label="First Name"
                            v-model="newInstructor.firstName"></v-text-field>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field label="Last Name"
                            v-model="newInstructor.lastName"></v-text-field>
                        </v-col>
                        
                        <v-col cols="12">
                            <v-select
                                :items="options"
                                item-title="text"
                                item-value="id"
                                label="Certified?"
                                v-model="newInstructor.certified" ></v-select>
                        </v-col>
                        <v-row justify="space-between">
                            <v-col>
                                <v-btn block density="compact"
                                style="background-color: #927396" @click="addInstructortoClass">Add Instructor </v-btn>
                            </v-col>
                            <v-col>
                                <v-btn block  density="compact"
                                style="background-color: #f5f5f5" @click="showDropdown = false"> Close </v-btn>
                            </v-col>
                        </v-row>
                    </v-row>
                    
                </v-col>
            </v-row>
            
            
            <v-row style="margin-top: 2%">
                <v-col cols="auto">
                    <v-icon icon="fa-solid fa-location-dot"> </v-icon>
                </v-col>
                <v-col cols="6">
                    <v-combobox
                    :rules="rules"
                    :items="locations"
                    item-title="text"
                    item-value="id"
                    label="Location"
                    variant="outlined"
                    density="compact"
                    v-model="selectedLocation">
                    </v-combobox>
                    <v-btn block variant="outlined" density="compact"
                    style="background-color: #cbacc1"> 
                        Add New Location
                    </v-btn>
                </v-col>
            </v-row>

            <v-row style="margin-top: 4%; width: 60%">
                <v-col>
                    <v-select
                    variant="outlined"
                    density="compact"
                    :items="options"
                    item-title="text"
                    item-value="id"
                    label="Mats Provided?"
                    v-model="matsAvailable">
                    </v-select>
                </v-col>
            </v-row> 

            <v-row style="width: 60%">
                <v-col>
                    <v-textarea
                    label="Write a brief description here."
                    v-model="classDescription"
                    row-height="25"
                    rows="2"
                    variant="outlined"
                    auto-grow
                    shaped
                    required
                    ></v-textarea>
                </v-col>
            </v-row> 

            <v-row justify="space-between">
                <v-col>
                    <v-btn @click="addClass" block
                    style="background-color: #927396">Add Class </v-btn>
                </v-col>
                <v-col>
                    <v-btn @click="reset" block
                    style="background-color: #f5f5f5"> Reset </v-btn>
                </v-col>
            </v-row>

            
        </v-form>
    </v-sheet>
    
  </template>
  
  <script>
  import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
  import { useInstructorsStore } from '@/stores/Instructors';

  export default {
    data() {
      return {
        showDropdown: false,
        selectedStart: null,
        selectedEnd: null,
        selectedLocation: null, 
        selectedInstructor: null, 
        selectedDate: null, // Bind the selected date to this data property
        minDate: new Date().toISOString().substr(0, 10), // Today's date in YYYY-MM-DD format
        helpme: false,
        className: null, 
        matsAvailable: true,
        classDescription: null,
        newInstructor: { firstName: null, lastName: null, certified: true },
        locations: [], 
        teachers: [],
        certified: true,
        rules: [
        v => !!v || 'Required',
        ],
        options: [ 
            {text: 'Yes', id: true},
            {text: 'No', id: false}
        ]
      }
    },
    methods: {
    async fetchEvents() {
        const yogaClassesStore = useYogaClassesStore();
        await yogaClassesStore.fetchTeacherNames();
        await yogaClassesStore.fetchLocations();
        this.teachers = yogaClassesStore.teacherNames.map((teacher) => {
            return {
                text: `${teacher.FirstName} ${teacher.LastName}`,
                id: teacher.InstructorID
            }
        });
        this.locations = yogaClassesStore.locations.map((location) => {
            return {
                text: `${location.BuildingName}, ${location.RoomNumber}`,
                id: location.LocationID
            }
        });
        this.newInstructor = { firstName: null, lastName: null, certified: true }

    },
    reset () {
        this.$refs.form.reset();
    },
    async addClass() {
        const { valid } = await this.$refs.form.validate()
        if (valid) {
            this.$emit('addingClass', {
            className: this.className,
            startTime: this.convertTimeFormat(this.selectedStart),
            endTime: this.convertTimeFormat(this.selectedEnd),
            classDate: this.convertDateFormat(this.selectedDate),
            instructorID: this.selectedInstructor.id,
            locationID: this.selectedLocation.id,
            matsAvailable: this.matsAvailable,
            classDescription: this.classDescription
        });
        this.$emit('close'); 
        }
    }, 
    closeForm() {
        this.$emit('close'); // Emit an event to close the modal
    },
    convertTimeFormat(initialTime) {
      const [time, period] = initialTime.split(' ');
      let [hours, minutes] = time.split(':');
      if (period === 'PM' && hours !== '12') {
        hours = parseInt(hours, 10) + 12; // Convert PM times to 24-hour format, except for 12 PM
      } else if (period === 'AM' && hours === '12') {
        hours = '00'; // Convert 12 AM to 00
      }
      hours = hours.toString().padStart(2, '0');
      minutes = minutes.padStart(2, '0');
      return `${hours}:${minutes}:00`;
    },
    convertDateFormat (initialDate) {
        const date = new Date(initialDate);
        const year = date.getFullYear();
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');
        return `${year}-${month}-${day}`;
    },
    async addInstructortoClass() {
        this.newInstructor.firstName.charAt(0).toUpperCase();
        this.newInstructor.lastName.charAt(0).toUpperCase();
       
        const instructorsStore = useInstructorsStore();
        await instructorsStore.addInstructor(this.newInstructor);

        this.fetchEvents();
        this.showDropdown = false;
        this.selectedInstructor = {
            text: `${this.newInstructor.firstName} ${this.newInstructor.lastName}`, 
            id: instructorsStore.instructorID};

    },
    openInstructorDropdown() {
        this.showDropdown = true,
        this.selectedInstructor = null;
    }
    },
    computed: {
    getTimes()
    {
        const times = [];
        const startTime = 6; // Start time in hours (6:00 AM)
        const endTime = 21; // End time in hours (9:00 PM)
        const increment = 15; // Increment in minutes
        for (let hour = startTime; hour <= endTime; hour++) {
        for (let minute = 0; minute < 60; minute += increment) {
            const hourFormatted = hour % 12 === 0 ? 12 : hour % 12;
            const minuteFormatted = minute.toString().padStart(2, '0');
            const amPm = hour < 12 ? 'AM' : 'PM';
            const time = `${hourFormatted}:${minuteFormatted} ${amPm}`;

            times.push(time);
        }
      }
      return times;
    },
    
  },
  mounted() {
    this.fetchEvents(); 
  },
  }
  </script>
  
  <style scoped>
  .add-form {
    padding: 2%;

    
  }
  
  .input-container {
    display: flex; 
    align-items: center;
    gap: 10px;
    margin-top: 5px;
  }
  

  .close {
    color: #aaa;
    float: right;
    font-size: 28px;
    font-weight: bold;
  }
  
  .close:hover,
  .close:focus {
    color: black;
    text-decoration: none;
    cursor: pointer;
  }

  :deep(.v-field__overlay) {
    background-color: white !important;
  }

  </style>
  