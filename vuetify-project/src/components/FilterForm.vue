<template>
    <v-card color="#ece1e9" class="filter-form">
        <v-card-title> Filter </v-card-title>
        <v-form ref="form" class="pa-3">
            <v-select label="Mats Provided" variant="solo" density="compact"
            :items="options" v-model="filters.matsProvided" item-title="text" item-value="id"></v-select>
            <v-combobox label="Instructor" variant="solo" density="compact"
            :items="instructors" v-model="filters.instructorFullName"> </v-combobox>
            <v-combobox label="Location" variant="solo" density="compact"
            :items="locations" v-model="filters.buildingName"> </v-combobox>
            <v-row>
                <v-col cols="12">
                    <v-btn density="compact" block @click="applyFilters" 
                    color="#BF98B2" variant="flat" :disabled="!isFormFilled"> Apply </v-btn>
                </v-col>
                <v-col cols="12">
                    <v-btn density="compact" block @click="resetForm" variant="tonal"
                     :disabled="!isFormFilled"> Reset </v-btn>
                </v-col>
            </v-row>
        </v-form>
    </v-card>
</template>

<script>
import { useInstructorsStore } from '@/stores/InstructorStore.js';
import { useLocationsStore } from '@/stores/ClassLocationStore.js';

export default {
    data() {
        return {
            instructors: [], 
            locations: [], 
            filters: { classID: null, buildingName: null, instructorFullName: null, matsProvided: null },
            options: [ 
                {text: 'Yes', id: true},
                {text: 'No', id: false}
            ]
        }
    }, 
    methods: {
        async fetchFormInformation() {
            const instructorsStore = useInstructorsStore();
            const classLocationsStore = useLocationsStore();
            await instructorsStore.fetchInstructors(null);
            await classLocationsStore.fetchLocations();
            this.instructors = instructorsStore.instructors.map(instructor => `${instructor.FirstName} ${instructor.LastName}`);;
            this.locations = classLocationsStore.classLocations.map(location => location.BuildingName);
        },
        applyFilters() {
            this.$emit("filter", this.filters);
        },
        resetForm() {
            this.$refs.form.reset();
            console.log(this.filters);
            this.$emit("reset", this.filters);
        }
    },
    computed: {
        isFormFilled() {
            return (
                this.filters.matsProvided !== null ||
                this.filters.instructorFullName !== null ||
                this.filters.buildingName !== null
            );
        }
    },
    mounted() {
        this.fetchFormInformation();
    }
}
</script>

<style scoped>
.filter-form {
    padding: 2%;
}
  
</style>