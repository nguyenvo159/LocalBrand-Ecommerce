<template>
    <div class="container-fluid">
        <div class="row">
            <DashBoard type="Order" />
            <div id="dv" class="col-lg-10 offset-lg-2 col-11 offset-1 admin-content">
                <h3 class="mb-4 text-uppercase">Quản lý đơn hàng </h3>

                <div class="row row-cols-1 row-cols-md-2 row-cols-xl-4">
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-info">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng đơn hàng</p>
                                        <h4 class="my-1 text-info">{{ orderAnalytics?.orderCount }}</h4>
                                        <p class="mb-0 font-13">so
                                            với tuần
                                            trước {{
                                                orderAnalytics?.orderCountLastWeek
                                            }} đơn
                                        </p>
                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-scooter text-white ms-auto">
                                        <i class="fa fa-shopping-cart"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-danger">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng doanh thu</p>
                                        <h4 class="my-1 text-danger">{{ formatPrice(orderAnalytics?.revenuesWeek) }}
                                        </h4>
                                        <p class="mb-0 font-13"> so
                                            với tuần trước {{ formatPrice(orderAnalytics?.revenuesLastWeek) }}</p>
                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-bloody text-white ms-auto"><i
                                            class="fa fa-dollar"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-success">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Doanh thu tháng</p>
                                        <h4 class="my-1 text-success">{{ formatPrice(orderAnalytics?.revenueMonth
                                        ) }}</h4>
                                        <p class="mb-0 font-13">so
                                            với tháng trước {{ formatPrice(orderAnalytics?.revenueLastMonth) }} </p>
                                    </div>
                                    <div
                                        class="widgets-icons-2 rounded-circle bg-gradient-ohhappiness text-white ms-auto">
                                        <i class="fa fa-bar-chart"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-warning">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Số đơn hủy</p>
                                        <h4 class="my-1 text-warning">{{ orderAnalytics?.canceledOrdersThisWeek
                                            }}</h4>
                                        <p class="mb-0 font-13"> so với tháng trước {{
                                            orderAnalytics?.canceledOrdersLastWeek
                                        }} đơn</p>
                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-blooker text-white ms-auto">
                                        <i class="fa fa-users"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 mb-3">
                        <div class="panel panel-default panel-order">
                            <div class="mb-2">
                                <h5>Danh sách đơn hàng</h5>
                                <div class="row justify-content-between">
                                    <div class="col-12 col-lg-8 btn-group pull-right">
                                        <div class="btn-group form-group d-flex align-items-center justify-content-">
                                            <i class="fa-solid fa-filter mr-2"></i><span class="mr-3">Filter: </span>
                                            <select class="form-control rounded-pill" @change="fectchOrder"
                                                v-model="filter">
                                                <option value="">Tất cả</option>
                                                <option value="0">Đang xử lý</option>
                                                <option value="1">Đã xác nhận</option>
                                                <option value="2">Đang giao</option>
                                                <option value="3">Đã giao</option>
                                                <option value="4">Đã hủy</option>
                                            </select>
                                            <div class="col-md-4">
                                                <div id="search-input"
                                                    class="w-100 input-group d-flex align-items-center">
                                                    <span class="pr-2">Từ: </span>
                                                    <input type="date" class="form-control rounded-pill"
                                                        v-model="fromDate" @change="fectchOrder"
                                                        style="box-shadow: none;" />
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div id="search-input"
                                                    class="w-100 input-group d-flex align-items-center">
                                                    <span class="pr-2">Đến: </span>
                                                    <input type="date" class="form-control rounded-pill"
                                                        v-model="toDate" @change="fectchOrder"
                                                        style="box-shadow: none;" />
                                                </div>
                                            </div>
                                            <button class="btn text-dark" style="box-shadow: none; border: none;"
                                                @click="refreshList()">
                                                <i class=" fa-solid fa-rotate-right"
                                                    style="font-size: 20px;"></i></button>
                                        </div>
                                    </div>
                                    <div class="col col-md-4">
                                        <div id="search-input" class="w-100 input-group d-flex align-items-center">
                                            <span class="pr-2">Search: </span>
                                            <input type="text" class="form-control rounded-pill"
                                                placeholder="Nhập thông tin cần tìm..." v-model="searchKey"
                                                @keyup.enter="searchOrder" style="box-shadow: none;" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel-body">
                                <div v-if="orders" v-for="(order, index) in orders.items" class="row">
                                    <div class="col-md-1 d-flex justify-content-center align-items-center"><span>{{
                                        index + 1 }}</span></div>
                                    <div class="col-md-11">
                                        <div class="row">
                                            <div class="col-md-12 mt-3">
                                                <div class="status-order btn-group form-group">
                                                    <select class="form-control text-white rounded-pill"
                                                        :class="getStatusClass(order.status)" v-model="order.status"
                                                        @change="updateStatus(order)">
                                                        <option class="bg-white text-dark" value="0">Đang xử lý</option>
                                                        <option class="bg-white text-dark" value="1">Đã xác nhận
                                                        </option>
                                                        <option class="bg-white text-dark" value="2">Đang giao</option>
                                                        <option class="bg-white text-dark" value="3">Đã giao</option>
                                                        <option class="bg-white text-dark" value="4">Đã hủy</option>
                                                    </select>
                                                </div>

                                                <span><strong>Đơn hàng:</strong></span> <span class="text-uppercase">#{{
                                                    order.id }}</span><br />
                                                Tổng số lượng: {{ order.orderItems.reduce((acc, item) => acc +
                                                    item.quantity,
                                                    0) }}, Tổng tiền: <span class="price">{{ formatPrice(order.totalAmount)
                                                    }}</span> <br />
                                                <div class="d-flex my-3">
                                                    <button class="btn btn-success mr-2" href="" data-toggle="modal"
                                                        data-target="#order-view" @click="orderView(order)"><i
                                                            class="fa-solid fa-eye"></i></button>
                                                    <button class="btn btn-danger"><i
                                                            class="fa-solid fa-trash-can"></i></button>
                                                </div>
                                            </div>
                                            <div class="col-md-12 create-by py-2">Đặt vào ngày: {{
                                                formatDate(order.createdAt) }} bởi
                                                <a href="#">{{ order.userName }}
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="my-3 px-4">
                                <nav aria-label="Page navigation example">
                                    <ul class="pagination">
                                        <li class="page-item" @click="changePage(pageNumber - 1)"><a class="page-link"
                                                href="#">
                                                <i class="fa-solid fa-chevron-left"></i>
                                            </a>
                                        </li>
                                        <li class="page-item" v-for="p in totalPages"
                                            :class="{ 'active': p == pageNumber }" @click="changePage(p)">
                                            <a class="page-link" href="#">{{ p }}</a>
                                        </li>

                                        <li class="page-item" @click="changePage(pageNumber + 1)"><a class="page-link"
                                                href="#">
                                                <i class="fa-solid fa-chevron-right"></i>
                                            </a></li>
                                    </ul>
                                </nav>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <OrderView :order="order" title="Chi tiết đơn hàng" modalId="order-view" />
