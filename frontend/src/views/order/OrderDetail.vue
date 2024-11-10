<template>
    <link rel="stylesheet"
        href="https://cdn.jsdelivr.net/npm/pixeden-stroke-7-icon@1.2.3/pe-icon-7-stroke/dist/pe-icon-7-stroke.min.css">
    <section class="pt-4" style="background-color: #f1f3f7;">
        <div class="container padding-bottom-3x mb-1">
            <button @click="pushOrderRoute" class="h1 border-0 py-2 bg-transparent"><i
                    class="fa-solid fa-arrow-left"></i> Đơn
                hàng</button>
            <div class="card mb-3">
                <div class="p-4 text-center text-white text-lg bg-dark rounded-top"><span class="text-uppercase">ĐƠN
                        HÀNG SỐ - </span><span class="text-medium text-uppercase">{{ order.id }}</span>
                </div>
                <div class="d-flex flex-wrap flex-sm-nowrap justify-content-between py-3 px-2 bg-secondary">
                    <div class="w-100 text-center py-1 px-2"><span class="text-medium">Hình thức thanh toán:</span> {{
                        payType[order.payType] ?? 'COD' }}
                    </div>
                    <div class="w-100 text-center py-1 px-2"><span class="text-medium">Trạng thái:</span>
                        {{ steps[order.status].title }}
                    </div>
                    <div class="w-100 text-center py-1 px-2"><span class="text-medium">Ngày giao dự kiến:</span>
                        {{ formatDate(order.updatedAt) }}</div>
                </div>
                <div class="card-body">
                    <div v-if="order.status != 4"
                        class="steps d-flex flex-wrap flex-sm-nowrap justify-content-between padding-top-2x padding-bottom-1x">
                        <div v-for="(step, index) in steps" :key="index"
                            :class="['step', { completed: index <= order.status }]">
                            <div class="step-icon-wrap">
                                <div class="step-icon"><i :class="step.icon"></i></div>
                            </div>
                            <h4 class="step-title">{{ step.title }}</h4>
                        </div>
                    </div>
                    <div v-else
                        class="steps d-flex flex-wrap flex-sm-nowrap justify-content-between padding-top-2x padding-bottom-1x">
                        <div class="step completed">
                            <div class="step-icon-wrap">
                                <div class="step-icon"><i class="pe-7s-close"></i></div>
                            </div>
                            <h4 class="step-title">Đã hủy</h4>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Details -->
        <div class="container">
            <!-- Title -->
            <div class="d-flex justify-content-between align-items-center py-3">
                <h2 class="h5 mb-0"><a href="#" class="text-muted"></a> Đơn hàng: <span class="text-uppercase">#{{
                    order.id }}</span>
                </h2>
            </div>

            <div class="row">
                <div class="col-lg-8">
                    <div class="card mb-4">
                        <div class="card-body">
                            <div class="mb-3 d-flex justify-content-between">
                                <div>
                                    <span class="me-3">{{ formatDate(order.createdAt) }}</span>
                                    <span class="me-3 text-uppercase">#{{ order.id }}</span>
                                    <span class="me-3">COD</span>
                                    <span class="badge rounded-pill bg-info">{{ steps[order.status].title }}</span>
                                </div>
                                <div class="d-flex">
                                    <button class="btn p-0 me-3 d-none d-lg-block btn-icon-text">
                                        <a class="text" style="text-decoration: none;"><i class="pe-7s-eyedropper"></i>
                                            Sửa</a>
                                    </button>
                                </div>
                            </div>
                            <table class="table table-borderless">
                                <thead>
                                    <tr>
                                        <th>Sản phẩm</th>
                                        <th class="text-center">Số lượng</th>
                                        <th class="text-end">Thành tiền</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="item in order.orderItems">
                                        <td>
                                            <div class="d-flex mb-2">
                                                <div class="flex-shrink-0">
                                                    <img :src="item.productImg" alt="" width="80" class="img-fluid">
                                                </div>
                                                <div class="flex-lg-grow-1 ms-3">
                                                    <h6 class="small mb-0 title-product">
                                                        <router-link
                                                            :to="{ name: 'ProductDetail', params: { id: item.productId } }"
                                                            class="text-reset">
                                                            {{ item.productName }}
                                                        </router-link>
                                                    </h6>
                                                    <span class="small">Size: <span class="text-uppercase">{{
                                                        item.sizeName
                                                            }}</span></span>
                                                </div>
                                            </div>
                                        </td>
                                        <td class="text-center">{{ item.quantity }}</td>
                                        <td class="text-end price">{{ formatPrice(item.productPrice * item.quantity) }}đ
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot class="border-top">
                                    <tr>
                                        <td colspan="2">Thành tiền</td>
                                        <td class="text-end price">{{ formatPrice(order.totalAmount -
                                            (shipCost[order.shipType] + priceReduceDiscount)) }}đ</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Vận chuyển</td>
                                        <td class="text-end price">{{ formatPrice(shipCost[order.shipType]) }}đ</td>
                                    </tr>
                                    <tr v-if="discount">
                                        <td colspan="2">Discount <span>(Code: {{ discount.code }})</span></td>
                                        <td class="price text-end">-{{ formatPrice(priceReduceDiscount) }}đ</td>
                                    </tr>
                                    <tr class="fw-bold">
                                        <td colspan="2">Tổng tiền</td>
                                        <td class="text-end price">{{ formatPrice(order.totalAmount) }}đ</td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </div>
                    <!-- Payment -->
                    <div class="card mb-4">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-lg-6">
                                    <h3 class="h6">Hình thức thanh toán <span class="badge bg-success rounded-pill">{{
                                        payType[order.payType] ?? 'COD' }}
                                        </span></h3>
                                    <p class="text-muted">
                                        <i>{{ method[order.payType] ?? 'Thanh toán khi nhận hàng' }}</i>
                                        <br>
                                        <i>Total: <span class="text-danger">{{ formatPrice(order.totalAmount) }}đ
                                            </span></i>
                                    </p>
                                </div>
                                <div class="col-lg-6">
                                    <h3 class="h6">Ngày đặt: <i class="text-muted">{{ formatFullDate(order.createdAt)
                                            }}</i></h3>
                                    <h3 class="h6">Ghi chú: </h3>
                                    <div v-if="order.note">
                                        <strong>{{ order.userName }}</strong><br>
                                        {{ order.note }}
                                    </div>
                                    <div v-else>
                                        <i class="text-muted">Không có ghi chú</i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="card mb-4">
                        <div class="card-body">
                            <h5 class="pb-2 border-bottom">Thông tin nhận hàng</h5>
                            <address>
                                <strong>{{ order.userName }}</strong><br>
                                {{ order.address }}<br>
                                Email: {{ order.userEmail }} <br>
                                Phone: {{ order.userPhone }}
                            </address>
                        </div>
                    </div>
                    <div class="card mb-4">
                        <!-- Shipping information -->
                        <div class="card-body">
                            <h5 class="pb-2 border-bottom">Chăm sóc khách hàng</h5>
                            <h3 class="h6 py-2"><strong>AMIE</strong></h3>
                            <h3 class="h6"><i>Số điện thoại: <span class="text-muted">076 3962 680</span></i></h3>
                            <h3 class="h6"><i>Địa chỉ:</i></h3>
                            <address>
                                <i class="text-muted">31, Ngô Sĩ Liên, KDC Metro, phường Hưng Lợi, quận Ninh Kiều, TP
                                    Cần Thơ.</i>
                            </address>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import OrderService from '@/services/order.service';
