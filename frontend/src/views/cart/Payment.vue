<template>
    <section style="background-color: #f1f3f7;">
        <div class="container pt-3">
            <div class="row">
                <div class="col-xl-8">
                    <div class="card">
                        <div class="card-body">
                            <ol class="activity-checkout mb-0 px-4 mt-3">
                                <li class="checkout-item">
                                    <div class="avatar checkout-icon p-1">
                                        <div class="avatar-title rounded-circle bg-primary">
                                            <i class="fa-solid fa-receipt"></i>
                                        </div>
                                    </div>
                                    <div class="feed-item-list">
                                        <div>
                                            <h5 class=" mb-1">Thông tin nhận hàng</h5>
                                            <p class="text-muted text-truncate mb-4">Nhập đầy đủ thông tin bên
                                                dưới
                                            </p>
                                            <div class="mb-3">
                                                <form ref="myForm" @submit.prevent="handleSubmit">
                                                    <div>
                                                        <div class="row">
                                                            <div class="col-lg-4">
                                                                <div class="mb-3">
                                                                    <label class="form-label" for="name">Tên đầy
                                                                        đủ</label>
                                                                    <input type="text" class="form-control" id="name"
                                                                        v-model="userInfo.name" required min="3"
                                                                        max="50" placeholder="Nhập tên của bạn">
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-4">
                                                                <div class="mb-3">
                                                                    <label class="form-label" for="email-address">Email
                                                                    </label>
                                                                    <input type="email" class="form-control"
                                                                        id="email-address" required
                                                                        v-model="userInfo.email"
                                                                        placeholder="Nhập địa chỉ email">
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-4">
                                                                <div class="mb-3">
                                                                    <label class="form-label" for="phone">Phone</label>
                                                                    <input type="tel" class="form-control" id="phone"
                                                                        v-model="userInfo.phone" required
                                                                        placeholder="Nhập số điện thoại">
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row mb-3">
                                                            <div class="col-lg-4">
                                                                <div class="mb-4 mb-lg-0">
                                                                    <select class="form-control form-select"
                                                                        title="province" v-model="province"
                                                                        @change="handleProvinceChange">
                                                                        <option value="">Tỉnh/Thành phố</option>
                                                                        <option v-for="prov in provinces" :key="prov.id"
                                                                            :value="prov.name">{{ prov.name }}</option>
                                                                    </select>
                                                                </div>
                                                            </div>

                                                            <div class="col-lg-4">
                                                                <div class="mb-4 mb-lg-0">
                                                                    <select class="form-control form-select"
                                                                        title="district" v-model="district"
                                                                        @change="handleDistrictChange">
                                                                        <option value="">Quận/Huyện</option>
                                                                        <option v-for="dist in districts" :key="dist.id"
                                                                            :value="dist.name">{{ dist.name }}</option>
                                                                    </select>
                                                                </div>
                                                            </div>

                                                            <div class="col-lg-4">
                                                                <div class="mb-0">
                                                                    <select class="form-control form-select"
                                                                        v-model="ward">
                                                                        <option value="">Xã/Phường/Thị trấn</option>
                                                                        <option v-for="ward in wards" :key="ward.id"
                                                                            :value="ward.name">{{ ward.name }}</option>
                                                                    </select>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="mb-3">
                                                            <label class="form-label" for="address">Địa chỉ</label>
                                                            <textarea class="form-control" id="address" rows="3"
                                                                name="address" v-model="addressForm"
                                                                placeholder="Nhập địa chỉ cụ thể"></textarea>
                                                        </div>
                                                        <div class="mb-3">
                                                            <label class="form-label" for="address">Ghi chú</label>
                                                            <textarea class="form-control" id="note" rows="2"
                                                                name="note" v-model="note"
                                                                placeholder="Ghi chú"></textarea>
                                                        </div>
                                                    </div>
                                                </form>
                                            </div>
                                        </div>
                                    </div>
                                </li>

                                <li class="checkout-item">
                                    <div class="avatar checkout-icon p-1">
                                        <div class="avatar-title rounded-circle bg-primary">
                                            <i class="fa-solid fa-credit-card"></i>
                                        </div>
                                    </div>
                                    <div class="feed-item-list">
                                        <div>
                                            <h5 class=" mb-1">Thông tin thanh toán</h5>
                                        </div>
                                        <div>
                                            <h5 class="font-size-16 text-muted mb-3">Phương thức thanh toán:</h5>
                                            <div class="row">
                                                <div class="col-lg-3 col-sm-6">
                                                    <div>
                                                        <label class="card-radio-label">
                                                            <input type="radio" name="pay-method" id="pay-methodoption3"
                                                                class="card-radio-input">

                                                            <span class="card-radio py-3 text-center text-truncate">
                                                                <i class="fa-solid fa-money-bill d-block h2 mb-3"></i>
                                                                <span>Cash On Delivery</span>
                                                            </span>
                                                        </label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </li>

                                <li class="checkout-item">
                                    <div class="avatar checkout-icon p-1">
                                        <div class="avatar-title rounded-circle bg-primary">
                                            <i class="fa-solid fa-info"></i>
                                        </div>
                                    </div>
                                    <div class="feed-item-list">
                                        <h5 class=" mb-1">Chi tiết đơn hàng</h5>

                                        <div class="table-responsive">
                                            <table class="table table-centered mb-0 table-nowrap">
                                                <thead>
                                                    <tr>
                                                        <th class="border-top-0" style="width: 110px;" scope="col">
                                                            Product
                                                        </th>
                                                        <th class="border-top-0" scope="col">Product Desc</th>
                                                        <th class="border-top-0 text-center" scope="col">Price</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr v-if="cart && cart.cartItems" v-for="item in cart.cartItems">
                                                        <th scope="row"><img :src="item.productImg" alt="product-img"
                                                                title="product-img" class="avatar-lg rounded">
                                                        </th>
                                                        <td>
                                                            <h5 class="font-size-16"><router-link
                                                                    :to="{ name: 'ProductDetail', params: { id: item.productId } }"
                                                                    class="title-product">{{ item.productName
                                                                    }}</router-link>
                                                            </h5>
                                                            <p class="text-muted mb-0">
                                                                <i>Size <span class="text-uppercase">
                                                                        {{ item.sizeName }}</span></i>
                                                            </p>

                                                        </td>
                                                        <td>
                                                            <p class="price mb-0 text-end">{{
                                                                formatPrice(item.productPrice)
                                                                }}đ
                                                                <br> x {{ item.quantity }}
                                                            </p>
                                                        </td>
                                                    </tr>
                                                    <tr v-else>
                                                        <td colspan="3">
                                                            <p class="text-center py-3">Không có sản phẩm nào trong giỏ
                                                                hàng
                                                            </p>
                                                        </td>
                                                    </tr>

                                                </tbody>
                                            </table>
                                            <div class="pt-4">
                                                <h5 class="text-end">Thành tiền:
                                                    <span class="price">
                                                        {{ cart.total ? formatPrice(cart.total) : 0 }}đ</span>
                                                </h5>

                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </ol>
                        </div>
                    </div>

                    <div class="row my-4">
                        <div class="col">
                            <router-link :to="{ name: 'Home' }" class="btn btn-link text-muted">
                                <i class="fa-solid fa-arrow-left"></i> Quay lại mua sắm</router-link>
                        </div>
                    </div>
                </div>
                <div class="col-xl-4">
                    <div class="card checkout-order-summary">
                        <div class="card-body">
                            <div class="p-3 bg-light mb-3">
                                <h3 class=" mb-0">Order Summary
                                </h3>
                            </div>
                            <div class="p-2">
                                <div class="d-flex justify-content-between mb-4">
                                    <h5 class="text-uppercase">{{ cart.cartItems.reduce((acc, item) => acc +
                                        item.quantity, 0) }} items</h5>
                                    <h5 v-if="cart && cart.total" class="price">{{ formatPrice(cart.total) }}đ
                                    </h5>
                                </div>

                                <div class="d-flex justify-content-between mb-4">
                                    <h5 class="text-uppercase">Shipping</h5>
                                    <h5 class="price">{{ formatPrice(shipCost[shipType]) }}đ
                                    </h5>
                                </div>
                                <div class="mb-4 pb-2">
                                    <select class="custom-select" v-model="shipType">
                                        <option value="1">Standard-Delivery</option>
                                        <option value="2">Fast-Delivery</option>
                                        <option value="3">Save-Delivery</option>
                                    </select>

                                </div>

                                <div class="d-flex justify-content-between mb-4">
                                    <h5 class="text-uppercase">Give code</h5>
                                    <h5 v-if="discount != 0" class="price">
                                        -{{ formatPrice(discount) }}đ
                                    </h5>
                                </div>

                                <div class="mb-3">
                                    <div data-mdb-input-init class="form-outline">
                                        <input type="text" id="form3Examplea2" v-model="code" @focus="resetDisCount"
                                            class="form-control form-control-lg" />
                                    </div>
                                </div>

                                <button v-if="code" @click="checkCode"
                                    class="btn btn-outline-primary btn-lg mb-2 w-100">
                                    Kiểm tra
                                </button>

                                <p v-if="errorDiscount" class=" text-danger text-start"><i>{{ errorDiscount
                                        }}</i></p>

                                <hr class="my-4">

                                <div class="d-flex justify-content-between mb-5">
                                    <h5 class="text-uppercase">Total price</h5>
                                    <h5 v-if="cart && cart.total" class="price">{{ formatPrice(cart.total +
                                        shipCost[shipType] -
                                        discount) }}đ
                                    </h5>
                                </div>

                                <button type="button" class="btn btn-dark btn-block btn-lg text-uppercase"
                                    @click="handleSubmit">
                                    <a class="text-white main-hover">Thanh
                                        toán</a></button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </section>
