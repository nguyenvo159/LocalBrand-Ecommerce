<template>
    <div class="modal fade" :id="modalId" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header mb-2">
                    <h3 class="w-100 modal-title text-uppercase text-center py-2" id="exampleModalLabel">{{ title }}
                    </h3>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div v-if="orderLocal" ass="modal-body">
                    <div class="container">
                        <div class="row">
                            <div class="col-12">
                                <div class="card mb-1 border-0">
                                    <div class="card-body">
                                        <h5 class="mb-3"><span class="pb-2 border-bottom">Thông tin nhận
                                                hàng</span></h5>
                                        <address>
                                            <strong>{{ orderLocal.userName }}</strong><br>
                                            {{ orderLocal.address }}<br>
                                            Email: {{ orderLocal.userEmail }} <br>
                                            Phone: {{ orderLocal.userPhone }}
                                        </address>
                                    </div>
                                </div>
                                <div class="card mb-1">
                                    <div class="card-body">
                                        <div class="mb-3 d-flex justify-content-between">
                                            <div>
                                                <span class="me-3">{{
                                                    formatDate(orderLocal.createdAt) }}</span>
                                                <span class="me-3 text-uppercase">#{{ orderLocal.id }}</span>
                                                <span class="me-3">{{ orderLocal.payType ? payType[orderLocal.payType] :
                                                    "COD" }}</span>
                                                <span class="badge rounded-pill bg-info">{{ steps[orderLocal.status]
                                                    }}</span>
                                            </div>
                                            <div class="d-flex">
                                                <button class="btn p-0 me-3 d-none d-lg-block btn-icon-text">
                                                    <a class="text" style="text-decoration: none;"><i
                                                            class="pe-7s-eyedropper"></i>
                                                        Sửa</a>
                                                </button>
                                            </div>
                                        </div>
                                        <table class="table table-borderless">
                                            <thead>
                                                <tr>
                                                    <th>Sản phẩm</th>
                                                    <th class="text-center">Số lượng</th>
                                                    <th class="text-center">Giá</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr v-for="item in orderLocal.orderItems">
                                                    <td>
                                                        <div class="d-flex mb-2">
                                                            <div class="flex-shrink-0">
                                                                <img :src="item.productImg" alt="" width="80"
                                                                    class="img-fluid">
                                                            </div>
                                                            <div class="flex-lg-grow-1 ms-3">
                                                                <h6 class="small mb-0 title-product">
                                                                    <router-link
                                                                        :to="{ name: 'ProductDetail', params: { id: item.productId } }"
                                                                        class="text-reset">
                                                                        {{ item.productName }}
                                                                    </router-link>
                                                                </h6>
                                                                <span class="small">Size: <span
                                                                        class="text-uppercase">{{
                                                                            item.sizeName
                                                                        }}</span></span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td class="text-center">{{ item.quantity }}</td>
                                                    <td class="text-center price">{{ formatPrice(item.productPrice) }}đ
                                                    </td>
                                                </tr>
                                            </tbody>
                                            <tfoot class="border-top">
                                                <tr>
                                                    <td colspan="2">Thành tiền</td>
                                                    <td class="text-end price">{{ formatPrice(orderLocal.totalAmount -
                                                        (shipCost[orderLocal.shipType] + priceReduceDiscount)) }}đ</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">Vận chuyển</td>
                                                    <td class="text-end price">{{
                                                        formatPrice(shipCost[orderLocal.shipType])
                                                    }}đ</td>
                                                </tr>
                                                <tr v-if="discount">
                                                    <td colspan="2">Discount <span>(Code: {{ discount.code }})</span>
                                                    </td>
                                                    <td class="price text-end">-{{ formatPrice(priceReduceDiscount) }}đ
                                                    </td>
                                                </tr>
                                                <tr class="fw-bold">
                                                    <td colspan="2">Tổng tiền</td>
                                                    <td class="text-end price">{{ formatPrice(orderLocal.totalAmount)
                                                        }}đ
                                                    </td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>

                                <!-- Payment -->
                                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col">
                                                <h3 class="h6">Ngày đặt: <i class="text-muted">{{
                                                    formatFullDate(orderLocal.createdAt)
                                                        }}</i></h3>
                                                <h3 class="h6">Ghi chú: </h3>
                                                <div v-if="orderLocal.note">
                                                    <strong>{{ orderLocal.userName }}</strong><br>
                                                    {{ orderLocal.note }}
                                                </div>
                                                <div v-else>
                                                    <i class="text-muted">Không có ghi chú</i>
                                                </div>
                                            </div>
                                            <div class="col">
                                                <h3 class="h6">Hình thức thanh toán <span
                                                        class="badge bg-success rounded-pill">
                                                        {{ orderLocal.payType ? payType[orderLocal.payType] : "COD" }}
                                                    </span></h3>
                                                <p class="text-muted">
                                                    <span>
                                                        {{ orderLocal.payType ?
                                                            method[orderLocal.payType] : "Thanh toán khi nhận hàng" }}
                                                    </span>
                                                    <br>
                                                    <i>Total: <span class="text-danger">{{
                                                        formatPrice(orderLocal.totalAmount) }}đ
                                                        </span></i>
                                                </p>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import DiscountService from '@/services/discount.service';
import { formatDate } from 'date-fns';
export default {
    data() {
        return {
            steps: [
                'Đang xử lý',
                'Đã xác nhận',
                'Đang vận chuyển',
                'Đã vận chuyển',
                'Đã hủy'
            ],
            shipCost: [0, 35000, 50000, 25000],
            payType: ['COD', 'VNPAY', 'VNPAY'],
            method: ['Thanh toán khi nhận hàng', 'Chờ thanh toán', 'Đã thanh toán'],
            discount: null,
            priceReduceDiscount: 0,
            orderLocal: { ...this.order },
        };
    },
    props: {
        modalId: {
            type: String,
            required: true
        },
        title: {
            type: String,
            required: true
        },
        order: {
            type: Object,
            required: true
        }
    },
    watch: {
        order: {
            handler(newVal) {
                this.orderLocal = { ...newVal };
            },
            deep: true
        }
    },
    methods: {
        getMethod(type) {
            return type ? method[type] : "Thanh toán khi nhận hàng";
        },
        async getDiscount() {
            try {
                if (this.order.discountId) {
                    this.discount = await DiscountService.getById(this.order.discountId);
                    var totalAmount = this.order.orderItems.reduce((total, item) => total + item.productPrice * item.quantity, 0);
                    var potentialDiscount = (totalAmount * this.discount.discountPercentage) / 100;
                    this.priceReduceDiscount = Math.min(potentialDiscount, this.discount.maximumDiscount);
                }
            }
            catch (error) {
                console.log(error);
            }
        },
        formatDate(date) {
            if (!date) {
                return 'Không có ngày';
            }
            return formatDate(new Date(date), 'dd/MM/yyyy');
        },
        formatFullDate(date) {
            if (!date) {
                return 'Không có ngày';
            }
            return formatDate(new Date(date), 'HH:mm dd/MM/yyyy');
        },
        formatPrice(price) {
            if (!price || isNaN(price)) {
                return '0đ';
            }
            return price.toLocaleString('vi-VN');
        }

    }
}
</script>
