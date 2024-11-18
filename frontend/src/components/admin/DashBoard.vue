<template>

    <!-- Admin Sidebar -->
    <div class="col-lg-3 bg-dark admin-sidebar">

        <h3 class="admin-title" style="height: 52px; letter-spacing: 10px;"><span>AMIE</span></h3>
        <hr>
        <div class="py-3">
            <h6 class="py-2 text-uppercase admin-title"><span>Manage</span></h6>
            <router-link :to="{ name: 'CustumerManage' }" :class="{ 'active': type == 'Custumer' }"><i
                    class="fas fa-chart-bar"></i><span>Custumer</span></router-link>
            <router-link :to="{ name: 'UserManage' }" :class="{ 'active': type == 'User' }"><i
                    class="fas fa-user"></i><span>User</span></router-link>
            <router-link :to="{ name: 'ProductManage' }" :class="{ 'active': type == 'Product' }"><i
                    class="fas fa-box"></i><span>Product</span></router-link>
            <router-link :to="{ name: 'OrderManage' }" :class="{ 'active': type == 'Order' }"><i
                    class="fas fa-shopping-cart"></i><span>Order</span></router-link>
            <a class="">
                <i class="fas fa-chart-bar"></i><span>Analytics</span></a>
        </div>
        <hr>
        <div>
            <router-link :to="{ name: 'Home' }" class=""><i
                    class="fas fa-sign-out-alt"></i><span>Exit</span></router-link>
        </div>
    </div>

    <!-- <ProductManager /> -->

</template>
<script>
export default {
    props: {
        type: { type: String, required: true },
    }
}
</script>

<style>
.admin-title {
    color: rgba(255, 255, 255, .55);
}

.admin-sidebar {
    position: fixed;
    width: 250px;
    height: 100%;
    padding-top: 20px;
    color: #ffffff;
    transition: width 0.3s;
}

.admin-content {
    padding: 20px;
    transition: margin-left 0.3s;
    /* Thêm hiệu ứng chuyển động cho Admin Content */
}

.admin-sidebar a {
    color: #ffffff;
    text-decoration: none;
    padding: 10px;
    display: block;
}

.admin-sidebar a:hover {
    background-color: #555;
    border-radius: 10px;
}

.admin-sidebar a.active {
    background-color: #007bff;
    border-radius: 10px;
}

.admin-sidebar i {
    margin-right: 8px;
    font-size: 16px;
}

/* Hiệu ứng width */

@media (max-width: 1440px) {
    .admin-sidebar {
        width: 65px;
    }

    .admin-sidebar span {
        display: none;
    }

    .admin-content {
        margin-left: 70px;
    }

}
</style>