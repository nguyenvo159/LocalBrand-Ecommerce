<template>
    <div>
      <h1>All Products</h1>
      <ul v-if="products.length">
        <li v-for="product in products" :key="product.id">
          <h3>{{ product.name }}</h3>
          <p>{{ product.description }}</p>
            <img v-for="img in product.imageUrls" :src="img" alt="Product Image" width="100">
          <p>Price: ${{ product.price }}</p>
        </li>
      </ul>
      <p v-else>Loading products...</p>
    </div>
  </template>
  
  <script>
  import ProductService from '@/services/product.service';
  
  export default {
    data() {
      return {
        products: []
      };
    },
    async created() {
      try {
        this.products = await ProductService.getAll();
      } catch (error) {
        console.error("Error fetching products:", error);
      }
    }
  };
  </script>
  
  <style scoped>
  h1 {
    text-align: center;
    margin-bottom: 20px;
  }
  
  ul {
    list-style-type: none;
    padding: 0;
  }
  
  li {
    border: 1px solid #ccc;
    margin-bottom: 10px;
    padding: 10px;
  }
  
  img {
    display: block;
    margin-bottom: 10px;
  }
  </style>
  