<template>
    <v-sheet class="pa-2 ma-3">
        <v-btn block @click="showAddForm=true"> Add Class </v-btn>
    </v-sheet>
    <v-dialog v-model="showAddForm" max-width="650px">
        <v-card style="padding: 2%">
            <v-form ref="form">
                <v-row>
                    <v-col> <h2> Add Form </h2></v-col>
                    <span class="close" @click="closeForm">&times;</span>
                </v-row>
                <v-row>
                    <v-col> <v-divider></v-divider> </v-col>
                </v-row>
                <v-row>
                    <v-col> Basic Information </v-col>
                </v-row>
                <v-row>
                    <v-col cols="6">
                        <!-- Class Name field -->
                        <v-text-field density="compact" variant="outlined" label="Class Name"
                        v-model="newClass.className" :rules="rules">
                        </v-text-field>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4" md="4">
                        <!-- Class Date field -->
                        <v-text-field density="compact" variant="outlined" label="Class Date" readonly prepend-inner-icon="$calendar"
                        v-model="formattedClassDate" :rules="rules" :active="menu">
                            <v-menu 
                            v-model="menu" :close-on-content-click="true"
                            activator="parent" transition="scale-transition">
                                <v-date-picker 
                                v-if="menu" v-model="newClass.classDate" 
                                :min="minDate" label="Select a date">
                            </v-date-picker>
                            </v-menu>
                        </v-text-field>
                    </v-col>
                    <v-col cols="4" md="4">
                        <!-- Class Date field -->
                        <v-combobox density="compact" variant="outlined" label="Start Time" prepend-inner-icon="$clock"
                        :items="getStartTimes" v-model="newClass.startTime" :rules="rules">
                        </v-combobox>
                    </v-col>
                    <v-col cols="4" md="4">
                        <!-- Class Date field -->
                        <v-combobox density="compact" variant="outlined" label="Duration" suffix="minutes" prepend-inner-icon="$timer" 
                        :items="getDurations" v-model="newClass.duration" :rules="rules">
                        </v-combobox>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col> <v-divider></v-divider> </v-col>
                </v-row>
                <v-row>
                    <v-col> Location and Instructor  </v-col>
                </v-row>
                <v-row>
                    <v-col cols="6"> 
                        <v-combobox density="compact" variant="outlined" label="Location" prepend-inner-icon="$location"
                        :items="locations" :rules="rules" item-title="FullName" item-value="id" v-model="selectedLocation"> 
                        </v-combobox>
                        <v-btn block density="compact" variant="outlined" prepend-icon="$addLocation"
                            @click="openLocationDropdown"> 
                            Add Location 
                        </v-btn>
                        <v-form v-if="locationDropdown" style="margin-top: 4%" ref="locationForm">
                            <v-combobox label="Building Name" v-model="selectedBuilding" :rules="rules"
                            variant="outlined" density="compact" :items="buildings" item-title="building" :hint="`${selectedBuilding.address}`">
                            </v-combobox>
                            <v-text-field label="Room Number" v-model="selectedRoom" :rules="rules"
                            variant="outlined" density="compact" style="margin-top: 2%">
                            </v-text-field>
                            <v-row>
                                <v-col cols="4">
                                    <v-btn size="small" block density="compact" variant="outlined" @click="closeLocationForm"> Close </v-btn>
                                </v-col>
                                <v-col cols="8">
                                    <v-btn size="small" block density="compact" variant="outlined" @click="addLocationToList"> Add Location </v-btn>
                                </v-col>
                            </v-row>
                        </v-form>
                    </v-col>
                    <v-col cols="6"> 
                        <v-combobox density="compact" variant="outlined" label="Instructor"  prepend-inner-icon="$instructor"
                        :items="instructors" :rules="rules" item-title="FullName" item-value="id" v-model="selectedInstructor"> 
                        </v-combobox>
                        <v-btn block density="compact" variant="outlined" prepend-icon="$addInstructor"
                        @click="openInstructorDropdown"> 
                            Add Instructor 
                        </v-btn>
                        <v-form v-if="instructorDropdown" ref="instructorForm" style="margin-top: 4%">
                            <v-text-field label="First Name" v-model="newInstructor.FirstName" :rules="rules"
                            variant="outlined" density="compact">
                            </v-text-field>
                            <v-text-field label="Last Name" v-model="newInstructor.LastName" :rules="rules"
                            variant="outlined" density="compact">
                            </v-text-field>
                            <v-select label="Certified?" v-model="newInstructor.Certified" :rules="rules"
                                :items="options" item-title="text" item-value="id"
                                variant="outlined" density="compact">
                            </v-select>
                            <v-row>
                                <v-col cols="4">
                                    <v-btn block size="small" density="compact" variant="outlined" @click="closeInstructorForm"> Close </v-btn>
                                </v-col>
                                <v-col cols="8">
                                    <v-btn block size="small" density="compact" variant="outlined" @click="addInstructortoList"> Add Instructor </v-btn>
                                </v-col>
                            </v-row>
                        </v-form>
                    </v-col>
                </v-row>
                
                <v-row>
                    <v-col> <v-divider></v-divider> </v-col>
                </v-row>
                <v-row>
                    <v-col> Additional Details </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <v-textarea label="Write a brief description here." row-height="25" rows="2" variant="outlined" auto-grow shaped 
                        v-model="newClass.classDescription" :rules="rules">
                        </v-textarea>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4">
                        <v-select
                        variant="outlined" density="compact" label="Mats Provided?"
                        :items="options" v-model="newClass.matsProvided" :rules="rules">
                        </v-select>
                    </v-col>
                </v-row> 
                <v-row>
                    <v-col> <v-divider></v-divider> </v-col>
                </v-row>
                <v-row justify="space-between">
                    <v-col>
                        <v-btn @click="addYogaClass" block>Add Class </v-btn>
                    </v-col>
                    <v-col>
                        <v-btn @click="reset" block variant="outlined"> Reset </v-btn>
                        
                    </v-col>
                </v-row>
            </v-form>
        </v-card>
    </v-dialog>
    <v-snackbar
        v-model="addConfirmation.show"
        timeout="3000">
        {{ addConfirmation.text }}
        <template v-slot:actions>
        <v-btn
            color="blue"
            variant="text"
            @click="addConfirmation.show = false">
            Close
        </v-btn>
        </template>
    </v-snackbar>
