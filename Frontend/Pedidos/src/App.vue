<template>
  <div id="app">
    <!-- Sidebar com navegação -->
    <Sidebar />

    <!-- Conteúdo principal -->
    <div class="content">
      <LoadingSpinner />
      <!-- Aqui o Vue Router renderiza os componentes das rotas -->
      <router-view />
    </div>
  </div>
</template>

<script>
import LoadingSpinner from './components/shared/LoadingSpinner.vue';
import Sidebar from './components/shared/SidebarComponent.vue';
import { useLoading } from './composables/useLoading';
import { provide } from 'vue';

export default {
  name: 'App',
  components: {
    Sidebar,
    LoadingSpinner
  },
  setup() {
    const { isLoading, startLoading, stopLoading } = useLoading();
    
    provide('isLoading', isLoading);
    provide('startLoading', startLoading);
    provide('stopLoading', stopLoading);

    return {};
  }
};
</script>

<style>
.content {
  display: flex;
  margin-left: 250px;
  padding: 20px;
  background-color: #f5f5f5;
  min-height: 100vh;
  position: relative; 
}
</style>
