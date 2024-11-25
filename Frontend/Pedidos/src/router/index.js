import { createRouter, createWebHistory } from 'vue-router';
import CadastroDePessoas from '@/views/CadastroDePessoas.vue';
import CadastroDeProdutos from '../views/CadastroDeProdutos.vue';
import CriacaoDePedidos from '../views/CriacaoDePedidos.vue';
import ListagemDePedidos from '@/views/ListagemDePedidos.vue';


const routes = [
  {
    path: '/pessoas',
    name: 'CadastroDePessoas',
    component: CadastroDePessoas
  },
  {
    path: '/produtos', 
    name: 'CadastroDeProdutos',
    component: CadastroDeProdutos
  },
  {
    path: '/pedidos', 
    name: 'CriacaoDePedidos',
    component: CriacaoDePedidos
  },
  {
    path: '/listagem-de-pedidos', 
    name: 'ListagemDePedidos',
    component: ListagemDePedidos
  }
];

const router = createRouter({
  history: createWebHistory('/'), 
  routes
});

export default router;
