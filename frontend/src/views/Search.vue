<template>
    <div class="search-results">
      <h3>Kết quả tìm kiếm</h3>
      <div v-if="results.length">
        <div class="row">
          <div v-for="product in results" :key="product.id" class="col-lg-3 col-md-4 col-sm-6 mb-3">
            <div class="card h-100">
              <img :src="product.imageUrls[0]" class="card-img-top" :alt="product.name" />
              <div class="card-body">
                <h5 class="card-title">{{ product.name }}</h5>
                <p class="card-text">{{ product.price }} VNĐ</p>
                <p class="card-text">{{ product.description }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div v-else>
        <p>Không tìm thấy kết quả nào.</p>
      </div>
    </div>
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
  