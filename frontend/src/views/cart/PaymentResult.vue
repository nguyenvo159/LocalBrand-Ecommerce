<template>
    <div v-if="status == 'success'" class="container my-5">
        <div class="alert alert alert-success text-center" role="alert">
            <h4 class="alert-heading"><i class="fa-regular fa-circle-check"></i> Thanh toán thành công!</h4>
            <p>Đơn hàng của bạn đã được thanh toán thành công.</p>
            <hr />
            <p class="mb-0">Tự động chuyển chuyển trang đơn hàng chi tiết sau <span class="text-danger"> {{ countdown }}
                    giây</span></p>
        </div>
        <div class="text-center mt-4">
            <button class="btn btn-primary" @click="redirectRoute">Xem chi tiết đơn hàng</button>
        </div>
    </div>
    <div v-else class="container my-5">
        <div class="alert alert alert-warning text-center" role="alert">
            <h4 class="alert-heading"><i class="fa-solid fa-triangle-exclamation"></i> Thanh toán thất bại!</h4>
            <p>Rất tiếc, quá trình thanh toán không thành công. Vui lòng kiểm tra thông tin của bạn và thử lại.</p>
            <hr />
            <p class="mb-0">Nếu vấn đề vẫn tiếp diễn, hãy liên hệ với bộ phận hỗ trợ của chúng tôi.</p>
        </div>
        <div class="text-center mt-4">
            <button class="btn btn-primary" @click="retryPayment">Thử lại thanh toán</button>
        </div>
    </div>

</template>

<script>
import orderService from '@/services/order.service';
export default {
    data() {
        return {
            status: "failed", // Mặc định là "failed" cho đến khi kiểm tra errorCode
            countdown: 15,
            errorCode: null, // Lưu giá trị errorCode từ URL
            orderId: null,
        };
    },
    created() {
        this.checkPaymentStatus();
        if (this.status === "success") {
            this.startCountdown();
        }
    },
    methods: {
        redirectRoute() {
            this.$router.push({ name: 'OrderDetail', params: { id: this.orderId } });
        },
        retryPayment() {
            this.$router.push({ name: 'OrderDetail', params: { id: this.orderId } });
        },
        startCountdown() {
            const interval = setInterval(() => {
                this.countdown -= 1;
                if (this.countdown <= 0) {
                    clearInterval(interval);
                    this.redirectRoute();
                }
            }, 1000);
        },
        async checkPaymentStatus() {
            const urlParams = new URLSearchParams(window.location.search);
            this.errorCode = urlParams.get('errorCode');
            this.orderId = urlParams.get('orderId');

            if (this.errorCode == '0') {
                this.status = "success";
                var data = {
                    id: this.orderId,
                    status: 1,
                    payType: 1
                }
                await orderService.update(data);
            } else {
                this.status = "failed";
            }
        },
    }

}
</script>

<style scoped>
.alert {
    max-width: 600px;
    margin: 0 auto;
}
</style>