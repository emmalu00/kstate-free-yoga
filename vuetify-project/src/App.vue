<template>
  <v-app>
    <v-main>
      <v-app-bar style="background-color: #f8f6f9;">
        <RouterLink to="/"> </RouterLink>
          <v-app-bar-title>
            <RouterLink to="/" class="app-bar-title">K-State Free Yoga </RouterLink>  
          </v-app-bar-title> 
        <v-spacer></v-spacer>
        <RouterLink to="/schedule" class="menu-option"> Schedule </RouterLink>
        <ProfileDropdown v-if="loggedIn" @loggingOut="handleLogout" />
        <a v-else class="menu-option" @click="showLoginDialog = true" > Login </a>
      </v-app-bar>
      <router-view></router-view>
      <LoginForm v-if="showLoginDialog" @closeLogin="closeLogin" @closeLoginSuccess="closeLoginSuccess"/>
    </v-main>
  </v-app>
</template>

<script >
import { RouterLink } from 'vue-router';
import LoginForm from './components/LoginForm.vue';
import { useUserStore } from '@/stores/UserStore';
import ProfileDropdown from './components/ProfileDropdown.vue';
import VueJwtDecode from 'vue-jwt-decode'

export default {
  data() {
    return {
      showLoginDialog: false,
      loggedIn: false,
      user: [], 
    }
  },
  methods: {
    closeLoginSuccess()
    {
      this.showLoginDialog = false;
      this.loggedIn = true;
      window.location.reload();
    },
    closeLogin() {
      this.showLoginDialog = false;
    },
    async userInfo()
    {
        console.log("Inside UserInfo method");
        const userStore = useUserStore();
        await userStore.getUser();
        this.user = userStore.user[0];
        console.log(this.user);
    },
    checkAuth()
    {
      const token = localStorage.getItem('userToken');
      if (token)
      {
        const decoded = VueJwtDecode.decode(token);
        const currentTime = Date.now() / 1000;
        if (decoded.exp < currentTime)
        {
          this.handleLogout();
        }
        else { this.loggedIn = true; }
      }
      else 
      {
        this.loggedIn = false;
      }
      
    }, 
    handleLogout()
    {
      this.loggedIn = false;
      console.log("removing header");
      localStorage.removeItem('userToken');
      this.$router.push('/');
    }
  }, 
  mounted() {
    this.checkAuth();
  }
}
</script>

<style>
.menu-option, .app-bar-title {
  text-decoration: none;
  color: black !important; 
  margin-right: 20px;
}
</style>
