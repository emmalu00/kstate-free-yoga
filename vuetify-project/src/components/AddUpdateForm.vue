<template>
    <v-card style="padding: 2%" color="#c8d7e4">
        <v-form ref="form">
            <v-row>
                <v-col> <h2> {{formInfo.formTitle}} </h2></v-col>
                <span class="close" @click="closeForm">&times;</span>
            </v-row>
            <v-row>
                <v-col> <v-divider></v-divider> </v-col>
            </v-row>
            <v-row>
                <v-col> <strong> Basic Information  </strong> </v-col>
            </v-row>
            <v-row>
                <v-col cols="6">
                    <v-text-field density="compact" variant="solo" label="Class Name"
                    v-model="selectedEvent.ClassName" :rules="rules" bg-color="white" >
                    </v-text-field>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="4" md="4">
                    <v-text-field density="compact" variant="solo" label="Class Date" readonly prepend-inner-icon="$calendar"
                    :rules="rules" :active="menu"  v-model="formattedClassDate" >
                        <v-menu 
                        v-model="menu" :close-on-content-click="true"
                        activator="parent" transition="scale-transition">
                            <v-date-picker 
                            v-if="menu" v-model="selectedDate" 
                            :min="minDate" label="Select a date">
                        </v-date-picker>
                        </v-menu>
                    </v-text-field>
                </v-col>
                <v-col cols="4" md="4">
                    <v-combobox density="compact" variant="solo" label="Start Time" prepend-inner-icon="$clock"
                    :items="getStartTimes" v-model="formatTime" :rules="rules">
                    </v-combobox>
                </v-col>
                <v-col cols="4" md="4">
                    <v-combobox density="compact" variant="solo" label="Duration" suffix="minutes" prepend-inner-icon="$timer" 
                    :items="getDurations" v-model="getCorrectDuration" :rules="rules">
                    </v-combobox>
                </v-col>
            </v-row>
            <v-row>
                <v-col> <v-divider></v-divider> </v-col>
            </v-row>
            <v-row>
                <v-col> <strong> Location and Instructor  </strong> </v-col>
            </v-row>
            <v-row>
                <v-col cols="6"> 
                    <v-combobox density="compact" variant="solo" label="Location" prepend-inner-icon="$location"
                    :items="locations" item-title="FullName" item-value="id" v-model="selectedLocation"> 
                    </v-combobox>
                    <v-btn block density="compact" variant="tonal" prepend-icon="$addLocation"  
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
                                <v-btn size="small" block density="compact" variant="tonal" @click="closeLocationForm"> Cancel </v-btn>
                            </v-col>
                            <v-col cols="8">
                                <v-btn size="small" block density="compact" variant="flat" color="#769cbc" 
                                @click="addLocationToList" :disabled="!enableAddLocation"> Add Location </v-btn>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-col>
                <v-col cols="6"> 
                    <v-combobox density="compact" variant="solo" label="Instructor"  prepend-inner-icon="$instructor"
                    :items="instructors" item-title="FullName" item-value="id" v-model="selectedInstructor"> 
                    </v-combobox>
                    <v-btn block density="compact" variant="tonal" prepend-icon="$addInstructor"
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
                        <v-select label="Certified?"  variant="outlined" density="compact"
                            v-model="newInstructor.Certified" :rules="rules" :items="options" item-title="text" item-value="id">
                        </v-select>
                        <v-row>
                            <v-col cols="4">
                                <v-btn block size="small" density="compact" variant="tonal" @click="closeInstructorForm"> Cancel </v-btn>
                            </v-col>
                            <v-col cols="8">
                                <v-btn block size="small" density="compact" variant="flat" color="#769cbc" 
                                @click="addInstructortoList" :disabled="!enableAddInstructor"> Add Instructor </v-btn>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-col>
            </v-row>
            <v-row>
                <v-col> <v-divider></v-divider> </v-col>
            </v-row>
            <v-row>
                <v-col> <strong> Additional Details </strong></v-col>
            </v-row>
            <v-row>
                <v-col>
                    <v-textarea label="Write a brief description here." row-height="25" rows="2" variant="outlined" auto-grow shaped 
                    v-model="selectedEvent.ClassDescription" :rules="rules">
                    </v-textarea>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="4">
                    <v-select variant="solo" density="compact" label="Mats Provided?"
                    :items="options" v-model="selectedEvent.MatsProvided" :rules="rules">
                    </v-select>
                </v-col>
            </v-row> 
            <v-row>
                <v-col> <v-divider></v-divider> </v-col>
            </v-row>
            <v-row justify="space-between">
                <v-col>
                    <v-btn v-if="formInfo.operation === 'Update'" @click="closeForm" block variant="tonal"> {{ formInfo.secondaryButton }} </v-btn>
                    <v-btn v-if="formInfo.operation === 'Add'" @click="reset" block variant="tonal"> {{ formInfo.secondaryButton }} </v-btn>

                </v-col>
                <v-col>
                    <v-btn @click="addUpdateYogaClass" block color="#769cbc"> {{ formInfo.primaryButton }} </v-btn>
                </v-col>
            </v-row>
        </v-form>
    </v-card>

