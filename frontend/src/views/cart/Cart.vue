<template>
    <section class="h-100 bg-info">
        <div class="container py-5 h-100">
            <div class="row d-flex justify-content-center align-items-center h-100">
                <div class="col-12">
                    <div class="card card-registration card-registration-2" style="border-radius: 15px 0 0 15px;">
                        <div class="card-body p-0">
                            <div class="row g-0">
                                <div class="col-lg-8">
                                    <div class="p-5">
                                        <div class="d-flex justify-content-between align-items-center mb-5">
                                            <h1 class="fw-bold mb-0">Giỏ Hàng</h1>
                                            <h6 class="mb-0 text-muted">{{ cart ? cart.cartItems.length : 0 }} sản phẩm
                                            </h6>
                                        </div>
                                        <hr class="my-4">

                                        <div v-if="cart && cart.cartItems && cart.cartItems.length > 0"
                                            v-for="item in cart.cartItems.slice().reverse()"
                                            class="row mb-4 d-flex justify-content-between align-items-center">
                                            <div class="col-md-2 col-lg-2 col-xl-2">
                                                <img :src="item.productImg" class="img-fluid rounded-3"
                                                    alt="Cotton T-shirt">
                                            </div>
                                            <div class="col-md-3 col-lg-3 col-xl-3 p-0">
                                                <h6 class="mb-2">{{ item.productName }}</h6>
                                                <span class="text-muted text-uppercase" style="font-size: 14px;">
                                                    <i>{{ item.sizeName }}</i></span>

                                            </div>
                                            <div class="col-md-3 col-lg-3 col-xl-2 d-flex decrease-btn p-1">
                                                <button data-mdb-button-init data-mdb-ripple-init
                                                    class="btn btn-link px-2" @click="decreaseQuantity(item)">
                                                    <i class="fas fa-minus"></i>
                                                </button>

                                                <input :id="'quantity' + item.id" min="1" max="99" name="quantity"
                                                    v-model="item.quantity" :value="item.quantity" type="number"
                                                    class="form-control form-control-sm text-center" />

                                                <button data-mdb-button-init data-mdb-ripple-init
                                                    class="btn btn-link px-2" @click="increaseQuantity(item)">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>
                                            <div class="col-md-3 col-lg-2 col-xl-2 offset-lg-1">
                                                <h6 class="mb-0 price">
                                                    {{ formatPrice(item.productPrice * item.quantity) }}đ
                                                </h6>
                                            </div>
                                            <div class="col-md-1 col-lg-1 col-xl-1 text-end cursor-pointer">
                                                <a @click="removeItem(item.id)" class=" text-muted">
                                                    <i class="fas fa-times"></i></a>
                                            </div>

                                        </div>
                                        <div v-else>
                                            <h5 class="text-center">Không có sản phẩm nào trong giỏ hàng</h5>
                                        </div>

                                        <hr class="my-4">

                                        <div class="pt-5">
                                            <h6 class="mb-0"><router-link :to="{ name: 'Home' }" class="text-body">
                                                    <i class="fas fa-long-arrow-alt-left me-2"></i>Back to
                                                    shop</router-link>
                                            </h6>
                                        </div>
                                    </div>
                                </div>

                                <!-- Sumary -->
                                <div class="col-lg-4 bg-body-tertiary">
                                    <div class="p-5">
                                        <h3 class="fw-bold mb-5 mt-2 pt-1">Summary</h3>
                                        <hr class="my-4">

                                        <div class="d-flex justify-content-between mb-4">
                                            <h5 class="text-uppercase">{{ items }} items</h5>
                                            <h5 v-if="cart && cart.total" class="price">{{ formatPrice(cart.total) }}đ
                                            </h5>
                                        </div>

                                        <h5 class="text-uppercase mb-3">Shipping</h5>

                                        <div class="mb-4 pb-2">
                                            <select class="custom-select">
                                                <option value="1">Standard-Delivery</option>
                                                <option value="2">Fast-Delivery</option>
                                                <option value="3">Save-Delivery</option>
                                            </select>
                                        </div>

                                        <h5 class="text-uppercase mb-3">Give code</h5>

                                        <div class="mb-3">
                                            <div data-mdb-input-init class="form-outline">
                                                <input type="text" id="form3Examplea2" v-model="code"
                                                    @focus="resetDisCount" class="form-control form-control-lg" />
                                            </div>
                                        </div>

                                        <button v-if="code" @click="checkCode"
                                            class="btn btn-outline-primary btn-lg mb-2 w-100">
                                            Kiểm tra
                                        </button>

                                        <p v-if="discount != 0" class="text-muted text-end"><i>Áp mã thành công, giảm
                                                được
                                                {{ formatPrice(discount) }}đ</i></p>
                                        <p v-if="errorDiscount" class=" text-danger text-start"><i>{{ errorDiscount
                                                }}</i></p>

                                        <hr class="my-4">

                                        <div class="d-flex justify-content-between mb-5">
                                            <h5 class="text-uppercase">Total price</h5>
                                            <h5 v-if="cart && cart.total" class="price">{{ formatPrice(cart.total -
                                                discount) }}đ
                                            </h5>
                                        </div>

                                        <button type="button" @click="pushPayment"
                                            class="btn btn-dark btn-block btn-lg text-uppercase">
                                            <a class="text-white main-hover">Thanh
                                                toán</a></button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import cartService from '@/services/cart.service';
import discountService from '@/services/discount.service';

export default {
    data() {
        return {
            user: null,
            cart: null,
            code: '',
            items: 0,
            discount: 0,
            errorDiscount: '',
        };
    },
    methods: {
        formatPrice(price) {
            return price.toLocaleString('vi-VN');
        },
        resetDisCount() {
            this.discount = 0;
            this.errorDiscount = '';
        },
        async pushPayment() {
            if (this.cart == null || this.cart.cartItems.length == 0) {
                return;
            } else {
                this.$router.push({ name: 'Payment' });
            }
        },
        async fetchCartItems() {
            if (this.user) {
                this.cart = await cartService.getByUserId(this.user.id);
                await this.$store.commit('setCart', this.cart);
                this.items = this.cart.cartItems.reduce((acc, item) => acc + item.quantity, 0);
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
        },
        async checkCode() {
            try {
                const discount = await discountService.getByCode(this.code);

                const currentDate = new Date();
                const expiryDate = new Date(discount.expiryDate);

                if (!discount || currentDate > expiryDate) {
                    this.errorDiscount = 'Mã giảm giá đã hết hạn';
                    return;
                }

                if (this.cart.total < discount.requireMoney) {
                    this.errorDiscount = 'Số tiền không đủ để áp dụng mã giảm giá';
                    return;
                }
                if (discount.isActive == false) {
                    this.errorDiscount = 'Mã giảm giá không hợp lệ hoặc đã hết hạn';
                    return;
                }

                const potentialDiscount = (this.cart.total * discount.discountPercentage) / 100;
                this.discount = Math.min(potentialDiscount, discount.maximumDiscount);

                this.errorDiscount = '';

            } catch (error) {
                this.errorDiscount = 'Mã giảm giá không hợp lệ hoăc đã hết hạn';
            }
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

<style scoped>
@media (min-width: 1025px) {
    .h-custom {
        height: 100vh !important;
    }
}

.card-registration .select-input.form-control[readonly]:not([disabled]) {
    font-size: 1rem;
    line-height: 2.15;
    padding-left: .75em;
    padding-right: .75em;
}

.card-registration .select-arrow {
    top: 13px;
}

.btn {
    outline: none !important;
    box-shadow: none !important;
}
</style>