<template>
    <div class="container-fluid row justify-content-center">
        <div class="col-lg-10 col-11 mb-5">
            <h1 class="text-center mt-3">Contact Us</h1>
            <p class="text-center text-muted">Liên hệ với chúng tôi nếu bạn có bất kì thắc mắc hay trải nghiệm không tốt
                nào về chúng tôi</p>
            <div class="row mt-5 ">
                <div class="col-lg-6 col-12 d-none d-lg-flex justify-content-center">
                    <img class="img-fluid w-75" src="../assets/images/contact.png" alt="">
                </div>
                <div class="col-lg-6 col-12 pt-4 mb-5">
                    <form @submit.prevent="submitForm">
                        <div class="form-group">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text bg-white">
                                        <i class="fa fa-user"></i>&nbsp
                                    </span>
                                </div>
                                <input v-model="name" type="text" placeholder="Name" class="form-control border-left-0"
                                    required>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text bg-white">
                                        <i class="fa fa-envelope"></i>
                                    </span>
                                </div>
                                <input v-model="email" type="email" placeholder="Email"
                                    class="form-control border-left-0" required>
                            </div>
                        </div>

                        <div class="form-group">
                            <textarea v-model="message" class="form-control" id="message" rows="5"
                                placeholder="Enter message" required></textarea>
                        </div>

                        <button type="submit" class="btn btn-primary float-right">
                            <i class="fa fa-paper-plane"></i>
                            Send
                        </button>
                    </form>
                </div>
            </div>
        </div>
        <NotificationOption v-if="showToast" :visible="showToast" :type="toastType" @close="showToast = false"
            :message="toastMessage" />
    </div>
</template>

<script>
import NotificationOption from '@/components/NotificationOption.vue';
import ContactService from '@/services/contact.service';
export default {
    components: {
        NotificationOption
    },
    data() {
        return {
            name: '',
            email: '',
            message: '',
            showToast: false,
            toastType: 'info',
            toastMessage: ''
        };
    },
    methods: {
        async submitForm() {
            const formUrl = "https://docs.google.com/forms/d/e/1FAIpQLSf4NYJiMutzMOcYPlhniO8CCo4AkvfMa-t37oHp1-UTktte6g/formResponse";
            const formData = new FormData();

            // Append the input fields
            formData.append("entry.442035508", this.name);
            formData.append("entry.1390559808", this.email);
            formData.append("entry.1978270797", this.message);
            let loader = this.$loading.show({
                container: null,
                width: 100,
                height: 100,
                color: '#808EF4',
                loader: 'bars',
                canCancel: true,
            });
            try {
                await ContactService.create({ name: this.name, email: this.email, message: this.message });
                const response = await fetch(formUrl, {
                    method: "POST",
                    body: formData,
                    mode: "no-cors"
                });
                this.name = '';
                this.email = '';
                this.message = '';
                loader.hide();
                this.showToast = true;
                this.toastType = 'success';
                this.toastMessage = 'Gởi biểu mẫu thành công';
            } catch (error) {
                loader.hide();
                alert("Gởi biểu mẫu không thành công" + error);
                console.error("Error:", error);
            }
        }
    },
    mounted() {
        window.scrollTo(0, 0);
    }
};
</script>
