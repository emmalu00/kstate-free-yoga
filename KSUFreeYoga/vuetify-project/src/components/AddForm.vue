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
                    v-model="newClass.className" :rules="rules">
                    </v-text-field>
                </v-col>
            </v-row> 
            <v-row>
                <v-col cols="12" md="4">
                    <v-text-field
                    label="Date in menu" variant="solo-filled" density="compact"
                    :active="helpme" v-model="newClass.classDate" :rules="rules" readonly>
                    <v-menu 
                    v-model="helpme" :close-on-content-click="false"
                    activator="parent" transition="scale-transition">
                        <v-date-picker 
                        v-if="helpme" v-model="newClass.classDate" 
                        :min="minDate" label="Select a date">
                    </v-date-picker>
                    </v-menu>
                    </v-text-field>
                </v-col>
                <v-spacer></v-spacer>
                <v-col cols="12" md="3">
                    <v-combobox 
                    label="Start Time" variant="solo-filled" density="compact"
                    :items="getTimes"  v-model="newClass.startTime" :rules="rules"
                    ></v-combobox>
                </v-col>
                <v-col cols="auto" >to</v-col>
                <v-col cols="12" md="3">
                    <v-combobox
                    density="compact" variant="solo-filled" label="End Time"
                    :items="getTimes" :rules="rules" v-model="newClass.endTime">
                    </v-combobox>                
                </v-col>
            </v-row>
            <v-row style="margin-top: 2%">
                <v-col cols="auto">
                    <v-icon icon="fa-solid fa-user"> </v-icon>
                </v-col>
                <v-col cols="6">
                    <v-combobox
                    label="Instructor" variant="outlined" density="compact"
                    :rules="rules" :items="teachers" item-title="text" item-valie="id" v-model="selectedInstructor">
                    </v-combobox>
                    <v-btn block variant="outlined" density="compact" style="background-color: #cbacc1" 
                    @click="openInstructorDropdown"> 
                        Add New Instructor
                    </v-btn>
                    <v-row v-if="showDropdownInstructor" style="margin-top: 2%">
                        <v-col cols="12">
                            <v-text-field label="First Name" variant="outlined" density="compact"
                            v-model="newInstructor.firstName">
                            </v-text-field>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field label="Last Name" variant="outlined" density="compact"
                            v-model="newInstructor.lastName">
                            </v-text-field>
                        </v-col>
                        
                        <v-col cols="12">
                            <v-select 
                                label="Certified?" variant="outlined" density="compact"
                                :items="options" item-title="text" item-value="id" v-model="newInstructor.certified">
                                </v-select>
                        </v-col>
                        <v-col cols="12" justify="space-between">
                            <v-row justify="space-between">
                                <v-col>
                                    <v-btn block density="compact" variant="outlined" @click="addInstructortoClass"> Add Instructor </v-btn>
                                </v-col>
                                <v-col>
                                    <v-btn block density="compact" variant="outlined" @click="showDropdownInstructor = false"> Close </v-btn>
                                </v-col>
                            </v-row>
                        </v-col>
                       
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
                    <v-btn block variant="outlined" density="compact" @click="openLocationDropdown"
                    style="background-color: #cbacc1"> 
                        Add New Location
                    </v-btn>
                    <v-row v-if="showDropdownLocation" style="margin-top: 2%">
                        <v-col cols="12">
                            <v-text-field label="Building Name"
                            v-model="newLocation.buildingName"
                            variant="outlined" density="compact"></v-text-field>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field label="Room Number"
                            v-model="newLocation.roomNumber"
                            variant="outlined" density="compact"></v-text-field>
                        </v-col>
                        
                        <v-col cols="12">
                            <v-combobox label="Address"
                            v-model="newLocation.address"
                            variant="outlined" density="compact"
                            ></v-combobox>
                        </v-col>
                        <v-row justify="space-between">
                            <v-col>
                                <v-btn block density="compact" variant="outlined" 
                                style="background-color: #927396" @click="addLocationToClass">Add Location </v-btn>
                            </v-col>
                            <v-col>
                                <v-btn block  density="compact" variant="outlined" 
                                style="background-color: #f5f5f5" @click="showDropdownLocation = false"> Close </v-btn>
                            </v-col>
                        </v-row>
                    </v-row>
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
                    v-model="matsProvided">
                    </v-select>
                </v-col>
            </v-row> 

            <v-row style="width: 60%">
                <v-col>
                    <v-textarea
                    label="Write a brief description here."
                    v-model="newClass.classDescription"
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
  import { useInstructorsStore } from '@/stores/Instructors';
  import { useLocationsStore } from '@/stores/ClassLocations';

  export default {
    data() {
      return {
        newClass: {className: null, startTime: null, endTime: null, classDate: null, 
                    instructorID: 0, locationID: 0,  matsProvided: true, classDescription: null},
        showDropdownInstructor: false,
        showDropdownLocation: false,
        selectedLocation: null, 
        selectedInstructor: null, 
        minDate: new Date().toISOString().substr(0, 10), // Today's date in YYYY-MM-DD format
        helpme: false,
        matsProvided: true,
        newInstructor: { firstName: null, lastName: null, certified: true },
        newLocation: { buildingName: null, roomNumber: null, locationAddress: null },
        locations: [], 
        teachers: [],
        rules: [v => !!v || 'Required'],
        options: [ 
            {text: 'Yes', id: true},
            {text: 'No', id: false}
        ]
      }
    },
    methods: {
    async fetchEvents() {
        const instructorsStore = useInstructorsStore();
        const classLocationsStore = useLocationsStore();
        await instructorsStore.fetchInstructors();
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
        this.newInstructor = { firstName: null, lastName: null, certified: true }
    },
    async addClass() {
        this.newClass.instructorID = this.selectedInstructor.id;
        this.newClass.locationID = this.selectedInstructor.id;
        const { valid } = await this.$refs.form.validate()
        if (valid) {
            this.$emit('addingClass', this.newClass);
        this.$emit('close'); 
        }
    }, 
    async addInstructortoClass() {
        console.log(this.selectedInstructor);
        this.newInstructor.firstName = this.newInstructor.firstName.charAt(0).toUpperCase() + this.newInstructor.firstName.slice(1);
        this.newInstructor.lastName = this.newInstructor.lastName.charAt(0).toUpperCase() + this.newInstructor.lastName.slice(1);
       
        const instructorsStore = useInstructorsStore();
        await instructorsStore.addInstructor(this.newInstructor);

        this.fetchEvents();
        this.showDropdownInstructor = false;
        this.selectedInstructor = {
            text: `${this.newInstructor.firstName} ${this.newInstructor.lastName}`, 
            id: instructorsStore.instructorID};
    },
    async addLocationToClass() {
        
        this.newLocation.buildingName = this.newLocation.buildingName.charAt(0).toUpperCase() + this.newLocation.buildingName.slice(1);
        this.newLocation.roomNumber = this.newLocation.roomNumber.charAt(0).toUpperCase() + this.newLocation.roomNumber.slice(1);

        const classLocationsStore = useLocationsStore();
        await classLocationsStore.addClassLocation(this.newLocation);
        
        // this.fetchEvents();
        // this.showDropdownLocation = false;
        // this.selectedLocation = {
        //     text: `${this.newLocation.buildingName} ${this.newLocation.roomNumber}`, 
        //     id: classLocationsStore.classLocationID};
        // console.log(classLocationsStore.classLocationID);
    },
    closeForm() {
        this.$emit('close'); 
    },
    reset () {
        console.log(this.selectedInstructor);
        this.$refs.form.reset();
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
    openInstructorDropdown() {
        this.showDropdownInstructor = true,
        this.selectedInstructor = null;
    },
    openLocationDropdown() {
        this.showDropdownLocation = true,
        this.selectedLocation = null;
    }
    },
    computed: {
        getTimes()
        {
            const times = [];
            for (let hour = 6; hour <= 21; hour++) {
                for (let minute = 0; minute < 60; minute += 15) {
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
  