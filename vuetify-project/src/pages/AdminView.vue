<template>
    <v-row no-gutters>
        <v-col cols="12" justify="center">
            <h2> Admin Schedule Manager </h2>
            <v-divider class="border-opacity-25" :thickness="2" color="black" style="width: 90%; margin: 0 auto;"></v-divider>
        </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-sheet style="width: 90%; margin: 0 auto;" >
          <v-btn block @click="addFormDialog = true" size="small" color="#769cbc"> Add Class </v-btn>
        </v-sheet>
        <v-dialog v-model="addFormDialog" max-width="650px">
          <AddUpdateForm @closeForm="addFormDialog = false"  @addingClass="addClass"
          :selectedEvent="this.classInformation" :formInfo="addFormInformation">
          </AddUpdateForm>
        </v-dialog>
      </v-col>
    </v-row>
    <v-row>
        <v-col> 
          <v-overlay :model-value="overlayItem" class="align-center justify-center">
            <v-progress-circular indeterminate color="#bfaec2"></v-progress-circular>
           </v-overlay>
          <AdminCalendar :Yogaevents="filteredEvents" @deletingClass="deleteClass" @updateClass="updateClass"></AdminCalendar>
        </v-col>
    </v-row>
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
import AdminCalendar from '../components/AdminCalendar.vue';
import AddUpdateForm from '../components/AddUpdateForm.vue';
import { useYogaClassesStore } from '@/stores/YogaClassStore.js'; 

export default {
  components: {
      AdminCalendar, AddUpdateForm
  }, 
  data() {
    return {
      filteredEvents: [], 
      overlayItem: true, 
      addFormDialog: false,
      formInfo: {formTitle: '', submitButton: '', secondButton: ''}, 
      classInformation: {
        ClassID: null, ClassName: null, ClassDate: null, 
        StartTime: null, EndTime: null, MatsProvided: null, ClassDescription: null,
      }, 
      addFormInformation: { 
        formTitle: 'Add New Class', 
        primaryButton: 'Add Class', 
        secondaryButton: 'Reset', 
        operation: 'Add'
      }, 
      confirmationDialog: { text: '', show: false}
    }
  }, 
  methods: {
    async fetchEvents() {
      await useYogaClassesStore().getClasses();
      this.filteredEvents = useYogaClassesStore().filteredClasses.map((yogaClass) => {
          return {
              title: yogaClass.ClassName,
              date: new Date(`${yogaClass.ClassDate.substr(0, 10)}T${yogaClass.StartTime}`),
              extendedProps: {
                  yogaClass: yogaClass,
              },
          };
      });
      this.overlayItem = false;
    }, 
    async deleteClass(classID) {
      console.log(classID);
      await useYogaClassesStore().deleteClass(classID);
      this.confirmationDialog = { text: 'Class successfully deleted.', show: true};
      this.fetchEvents();
    },
    async addClass(newClass)
    {  
      this.addFormDialog = false;
      await useYogaClassesStore().addClass(newClass);
      this.confirmationDialog = { text: 'Class successfully added to schedule.', show: true};
      this.fetchEvents();
    },
    async updateClass(classInfo) {
      console.log("Admin Calendar view");
      console.log(classInfo);
      await useYogaClassesStore().updateClass(classInfo.id, classInfo.info);
      this.confirmationDialog = { text: 'Class successfully updated.', show: true};
      this.fetchEvents();
    }
  },   
  mounted() {
    this.fetchEvents();
  },
}
</script>

<style scoped>
h2 {
    text-align: center;
    margin: 1% auto;
}
</style>