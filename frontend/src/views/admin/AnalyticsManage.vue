<template>
    <div class="container-fluid">
        <div class="row">
            <DashBoard type="Analytics" />
            <div id="dv" class="col-lg-10 offset-lg-2 col-11 offset-1 admin-content">
                <h3 class="mb-4 text-uppercase">Thống kê</h3>

                <div class="row row-cols-1 row-cols-md-2 row-cols-xl-4">
                    <div class="col  mb-3">
                        <div class="card radius-10 border-start border-0 border-3 border-info">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div>
                                        <p class="mb-0 text-secondary">Tổng đơn hàng</p>
                                        <h4 class="my-1 text-info">{{ result != null ? result.totalOrders : 0 }}</h4>
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
                                        <h4 class="my-1 text-danger">
                                            {{ formatPrice(result != null ? result.totalRevenue : 0) }}
                                        </h4>
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
                                        <p class="mb-0 text-secondary">
                                            Tổng sản phẩm bán ra
                                        </p>
                                        <h4 class="my-1 text-success">
                                            {{ result != null ? result.totalProducts : 0 }}
                                        </h4>
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
                                        <p class="mb-0 text-secondary">Số khách hàng</p>
                                        <h4 class="my-1 text-warning">
                                            {{ result != null ? result.totalCustomers : 0 }}
                                        </h4>
                                    </div>
                                    <div class="widgets-icons-2 rounded-circle bg-gradient-blooker text-white ms-auto">
                                        <i class="fa fa-users"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="mt-4 d-flex align-items-center gap-2">
                    <h5 class="mb-0"><i class="fa-solid fa-filter mr-2"></i></h5>
                    <div class="form-group mb-0">
                        <select class="form-control auto-width" id="selectMonth" v-model="month" @change="fectchData">
                            <option v-for="m in months" :value="m">Tháng {{ m }}</option>
                        </select>
                    </div>
                    <div class="form-group mb-0">
                        <select class="form-control auto-width" id="selectYear" v-model="year" @change="fectchData">
                            <option v-for="y in years" :value="y">{{ y }}</option>
                        </select>
                    </div>
                    <div>
                        <button class="btn btn-primary" @click="fectchData">Thống kê</button>
                    </div>
                </div>


                <div class="row justify-content-center">
                    <div class="col">
                        <h2 class="mt-4">Doanh Thu Tháng {{ month }}/{{ year }}: <span class="price">
                                {{ formatPrice(result != null ? result.totalRevenue : 0) }}</span></h2>
                        <div class="col-lg-12 row justify-content-center" style=" max-height: 530px;">
                            <canvas class="bg-white shadow p-4" id="areaChart"></canvas>
                            <p class="mt-2 w-100 text-muted text-center font-italic"> Biểu đồ doanh thu tháng
                                {{ month }}</p>
                        </div>
                    </div>
                    <div class="row justify-content-center mt-3">
                        <h2 class="col-12 my-4 w-100">Số Liệu Sản Phẩm</h2>
                        <div class="col-lg-8" style=" height: 400px;">
                            <canvas class="bg-white shadow p-4" id="barChart" style="width: auto;"></canvas>
                            <p class="mt-2 w-100 text-muted text-center font-italic"> Biểu đồ số sản phẩm đã bán</p>
                        </div>
                        <div class="col-lg-4">
                            <canvas class="bg-white shadow p-4" id="pieChart"></canvas>
                            <p class="mt-2 w-100 text-muted text-center font-italic"> Biểu đồ tỉ lệ sản phẩm đã bán</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import DashBoard from '@/components/admin/DashBoard.vue';
import OrderService from '@/services/order.service';
// import { Chart } from 'chart.js';
import {
    Chart,
    LineController,
    LineElement,
    PointElement,
    LinearScale,
    CategoryScale,
    Title,
    Tooltip,
    Legend,
    Filler,
    BarElement,
    BarController,
    PieController,
    ArcElement
} from 'chart.js';

// Đăng ký các thành phần cần thiết
Chart.register(
    LineController,
    LineElement,
    PointElement,
    LinearScale,
    CategoryScale,
    BarElement,
    BarController,
    PieController,
    ArcElement,
    Title,
    Tooltip,
    Legend,
    Filler
);

export default {
    components: {
        DashBoard
    },
    data() {
        return {
            result: null,
            month: 11,
            year: 2024,
            areaChartData: {},
            areaChart: null,
            barChart: null,
            pieChart: null,
            months: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
            years: [2022, 2023, 2024],
        };
    },
    methods: {
        async fectchData() {
            this.result = await OrderService.getAnalytics(this.month, this.year);

            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'bars',
                canCancel: true,
            });
            setTimeout(async () => {
                this.updateChart();
            }, 500);
            setTimeout(() => {
                loader.hide();
            }, 500);

        },
        formatPrice(price) {
            return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
        },
        formatDate(date) {
            return formatDate(new Date(date), 'hh:mm dd/MM/yyyy');
        },
        updateChart() {
            if (this.areaChart) this.areaChart.destroy();

            const labels = this.result.revenues.map(r => `Ngày ${r.day}`);
            const data = this.result.revenues.map(r => r.revenue);

            this.areaChartData = {
                labels: labels,
                datasets: [
                    {
                        label: 'Doanh thu',
                        data: data,
                        fill: true,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        tension: 0.4,
                    }
                ]
            };

            const ctx = document.getElementById('areaChart').getContext('2d');
            this.areaChart = new Chart(ctx, {
                type: 'line',
                data: this.areaChartData,
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Doanh thu (VND)',
                            },
                        },
                        x: {
                            title: {
                                display: true,
                                text: `Tháng ${this.month}/${this.year}`,
                            },
                        },
                    },
                },
            });
            this.createBarChart();
            this.createPieChart();
        },
        createBarChart() {
            if (this.barChart) this.barChart.destroy();

            const labels = this.result.categories.map(c => c.name);
            const data = this.result.categories.map(c => c.count);

            const ctx = document.getElementById('barChart').getContext('2d');
            this.barChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Số lượng sản phẩm',
                        data: data,
                        backgroundColor: 'rgba(54, 162, 235, 0.5)',
                        borderColor: 'rgba(54, 162, 235, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Số lượng'
                            }
                        },
                        x: {
                            title: {
                                display: true,
                                text: 'Loại sản phẩm'
                            }
                        }
                    }
                }
            });
        },
        createPieChart() {
            if (this.pieChart) this.pieChart.destroy();

            const labels = this.result.categories.map(c => c.name);
            const data = this.result.categories.map(c => c.count);
            const total = data.reduce((sum, value) => sum + value, 0);
            const percentages = data.map(value => ((value / total) * 100).toFixed(2));

            const ctx = document.getElementById('pieChart').getContext('2d');
            this.pieChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        data: percentages,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.5)',
                            'rgba(54, 162, 235, 0.5)',
                            'rgba(255, 206, 86, 0.5)',
                            'rgba(75, 192, 192, 0.5)',
                            'rgba(153, 102, 255, 0.5)'
                        ],
                        borderColor: [
                            'rgba(255, 99, 132, 1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top'
                        }
                    }
                }
            });
        }

    },
    mounted() {
        this.fectchData();
    },
};
</script>

<style scoped>
.card {
    box-shadow: 0 2px 6px 0 rgb(218 218 253 / 65%), 0 2px 6px 0 rgb(206 206 238 / 54%);
}
</style>