</template>

<script>
import router from '@/router';
import CartService from '@/services/cart.service';
import discountService from '@/services/discount.service';
import orderService from '@/services/order.service';
import { add } from 'date-fns';

export default {
    data() {
        return {
            code: '',
            discount: 0,
            errorDiscount: '',
            items: 0,
            shipType: 1,
            shipCost: [0, 35000, 50000, 25000],
            userInfo: {},
            sortedData: [],
            provinces: [],
            districts: [],
            wards: [],
            province: '',
            district: '',
            ward: '',
            addressForm: '',
        };
    },
    computed: {
        cart() {
            return this.$store.getters.getCart;
        }
    },
    methods: {
        formatPrice(price) {
            return price.toLocaleString('vi-VN');
        },
        resetDisCount() {
            this.discount = 0;
            this.errorDiscount = '';
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
                    this.errorDiscount = 'Số tiền không đủ để áp dụng mã giảm giá, cần ít nhất ' + discount.requireMoney + 'đ';
                    return;
                }
                if (discount.isActive == false) {
                    this.errorDiscount = 'Mã giảm giá không hợp lệ hoặc đã hết hạn';
                    return;
                }

                const potentialDiscount = (this.cart.total * discount.discountPercentage) / 100;
                this.discount = Math.min(potentialDiscount, discount.maximumDiscount);

            } catch (error) {
                this.errorDiscount = 'Mã giảm giá không hợp lệ hoăc đã hết hạn';
            }
        },
        async submitForm(event) {
            this.$refs.myForm.submit();
        },
        async handleSubmit() {
            let loader = this.$loading;
            try {
                var address = [this.addressForm, this.ward, this.district, this.province].filter(e => e).join(', ');
                const data = {
                    userId: this.userInfo.id,
                    userName: this.userInfo.name,
                    userPhone: this.userInfo.phone,
                    userEmail: this.userInfo.email,
                    address: address,
                    shipType: this.shipType,
                    note: '',
                    code: this.code ?? ''
                };
                loader = loader.show({
                    container: null,
                    width: 100,
                    height: 100,
                    color: '#808EF4',
                    loader: 'bars',
                    canCancel: true,
                });
                var order = await orderService.create(data);
                loader.hide();
                this.$router.push({ name: 'OrderDetail', params: { id: order.id } }); s
            } catch (error) {
                loader.hide();
                console.log(error);
            }
            finally {
                loader.hide();
            }
        },
        // Xử lý địa chỉ
        loadSortedData() {
            import('@/store/sorted.json').then(data => {
                this.sortedData = data.default;
                this.provinces = this.sortedData.map(province => ({
                    id: province[0],
                    name: province[1]
                }));
            });
        },
        handleProvinceChange() {
            this.district = '';
            this.ward = '';
            this.wards = [];

            const selectedProvince = this.sortedData.find(prov => prov[1] === this.province);
            if (selectedProvince) {
                this.districts = selectedProvince[4].map(district => ({
                    id: district[0],
                    name: district[1]
                }));
            }
        },
        handleDistrictChange() {
            this.ward = '';

            const selectedProvince = this.sortedData.find(prov => prov[1] === this.province);
            if (selectedProvince) {
                const selectedDistrict = selectedProvince[4].find(dist => dist[1] === this.district);
                if (selectedDistrict) {
                    this.wards = selectedDistrict[4].map(ward => ({
                        id: ward[0],
                        name: ward[1]
                    }));
                }
            }
        }

    },
    mounted() {
        if (!this.$store.getters.getUser) {
            this.$store.dispatch('loadUser');
            this.$store.dispatch('fillCart');
        } else {
            this.userInfo = { ...this.$store.getters.getUser };
        }
        this.loadSortedData();
    },
};

