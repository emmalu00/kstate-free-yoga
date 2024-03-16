<template>
    <div>
      <button @click="showModal = true">Add Yoga Class</button>
  
      <div v-if="showModal" class="modal">
        <div class="modal-content">
          <span class="close" @click="closeModal">&times;</span>
          <h2>Add Yoga Class</h2>
          <form @submit.prevent="submitForm">
            <input type="number" v-model="formData.classID" placeholder="Class ID" required />
            <input type="text" v-model="formData.className" placeholder="Class Name" required />
            <input type="time" v-model="formData.startTime" placeholder="Start Time" required />
            <input type="number" v-model="formData.duration" placeholder="Duration (minutes)" required />
            <input type="date" v-model="formData.classDate" placeholder="Class Date" required />
            <input type="text" v-model="formData.teacherName" placeholder="Teacher Name" required />
            <input type="number" v-model="formData.locationID" placeholder="Location ID" required />
            <input type="number" v-model="formData.matsAvailable" placeholder="Mats Available" required />
            <textarea v-model="formData.classDescription" placeholder="Class Description" required></textarea>
            <button type="submit">Add Class</button>
          </form>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import { ref } from 'vue';
  import { useYogaClassesStore } from '@/stores/YogaClasses'; // Adjust the path to your store file
  
  export default {
    setup() {
      const showModal = ref(false);
      const formData = ref({
        classID: null,
        className: '',
        startTime: '',
        duration: null,
        classDate: '',
        teacherName: '',
        locationID: null,
        matsAvailable: null,
        classDescription: ''
      });
  
      const closeModal = () => {
        showModal.value = false;
        // Optionally reset form here if desired
      };
  
      const submitForm = async () => {
        const yogaClassesStore = useYogaClassesStore();
        try {
          await yogaClassesStore.addYogaClassInformation(formData.value);
          alert('Class added successfully');
          closeModal();
          // Reset form after successful submission if not resetting on closeModal
        } catch (error) {
          console.error('Error adding yoga class:', error);
          alert('Failed to add class');
        }
      };
  
      return {
        formData,
        showModal,
        closeModal,
        submitForm
      };
    }
  };
  </script>
  
  <style scoped>
  .modal {
    display: block;
    position: fixed;
    z-index: 1;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    overflow: auto;
    background-color: rgb(0, 0, 0);
    background-color: rgba(0, 0, 0, 0.4);
  }
  
  .modal-content {
    background-color: #fefefe;
    margin: 15% auto;
    padding: 20px;
    border: 1px solid #888;
    width: 80%;
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
  
  button {
    cursor: pointer;
  }
  </style>
  