</template>

<script>
import SearchInput from '@/components/SearchInput.vue';
import NotificationModal from '@/components/NotificationModal.vue';
import DashBoard from '@/components/admin/DashBoard.vue';
import OrderService from '@/services/order.service';
import OrderView from '@/components/admin/OrderView.vue';
import { formatDate, set } from 'date-fns';

export default {
    components: {
        DashBoard,
        SearchInput,
        NotificationModal,
        OrderView
    },
    data() {
        return {
            orders: null,
            orderAnalytics: null,
            pageNumber: 1,
            pageSize: 20,
            totalPages: 1,
            searchKey: '',
            status: ['Đang xử lý', 'Đã xác nhận', 'Đang giao hàng', 'Đã giao', 'Đã hủy'],
            filter: '',
            order: null,
            fromDate: this.fromDate ? new Date(this.fromDate).toISOString() : null,
            toDate: this.toDate ? new Date(this.toDate).toISOString() : null
        };
    },
    methods: {
        async fectchOrder() {
            var data = {
                status: this.filter,
                pageNumber: this.pageNumber,
                pageSize: this.pageSize,
                search: this.searchKey,
                fromDate: this.fromDate,
                toDate: this.toDate
            }
            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'bars',
                canCancel: true,
            });
            setTimeout(async () => {
                this.orders = await OrderService.getPaging(data);
                this.orderAnalytics = await OrderService.getOrdersAnalytics();
            }, 500);
            setTimeout(() => {
                loader.hide();
            }, 500);

            this.totalPages = this.orders.totalPages;
        },
        async searchOrder() {
            this.pageNumber = 1;
            this.fectchOrder();
        },
        async refreshList() {
            this.pageNumber = 1;
            this.searchKey = '';
            this.filter = '';
            this.fectchOrder();
        },
        async updateStatus(order) {
            var data = {
                id: order.id,
                userPhone: order.userPhone,
                status: order.status
            }
            await OrderService.update(data);
            this.fectchOrder();
        },
        getStatusClass(status) {
            switch (status) {
                case 0: return 'bg-warning';   // Đang xử lý
                case 1: return 'bg-info';      // Đã xác nhận
                case 2: return 'bg-primary';   // Đang giao
                case 3: return 'bg-success';   // Đã giao
                case 4: return 'bg-danger';    // Đã hủy
                default: return 'bg-secondary'; // Trạng thái khác
            }
        },
        formatPrice(price) {
            return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
        },
        formatDate(date) {
            return formatDate(new Date(date), 'mm:hh dd/MM/yyyy');
        },
        filterOrder() {
            this.orders
        },
        changePage(page) {
            if (page > 0 && page <= this.totalPages) {
                this.pageNumber = page;
                this.fectchOrder();
            }
        },
        orderView(order) {
            this.order = order;
        }
    },
    mounted() {
        this.fectchOrder();
    },

}

</script>
<style scoped>
.status-order {
    position: absolute;
    right: 10%;
    top: 20%;
}

.panel-order .row {
    border-bottom: 1px solid #ccc;
    border-top: 1px solid #ccc;
}

.panel-order .row:last-child {
    border: 0px;
}



.panel-order .row .col-md-11 {
    border-left: 1px solid #ccc;
}

.panel-order .create-by {
    font-size: 14px;
    color: #555;
    background: #efefef;

}

.card {
    box-shadow: 0 2px 6px 0 rgb(218 218 253 / 65%), 0 2px 6px 0 rgb(206 206 238 / 54%);
}
</style>