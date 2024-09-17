<template>
  <section id="collection" class="py-5">
    <div class="container">
      <div class="row g-0">
        <div class="col-md-12">
          <h2 class="text-center">Kết quả tìm kiếm</h2>
        </div>

        <div class="collection-list row gx-0 gy-1">
          <div v-for="item in results" :key="item.id"
            class="collection-item col-md-6 col-lg-4 col-xl-3 p-2 cursor-pointer">
            <div class="collection-img position-relative">
              <img :src="item.imageUrls[0]" class="w-100">
            </div>
            <div class="text-center">
              <p class="text-capitalize my-1"><router-link class="title-product"
                  :to="{ name: 'ProductDetail', params: { id: item.id } }">{{
                    item.name }}</router-link></p>
              <span class="price">{{ item.price }}đ</span>
            </div>
          </div>
        </div>
        <div v-if="results.length == 0" class="w-100 d-flex justify-content-center align-items-center py-5">
          <span><i>Hiện chưa có sản phẩm,...</i></span>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  props: ['keyword'],
  computed: {
    results() {
      return this.$store.getters.getSearchResults;
    },
  },
};
</script>

<style scoped>
.search-results {
  margin: 20px 0;
}

.card {
  height: 100%;
}
</style>