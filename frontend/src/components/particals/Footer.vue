<template>
    <footer v-if="!isAdmin" class="bg-dark py-5">
        <div class="container">
            <div class="row text-white g-4">
                <div class="col-md-6 col-lg-3">
                    <a class="text-uppercase text-decoration-none brand text-white" href="index.html"
                        style="letter-spacing: 10px;">AMIE</a>
                    <p class="text-white mt-3"></p>
                </div>

                <div class="col-md-6 col-lg-3">
                    <h5 class="fw-light">Links</h5>
                    <ul class="list-unstyled">
                        <li class="my-3">
                            <a href="#" class="text-white text-decoration-none main-hover">
                                <i class="fas fa-chevron-right me-1"></i> Home
                            </a>
                        </li>
                        <li class="my-3">
                            <a href="#" class="text-white text-decoration-none main-hover">
                                <i class="fas fa-chevron-right me-1"></i> Collection
                            </a>
                        </li>
                        <li class="my-3">
                            <a href="#" class="text-white text-decoration-none main-hover">
                                <i class="fas fa-chevron-right me-1"></i> Contact Us
                            </a>
                        </li>
                        <li class="my-3">
                            <a href="#" class="text-white text-decoration-none main-hover">
                                <i class="fas fa-chevron-right me-1"></i> About Us
                            </a>
                        </li>
                    </ul>
                </div>

                <div class="col-md-6 col-lg-3">
                    <h5 class="fw-light mb-3">Contact Us</h5>
                    <div class="d-flex justify-content-start align-items-start my-2">
                        <span class="me-3">
                            <i class="fas fa-map-marked-alt"></i>
                        </span>
                        <span class="fw-light">
                            31, Ngô Sĩ Liên, Hưng Lợi, Ninh Kiều, Cần Thơ.
                        </span>
                    </div>
                    <div class="d-flex justify-content-start align-items-start my-2">
                        <span class="me-3">
                            <i class="fas fa-envelope"></i>
                        </span>
                        <span class="fw-light">
                            nguyenvo1373.hg@gmail.com
                        </span>
                    </div>
                    <div class="d-flex justify-content-start align-items-start my-2">
                        <span class="me-3">
                            <i class="fas fa-phone-alt"></i>
                        </span>
                        <span class="fw-light">
                            076 3962 680
                        </span>
                    </div>
                </div>

                <div class="col-md-6 col-lg-3">
                    <h5 class="fw-light mb-3">Follow Us</h5>
                    <div>
                        <ul class="list-unstyled d-flex">
                            <li>
                                <a href="https://www.facebook.com/nguyenph2212/" target="_blank"
                                    class="text-white text-decoration-none me-4">
                                    <i class="fab fa-facebook-f" style="font-size: 24px;"></i>
                                </a>
                            </li>
                            <li>
                                <a href="https://www.instagram.com/v_zn1202/" target="_blank"
                                    class="text-white text-decoration-none me-4">
                                    <i class="fab fa-instagram" style="font-size: 24px;"></i>
                                </a>
                            </li>
                            <li>
                                <a href="https://www.tiktok.com/@crk_znh" target="_blank"
                                    class="text-white text-decoration-none me-4">
                                    <i class="fab fa-tiktok" style="font-size: 24px;"></i>
                                </a>
                            </li>
                            <li>
                                <a href="#" target="_blank" class="text-white text-decoration-none me-4">
                                    <i class="fab fa-youtube" style="font-size: 24px;"></i>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </footer>
</template>

<script>
export default {
    data() {
        return {
            isAdmin: false,
        };
    },
    watch: {
        $route(to, from) {
            this.isAdmin = to.path.includes('admin');
        },
    },

};

</script>