</script>

<style scoped>
.card {
    margin-bottom: 24px;
    -webkit-box-shadow: 0 2px 3px #e4e8f0;
    box-shadow: 0 2px 3px #e4e8f0;
}

.card {
    position: relative;
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -webkit-box-orient: vertical;
    -webkit-box-direction: normal;
    -ms-flex-direction: column;
    flex-direction: column;
    min-width: 0;
    word-wrap: break-word;
    background-color: #fff;
    background-clip: border-box;
    border: 1px solid #eff0f2;
    border-radius: 1rem;
}

.activity-checkout {
    list-style: none
}

.activity-checkout .checkout-icon {
    position: absolute;
    top: -4px;
    left: -24px
}

.activity-checkout .checkout-item {
    position: relative;
    padding-bottom: 24px;
    padding-left: 35px;
    border-left: 2px solid #f5f6f8
}

.activity-checkout .checkout-item {
    border-color: #3b76e1
}

.activity-checkout .checkout-item:first-child:after {
    background-color: #3b76e1
}

.activity-checkout .checkout-item:last-child {
    border-color: transparent
}

.activity-checkout .checkout-item.crypto-activity {
    margin-left: 50px
}

.activity-checkout .checkout-item .crypto-date {
    position: absolute;
    top: 3px;
    left: -65px
}

