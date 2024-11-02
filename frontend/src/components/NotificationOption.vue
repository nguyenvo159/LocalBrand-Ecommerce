<template>
    <div v-if="visible" class="shadow  alert-custom"
        :class="['alert-custom-' + type, { 'alert-custom-show': visible }]">
        <div class="alert-body d-flex justify-content-between">
            <span class="fs-4">{{ message }}</span>
            <button type="button" class="btn-close d-flex align-items-center" @click="hideToast"><i
                    class="fa-solid fa-xmark"></i></button>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        type: {
            type: String,
            default: 'info' // 'success', 'error', 'info'
        },
        visible: {
            type: Boolean,
            default: false
        },
        message: {
            type: String,
            default: ''
        }
    },
    watch: {
        visible(newValue) {
            if (newValue) {
                this.startAutoClose();
            }
        }
    },
    methods: {
        hideToast() {
            this.$emit('close');
        },
        startAutoClose() {
            setTimeout(() => {
                this.hideToast();
            }, 1000);
        }
    },
    mounted() {
        if (this.visible) {
            this.startAutoClose();
        }
    }
};
</script>


<style>
/* Alert */
:root {
    --alert-info-border-color: #007bff;
    --alert-info-bg-color: #cce5ff;
    --alert-info-text-color: #004085;

    --alert-success-border-color: #28a745;
    --alert-success-bg-color: #d4edda;
    --alert-success-text-color: #155724;

    --alert-error-border-color: #dc3545;
    --alert-error-bg-color: #f8d7da;
    --alert-error-text-color: #721c24;
}

.alert-custom {
    position: fixed;
    top: 20px;
    right: 20px;
    background-color: #fff;
    border-left: 5px solid;
    border-radius: 5px;
    padding: 30px 20px;
    min-width: 300px;
    max-width: 400px;
    align-items: center;
    z-index: 1050;
    opacity: 0;
    transform: translateY(-20px);
    transition: opacity 0.3s ease, transform 0.3s ease;
    animation: slideInLeft 0.3s ease forwards;
}


.alert-custom-show {
    opacity: 1;
    transform: translateY(0);
}


.alert-custom-info {
    border-left-color: var(--alert-info-border-color);
    background-color: var(--alert-info-bg-color);
    color: var(--alert-info-text-color);
}

.alert-custom-success {
    border-left-color: var(--alert-success-border-color);
    background-color: var(--alert-success-bg-color);
    color: var(--alert-success-text-color);
}

.alert-custom-error {
    border-left-color: var(--alert-error-border-color);
    background-color: var(--alert-error-bg-color);
    color: var(--alert-error-text-color);
}

.btn-close {
    background: transparent;
    border: none;
    font-size: 24px;
    cursor: pointer;
    color: rgba(0, 0, 0, 0.6);
    margin-left: 15px;
}

.btn-close:hover {
    color: rgba(0, 0, 0, 0.8);
}

@keyframes slideInLeft {
    from {
        transform: translateX(100%);
        opacity: 0;
    }

    to {
        transform: translateX(0);
        opacity: 1;
    }
}

@keyframes fadeOut {
    to {
        opacity: 0;
        transform: translateY(-20px);
    }
}
</style>
