<template>
  <div class="select-filter">
    <label :for="label" class="label">{{ label }}</label>
    <select v-model="selectedValue" @change="applyFilter" class="select">
      <option value="">Todos</option>
      <option v-for="option in options" :key="option" :value="option">
        {{ option }}
      </option>
    </select>
  </div>
</template>

<script>
export default {
  props: {
    label: {
      type: String,
      required: true,
    },
    data: {
      type: Array,
      required: true,
    },
    field: {
      type: String,
      required: true,
    },
    onFilterChange: {
      type: Function,
      required: true,
    },
  },
  data() {
    return {
      selectedValue: '',
      options: [],
    };
  },
  watch: {
    data: {
      immediate: true,
      handler() {
        this.generateOptions();
      },
    },
  },
  methods: {
    generateOptions() {
      const uniqueOptions = new Set(this.data.map(item => item[this.field]));
      this.options = [...uniqueOptions];
    },
    applyFilter() {
      this.onFilterChange(this.selectedValue);
    },
  },
};
</script>

<style scoped>
.select-filter {
  display: flex;
  flex-direction: column;
  margin-bottom: 1rem;
}

.label {
  font-size: 1rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
  color: #333;
}

.select {
  padding: 0.5rem;
  font-size: 1rem;
  border-radius: 4px;
  border: 1px solid #ccc;
  transition: all 0.3s ease;
  background-color: #fff;
}

.select:focus {
  border-color: #007bff;
  outline: none;
}

select option {
  font-size: 1rem;
  padding: 0.5rem;
}
</style>
