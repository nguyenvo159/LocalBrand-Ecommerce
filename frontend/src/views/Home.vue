<template>
  <header id="header" class="vh-100 carousel slide" data-ride="carousel"
    style="padding-top: 104px; background-image: url('https://amiri.com/cdn/shop/files/Slideshow-Banner_Desktop_AW23-Mens-6D.jpg');">

    <div class="container h-100 d-flex align-items-center carousel-inner">
      <div class="text-center carousel-item active">
        <h2 class="text-capitalize text-white">Best collection</h2>
        <h1 class="text-uppercase py-2 fw-bold text-white">Modern fashion</h1>
        <a href="#" class="btn btn-outline-secondary mt-3 text-uppercase text-white">Buy now</a>
      </div>
      <div class="text-center carousel-item">
        <h2 class="text-capitalize text-white">best price & offer</h2>
        <h1 class="text-uppercase py-2 fw-bold text-white">New Season</h1>
        <a href="#" class="btn btn-outline-secondary mt-3 text-uppercase text-white">buy now</a>
      </div>
    </div>

    <button class="carousel-control-prev" type="button" data-target="#header" data-slide="prev">
      <span class="carousel-control-prev-icon"></span>
    </button>
    <button class="carousel-control-next" type="button" data-target="#header" data-slide="next">
      <span class="carousel-control-next-icon"></span>
    </button>
  </header>

  <!-- Collection -->
  <section id="collection" class="py-5">
    <div class="container">
      <div class="topic-title text-center">
        <h2 class="position-relative d-inline-block">Bộ Sưu Tập</h2>
      </div>

      <div class="row g-0">
        <div class="d-flex flex-wrap justify-content-center mt-3 filter-button-group">
          <button type="button" class="btn rounded-pill py-2 px-3 m-2 btn-outline-dark text-capitalize"
            :class="{ 'active-filter-btn': activeFilter === '*' }" @click="setFilter('*')">All</button>

          <button v-for="category in categories" :key="category.name" type="button"
            class="btn rounded-pill py-2 px-3 m-2 btn-outline-dark text-capitalize"
            :class="{ 'active-filter-btn': activeFilter === category.name }" @click="setFilter(category.name)">
            {{ category.name }}
          </button>
        </div>

        <transition-group name="list" tag="div" class="collection-list row gx-0 gy-1">
          <div v-for="item in filteredCollections.slice(0, 8)" :key="item.id"
            :class="['collection-item col-md-6 col-lg-4 col-xl-3 p-2 mb-3', item.categoryName]">
            <div class="collection-img position-relative" @click="goToDetail(item.id)" style="min-height: 308px;">
              <img :src="item.imageUrls[0]" class="w-100 cursor-pointer">
            </div>
            <div class="text-center">
              <p class="text-capitalize my-1"><router-link class="title-product"
                  :to="{ name: 'ProductDetail', params: { id: item.id } }">{{
                    item.name }}</router-link></p>
              <span class="price">{{ formatPrice(item.price) }}₫</span>
            </div>
          </div>
        </transition-group>
      </div>

      <div class="container-fluid mt-5 mb-5 d-flex justify-content-center">
        <div class="row justify-content-center align-items-center w-50">
          <h2 class="text-center">Enjoy Your Youth!</h2>
          <div class="col-12 text-center text-justify">
            <p class="">Không chỉ là thời trang, Amiri còn là “phòng thí nghiệm” của tuổi trẻ - nơi
              nghiên cứu và cho ra đời nguồn năng lượng mang tên “Youth”. Chúng mình luôn muốn tạo nên
              những trải nghiệm vui vẻ, năng động và trẻ trung.</p>
            <br>
          </div>
        </div>
        <br><br><br><br>
      </div>
    </div>
  </section>

  <!-- Offer -->
  <section id="offers" class="py-5">
    <div class="container">
      <div
        class="row d-flex align-items-center justify-content-center text-center justify-content-lg-start text-lg-start">
        <div class="offers-content">
          <span class="text-white">Discount Up To 40%</span>
          <h2 class="mt-2 mb-4 text-white">Grand Sale Offer!</h2>
          <a href="#" class="btn btn-outline-dark text-capitalize text-white border-white rounded-pill mt-3 py-2 px-3 ">
            buy now</a>
        </div>
      </div>
    </div>
  </section>

  <!-- Blog -->
  <section id="blogs" class="py-5">
    <div class="container">
      <div class="topic-title text-center py-5">
        <h2 class="position-relative d-inline-block">Blog mới nhất</h2>
      </div>

      <div class="row g-3">
        <div class="card border-0 col-md-6 col-lg-4 bg-transparent my-3">
          <img class="blog-img" src="../assets/images/blog1.png" alt="">
          <div class="card-body px-0">
            <h4 class="card-title">AMIRI's Autumn-Winter 2024 Đại sứ toàn cầu</h4>
            <p class="card-text mt-3 text-muted">“AMIRI's Autumn-Winter 2024 là sàn diễn đầu tiên của tôi và đây cũng
              là chiến dịch đầu tiên của tôi. Sự giao thoa giữa niềm đam mê mà tôi chia sẻ về cả âm nhạc và thời trang,
              sẽ mở ra mối liên kết của tôi với Mike Amiri, chiến dịch này giống như một cách hoàn hảo để tôi bắt đầu
              vai trò Đại sứ thương hiệu AMIRI”</p>
            <p class="card-text">
              <small class="text-muted">
                <span class="fw-bold">Author: </span>Sunwoo
              </small>
            </p>
            <a href="https://www.facebook.com/share/p/PfwMVw2tSSGPvzHm/" target="_blank"
              class="btn btn-outline-dark">Xem thêm</a>
          </div>
        </div>

        <div class="card border-0 col-md-6 col-lg-4 bg-transparent my-3">
          <img class="blog-img" src="../assets/images/blog2.png" alt="">
          <div class="card-body px-0">
            <h4 class="card-title">Làm Sáng Tỏ Logo Và Thương Hiệu Thời Trang Aimiri </h4>
            <p class="card-text mt-3 text-muted"> Logo của Amiri thể hiện tinh thần nổi loạn kết hợp với sự tinh tế,
              hiện đại trở
              thành biểu
              tượng của cá nhân hóa và thời trang cao cấp và hiện đại. Logo này không chỉ đại diện cho thời trang mà còn
              là phong
              cách sống, thu hút những người muốn thể hiện bản thân và sự khác biệt trước mọi người​.</p>
            <p class="card-text">
              <small class="text-muted">
                <span class="fw-bold">Author: </span>John Doe
              </small>
            </p>
            <a href="https://indibloghub.com/post/unraveling-the-symbolism-and-legacy-of-the-amiri-logo-a-brand-identity-icon"
              class="btn btn-outline-dark" target="_blank">Xem thêm</a>
          </div>
        </div>

        <div class="card border-0 col-md-6 col-lg-4 bg-transparent my-3">
          <img class="blog-img" src="../assets/images/blog3.png" alt="">
          <div class="card-body px-0">
            <h4 class="card-title">Vì Sao Quần Jean Amiri Lại Đắt Đỏ Mà Vẫn Được Ưa Chuộng?</h4>
            <p class="card-text mt-3 text-muted">Bài viết phân tích lý do quần jean Amiri có giá cao ngất ngưởng. Nguyên
              nhân chính là việc sử dụng các vật liệu cao cấp, quy trình sản xuất tỉ mỉ bằng tay và các thiết kế độc
              quyền. Chiến lược xây dựng thương hiệu của Amiri, tập trung vào sự sang trọng và hiếm có, đã tạo
              ra giá trị cao cho sản phẩm.
            </p>
            <p class="card-text">
              <small class="text-muted">
                <span class="fw-bold">Author: </span>DoTricks
              </small>
            </p>
            <a href="https://dotricks.io/why-are-amiri-jeans-so-expensive/" class="btn btn-outline-dark"
              target="_blank">Xem thêm</a>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Popular -->
  <section id="popular" class="py-5">
    <div class="container">
      <div class="topic-title text-center pt-3 pb-5">
        <h2 class="position-relative d-inline-block ms-4">Top phổ biến</h2>
      </div>

      <div class="row align-items-start">
        <div class="col-md-6 col-lg-4 row g-3">
          <h3 class="fs-5"><i class="fa-solid fa-fire" style="color: #ed0202;"></i> Top đánh giá</h3>
          <div v-for="product in bestRate" class="d-flex align-items-start justify-content-start cursor-pointer"
            @click="goToDetail(product.id)">
            <img :src="product.imageUrls[0]" alt="" class="img-fluid pe-3 w-25">
            <div style="font-size: 12px; text-align: justify;">
              <p class="mb-0 title-product">{{ product.name }}</p>
              <span class="price">{{ formatPrice(product.price) }}₫</span>
            </div>
          </div>
        </div>

        <div class="col-md-6 col-lg-4 row g-3">
          <h3 class="fs-5"><i class="fa-solid fa-fire" style="color: #ed0202;"></i> Top bán chạy</h3>
          <div v-for="product in bestSeller" class="d-flex align-items-start justify-content-start cursor-pointer"
            @click="goToDetail(product.id)">
            <img :src="product.imageUrls[0]" alt="" class="img-fluid pe-3 w-25">
            <div style="font-size: 12px; text-align: justify;">
              <p class="mb-0 title-product">{{ product.name }}</p>
              <span class="price">{{ formatPrice(product.price) }}₫</span>
            </div>
          </div>
        </div>

        <div class="col-md-6 col-lg-4 row g-3">
          <h3 class="fs-5"><i class="fa-solid fa-fire" style="color: #ed0202;"></i> Mới nhất</h3>
          <div v-for="product in lastest" class="d-flex align-items-start justify-content-start cursor-pointer"
            @click="goToDetail(product.id)">
            <img :src="product.imageUrls[0]" alt="" class="img-fluid pe-3 w-25">
            <div style="font-size: 12px; text-align: justify;">
              <p class="mb-0 title-product">{{ product.name }}</p>
              <span class="price">{{ formatPrice(product.price) }}₫</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

