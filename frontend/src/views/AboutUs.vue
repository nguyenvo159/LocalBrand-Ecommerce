<template>
    <main>
        <div class="banner position-relative font-vie">
            <img class="w-100" src="../assets/images/about.png" alt="">
            <div class=" pb-5 w-100 d-flex justify-content-center position-absolute text-white" style="top: 50px">
                <h1><b>VỀ CHÚNG TÔI</b></h1>
            </div>
            <div class="slogan-content p-5">
                <p class="p-5 position-relative">
                    <span class="slogan slogan-left">"</span>
                    Your proposed, our Pleasure <br>
                    Amiri!
                    <span class="slogan slogan-right">"</span>
                </p>


            </div>
        </div>

        <div class="container mt-5">
            <div class="row justify-content-center align-items-center">
                <div class="p-0 col-md-6 col-12">
                    <h1>Amiri - Fashion in Futher</h1>
                    <p class="text-justify"><br> Amiri - Một Thương hiệu Thời trang với phong cách giản dị. Với sứ mệnh
                        cung
                        cấp cho khách hàng
                        những sản phẩm chất lượng cao với giá cả vô cùng hợp lý, Amiri luôn tìm kiếm những nhân
                        viên tài năng và đam mê thời trang để đưa thương hiệu của mình đến với khách hàng trên toàn
                        quốc. <br><br> Với sự tận tâm và sự chuyên nghiệp của đội ngũ nhân viên, Amiri tự tin
                        sẽ
                        tiếp tục phát triển và đem lại những sản phẩm thời trang đẳng cấp cho khách hàng của mình.</p>

                    <div class=" m-1 row justify-content-start">
                        <div class="col-6">
                            <h1>&gt;1,000</h1>
                            <p>Sản phẩm được bán mỗi ngày</p>
                        </div>
                        <div class="col-6">
                            <h1>12</h1>
                            <p>Nhân viên chăm sóc tận tình</p>
                        </div>
                        <div class="col-6">
                            <h1>5</h1>
                            <p>Cửa hàng trên toàn quốc</p>
                        </div>
                    </div>
                </div>

                <div class="p-0 col-md-6 col-12 row justify-content-center">
                    <div class="p-0 col-md-4 row align-items-center justify-content-end">
                        <img class="" src="../assets/images/about1.png" alt="" style="width: 180px; height: 180px;">
                    </div>

                    <div class="p-0 pl-3 col-md-4 text-start row align-items-center justify-content-center">
                        <img class="" src="../assets/images/about2.png" alt="" style="width: 180px; height: 180px;">
                        <img class="mt-3" src="../assets/images/about3.png" alt="" style="width: 180px; height: 180px;">
                    </div>

                    <div class="p-0 col-md-4 row align-items-center justify-content-start">
                        <img class="" src="../assets/images/about1.png" alt="" style="width: 180px; height: 180px;">
                    </div>
                </div>
            </div>
        </div>
    </main>
    <div>

    </div>
</template>
<script>
export default {
    mounted() {
        window.scrollTo(0, 0);
    }
}
</script>

<style>
/* About page */

.slogan-content {
    width: 40%;
    position: absolute;
    right: 20px;
    bottom: 50px;
}

.slogan-content p {
    font-size: 48px;
    font-style: italic;
    font-weight: 700;
    line-height: 113.5%;
    letter-spacing: 0.02em;
    margin-bottom: 16px;
    color: #fff;
}

.slogan {
    font-style: italic;
    font-weight: 700;
    font-size: 96px;
    position: absolute;
    color: #fff;
}

.slogan-left {
    top: 0;
    left: 0;
}

.slogan-right {
    bottom: 0;
    right: 100px;
}
</style>