<template>
    <div class="container-fluid">
        <div class="row">
            <DashBoard type="Custumer" />
            <div id="dv" class="col-lg-10 offset-lg-2 col-11 offset-1 admin-content">
                <h3 class="mb-4 text-uppercase">Quản lý khách hàng</h3>
                <div class="row row-cols-1 row-cols-md-2 row-cols-xl-4">
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-info">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng đơn hàng</p>
                                        <h4 class="my-1 text-info">4805</h4>
                                        <p class="mb-0 font-13">+2.5% so với 7 ngày trước</p>
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
                                        <h4 class="my-1 text-danger">{{ formatPrice(10000) }}</h4>
                                        <p class="mb-0 font-13">+5.4% so với 7 ngày trước</p>
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
                                        <h4 class="my-1 text-success">{{ formatPrice(21000) }}</h4>
                                        <p class="mb-0 font-13">-4.5% so với tháng trước</p>
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
                                        <h4 class="my-1 text-warning">12</h4>
                                        <p class="mb-0 font-13">+8.4% so với tháng trước</p>
                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-blooker text-white ms-auto">
                                        <i class="fa fa-users"></i>
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
import DashBoard from '@/components/admin/DashBoard.vue';

export default {
    components: {
        DashBoard
    },
    methods: {
        formatPrice(value) {
            return value.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
        }
    }
}
</script>

<style scoped>
.card {
    box-shadow: 0 2px 6px 0 rgb(218 218 253 / 65%), 0 2px 6px 0 rgb(206 206 238 / 54%);
}
</style>