import DiscountService from '@/services/discount.service';
import { formatDate } from 'date-fns';
export default {
    data() {
        return {
            order: null,
            steps: [
                { title: 'Đang xử lý', icon: 'pe-7s-config' },
                { title: 'Đã xác nhận', icon: 'pe-7s-check' },
                { title: 'Đang vận chuyển', icon: 'pe-7s-car' },
                { title: 'Đã vận chuyển', icon: 'pe-7s-home' },
                { title: 'Đã hủy', icon: 'pe-7s-close' },
            ],
            shipCost: [0, 35000, 50000, 25000],
            payType: ['COD', 'MoMo', 'MoMo'],
            method: ['Thanh toán khi nhận hàng', 'Đã thanh toán', 'Chờ thanh toán'],
            discount: null,
            priceReduceDiscount: 0
        };
    },
    async created() {
        this.getOrder();
    },
    methods: {
        async getOrder() {
            const orderId = this.$route.params.id;
            // const orderId = "ae0013d6-ca10-4362-a774-9fc79be69372";
            try {
                this.order = await OrderService.getById(orderId);
                if (this.order.userId != this.$store.getters.getUser.id) {
                    this.$router.push({ name: 'NotFound' });
                }
                if (this.order.discountId) {
                    this.discount = await DiscountService.getById(this.order.discountId);
                    var totalAmount = this.order.orderItems.reduce((total, item) => total + item.productPrice * item.quantity, 0);
                    var potentialDiscount = (totalAmount * this.discount.discountPercentage) / 100;
                    this.priceReduceDiscount = Math.min(potentialDiscount, this.discount.maximumDiscount);
                    // this.priceReduceDiscount = totalAmount + shipCost[order.shipType] - this.order.totalAmount;
                }
            }
            catch (error) {
                console.log(error);
            }
        },
        formatDate(date) {
            return formatDate(new Date(date), 'dd/MM/yyyy');
        },
        formatFullDate(date) {
            return formatDate(new Date(date), 'HH:mm dd/MM/yyyy');
        },
        formatPrice(price) {
            return price.toLocaleString('vi-VN');
        },
        pushOrderRoute() {
            this.$router.push({ name: 'OrderList' });
        }
    }
}
</script>