</template>

<script>
import { useInstructorsStore } from '@/stores/InstructorStore.js';
import { useLocationsStore } from '@/stores/ClassLocationStore.js';
import { format, set } from 'date-fns';

export default {
    props: ['formInfo', 'selectedEvent'],
    emits: ['addingClass', 'updatingClass'],
    data() {
        return {
            options: ['Yes', 'No'],
            buildings: [], 
            locations: [], 
            instructors: [], 
            selectedInstructor: { InstructorID: 0, FirstName: null, LastName: null, 
                Certified: null, FullName: null },
            selectedLocation: { LocationID: 0, BuildingName: null, RoomNumber: null,
                LocationAddress: null, FullName: null },
            instructorDropdown: false, 
            newInstructor: { FirstName: null, LastName: null, Certified: null },
            locationDropdown: false,
            selectedBuilding: { building: '', address: ''},
            selectedRoom: null, 
            menu: false,
            selectedDuration: null,
            minDate: new Date().toISOString().substr(0, 10),
            rules: [v => !!v || 'Required'],

        }
    }, 
    methods: {
        async fetchInstructorsandLocations() {
            await useInstructorsStore().fetchInstructors(null);
            await useLocationsStore().fetchLocations();
            this.instructors = useInstructorsStore().instructors.map(instructor => ({
                ...instructor, FullName: `${instructor.FirstName} ${instructor.LastName}` }));
            this.locations = useLocationsStore().classLocations.map(location => ({
                ...location, FullName: `${location.BuildingName} - ${location.RoomNumber}` }));
            this.buildings = useLocationsStore().buildingsList;
        },
        async addUpdateYogaClass() {   
            console.log(this.selectedDuration);
            const { valid } = await this.$refs.form.validate();
            if (valid)
            {   
                if (this.selectedInstructor.InstructorID === 0)
                {
                    await useInstructorsStore().addInstructor({
                        firstName: this.selectedInstructor.FirstName, 
                        lastName: this.selectedInstructor.LastName,
                        certified: this.selectedInstructor.Certified === 'Yes' ? true : false,
                    });
                    this.selectedInstructor.InstructorID = useInstructorsStore().instructorID;
                }
                if (this.selectedLocation.LocationID === 0)
                {
                    await useLocationsStore().addClassLocation({
                        buildingName: this.selectedLocation.BuildingName, 
                        roomNumber: this.selectedLocation.RoomNumber, 
                        LocationAddress: this.selectedLocation.LocationAddress,
                    })
                    this.selectedLocation.LocationID = useLocationsStore().classLocationID;
                }
                this.selectedEvent.BuildingName = this.selectedLocation.BuildingName;
                this.selectedEvent.RoomNumber = this.selectedLocation.RoomNumber;
                this.selectedEvent.LocationAddress = this.selectedLocation.LocationAddress;
                this.selectedEvent.LocationID = this.selectedLocation.LocationID;
                this.selectedEvent.FirstName = this.selectedInstructor.FirstName;
                this.selectedEvent.LastName = this.selectedInstructor.LastName;
                this.selectedEvent.Certified = this.selectedInstructor.Certified;
                this.selectedEvent.EndTime = this.calculateEndTime(this.selectedEvent.StartTime, this.selectedDuration);
                this.selectedEvent.StartTime = this.convertTimeFormat(this.selectedEvent.StartTime);
                if (this.selectedEvent.MatsProvided === 'Yes') { this.selectedEvent.MatsProvided = true; }
                else {this.selectedEvent.MatsProvided = false; }
                const newClass = {
                    className: this.selectedEvent.ClassName, 
                    startTime: this.selectedEvent.StartTime, 
                    endTime: this.selectedEvent.EndTime,
                    classDate: this.convertDateFormat(this.selectedEvent.ClassDate), 
                    instructorID:  this.selectedInstructor.InstructorID, 
                    locationID: this.selectedLocation.LocationID, 
                    matsProvided: this.selectedEvent.MatsProvided == 'Yes' ? true : false,
                    classDescription: this.selectedEvent.ClassDescription,
                };
                console.log(newClass.endTime);
                if (this.formInfo.operation === 'Add') { this.$emit('addingClass', newClass); }
                if (this.formInfo.operation === 'Update') { 
                    this.$emit('updatingClass', {id: this.selectedEvent.ClassID, info: newClass});
                 //   this.selectedEvent = null; 
                }
            }
        },
        closeForm() {
            this.$emit("closeForm");
        },
        reset() {
            this.$refs.form.reset();
        }, 
        openInstructorDropdown() {
            this.selectedInstructor = null;
            this.instructorDropdown = true;
        },
        closeInstructorForm() {
            this.instructorDropdown = false;
            this.$refs.instructorForm.reset();
        }, 
        addInstructortoList() {
             this.selectedInstructor = {
                InstructorID: 0, 
                ...this.newInstructor, 
                FullName: `${this.newInstructor.FirstName} ${this.newInstructor.LastName}`
            };
             this.instructors.push(this.selectedInstructor);
             this.instructorDropdown = false;
             this.$refs.instructorForm.reset();
        },
        openLocationDropdown() {
            this.selectedLocation = null;
            this.locationDropdown = true;
        },
        closeLocationForm() {
            this.locationDropdown = false;
            this.$refs.locationForm.reset();
            this.selectedBuilding = { building: '', address: ''},
            this.selectedRoom = null;
        },
        addLocationToList() {
            this.selectedLocation = { 
                LocationID: 0, 
                BuildingName: this.selectedBuilding.building, 
                RoomNumber: this.selectedRoom, 
                LocationAddress: this.selectedBuilding.address,
                FullName: `${this.selectedBuilding.building} - ${this.selectedRoom}`
            }
            this.locations.push(this.selectedLocation);
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
            if (hours === 12) { hours = modifier === 'AM' ? 0 : 12;} 
            else if (modifier === 'PM') { hours += 12; }
            const start = new Date();
            start.setHours(hours, minutes, 0); 
            start.setMinutes(start.getMinutes() + duration);
            const endHours = start.getHours().toString().padStart(2, '0');
            const endMinutes = start.getMinutes().toString().padStart(2, '0');
            const endSeconds = start.getSeconds().toString().padStart(2, '0');
            return `${endHours}:${endMinutes}:${endSeconds}`;
        },
        getSelectedDuration() {
            const [startHour, startMinute, startSecond] = this.selectedEvent.StartTime.split(':').map(Number);
            const [endHour, endMinute, endSecond] = this.selectedEvent.EndTime.split(':').map(Number);
            const startDate = new Date().setHours(startHour, startMinute, startSecond, 0);  
            const endDate = new Date().setHours(endHour, endMinute, endSecond, 0);
            const duration = endDate - startDate;
            return Math.floor(duration / 60000); 
        },
        convertDateFormat(initialDate) {
            const date = new Date(initialDate);
            return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
        },
    },
    computed: {
        formatTime: {
            get () {
                if (this.selectedEvent.StartTime) {
                    const timeParts = this.selectedEvent.StartTime.split(':');
                    const hours24 = parseInt(timeParts[0], 10);
                    const minutes = timeParts[1].substring(0, 2); 
                    const amPm = hours24 >= 12 ? 'PM' : 'AM';
                    const hours12 = hours24 % 12 || 12;
                    return `${hours12}:${minutes.padStart(2, '0')} ${amPm}`;
                }
                return null;
            },
            set (newValue) { this.selectedEvent.StartTime = newValue; }
        },
        selectedDate: {
            get() {
                if (this.selectedEvent.ClassDate == null) {
                    return null;
                }
                return new Date(new Date(this.selectedEvent.ClassDate.substr(0, 19)).toLocaleDateString('en-US', {
                year: 'numeric',
                month: 'long',
                day: 'numeric'
                }));
            }, 
            set(newValue) {
                var originalDate = new Date(newValue);
                var year = originalDate.getFullYear();
                var month = (originalDate.getMonth() + 1).toString().padStart(2, '0'); // Month is zero-based
                var day = originalDate.getDate().toString().padStart(2, '0');
                this.selectedEvent.ClassDate = `${year}-${month}-${day}T00:00:00`;
            }
        },
        formattedClassDate: {
            get() { return this.selectedEvent.ClassDate ? format(new Date(this.selectedEvent.ClassDate), 'MMMM, d yyyy') : '';  },
            set(newValue) { this.selectedEvent.ClassDate = newValue; }
        },
        getCorrectDuration: {
            get() {
                return this.selectedDuration;
            },
            set(newValue) { this.selectedDuration = newValue; }
        },
        selectedDuration() {
            if (!this.selectedEvent.StartTime || !this.selectedEvent.EndTime) {
                return null;
            }
            return this.getSelectedDuration();
        },
        getDurations() {
            const durations = [];
            for (let duration = 35; duration <= 120; duration += 5) { durations.push(duration); }
            return durations;
        }, 
        getStartTimes() {
            const times = [];
            for (let hour = 6; hour <= 20; hour++) { 
                for (let minute = 0; minute < 60; minute += 5) {
                    if (hour === 20 && minute > 0) { continue; }
                    const hourFormatted = hour % 12 === 0 ? 12 : hour % 12;
                    const minuteFormatted = minute.toString().padStart(2, '0');
                    const amPm = hour < 12 ? 'AM' : 'PM';
                    const time = `${hourFormatted}:${minuteFormatted} ${amPm}`;
                    times.push(time);
                }
            }
            return times;
        },
        enableAddLocation() {
            return (
                this.selectedBuilding.building != null && this.selectedRoom != null
            );
        },
        enableAddInstructor () {
            return (
                this.newInstructor.FirstName != null &&
                this.newInstructor.LastName != null &&
                this.newInstructor.Certified != null
            );
        }
    }, 
    mounted() {
        this.fetchInstructorsandLocations();
        if (this.selectedEvent.ClassID) {
            console.log(this.selectedEvent);
            this.selectedDuration = this.getSelectedDuration()
            this.selectedEvent.MatsProvided = this.selectedEvent.MatsProvided ? "Yes" : "No";
            this.selectedInstructor = {
                InstructorID: this.selectedEvent.InstructorID,
                FirstName: this.selectedEvent.FirstName, 
                LastName: this.selectedEvent.LastName, 
                Certified: this.selectedEvent.Certified, 
                FullName: `${this.selectedEvent.FirstName} ${this.selectedEvent.LastName}`
             };
            this.selectedLocation = {
                LocationID: this.selectedEvent.LocationID, 
                BuildingName: this.selectedEvent.BuildingName, 
                RoomNumber: this.selectedEvent.RoomNumber,
                LocationAddress: this.selectedEvent.LocationAddress,
                FullName: `${this.selectedEvent.BuildingName} - ${this.selectedEvent.RoomNumber}`
            }
        }
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
    
.close:hover, .close:focus {
    color: black;
    text-decoration: none;
    cursor: pointer;
}
</style>
