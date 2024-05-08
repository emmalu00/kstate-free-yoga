<template>
<div class="background-container">
    <div class="background-image"></div>

    <div class="overlay">
    Welcome to K-State Free Yoga!
    <v-divider style="padding:2%"></v-divider>
    <RouterLink to="/schedule" class="menu-option">
        <v-btn block color="#644874" variant="flat"> View Schedule </v-btn>
    </RouterLink>
    </div>
</div>
</template>

<script setup>
import { RouterLink } from 'vue-router';

</script>

<style scoped>
.background-container {
    position: relative;
    width: 100%;
    height: 100vh; 
    overflow: hidden; 
}

.background-image {
    position: absolute;
    width: 100%;
    height: 100%;
    background-image: url('@/assets/kstateYoga1.jpg'); 
    background-size: cover; 
    background-position: center; 
    opacity: 0.5;
}

.overlay {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    font-size: 32px; 
    font-weight: bold;
    color: white; 
    background-color: rgba(0, 0, 0, 0.5); 
    padding: 20px; 
    border-radius: 5px; 
    opacity: 1.0;
}
</style>
