<template>
  <div class="sidebar">
    <ul>
      <li>
        <router-link to="/pessoas" active-class="active">
          <i class="fas fa-user"></i> Cadastro de Pessoas
        </router-link>
      </li>
      <li>
        <router-link to="/produtos" active-class="active">
          <i class="fas fa-box"></i> Cadastro de Produtos
        </router-link>
      </li>
      <li>
        <router-link to="/pedidos" active-class="active">
          <i class="fas fa-shopping-cart"></i> Criação de Pedidos
        </router-link>
      </li>
      <li>
        <router-link to="/listagem-de-pedidos" active-class="active">
          <i class="fas fa-users"></i> Listagem de Pedidos Realizados
        </router-link>
      </li>
    </ul>
  </div>
</template>

<script>
  export default {
    name: 'SidebarComponent',
  };
</script>

<style scoped>
/* Estilo geral do sidebar */
.sidebar {
  width: 225px;
  position: fixed;
  top: 0;
  left: 0;
  height: 100%;
  background: linear-gradient(135deg, #1a73e8, #5cdb95);
  box-shadow: 2px 0 10px rgba(0, 0, 0, 0.2);
  padding: 20px 15px;
  color: #fff;
  font-family: 'Roboto', sans-serif;
  overflow-y: auto;
}

ul {
  list-style-type: none;
  padding: 0;
  margin: 0;
}

a {
  text-decoration: none;
}

li {
  margin: 20px 0;
  font-size: 1.1rem;
  display: flex;
  align-items: center;
}

li i {
  margin-right: 10px;
  font-size: 1.3rem;
}

/* Links padrão */
router-link {
  color: #fff;
  text-decoration: none;
  display: flex;
  align-items: center;
  padding: 10px 15px;
  border-radius: 8px;
  transition: all 0.3s ease;
}

router-link:hover {
  background: rgba(255, 255, 255, 0.2);
  text-decoration: none;
  transform: scale(1.05);
}

.active {
  background: #f5a623;
  font-weight: bold;
  border: 10px solid #f5a623; 
  border-radius: 10px; 
  color: #333;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);
  transition: all 0.2s;
}

/* Ajustes responsivos */
@media (max-width: 768px) {
  .sidebar {
    width: 200px;
  }

  li {
    font-size: 1rem;
  }
}
</style>
