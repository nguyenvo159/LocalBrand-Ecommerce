<template>
    <div id="search-input" class="w-100 input-group d-flex align-items-center">
        <span class="pr-2">Search: </span>
        <input type="text" class="form-control rounded-0" placeholder="Nhập thông tin cần tìm..." :value="modelValue"
            @input="updateModelValue" @keyup.enter="submit" style="box-shadow: none;" />
        <!-- <div class="input-group-append">
            <button class="btn btn-outline-secondary" type="button" @click="submit">
                <i class="fas fa-search"></i> Tìm
            </button>
        </div> -->
    </div>
</template>

<script>
export default {
    props: {
        modelValue: { type: String, default: "" },
    },
    emits: ["submit", "update:modelValue"],
    methods: {
        updateModelValue(e) {
            this.$emit("update:modelValue", e.target.value);
        },
        submit() {
            this.$emit("submit");
        },
    },
};
</script>