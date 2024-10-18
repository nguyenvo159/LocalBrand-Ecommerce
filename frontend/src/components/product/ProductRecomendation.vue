<template>
    <div>
        <h2 class="text-center">Sản phẩm gợi ý</h2>
        <div v-if="products.length != 0" class="collection-list row gx-0 gy-1">
            <div v-for="item in products" :key="item.id"
                :class="['collection-item col-md-6 col-lg-4 col-xl-3 p-2 cursor-pointer', item.categoryName]">
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
    </div>
</template>

<script>
import ProductService from '@/services/product.service';
export default {
    data() {
        return {
            products: [],
            pageNumber: 1,
            pageSize: 4,
            totalPage: 1,
        };
    },
    props: {
        category: {
            type: String,
            required: true,
        },
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

            var data = {
                pageNumber: this.pageNumber,
                pageSize: this.pageSize,
                categoryName: this.category,
            };
            setTimeout(() => {
                loader.hide();
            }, 300);

            this.products = (await ProductService.getPaging(data)).items;
            this.totalPage = this.products.totalPages;

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
        this.pageNumber = 1;
        this.totalPage = 1;
        await this.fetchCollection();
        window.scrollTo(0, 0);
    },
};
</script>