</template>

<script>
import { useInstructorsStore } from '@/stores/InstructorStore.js';
import { useLocationsStore } from '@/stores/ClassLocationStore.js';
import { format } from 'date-fns';


export default {
    props: ['formInfo', 'selectedEvent'],
    data() {
        return {
            showAddForm: false, 
            options: ['Yes', 'No'],
            buildings: [], 
            locations: [], 
            instructors: [], 
            newClass: {className: null, startTime: null, duration: null, classDate: null, 
                    instructorID: 0, locationID: 0, matsProvided: null, classDescription: null},
            selectedInstructor: null,
            instructorDropdown: false, 
            newInstructor: { FirstName: null, LastName: null, Certified: null },
            selectedLocation: null, 
            locationDropdown: false,
            selectedBuilding: { building: '', address: ''},
            selectedRoom: null, 
            menu: false,
            minDate: new Date().toISOString().substr(0, 10),
            rules: [v => !!v || 'Required'],
            addConfirmation: { show: false, text: "Class successfully added to schedule."},  
        }
    }, 
    methods: {
        async fetchInstructorsandLocations() {
            const instructorsStore = useInstructorsStore();
            const classLocationsStore = useLocationsStore();
            await instructorsStore.fetchInstructors(null);
            await classLocationsStore.fetchLocations();
            this.instructors = instructorsStore.instructors.map(instructor => ({
                ...instructor,
                FullName: `${instructor.FirstName} ${instructor.LastName}`
            }));
            this.locations = classLocationsStore.classLocations.map(location => ({
                ...location, 
                FullName: `${location.BuildingName} - ${location.RoomNumber}`
            }));
            this.buildings = classLocationsStore.buildingsList;
            console.log(this.buildings);
        },
        async addYogaClass() 
        {   
            console.log(this.selectedLocation);
            const { valid } = await this.$refs.form.validate();
            if (this.selectedInstructor.InstructorID === 0)
            {
                const instructorsStore = useInstructorsStore();
                await instructorsStore.addInstructor({
                    firstName: this.selectedInstructor.FirstName, 
                    lastName: this.selectedInstructor.LastName,
                    certified: this.selectedInstructor.Certified === 'Yes' ? true : false,
                });
                this.selectedInstructor.InstructorID = instructorsStore.instructorID;
            }
            if (this.selectedLocation.LocationID === 0)
            {
                const classLocationsStore = useLocationsStore();
                await classLocationsStore.addClassLocation({
                    buildingName: this.selectedLocation.BuildingName, 
                    roomNumber: this.selectedLocation.RoomNumber, 
                    LocationAddress: this.selectedLocation.LocationAddress,
                })
                this.selectedInstructor.LocationID = classLocationsStore.classLocationID;
            }
            
            if (valid)
            {
                this.$emit('addingClass', {
                    className: this.newClass.className, 
                    startTime: this.convertTimeFormat(this.newClass.startTime), 
                    endTime: this.calculateEndTime(this.newClass.startTime, this.newClass.duration), 
                    classDate: this.convertDateFormat(this.newClass.classDate), 
                    instructorID:  this.selectedInstructor.InstructorID, 
                    locationID: this.selectedLocation.LocationID, 
                    matsProvided: this.newClass.matsProvided == 'Yes' ? true : false,
                    classDescription: this.newClass.classDescription,
                })
                this.showAddForm = false;
                this.addConfirmation.show = true;
            }
        },
        closeForm() {
            this.showAddForm = false;
        },
        reset()
        {
            console.log(this.newClass.classDate);
            this.$refs.form.reset();
        }, 
        openInstructorDropdown()
        {
            this.selectedInstructor = null;
            this.instructorDropdown = true;
        },
        closeInstructorForm() {
            this.instructorDropdown = false;
            this.$refs.instructorForm.reset();
        }, 
        addInstructortoList() {
             const newInstructorData = { InstructorID: 0, ...this.newInstructor, FullName: `${this.newInstructor.FirstName} ${this.newInstructor.LastName}`}
             console.log(newInstructorData);
             this.instructors.push(newInstructorData);
             this.selectedInstructor = newInstructorData;
             this.instructorDropdown = false;
             this.$refs.instructorForm.reset();
        },
        openLocationDropdown()
        {
            this.selectedLocation = null;
            this.locationDropdown = true;
        },
        closeLocationForm()
        {
            this.locationDropdown = false;
            this.$refs.locationForm.reset();
            this.selectedBuilding = { building: '', address: ''}
        },
        addLocationToList() {
            const newLocationData = { 
                LocationID: 0, 
                BuildingName: this.selectedBuilding.building, 
                RoomNumber: this.selectedRoom, 
                LocationAddress: this.selectedBuilding.address,
                FullName: `${this.selectedBuilding.building} - ${this.selectedRoom}`
            }
            this.locations.push(newLocationData);
            this.selectedLocation = newLocationData;
            this.locationDropdown = false;
            this.$refs.locationForm.reset();
        }, 
        convertTimeFormat(initialTime) {
            let [time, period] = initialTime.split(' ');
            let [hours, minutes] = time.split(':');
            if (period === 'PM' && hours !== '12') hours = (parseInt(hours, 10) + 12).toString();
            else if (period === 'AM' && hours === '12') hours = '00';
            return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}:00`;
        },
        calculateEndTime(startTime, duration) {
            let [time, modifier] = startTime.split(' ');
            let [hours, minutes] = time.split(':').map(Number);

            if (hours === 12) {
                hours = modifier === 'AM' ? 0 : 12;
            } else if (modifier === 'PM') {
                hours += 12;
            }

            const start = new Date();
            start.setHours(hours, minutes, 0); // Set hours, minutes, and seconds

            start.setMinutes(start.getMinutes() + duration);

            const endHours = start.getHours().toString().padStart(2, '0');
            const endMinutes = start.getMinutes().toString().padStart(2, '0');
            const endSeconds = start.getSeconds().toString().padStart(2, '0');

            return `${endHours}:${endMinutes}:${endSeconds}`;
        },
        convertDateFormat(initialDate) {
            const date = new Date(initialDate);
            return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
        },

    },
    computed: {
        getDurations() {
            const durations = [];
            for (let duration = 35; duration <= 120; duration += 5)
            {
                durations.push(duration);
            }
            return durations;
        }, 
        getStartTimes() {
            const times = [];
            for (let hour = 6; hour <= 20; hour++) { // Adjusted to include hour 20
                for (let minute = 0; minute < 60; minute += 5) {
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
        formattedClassDate: {
            get() { return this.newClass.classDate ? format(new Date(this.newClass.classDate), 'MMMM, d yyyy') : '';  },
            set(newValue) { this.newClass.classDate = newValue; }
        },

        instructorText() {
        return instructor => `${instructor.FirstName} ${instructor.LastName}`;
    },
    }, 
    mounted() {
        this.fetchInstructorsandLocations();
    }
}
</script>

<style scoped>
h1 {
    color: #b195c1;
    text-align: center;
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

</style>