<style scoped>
.btn {
    cursor: pointer;
    border: 0 !important;
}

.steps .step {
    display: block;
    width: 100%;
    margin-bottom: 35px;
    text-align: center
}

.steps .step .step-icon-wrap {
    display: block;
    position: relative;
    width: 100%;
    height: 80px;
    text-align: center
}

.steps .step .step-icon-wrap::before,
.steps .step .step-icon-wrap::after {
    display: block;
    position: absolute;
    top: 50%;
    width: 50%;
    height: 3px;
    margin-top: -1px;
    background-color: #e1e7ec;
    content: '';
    z-index: 1
}

.steps .step .step-icon-wrap::before {
    left: 0
}

.steps .step .step-icon-wrap::after {
    right: 0
}

.steps .step .step-icon {
    display: inline-block;
    position: relative;
    width: 80px;
    height: 80px;
    border: 1px solid #e1e7ec;
    border-radius: 50%;
    background-color: #f5f5f5;
    color: #374250;
    font-size: 38px;
    line-height: 81px;
    z-index: 5
}

.steps .step .step-title {
    margin-top: 16px;
    margin-bottom: 0;
    color: #606975;
    font-size: 14px;
    font-weight: 500
}

.steps .step:first-child .step-icon-wrap::before {
    display: none
}

.steps .step:last-child .step-icon-wrap::after {
    display: none
}

.steps .step.completed .step-icon-wrap::before,
.steps .step.completed .step-icon-wrap::after {
    background-color: #0da9ef
}

.steps .step.completed .step-icon {
    border-color: #0da9ef;
    background-color: #0da9ef;
    color: #fff
}

@media (max-width: 576px) {

    .flex-sm-nowrap .step .step-icon-wrap::before,
    .flex-sm-nowrap .step .step-icon-wrap::after {
        display: none
    }
}

@media (max-width: 768px) {

    .flex-md-nowrap .step .step-icon-wrap::before,
    .flex-md-nowrap .step .step-icon-wrap::after {
        display: none
    }
}

@media (max-width: 991px) {

    .flex-lg-nowrap .step .step-icon-wrap::before,
    .flex-lg-nowrap .step .step-icon-wrap::after {
        display: none
    }
}

@media (max-width: 1200px) {

    .flex-xl-nowrap .step .step-icon-wrap::before,
    .flex-xl-nowrap .step .step-icon-wrap::after {
        display: none
    }
}

.bg-faded,
.bg-secondary {
    background-color: #f5f5f5 !important;
}
</style>