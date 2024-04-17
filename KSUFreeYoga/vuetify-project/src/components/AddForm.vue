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
                <v-col cols="11">
                    <v-text-field label="Class Name" v-model="newClass.className" variant="outlined" :rules="rules"></v-text-field>
                </v-col>
            </v-row> 
            <v-row >
                <v-col cols="4" >
                    <v-text-field prepend-inner-icon="fa-regular fa-calendar"
                    label="Date" variant="solo-filled" density="compact"
                    :active="helpme" v-model="formattedClassDate" :rules="rules" readonly>
                    <v-menu 
                    v-model="helpme" :close-on-content-click="true"
                    activator="parent" transition="scale-transition">
                        <v-date-picker 
                        v-if="helpme" v-model="newClass.classDate" 
                        :min="minDate" label="Select a date">
                    </v-date-picker>
                    </v-menu>
                    </v-text-field>
                </v-col>
                <v-col cols="3" >
                    <v-combobox 
                    label="Start Time" variant="solo-filled" density="compact"
                    :items="getTimes"  v-model="newClass.startTime" :rules="rules"
                    ></v-combobox>
                </v-col>
                <v-col cols="auto">
                    <p style="margin-bottom: 5%; margin-right: 4%; margin-left: 4%;"> to </p>
                </v-col>
                <v-col cols="3" >
                    <v-combobox
                    density="compact" variant="solo-filled" label="End Time"
                    :items="getEndTimes" :rules="rules" v-model="newClass.endTime">
                    </v-combobox>       
                </v-col>
                
            </v-row> 
            <v-divider></v-divider>
            <v-row style="margin-top: 2%"> <!-- Instructor and Location  -->
                <v-col cols="6">
                    <v-combobox label="Location" v-model="selectedLocation"
                    :items="locations" item-title="text" item-value="id"
                    :rules="rules" variant="outlined" density="compact"
                    prepend-inner-icon="fa-solid fa-location-dot">
                    </v-combobox>
                    <v-btn @click="openLocationDropdown"  block variant="outlined" density="compact"> Add New Location </v-btn>
                    <v-form v-if="showDropdownLocation" style="margin-top: 2%" ref="locationForm">
                        <v-text-field label="Building Name" v-model="newLocation.buildingName" :rules
                        variant="outlined" density="compact">
                        </v-text-field>
                        <v-text-field label="Room Number" v-model="newLocation.roomNumber" :rules
                        variant="outlined" density="compact">
                        </v-text-field>
                        <v-combobox label="Address" v-model="newLocation.locationAddress" :rules
                        variant="outlined" density="compact">
                        </v-combobox>
                        <v-row>
                            <v-col cols="4">
                                <v-btn block density="compact" variant="outlined" @click="closeLocationForm"> Close </v-btn>
                            </v-col>
                            <v-col cols="8">
                                <v-btn block density="compact" variant="outlined" @click="addLocationToClass"> Add Location </v-btn>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-col>
                <v-col cols="6"> <!-- Instructor -->
                    <v-combobox label="Instructor" v-model="selectedInstructor" 
                        :items="teachers" item-title="text" item-value="id"
                        :rules="rules"
                        variant="outlined" density="compact"
                        prepend-inner-icon="fa-solid fa-user">
                    </v-combobox>
                    <v-btn @click="openInstructorDropdown" block variant="outlined" density="compact"> Add New Instructor </v-btn>
                    <v-form v-if="showDropdownInstructor" style="margin-top: 2%" ref="instructorForm">
                        <v-text-field label="First Name" v-model="newInstructor.firstName" :rules="rules"
                        variant="outlined" density="compact">
                        </v-text-field>
                        <v-text-field label="Last Name" v-model="newInstructor.lastName" :rules="rules"
                        variant="outlined" density="compact">
                        </v-text-field>
                        <v-select label="Certified?" v-model="newInstructor.certified" :rules="rules"
                            :items="options" item-title="text" item-value="id"
                            variant="outlined" density="compact">
                        </v-select>
                        <v-row>
                            <v-col cols="4">
                                <v-btn block density="compact" variant="outlined" @click="closeInstructorForm"> Close </v-btn>
                            </v-col>
                            <v-col cols="8">
                                <v-btn block density="compact" variant="outlined" @click="addInstructortoClass"> Add Instructor </v-btn>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-col>
                <v-divider></v-divider>
            </v-row>
            <v-row style="margin-top: 4%;">
                <v-col>
                    <v-textarea label="Write a brief description here." v-model="newClass.classDescription"
                    row-height="25" rows="2" variant="outlined" auto-grow shaped :rules="rules">
                    </v-textarea>
                </v-col>
            </v-row> 
            <v-row>
                <v-col cols="4">
                    <v-select label="Mats Provided?" v-model="newClass.matsProvided"
                    :items="options" :rules="rules"
                    variant="outlined" density="compact">
                    </v-select>
                </v-col>
            </v-row> 
            <v-row justify="space-between">
                <v-col>
                    <v-btn @click="reset" block> Reset </v-btn>
                </v-col>
                <v-col>
                    <v-btn @click="addClass" block>Add Class </v-btn>
                </v-col>
            </v-row>
            
        </v-form>
    </v-sheet>
    
  </template>
  
  <script>
  import { useInstructorsStore } from '@/stores/Instructors';
  import { useLocationsStore } from '@/stores/ClassLocations';
  import { format, isThisISOWeek } from 'date-fns';

  export default {
    data() {
      return {
        newClass: {className: null, startTime: null, endTime: null, classDate: null, 
                    matsProvided: null, classDescription: null},
        showDropdownInstructor: false,
        showDropdownLocation: false,
        selectedLocation: null, 
        selectedInstructor: null, 
        minDate: new Date().toISOString().substr(0, 10), // Today's date in YYYY-MM-DD format
        helpme: false,
        newInstructor: { firstName: null, lastName: null, certified: null },
        newLocation: { buildingName: null, roomNumber: null, locationAddress: null },
        locations: [], 
        teachers: [],
        rules: [v => !!v || 'Required'],
        options: ['Yes', 'No'], 
      }
    },
    methods: {
    async fetchEvents() {
        const instructorsStore = useInstructorsStore();
        const classLocationsStore = useLocationsStore();
        await instructorsStore.fetchInstructors(null);
        await classLocationsStore.fetchLocations();
        this.teachers = instructorsStore.instructors.map((teacher) => {
            return {
                text: `${teacher.FirstName} ${teacher.LastName}`,
                id: teacher.InstructorID
            }
        });
        this.locations = classLocationsStore.classLocations.map((location) => {
            return {
                text: `${location.BuildingName}, ${location.RoomNumber}`,
                id: location.LocationID
            }
        });
    },
    async addClass() {
        const { valid } = await this.$refs.form.validate()
        if (valid) {
            this.$emit('addingClass', {
                className: this.newClass.className,    
                startTime: this.convertTimeFormat(this.newClass.startTime),
                endTime: this.convertTimeFormat(this.newClass.endTime),
                classDate: this.convertDateFormat(this.newClass.classDate),
                instructorID: this.selectedInstructor.id, 
                locationID: this.selectedLocation.id,
                matsProvided: this.newClass.matsProvided == 'Yes' ? true : false,
                classDescription: this.newClass.classDescription,
            });
        this.$emit('close'); 
        }
    }, 
    async addInstructortoClass() {
        const { valid } = await this.$refs.instructorForm.validate()
        if (valid) {
            const instructorsStore = useInstructorsStore();
            await instructorsStore.addInstructor({
                firstName: this.newInstructor.firstName = this.newInstructor.firstName.charAt(0).toUpperCase() + this.newInstructor.firstName.slice(1), 
                lastName: this.newInstructor.lastName = this.newInstructor.lastName.charAt(0).toUpperCase() + this.newInstructor.lastName.slice(1),
                certified: this.newInstructor.certified === 'Yes' ? true : false
            });
            console.log(instructorsStore.instructorID);
            const newInstructorData = { text: `${this.newInstructor.firstName} ${this.newInstructor.lastName}`, id: instructorsStore.instructorID };
            this.teachers.push(newInstructorData);
            this.selectedInstructor = newInstructorData;
            this.showDropdownInstructor = false;
            this.$refs.instructorForm.reset();
        }
    },
    async addLocationToClass() {
        console.log('Adding method for location. ');
        console.log(this.newLocation.locationAddress);

        const classLocationsStore = useLocationsStore();
        await classLocationsStore.addClassLocation({
            buildingName: this.newLocation.buildingName = this.newLocation.buildingName.charAt(0).toUpperCase() + this.newLocation.buildingName.slice(1),
            roomNumber: this.newLocation.roomNumber = this.newLocation.roomNumber.charAt(0).toUpperCase() + this.newLocation.roomNumber.slice(1),
            locationAddress: this.newLocation.locationAddress
        });
        console.log(classLocationsStore.classLocationID);
        const newLocationData = { text: `${this.newLocation.buildingName} ${this.newLocation.roomNumber}`, id: classLocationsStore.classLocationID};
        this.locations.push(newLocationData)
        this.selectedLocation = newLocationData;
        this.showDropdownLocation = false;
        this.$refs.locationForm.reset();
    },
    closeForm() {
        this.$emit('close'); 
    },
    reset () {
        this.newClass.classDate = this.convertDateFormat(this.newClass.classDate);

        console.log(this.newClass.classDate);
        this.$refs.form.reset();
    },
    openInstructorDropdown() {
        this.showDropdownInstructor = true,
        this.selectedInstructor = null;
    },
    openLocationDropdown() {
        this.showDropdownLocation = true,
        this.selectedLocation = null;
    },
    closeLocationForm()
    {
        this.showDropdownLocation = false;
        this.$refs.locationForm.reset();
    },
    closeInstructorForm()
    {
        this.showDropdownInstructor = false;
        this.$refs.instructorForm.reset();
    },
    convertTimeFormat(initialTime) {
        let [time, period] = initialTime.split(' ');
        let [hours, minutes] = time.split(':');
        if (period === 'PM' && hours !== '12') hours = (parseInt(hours, 10) + 12).toString();
        else if (period === 'AM' && hours === '12') hours = '00';
        return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}:00`;
    },
    convertDateFormat(initialDate) {
        const date = new Date(initialDate);
        return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
    },
    
    },
    computed: {
        getTimes()
        {
            const times = [];
            for (let hour = 6; hour <= 20; hour++) { // Adjusted to include hour 20
                for (let minute = 0; minute < 60; minute += 15) {
                    // Special case for 8:00 PM to only add the :00 minute
                    if (hour === 20 && minute > 0) {
                        continue;
                    }
                    const hourFormatted = hour % 12 === 0 ? 12 : hour % 12;
                    const minuteFormatted = minute.toString().padStart(2, '0');
                    const amPm = hour < 12 ? 'AM' : 'PM';
                    const time = `${hourFormatted}:${minuteFormatted} ${amPm}`;
                    times.push(time);
                }
            }
            return times;
        },
        getEndTimes()
        {
            if (!this.newClass.startTime) return [];
        const startTime = this.newClass.startTime.match(/^(\d+):(\d+)\s(AM|PM)$/);
        let hour = parseInt(startTime[1], 10);
        let minute = parseInt(startTime[2], 10);
        const amPm = startTime[3];

        // Convert to 24-hour format for easier calculation
        if (amPm === 'PM' && hour < 12) hour += 12;
        if (amPm === 'AM' && hour === 12) hour = 0;

        const startMinutesFromMidnight = hour * 60 + minute;
        const startOffset = 30; // 30 minutes after start time
        const endOffset = 120; // 120 minutes after start time
        const minMinutesFromMidnight = startMinutesFromMidnight + startOffset;
        const maxMinutesFromMidnight = startMinutesFromMidnight + endOffset;

        const times = [];
        for (let currentMinutesFromMidnight = minMinutesFromMidnight; currentMinutesFromMidnight <= maxMinutesFromMidnight; currentMinutesFromMidnight += 15) {
            const currentHour = Math.floor(currentMinutesFromMidnight / 60);
            const currentMinute = currentMinutesFromMidnight % 60;

            // Skip times outside the normal operating hours
            if (currentHour > 20 || (currentHour === 20 && currentMinute > 0)) continue;
            if (currentHour < 6) continue;

            const hourFormatted = currentHour % 12 === 0 ? 12 : currentHour % 12;
            const minuteFormatted = currentMinute.toString().padStart(2, '0');
            const currentAmPm = currentHour < 12 ? 'AM' : 'PM';
            const time = `${hourFormatted}:${minuteFormatted} ${currentAmPm}`;
            times.push(time);
        }
        return times;


        },
        formattedClassDate: {
            get() {
                // Return the formatted date only if newClass.classDate is not null
                return this.newClass.classDate ? format(new Date(this.newClass.classDate), 'MMMM, d yyyy') : '';
            },
            set(newValue) {
                // Update the raw date value from the date picker
                this.newClass.classDate = newValue;
        }
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
  