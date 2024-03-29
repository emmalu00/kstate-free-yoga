<template>
    <v-sheet class="pa-2 ma-2">
      <v-card
      :style="{ backgroundColor: '#f0f0f0' }">
      <v-form ref="form" class="add-form">
        <h2> Add Class </h2>
  
        <v-text-field
        label="Class Name"
        v-model="className">
        </v-text-field>

 
        <v-text-field
        label="Date in menu"
        :active="helpme"
        v-model="selectedDate"
        readonly
        >
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


        <v-combobox
        :items="timeOptions"
        label="Select a start time"
        v-model="selectedStartTime"
        return-object
        ></v-combobox>

        <v-combobox
        :items="timeOptions"
        label="Select an end time"
        v-model="selectedEndTime"
        return-object
        ></v-combobox>

        


        <p> Mats Available </p>
            <v-btn-toggle
            rounded="0"
            group
            >
                <v-btn value="Yes"> Yes </v-btn>
                <v-btn value="No"> No </v-btn>
            </v-btn-toggle>

        
        <v-text-field
        label="Class Description"
        v-model="classDescription">
        </v-text-field>
    
  
        <v-btn @click="addClass">
          Add Class
        </v-btn>
  
        <v-btn
        @click="reset">
          Reset 
        </v-btn>
        
      </v-form>
      </v-card>
      
    </v-sheet>
  </template>
  
  <script>
  import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
  
  export default {
    data() {
      return {
        selectedStartTime: null,
        selectedEndTime: null,
        selectedDate: null, // Bind the selected date to this data property
        minDate: new Date().toISOString().substr(0, 10), // Today's date in YYYY-MM-DD format
        helpme: false,
        className: null, 
        classLocation: 1, 
        instructor: 1, 
        matsAvailable: true,
        classDescription: null
      }
    },
    methods: {
    async fetchEvents() {
      // api cals for locations and teachers
    },
    reset () {
        this.$refs.form.reset();
    },
    addClass()
    {
    //   this.$emit('addingClass', {
    //   className: this.className, 
    //   startTime: this.convertTimeFormat(this.selectedStartTime), 
    //   endTime: this.convertTimeFormat(this.selectedEndTime), 
    //   classDate: this.selectedDate.toISOString().substring(0, 10),
    //   instructorID: this.instructor, 
    //   locationID: this.classLocation,
    //   matsAvailable: this.matsAvailable, 
    //   classDescription: this.classDescription
    // });

    console.log(this.selectedDate)
        this.$emit('addingClass', {
        className: this.className, 
        startTime: this.selectedStartTime, 
        endTime: this.selectedEndTime, 
        classDate: this.selectedDate,
        instructorID: this.instructor, 
        locationID: this.classLocation,
        matsAvailable: this.matsAvailable, 
        classDescription: this.classDescription
        });
    }, 
    convertTimeFormat(timeString) {
    const parts = timeString.split(' ');
    const timeParts = parts[0].split(':');
    
    let hours = parseInt(timeParts[0], 10);
    const minutes = timeParts[1];
    
    if (parts[1] === 'PM' && hours < 12) {
        hours += 12;
    } else if (parts[1] === 'AM' && hours === 12) {
        hours = 0; 
    }
    

    const formattedHours = hours.toString().padStart(2, '0');
    
    return `${formattedHours}:${minutes}:00`;
    }

  },
    computed: {
    timeOptions() {
      const times = [];
      for (let hour = 6; hour <= 20; hour++) {
        // Adjust for AM/PM format
        const displayHour = hour % 12 === 0 ? 12 : hour % 12;
        const ampm = hour < 12 ? 'AM' : 'PM';
        const lastMinute = hour === 20 ? 0 : 55;
        for (let minute = 0; minute <= lastMinute; minute += 5) {
          // Format minutes to always be two digits
          const displayMinute = minute.toString().padStart(2, '0');
          times.push(`${displayHour}:${displayMinute} ${ampm}`);
        }
      }
      return times;
    },
  },
  }
  </script>
  
  <style scoped>
  .add-form {
    padding: 2%
    
  }
  
  
  </style>
  