.avatar {
    height: 3rem;
    width: 3rem
}


.avatar-lg {
    height: 5rem;
    width: 5rem;
    object-fit: contain;
}



.avatar-title {
    -webkit-box-align: center;
    -ms-flex-align: center;
    align-items: center;
    background-color: #3b76e1;
    color: #fff;
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    font-weight: 500;
    height: 100%;
    -webkit-box-pack: center;
    -ms-flex-pack: center;
    justify-content: center;
    width: 100%
}


.card-radio {
    background-color: #fff;
    border: 2px solid #eff0f2;
    border-radius: .75rem;
    padding: .5rem;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    display: block
}

.card-radio:hover {
    cursor: pointer
}

.card-radio-label {
    display: block
}

.card-radio-input {
    display: none
}

.card-radio-input:checked+.card-radio {
    border-color: #3b76e1 !important
}


.font-size-16 {
    font-size: 16px !important;
}

.font-size-14 {
    font-size: 14px !important;
}

.text-truncate {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}

a {
    text-decoration: none !important;
}


.form-control {
    display: block;
    width: 100%;
    padding: 0.47rem 0.75rem;
    font-size: .875rem;
    font-weight: 400;
    line-height: 1.5;
    color: #545965;
    background-color: #fff;
    background-clip: padding-box;
    border: 1px solid #e2e5e8;
    -webkit-appearance: none;
    -moz-appearance: none;
    appearance: none;
    border-radius: 0.75rem;
    -webkit-transition: border-color .15s ease-in-out, -webkit-box-shadow .15s ease-in-out;
    transition: border-color .15s ease-in-out, -webkit-box-shadow .15s ease-in-out;
    transition: border-color .15s ease-in-out, box-shadow .15s ease-in-out;
    transition: border-color .15s ease-in-out, box-shadow .15s ease-in-out, -webkit-box-shadow .15s ease-in-out;
}
</style>