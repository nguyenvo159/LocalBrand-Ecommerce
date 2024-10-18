<template>
  <section id="collection" class="py-5">
    <div class="container">
      <div class="row g-0">
        <div class="col-md-12">
          <h2 class="">Kết quả tìm kiếm</h2>
        </div>

        <div class="collection-list row gx-0 gy-1">
          <div v-for="item in results" :key="item.id"
            class="collection-item col-md-6 col-lg-4 col-xl-3 p-2 cursor-pointer">
            <div class="collection-img position-relative">
              <img :src="item.imageUrls[0]" class="w-100">
            </div>
            <div class="text-center py-2 row align-items-center" style="min-height: 72px;">
              <router-link class="title-product" :to="{ name: 'ProductDetail', params: { id: item.id } }">{{
                item.name }}</router-link>
              <div class="mt-3 d-flex justify-content-around">
                <span class="price">{{ formatPrice(item.price) }}₫</span>
                <div>
                  <span v-for="n in 5" :key="n" class="star">
                    <i :class="getStarClass(n, item.rating)" aria-hidden="true"></i>
                  </span>
                </div>
              </div>
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
import { format } from 'date-fns';
import productService from '@/services/product.service';
export default {
  computed: {
    results() {
      return this.$store.getters.getSearchResults;
    },
  },
  watch: {
    '$route.query.keyword'(newQuery) {
      if (newQuery) {
        this.search(newQuery);
      }
    },
  },
  created() {
    const keyword = this.$route.query.keyword;
    if (keyword) {
      this.search(keyword);
    }
  },
  methods: {
    async search(query) {
      try {
        const response = await productService.search(query.trim());
        this.$store.commit('setSearchResults', response);
      } catch (error) {
        console.error('Search failed', error);
      }
    },
    formatPrice(price) {
      return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
    },
    getStarClass(index, rating) {
      if (rating >= index) {
        return 'fa-solid fa-star';
      } else if (rating >= index - 0.5) {
        return 'fa-solid fa-star-half-stroke';
      } else {
        return 'fa-regular fa-star';
      }
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