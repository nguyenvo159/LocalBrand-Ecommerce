<template>
    <section id="collection" class="py-3">
        <div class="container">
            <div class="row g-0">
                <div class="d-flex flex-wrap justify-content-center mt-3 filter-button-group">
                    <button type="button" class="btn rounded-pill py-2 px-3 m-2 btn-outline-dark text-capitalize"
                        @click="changeCategory('all-collection')"
                        :class="{ 'active-filter-btn': category === 'all-collection' }">All</button>

                    <button v-for="category in categories" :key="category.name" type="button"
                        class="btn main-hover rounded-pill py-2 px-3 m-2 btn-outline-dark text-capitalize"
                        @click="changeCategory(category.name)"
                        :class="{ 'active-filter-btn': this.category == category.name }"
                        style="transition: 0.3 ease-in-out;">
                        {{ category.name }}
                    </button>
                </div>

                <div class="collection-list row gx-0 gy-1">
                    <div v-for="item in products" :key="item.id"
                        :class="['collection-item col-md-6 col-lg-4 col-xl-3 p-2 cursor-pointer', item.categoryName]">
                        <div class="collection-img position-relative">
                            <img :src="item.imageUrls[0]" class="w-100">
                        </div>
                        <div class="text-center py-2 row align-items-center" style="min-height: 72px;">
                            <router-link class="title-product"
                                :to="{ name: 'ProductDetail', params: { id: item.id } }">{{
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
                <div v-if="products.length == 0" class="w-100 d-flex justify-content-center align-items-center py-5">
                    <span><i>Hiện chưa có sản phẩm,...</i></span>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import ProductService from '@/services/product.service';
export default {
    data() {
        return {
            category: null,
            products: [],
        };
    },
    computed: {
        categories() {
            return this.$store.getters.getCategories;
        },
    },
    watch: {
        '$route.params.category': {
            immediate: true,
            handler() {
                this.fetchCollection();
            }
        }
    },
    methods: {
        async fetchCollection() {
            this.category = this.$route.params.category.toLocaleLowerCase();
            if (this.category === 'all-collection') {
                this.products = await ProductService.getAll();
                return;
            }
            else
                this.products = await ProductService.getByCategory(this.category);
        },
        changeCategory(category) {
            this.$router.push({ name: 'ProductList', params: { category } });
        },
        formatPrice(price) {
            return price.toLocaleString('vi-VN');
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
        goToDetail(id) {
            this.$router.push({ name: 'ProductDetail', params: { id } });
        },
    },
    async mounted() {
        if (this.categories.length === 0) {
            await this.$store.dispatch('fillCategories');
        }
        await this.fetchCollection();
        window.scrollTo(0, 0);
    },
};
</script>