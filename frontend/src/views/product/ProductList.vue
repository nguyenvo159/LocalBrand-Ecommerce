<template>
    <section id="collection" class="py-3">
        <div class="container">
            <div class="row g-0">
                <div class="d-flex flex-wrap justify-content-center my-3 filter-button-group">
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

                <div class="py-3 px-5 d-flex justify-content-between align-items-center border-top">
                    <div class="form-group d-flex align-items-center m-0">
                        <span class="m-0 pr-3 text-muted">Sắp xếp</span>
                        <button class="btn mr-3" :class="{ 'active-sort-btn': this.sortType == 'newest' }"
                            @click="changeSort('newest')">Mới
                            nhất</button>
                        <button class="btn mr-3" :class="{ 'active-sort-btn': this.sortType == 'popular' }"
                            @click="changeSort('popular')">Phổ
                            biến</button>
                        <button class="btn mr-3" :class="{ 'active-sort-btn': this.sortType == 'related' }"
                            @click="changeSort('related')">Liên
                            quan</button>
                        <div>
                            <select class="form-control" id="sortProduct" placeholder="Giá"
                                @change="changeSortByPrice($event)">
                                <option>Giá</option>
                                <option value="price-asc">Giá: Thấp đến Cao</option>
                                <option value="price-desc">Giá: Cao đến Thấp</option>
                            </select>
                        </div>
                    </div>
                    <div class="d-flex align-items-center">
                        <nav aria-label="Page navigation example">
                            <ul class="pagination m-0">
                                <li class="page-item" @click="changePage(pageNumber - 1)"><a class="page-link" href="#">
                                        <i class="fa-solid fa-chevron-left"></i>
                                    </a>
                                </li>
                                <li class="page-item" v-for="p in totalPage" :class="{ 'active': p == pageNumber }"
                                    @click="changePage(p)">
                                    <a class="page-link" href="#">{{ p }}</a>
                                </li>

                                <li class="page-item" @click="changePage(pageNumber + 1)"><a class="page-link" href="#">
                                        <i class="fa-solid fa-chevron-right"></i>
                                    </a></li>
                            </ul>
                        </nav>
                    </div>
                </div>
                <div v-if="products.length != 0" class="collection-list row gx-0 gy-1">
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
                    <div class="d-flex justify-content-center align-items-center">
                        <nav aria-label="Page navigation example">
                            <ul class="pagination m-0">
                                <li class="page-item" @click="changePage(pageNumber - 1)"><a class="page-link" href="#">
                                        <i class="fa-solid fa-chevron-left"></i>
                                    </a>
                                </li>
                                <li class="page-item" v-for="p in totalPage" :class="{ 'active': p == pageNumber }"
                                    @click="changePage(p)">
                                    <a class="page-link" href="#">{{ p }}</a>
                                </li>

                                <li class="page-item" @click="changePage(pageNumber + 1)"><a class="page-link" href="#">
                                        <i class="fa-solid fa-chevron-right"></i>
                                    </a></li>
                            </ul>
                        </nav>
                    </div>
                </div>
                <div v-else class="w-100 d-flex justify-content-center align-items-center py-5">
                    <span><i>Hiện chưa có sản phẩm,...</i></span>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import ProductService from '@/services/product.service';
import { set } from 'date-fns';
import { data } from 'isotope-layout';
export default {
    data() {
        return {
            category: null,
            products: [],
            originalProducts: [],
            sortType: 'newest', // newest, popular, related
            sortPrice: 'price-asc', // price-asc, price-desc
            pageNumber: 1,
            pageSize: 5,
            totalPage: 1,
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
            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'dots',
                canCancel: true,
            });
            this.category = this.$route.params.category.toLocaleLowerCase();
            let fetchedProducts = [];

            const pageQuery = this.$route.query.page ? parseInt(this.$route.query.page) : 1;
            const sizeQuery = this.$route.query.size ? parseInt(this.$route.query.size) : this.pageSize;

            this.pageNumber = pageQuery;
            this.pageSize = sizeQuery;
            var data = {
                pageNumber: this.pageNumber,
                pageSize: this.pageSize,
                categoryName: null
            };
            if (this.category != 'all-collection') {
                data.categoryName = this.category;
            }
            fetchedProducts = await ProductService.getPaging(data);
            setTimeout(() => {
                loader.hide();
            }, 300);

            this.totalPage = fetchedProducts.totalPages;

            this.originalProducts = [...fetchedProducts.items];
            this.products = [...fetchedProducts.items];
            this.sortType = 'newest';
            this.sortProducts();
        },
        changeCategory(category) {
            this.pageNumber = 1;
            this.totalPage = 1;
            this.$router.push({ name: 'ProductList', params: { category } });

        },
        changeSort(sort) {
            this.sortType = sort;
            this.sortProducts();
        },
        changeSortByPrice(event) {
            this.sortPrice = event.target.value;
            this.sortProducts();
        },
        changePage(page) {
            if (page > 0 && page <= this.totalPage) {
                this.$router.replace({
                    name: 'ProductList',
                    params: { category: this.category },
                    query: { page: page, size: this.pageSize }
                }).then(() => {
                    this.fetchCollection();
                });
            }
        },
        sortProducts() {
            this.products = [...this.originalProducts];
            if (this.sortType === 'newest') {
                this.products.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
            } else if (this.sortType === 'popular') {
                this.products.sort((a, b) => b.rating - a.rating);
            }
            if (this.sortPrice === 'price-asc') {
                this.products.sort((a, b) => a.price - b.price);
            } else if (this.sortPrice === 'price-desc') {
                this.products.sort((a, b) => b.price - a.price);
            }
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
        this.pageNumber = 1;
        this.totalPage = 1;
        await this.fetchCollection();
        window.scrollTo(0, 0);
    },
};
</script>

<style>
.active-sort-btn,
.active-sort-btn:hover {
    background-color: red;
    color: #fff;
}
</style>