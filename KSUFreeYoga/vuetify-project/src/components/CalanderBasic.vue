<template>
    <FullCalendar :options="calendarOptions" />
    <div v-if="showModal" class="modal">
      <div class="modal-content">
        <span class="close" @click="showModal = false">&times;</span>
        <h2>{{ selectedEvent.title }}</h2>
        <p>{{ selectedEvent.startStr }}</p>
        <!-- Add more event details here -->
      </div>
    </div>
  </template>
  
  <script>
  import FullCalendar from '@fullcalendar/vue3';
  import dayGridPlugin from '@fullcalendar/daygrid';
  
  export default {
    components: { FullCalendar },
    data() {
      return {
        showModal: false,
        selectedEvent: {},
        calendarOptions: {
          plugins: [dayGridPlugin],
          initialView: 'dayGridMonth',
          events: [
            { title: 'Event 1', date: '2024-03-01' },
            { title: 'Event new', date: '2024-03-05' },
            { title: 'Event 2', date: '2024-03-05' },
            // Add more events here
          ],
          eventClick: this.handleEventClick,
        },
      };
    },
    methods: {
      handleEventClick(clickInfo) {
        this.selectedEvent = {
          title: clickInfo.event.title,
          startStr: clickInfo.event.startStr,
          // You can add more event properties here
        };
        this.showModal = true;
      },
    },
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
  </style>
  