<script  lang="ts">
import { GoogleSignInButton, type CredentialResponse } from "vue3-google-signin";
import api from '@/router/api';


export default {
  emits: ['closeLoginSuccess', 'closeLogin'],
  components: {
    GoogleSignInButton
  },
  data() {
    return {
      showLoginForm: true,
    }
  },
  methods: {
    handleLoginSuccess(response: CredentialResponse) {
      const { credential } = response;
      console.log("Access Token", credential);
        api.post('/auth/signin-google', { token: credential })
      .then(response => {
     
        console.log('Login successful, session started:', response.data);
        localStorage.setItem('userToken', response.data.token);
        console.log(response.data.token);
        this.$emit('closeLoginSuccess');
      })
      .catch(error => {
        console.error('Error in token validation', error);
        this.handleLoginError(); 
      });
    },
    handleLoginError() {
      console.error("Login failed");
    },
    closeLoginForm() {
      this.$emit('closeLogin');
    }
  }
};
</script>

<template>
  <v-dialog v-model="showLoginForm" persistent style="width: 60%;">
    <v-sheet class="pa-2 ma-2" style="background-color: #f5f5f5">
      <v-form style="margin: 2%">
        <span class="close" @click="closeLoginForm">&times;</span>
        <h2> Welcome to K-State Free Yoga!</h2>
        <p> Use Google to sign in.</p>
        <GoogleSignInButton class="google-button"
        @success="handleLoginSuccess"
        @error="handleLoginError"
        ></GoogleSignInButton>
        <p style="font-size: small">
          By continuing, you agree to K-State Free Yoga's Terms of Use. Read our Privacy Policy.
        </p>
      </v-form>
    </v-sheet>
  </v-dialog>
  
</template>

<style scoped>
.google-button {
  margin: 5% 0%;
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