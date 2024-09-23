<template>
    <section class="container d-flex justify-content-center">
        <div class="row col-lg-8 col-12">
            <router-link :to="{ name: 'Cart' }" class="h3 mt-3 main-hover text-dark">Giỏ Hàng</router-link>
            <ul v-if="cart && cart.cartItems && cart.cartItems.length > 0" class="w-100 pl-0">
                <li v-for="item in cart.cartItems.slice().reverse()" :key="item.id"
                    class="position-relative list-unstyled">
                    <div class="card shadow-sm mt-3 px-3 flex-row border-0 rounded-0">
                        <div class="card-img-left pt-3 d-flex align-items-start">
                            <img :src="item.productImg" style="width:154px; max-height: 154px; object-fit: contain;"
                                class="img-fluid" alt="Ảnh sản phẩm">
                        </div>
                        <div class="card-body">
                            <router-link :to="{ name: 'ProductDetail', params: { id: item.productId } }"
                                class="px-3 card-title h5 title-product">{{ item.productName }}</router-link>
                            <div class="mb-2">
                                <span class="px-3 text-muted font-italic text-uppercase" style="font-size: 12px;">{{
                                    item.sizeName == 'free-size'
                                        ?
                                        item.sizeName :
                                        `Size
                                    ${item.sizeName}` }}</span>
                            </div>
                            <p class="price card-text px-3">{{ formatPrice(item.productPrice) }}đ</p>

                            <div class="row justify-content-between w-100 m-0">
                                <div class="input-group" style="width: auto; height: auto;">
                                    <div class="input-group-prepend">
                                        <button class="btn border rounded-0 decrease-btn"
                                            @click="decreaseQuantity(item)">
                                            <i class="fas fa-minus"></i>
                                        </button>
                                    </div>
                                    <input type="text" :id="'quantity' + item.id" name="quantity"
                                        v-model="item.quantity" @change="changeQuantity(item)"
                                        class="form-control input-number text-center" :value="item.quantity" min="1"
                                        max="99" style="max-width: 52px;" />
                                    <div class="input-group-append">
                                        <button class="btn border rounded-0 increase-btn"
                                            @click="increaseQuantity(item)">
                                            <i class="fas fa-plus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="d-flex justify-content-end">
                                    <p class="price m-0 mt-2"> <b class="text-dark">Thành tiền:</b> {{
                                        formatPrice(item.productPrice * item.quantity) }}đ</p>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="mt-2 mr-2 position-absolute" style="right: 0; top: 0;">
                        <button class="btn delete-btn" @click="removeItem(item.id)">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                </li>

            </ul>

            <div v-else class=" mb-5">
                <p class="h5 text-muted font-italic">Giỏ hàng trống, hãy quay lại khi đã thêm sản phẩm vào!</p>
            </div>

            <div class="w-100 mb-5 mt-4 d-flex justify-content-between align-items-center">
                <div>
                    <strong>Tổng tiền: </strong>
                    <span v-if="cart" class="price">{{ formatPrice(cart.total ? cart.total : '0.00') }}đ</span>
                    <span v-else class="price">0đ</span>

                </div>
                <button type="button" class="p-2 pl-4 pr-4 rounded-0 btn btn-dark text-uppercase" data-toggle="modal"
                    data-target="#orderNow">Đặt
                    hàng</button>
            </div>
        </div>
    </section>
</template>

<script>
import cartService from '@/services/cart.service';

export default {
    data() {
        return {
            user: null,
            cart: null, // move cart from computed to data
        };
    },
    methods: {
        formatPrice(price) {
            return price.toLocaleString('vi-VN');
        },
        async fetchCartItems() {
            if (this.user && this.user.id) {
                this.cart = await cartService.getByUserId(this.user.id);
                await this.$store.commit('setCart', this.cart);
            }
        },
        async removeItem(itemId) {
            await cartService.deleteItemCart(itemId);
            this.fetchCartItems();
        },
        async increaseQuantity(item) {
            if (item.quantity + 1 < 100) {
                item.quantity += 1;
                await cartService.updateProductInCart(item);
                this.fetchCartItems();
            }
        },
        async decreaseQuantity(item) {
            if (item.quantity > 1) {
                item.quantity -= 1;
                await cartService.updateProductInCart(item);
                this.fetchCartItems();
            }
        },
        async changeQuantity(item) {
            item.quantity = Math.max(1, Math.min(parseInt(item.quantity), 99));
            await cartService.updateProductInCart(item);
            this.fetchCartItems();
        }
    },
    mounted() {
        window.scrollTo(0, 0);
        if (this.$store.getters.isLogged) {
            this.user = this.$store.getters.getUser;
            this.fetchCartItems();
        }
    },
};
</script>


<style scoped></style>