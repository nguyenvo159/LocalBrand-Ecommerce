<template>
    <section class="pt-3" style="background-color: #f1f3f7;">
        <div class="container">
            <h3 class="text-uppercase">Danh sách đơn hàng</h3>
            <div class="row">
                <div class="d-flex flex-wrap justify-content-center mb-3 filter-button-group">
                    <button type="button" class="btn rounded-pill py-2 px-3 m-2 btn-outline-dark"
                        :class="{ 'active-filter-btn': filter == -1 }" @click="changeFilter(-1)">Tất cả</button>

                    <button v-for="(step, index) in steps" :key="step.title" type="button"
                        class="btn main-hover rounded-pill py-2 px-3 m-2 btn-outline-dark" @click="changeFilter(index)"
                        :class="{ 'active-filter-btn': filter == index }" style="transition: 0.3 ease-in-out;">
                        {{ step.title }}
                    </button>
                </div>
                <div class="col-xs-12" v-for="order in filteredOrders" :key="order.id">
                    <div class="grid invoice">
                        <div class="grid-body">
                            <div class="invoice-title">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <span class="mb-2 badge rounded-pill"
                                            :class="order.status == 3 || order.status == 4 ? 'bg-success' : 'bg-info'">{{
                                                steps[order.status].title
                                            }}</span>
                                        <h4>Đơn hàng
                                            <span class="text-uppercase"> #{{ order.id }}</span>
                                        </h4>
                                    </div>
                                </div>
                                <div>
                                    <p class="text-muted">
                                        <i><span>{{ formatDate(order.createdAt) }}</span></i>
                                    </p>
                                </div>
                            </div>
                            <hr>
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <address>
                                        <strong>Thông tin nhận hàng:</strong><br>
                                        {{ order.userName }}<br>
                                        <i class="fa-solid fa-map-location-dot mr-2"></i>{{ order.address }}<br>
                                        <i class="fa fa-phone mr-2"></i> {{ order.userPhone }}
                                    </address>
                                </div>
                                <div class="col-md-6">
                                    <address>
                                        <strong>Phương thức thanh toán</strong><br>
                                        <span>Thanh toán khi nhận hàng</span>
                                    </address>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <span class="h5">{{ order.orderItems.length }} sản phẩm</span>
                                </div>
                                <div class="col-md-12">
                                    <table class="table table-striped">
                                        <thead>
                                            <tr class="line">
                                                <td><strong>#</strong></td>
                                                <td class="text-start"><strong>Sản Phẩm</strong></td>
                                                <td class="text-center"><strong>Số Lượng</strong></td>
                                                <td class="text-center"><strong>Giá</strong></td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="(item, index) in order.orderItems" :key="item.id" class="line">
                                                <td>{{ index + 1 }}</td>
                                                <td><span>{{ item.productName }}</span></td>
                                                <td class="text-center">2</td>
                                                <td class="text-center">$75</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td class="text-right"><strong>Total</strong></td>
                                                <td class="text-right"><strong>{{ formatPrice(order.totalAmount)
                                                        }}</strong></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 d-flex justify-content-between">
                                    <router-link :to="{ name: 'OrderDetail', params: { id: order.id } }" class="h5 ">Xem
                                        chi tiết</router-link>
                                    <button v-if="order.status == 0" @click="comfirmDelete(order.id)"
                                        data-toggle="modal" data-target="#cancel-order" class="btn btn-danger">Hủy đơn
                                        hàng</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <NotificationModal modalId="cancel-order" title="Hủy đơn hàng"
                    message="Bạn có chắc chắn muốn hủy đơn hàng này không?" :confirmAction="cancelOrder"
                    :idToDelete="idDelete" />
            </div>
        </div>
    </section>
</template>

<script>
import orderService from '@/services/order.service';
import NotificationModal from '@/components/NotificationModal.vue';
import { format } from 'date-fns';
export default {
    components: {
        NotificationModal,
    },
    data() {
        return {
            filter: -1,
            orders: [],
            user: null,
            steps: [
                { title: 'Đang xử lý', icon: 'pe-7s-config' },
                { title: 'Đã xác nhận', icon: 'pe-7s-check' },
                { title: 'Đang vận chuyển', icon: 'pe-7s-car' },
                { title: 'Đã vận chuyển', icon: 'pe-7s-home' },
                { title: 'Đã hủy', icon: 'pe-7s-home' },
            ],
            idDelete: null,
        };
    },
    computed: {
        filteredOrders() {
            if (this.filter === -1) return this.orders;
            return this.orders.filter(order => order.status === this.filter);
        },
    },
    methods: {
        async retrieveOrder() {
            this.orders = await orderService.getByUserId(this.user.id);
            console.log(this.orders);
            console.log(this.user.id);
        },
        changeFilter(index) {
            this.filter = index;
        },
        formatDate(date) {
            return format(new Date(date), 'hh:mm dd/MM/yyyy');
        },
        formatPrice(price) {
            return price.toLocaleString('vi', { style: 'currency', currency: 'VND' });
        },
        comfirmDelete(id) {
            this.idDelete = id;
        },
        async cancelOrder(orderId) {
            await orderService.cancel(orderId);
            this.orders = this.orders.filter(order => order.id !== orderId);
        },
    },
    async mounted() {
        if (!this.$store.getters.getUser) {
            this.$store.dispatch('loadUser');
        }
        this.user = await this.$store.getters.getUser;
        this.orders = this.retrieveOrder();
    },
};

</script>

<style scoped>
.invoice {
    padding: 30px;
}

.invoice h2 {
    margin-top: 0px;
    line-height: 0.8em;
}

.invoice .small {
    font-weight: 300;
}

.invoice hr {
    margin-top: 10px;
    border-color: #ddd;
}

.invoice .table tr.line {
    border-bottom: 1px solid #ccc;
}

.invoice .table td {
    border: none;
}

.invoice .identity {
    margin-top: 10px;
    font-size: 1.1em;
    font-weight: 300;
}

.invoice .identity strong {
    font-weight: 600;
}

.grid {
    position: relative;
    width: 100%;
    background: #fff;
    border-radius: 2px;
    margin-bottom: 25px;
    box-shadow: 0px 1px 4px rgba(0, 0, 0, 0.1);
}
</style>