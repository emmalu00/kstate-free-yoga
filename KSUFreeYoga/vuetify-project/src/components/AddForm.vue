<template>
    <v-sheet class="pa-2 ma-2">
        <v-form ref="form"class="add-form">
            <h2> Add Class </h2>
    
            <v-text-field
            label="Class Name"
            v-model="className"
            :rules="rules"
            required
            variant="outlined"
            density="compact">
            </v-text-field>

            <v-text-field
            label="Date in menu"
            :active="helpme"
            v-model="selectedDate"
            :rules="rules"
            readonly
            required
            variant="outlined"
            density="compact"
            >
                <v-menu
                v-model="helpme"
                :close-on-content-click="false"
                activator="parent"
                transition="scale-transition">
                    <v-date-picker
                    v-if="helpme"
                    v-model="selectedDate"
                    :min="minDate"
                    label="Select a date"
                    ></v-date-picker>
                </v-menu>
            </v-text-field>

            <v-row>
                <v-col cols="5">
                    <v-combobox
                    density="compact"
                    :items="getTimes"
                    v-model="selectedStart"
                    :rules="rules"
                    variant="outlined"
                    label="Start Time">
                    </v-combobox>
                </v-col>
                <v-col cols="auto" style="font-weight: bold; margin-top: 2%; padding: 2% 0%"> to </v-col>
                <v-col cols="5">
                    <v-combobox
                    density="compact"
                    :items="getTimes"
                    v-model="selectedEnd"
                    :rules="rules"
                    variant="outlined"
                    label="End Time">
                    </v-combobox>                
                </v-col>
            </v-row> 

            <!-- <v-row>
                <v-col>
                    Start Time
                </v-col>
                <v-col cols="3">
                    <v-select v-model="startH" density="compact" :items="hours" :rules="rules" variant="outlined"> </v-select>
                </v-col>
                <v-col cols="auto" style="font-weight: bold; margin-top: 2%; padding: 2% 0%"> : </v-col>
                <v-col cols="3">
                    <v-select v-model="startM" density="compact" :items="minutes" :rules="rules" variant="outlined"> </v-select>
                </v-col>
                <v-col cols="3">
                    <v-select v-model="startAMPM" density="compact" :items="AMPM" :rules="rules" variant="outlined"> </v-select>  
                </v-col>
            </v-row> 

            <v-row>
                <v-col>
                    End Time
                </v-col>
                <v-col cols="3">
                    <v-select v-model="endH" density="compact" :items="hours" :rules="rules" variant="outlined"> </v-select>
                </v-col>
                <v-col cols="auto" style="font-weight: bold; margin-top: 2%; padding: 2% 0%"> : </v-col>
                <v-col cols="3">
                    <v-select v-model="endM" density="compact" :items="minutes" :rules="rules" variant="outlined"> </v-select>
                </v-col>
                <v-col cols="3">
                    <v-select v-model="endAMPM" density="compact" :items="AMPM" :rules="rules" variant="outlined"> </v-select>  
                </v-col>
            </v-row> -->

            <v-row>
                <v-col cols="8"> 
                    <v-select
                    :rules="rules"
                    :items="teachers"
                    item-title="text"
                    item-value="id"
                    label="Instructor"
                    variant="outlined"
                    density="compact"
                    v-model="selectedInstructor">
                    </v-select>
                </v-col>
                <v-col cols="4">
                    <v-btn variant="outlined"> 
                        Add New ...
                    </v-btn>
                </v-col>
            </v-row>

            <v-row>
                <v-col cols="8"> 
                    <v-select
                    :rules="rules"
                    :items="locations"
                    item-title="text"
                    item-value="id"
                    label="Location"
                    variant="outlined"
                    density="compact"
                    v-model="selectedLocation"
                    >
                    </v-select>
                </v-col>
                <v-col cols="4">
                    <v-btn variant="outlined"> 
                        Add New ...
                    </v-btn>
                </v-col>
            </v-row>

            <v-select
            variant="outlined"
            density="compact"
            :items="matsOptions"
            item-title="text"
            item-value="id"
            label="Mats Provided?"
            v-model="matsAvailable" >
            </v-select>
    

            <p> Description </p>
            <v-textarea
            label="Write a brief description here."
            v-model="classDescription"
            row-height="25"
            rows="2"
            variant="outlined"
            auto-grow
            shaped
            ></v-textarea>

            <v-btn @click="addClass" block>Add Class </v-btn>
            <v-btn @click="reset" block> Reset </v-btn>
            <v-btn @click="closeForm" block> Close </v-btn>
            
        </v-form>
    </v-sheet>
  </template>
  
  <script>
  import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
  
  export default {
    data() {
      return {
        selectedStart: null,
        selectedEnd: null,
        AMPM: ['AM', 'PM'],
        startH: null,
        startM: null,
        startAMPM: null, 
        endH: null,
        endM: null,
        endAMPM: null, 
        selectedLocation: null, 
        selectedInstructor: null, 
        selectedDate: null, // Bind the selected date to this data property
        minDate: new Date().toISOString().substr(0, 10), // Today's date in YYYY-MM-DD format
        helpme: false,
        className: null, 
        matsAvailable: null,
        classDescription: null,
        locations: [], 
        teachers: [],
        rules: [
        v => !!v || 'Required',
        ],
        matsOptions: [ 
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
            classDate: this.selectedDate,
            instructorID: this.selectedInstructor,
            locationID: this.selectedLocation,
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
      // Split the time and the period (AM/PM)
      const [time, period] = initialTime.split(' ');
      let [hours, minutes] = time.split(':');

      // Convert hours to 24-hour format if necessary
      if (period === 'PM' && hours !== '12') {
        hours = parseInt(hours, 10) + 12; // Convert PM times to 24-hour format, except for 12 PM
      } else if (period === 'AM' && hours === '12') {
        hours = '00'; // Convert 12 AM to 00
      }

      // Ensure hours and minutes are in two-digit format
      hours = hours.toString().padStart(2, '0');
      minutes = minutes.padStart(2, '0');

      // Return the converted time
      return `${hours}:${minutes}:00`;
    },
    convertTime(hour, minute, AMorPM)
    {
        // let hourInt = parseInt(hour, 10);
        // if (hourInt === 12) {
        //     hourInt = AMorPM.toUpperCase() === 'AM' ? 0 : 12;
        // }
        // else if (AMorPM.toUpperCase() === 'PM') {
        //     hourInt += 12;
        // }
        // let formattedHour = hourInt.toString().padStart(2, '0');
        // let formattedMinute = minute.toString().padStart(2, '0');
        // return `${formattedHour}:${formattedMinute}:00`;
    },
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
          // Formatting the hour for AM/PM format
          const hourFormatted = hour % 12 === 0 ? 12 : hour % 12;
          // Formatting minutes to always be two digits
          const minuteFormatted = minute.toString().padStart(2, '0');
          // Determining AM or PM
          const amPm = hour < 12 ? 'AM' : 'PM';
          // Constructing the time string
          const time = `${hourFormatted}:${minuteFormatted} ${amPm}`;

          times.push(time);
        }
      }
      return times;

    },
    hours() {
        const times = [];
        for (let hour = 6; hour <= 20; hour++) {
            const displayHour = hour % 12 === 0 ? 12 : hour % 12;
            times.push(`${displayHour}`);
        }
        return times;
    },
    minutes() {
        const times = [];
        for (let min = 0; min < 60; min += 5) {
            const displayMinute = min.toString().padStart(2, '0');
            times.push(`${displayMinute}`);
        }
        return times;
    },
  },
  mounted() {
    this.fetchEvents(); // Call fetchTeacherNames when the component is mounted
  },
  }
  </script>
  
  <style scoped>
  .add-form {
    padding: 2%
    
  }
  

  
  </style>
  