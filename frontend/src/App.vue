<template>
  <Header />
  <router-view />
  <Footer />

</template>
<script>
import Header from '@/components/particals/Header.vue';
import Footer from '@/components/particals/Footer.vue';
export default {
  components: {
    Header,
    Footer,
  },
  data() {
    return {
      specialRoute: ['UserManage', 'ProductMange'],
    };
  },
};
</script>

<style></style>
