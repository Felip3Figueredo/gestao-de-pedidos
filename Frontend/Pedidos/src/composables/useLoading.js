import { ref } from 'vue';

const isLoading = ref(false);

export const useLoading = () => {
  const startLoading = () => {
    isLoading.value = true;
  };

  const stopLoading = () => {
    isLoading.value = false;
  };

  return {
    isLoading,
    startLoading,
    stopLoading,
  };
};
