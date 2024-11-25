<template>
  <div class="form-input">
    <label :for="name" class="label">{{ label }}</label>
    <input
      v-model="inputValue"
      :type="type"
      :name="name"
      @input="updateValue"
      class="input"
    />
  </div>
</template>

<script>
export default {
  props: {
    modelValue: {
      type: [String, Number],
      default: '', 
    },
    label: [String, Number],
    name: String,
    type: {
      type: [String, Number],
      default: 'text',
    },
  },
  data() {
    return {
      inputValue: this.modelValue, 
    };
  },
  watch: {
    modelValue(newValue) {
      this.inputValue = newValue;
    },
  },
  methods: {
    updateValue() {
      this.$emit('update:modelValue', this.inputValue); 
    },
  },
};
</script>

<style scoped>
.form-input {
  display: flex;
  flex-direction: column;
  margin-bottom: 20px;
  font-family: 'Arial', sans-serif;
}

.label {
  font-size: 1rem;
  color: #333;
  margin-bottom: 8px;
  font-weight: 500;
}

.input {
  padding: 10px;
  font-size: 1rem;
  border-radius: 5px;
  border: 1px solid #ccc;
  outline: none;
  transition: all 0.3s ease;
  background-color: #fff;
}

.input:focus {
  border-color: #007bff;
  box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
}

.input:not(:placeholder-shown) {
  background-color: #f1f1f1;
}

@media (max-width: 600px) {
  .form-input {
    margin-bottom: 15px;
  }

  .input {
    font-size: 0.9rem;
    padding: 8px;
  }
}
</style>