</template>

<script>
import productService from '@/services/product.service';

export default {
  data() {
    return {
      collections: [],
      newCollections: [],
      categories: [],
      bestRate: [],
      bestSeller: [],
      lastest: [],
      activeFilter: '*',
    };
  },
  computed: {
    user() {
      return this.$store.getters.getUser;
    },
    isLogged() {
      return this.$store.getters.isLogged;
    },
    filteredCollections() {
      if (this.activeFilter === '*') {
        return this.collections;
      }
      return this.collections.filter(item => item.categoryName === this.activeFilter);
    }
  },
  mounted() {
    window.scrollTo(0, 0);
    this.fetchCollection();
  },
  methods: {
    logout() {
      this.$store.dispatch('logout');
      this.$router.push('/auth/login');
    },
    async fetchCollection() {
      this.collections = await productService.getAll();
      this.newCollections = this.collections.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt)).slice(0, 8);
      this.categories = this.newCollections.reduce((acc, item) => {
        if (!acc.find((category) => category.name === item.categoryName)) {
          acc.push({ name: item.categoryName });
        }
        return acc;
      }, []);
      this.bestRate = await productService.getBestRate();
      this.bestSeller = await productService.getBestSeller();
      this.lastest = this.collections.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt)).slice(0, 5);
    },
    goToDetail(id) {
      this.$router.push({ name: 'ProductDetail', params: { id } });
    },
    setFilter(filter) {
      this.activeFilter = filter;
    },
    formatPrice(price) {
      return price.toLocaleString('vi-VN');
    }
  },
  // beforeRouteEnter(to, from, next) {
  //   next(vm => {
  //     vm.$store.dispatch('loadUser').then(() => {
  //       next();
  //     });
  //   });
  // }
}
</script>

<style scoped>
#popular img {
  max-height: 86px !important;
  object-fit: contain;
}
</style>