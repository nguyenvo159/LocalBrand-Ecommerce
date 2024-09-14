<template>
    <div class="search-engine row justify-content-center">
      <div class="col-lg-9 col-12">
        <div class="input-group">
          <input type="text" class="form-control" v-model="searchQuery" @keyup.enter="searchByText" placeholder="Tìm kiếm sản phẩm...">
          <div class="input-group-append">
            <button class="btn btn-outline-secondary border-0" type="button" @click="searchByText">
              <i class="fas fa-search"></i>
            </button>
            <button class="btn btn-outline-secondary border-0" type="button" @click="openFileDialog">
              <i class="fas fa-camera"></i>
            </button>
            <input ref="fileInput" type="file" accept="image/*" class="d-none" @change="searchByImage" />
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import ProductService from '@/services/product.service'; 
  import ProductImageService from '@/services/image.service'; 
  
  export default {
    data() {
      return {
        searchQuery: '',
      };
    },
    methods: {
      async searchByText() {
        if (this.searchQuery.trim()) {
          try {
            const response = await ProductService.search(this.searchQuery.trim());
            this.$store.commit('setSearchResults', response);
            this.$router.push({ path: '/search', query: { keyword: this.searchQuery } });
          } catch (error) {
            console.error('Lỗi khi tìm kiếm sản phẩm bằng từ khóa:', error);
          }
        }
      },
      
      openFileDialog() {
        this.$refs.fileInput.click();
      },
  
      // Tìm kiếm bằng ảnh
      async searchByImage(event) {
        const file = event.target.files[0]; 
        if (file) {
            const formData = new FormData();
            formData.append('file', file); 

            try {
            const response = await ProductImageService.searchByImage(formData);
            this.$store.commit('setSearchResults', response);
            this.$router.push({ path: '/search', query: { keyword: 'search-by-image' } });
            } catch (error) {
            console.error('Lỗi khi tìm kiếm sản phẩm bằng hình ảnh:', error);
            }
        }
      },
    },
  };
  </script>
  
  <style scoped>
  .search-engine {
    margin: 10px 0;
  }
  
  .input-group-append {
    display: flex;
  }
  
  .input-group-append .btn {
    border-left: none;
  }
  </style>
  