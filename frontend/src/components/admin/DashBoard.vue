<template>

    <!-- Admin Sidebar -->
    <div class="col-lg-3 admin-sidebar">

        <h3 style="height: 52px;"><span>Quản Trị</span></h3>

        <!-- <router-link :to="{name: 'Analytics'}" :class="{'active': type == 'Analytics'}"><i class="fas fa-chart-bar"></i><span>Thống kê</span></router-link> -->
        <router-link :to="{name: 'UserManage'}" :class="{'active': type == 'User'}"><i class="fas fa-user"></i><span>Người dùng</span></router-link>
        <router-link :to="{name: 'ProductManage'}" :class="{'active': type == 'Product'}"><i class="fas fa-box"></i><span>Sản phẩm</span></router-link>
        <!-- <router-link :to="{name: 'OrderManage'}"  :class="{'active': type == 'Order'}"><i class="fas fa-shopping-cart"></i><span>Đơn hàng</span></router-link> -->
        <router-link :to="{name: 'Home'}" class=""><i class="fas fa-sign-out-alt"></i><span>Thoát</span></router-link>
    </div>

    <!-- <ProductManager /> -->

</template>
<script>
export default {
props: {
type: {type: String, required: true },
}
}
</script>

<style>
body {
font-family: 'Arial', sans-serif;
margin: 0;
padding: 0;
}

.admin-sidebar {
position: fixed;
width: 250px;
height: 100%;
background-color: #343a40;
padding-top: 20px;
color: #ffffff;
transition: width 0.3s;
}

.admin-content {
margin-left: 275px;
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

@media (max-width: 1024px) {
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