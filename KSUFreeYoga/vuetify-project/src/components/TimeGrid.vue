<template>
    <FullCalendar :options="calendarOptions" />
  </template>
  
  <script>
  // Import the FullCalendar Vue component
  import FullCalendar from '@fullcalendar/vue3'
  // Import the plugins you need
  import timeGridPlugin from '@fullcalendar/timegrid'
  
  export default {
    components: {
      FullCalendar // Register the FullCalendar component
    },
    data() {
      return {
        // Define your FullCalendar options
        calendarOptions: {
          plugins: [timeGridPlugin], // Use the TimeGrid plugin
          initialView: 'timeGridWeek', // Set the initial view
          headerToolbar: { // Configure the header toolbar
            left: 'prev,next',
            center: 'title',
            right: 'timeGridWeek,timeGridDay' // Allow switching views
          }
        }
      }
    }
  }
  </script>
  