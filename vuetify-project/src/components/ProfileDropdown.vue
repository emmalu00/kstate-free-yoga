<template>
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-btn icon="$account" v-bind="props" > </v-btn>
        </template>
        <v-card v-if="user">
            <v-card-title> {{user.FirstName}} {{user.LastName}} </v-card-title>
            <v-card-text> {{user.Email}}</v-card-text>
            <v-list-item>
              <RouterLink to="/account">
                <v-btn density="compact" variant="flat" color="#BF98B2"> Account Info & Schedule </v-btn>
              </RouterLink>
            </v-list-item>
            <v-divider> </v-divider>
            <v-list>
              <v-list-item v-if="user.UserRole === 'admin'" >
                <RouterLink to="/admin">
                  <v-btn density="compact" variant="flat" color="#BF98B2"> Admin Manager </v-btn>
                </RouterLink>
              </v-list-item>
              <v-list-item>
                <v-btn density="compact" variant="tonal" @click="userLogout"> Logout </v-btn>
              </v-list-item>
            </v-list>
        </v-card>
    </v-menu>
</template>
  
  <script >
  import { RouterLink } from 'vue-router';

  import { useUserStore } from '@/stores/UserStore';
  
  export default {
    emits: ['loggingOut'],
    data() {
      return {
        menuShow: false,
        user: null, 
      }
    },
    methods: {
      async userInfo()
      {
          const userStore = useUserStore();
          await userStore.getUser();
          this.user = userStore.user[0];
          console.log(this.user);
          this.loginSuccess = true;
      },
      userLogout()
      {
        this.$emit('loggingOut');
      }
    }, 
    mounted() {
      this.userInfo();
    }
  }
  </script>
  
  <style>

  </style>
  