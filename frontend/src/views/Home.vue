<template>
    <div class="">
      <SearchEngine />
      <h1 v-if="user">Xin chào, {{ user.name }} , {{ user.role }} 
        , Đã đăng nhập: {{ isLogged }}</h1>
      <p v-else>Vui lòng 
            <router-link :to="{ name: 'Login' }">Đăng nhập</router-link>
      </p>
      <br>
      <button @click="logout" class="btn btn-primary">Log out</button> <br>
      <router-link class="btn btn-success" :to="{name: 'UserManage'}">Admin</router-link>
    </div>
  </template>
  
  <script>
  import SearchEngine from '@/components/SearchEngine.vue';
  export default {
    components: {
      SearchEngine
    },
    computed: {
      user() {
        return this.$store.getters.getUser;
      },
      isLogged(){
        return this.$store.getters.isLogged;
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
  