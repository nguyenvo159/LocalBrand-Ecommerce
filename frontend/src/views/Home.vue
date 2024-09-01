<template>
    <div class="p-4">
      <h1 v-if="user">Xin chào, {{ user.name }}</h1>
      <p v-else>Vui lòng 
            <router-link :to="{ name: 'Login' }">Đăng nhập</router-link>
      </p>
      <br>
      <button @click="logout" class="btn btn-primary">Log out</button>
    </div>
  </template>
  
  <script>
  export default {
    computed: {
      user() {
        return this.$store.getters.getUser;
      }
    },
    methods: {
      logout() {
        this.$store.dispatch('logout');
        this.$router.push('/auth/login');
      }
    },
    beforeRouteEnter(to, from, next) {
      // Trước khi vào route Home, gọi action loadUser
      next(vm => {
        vm.$store.dispatch('loadUser').then(() => {
          next();
        });
      });
    }
  }
  </script>
  