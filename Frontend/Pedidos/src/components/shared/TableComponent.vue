<template>
  <div>
    <div class="filters">
      <label for="cliente">Cliente:</label>
      <select id="cliente" v-model="selectedCliente" @change="applyFilters">
        <option value="">Todos</option>
        <option v-for="cliente in clientes" :key="cliente.id" :value="cliente.id">
          {{ cliente.nome }}
        </option>
      </select>

      <label for="dataInicio">Data Início:</label>
      <input type="date" id="dataInicio" v-model="dataInicio" @change="applyFilters" />

      <label for="dataFim">Data Fim:</label>
      <input type="date" id="dataFim" v-model="dataFim" @change="applyFilters" />

      <label for="codigo">Código Pedido:</label>
      <input type="text" id="codigo" v-model="codigoPedido" @input="applyFilters" placeholder="Pesquisar pelo código" />
    </div>

    <table>
      <thead>
        <tr>
          <th>#</th>
          <th>Nome do Cliente</th>
          <th>Produtos</th>
          <th>Data do Pedido</th>
          <th>Valor Total</th>
          <th>Detalhes</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(pedido) in filteredPedidos" :key="pedido.id">
          <td>{{ pedido.id }}</td>
          <td>{{ getClienteNome(pedido.idCliente) }}</td>
          <td>
            <ul>
              <li v-for="detalhe in pedido.detalhes" :key="detalhe.id">
                {{ getProdutoNome(detalhe.idProduto) }} (x{{ detalhe.quantidade }})
              </li>
            </ul>
          </td>
          <td>{{ formatDate(pedido.dataPedido) }}</td>
          <td>R$ {{ pedido.valorTotal.toFixed(2) }}</td>
          <td>
            <button @click="showDetails(pedido)">Detalhes</button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Modal -->
    <div v-if="selectedPedido" class="modal-overlay">
      <div class="modal">
        <h3>Detalhes do Pedido</h3>
        <table>
          <thead>
            <tr>
              <th>Produto</th>
              <th>Quantidade</th>
              <th>Valor Unitário</th>
              <th>Valor Total</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="detalhe in selectedPedido.detalhes" :key="detalhe.id">
              <td>{{ getProdutoNome(detalhe.idProduto) }}</td>
              <td>{{ detalhe.quantidade }}</td>
              <td>R$ {{ detalhe.valorUnitario.toFixed(2) }}</td>
              <td>R$ {{ detalhe.valorTotal.toFixed(2) }}</td>
            </tr>
          </tbody>
        </table>
        <button @click="closeModal">Fechar</button>
      </div>
    </div>
  </div>
</template>


<script>
export default {
  props: {
    pedidos: Array,
    clientes: Array,
    produtos: Array,
  },
  data() {
    return {
      selectedCliente: "",
      dataInicio: "",
      dataFim: "",
      codigoPedido: "",
      filteredPedidos: [],
      selectedPedido: null,
    };
  },
  watch: {
    pedidos: {
      immediate: true,
      handler() {
        this.filteredPedidos = [...this.pedidos];
      },
    },
  },
  methods: {
    formatDate(dateString) {
      const date = new Date(dateString);
      const datePart = date.toLocaleDateString('pt-BR');
      const timePart = date.toLocaleTimeString('pt-BR');
      return `${datePart} ${timePart}`;
    },
    getClienteNome(clienteId) {
      const cliente = this.clientes.find((c) => c.id === clienteId);
      return cliente ? cliente.nome : "Desconhecido";
    },
    getProdutoNome(produtoId) {
      const produto = this.produtos.find((p) => p.id === produtoId);
      return produto ? produto.nome : "Desconhecido";
    },
    applyFilters() {
      this.filteredPedidos = this.pedidos.filter((pedido) => {
        const matchesCliente = !this.selectedCliente || pedido.idCliente === this.selectedCliente;
        const matchesCodigo = !this.codigoPedido || pedido.id.toString().includes(this.codigoPedido);
        const matchesDataInicio =
          !this.dataInicio || new Date(pedido.dataPedido) >= new Date(this.dataInicio);
        const matchesDataFim = !this.dataFim || new Date(pedido.dataPedido) <= new Date(this.dataFim);
        return matchesCliente && matchesCodigo && matchesDataInicio && matchesDataFim;
      });
    },
    showDetails(pedido) {
      this.selectedPedido = pedido;
    },
    closeModal() {
      this.selectedPedido = null;
    },
    getFilteredPedidos() {
      return this.filteredPedidos;
    },
  },
};

</script>

<style scoped>

.filters {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 1rem;
  padding: 1rem;
  background-color: #f8f8f8;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.filters label {
  display: block;
  font-weight: bold;
  margin-bottom: 0.3rem;
}

.filters select,
.filters input {
  padding: 0.5rem;
  font-size: 1rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  outline: none;
  width: 200px;
}

.filters select:focus,
.filters input:focus {
  border-color: #007bff;
  box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

table, th, td {
  border: 1px solid #ddd;
}

th, td {
  padding: 12px;
  text-align: left;
  font-size: 0.9rem;
}

th {
  background-color: #007bff;
  color: white;
  font-weight: bold;
}

tr:nth-child(even) {
  background-color: #f9f9f9;
}

tr:hover {
  background-color: #f1f1f1;
}

ul {
  padding-left: 20px;
}

ul li {
  font-size: 0.9rem;
}

td:last-child {
  font-weight: bold;
  color: #28a745;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  width: 500px;
  max-width: 90%;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.modal h3 {
  margin-bottom: 1rem;
}

.modal table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
}

.modal table, th, td {
  border: 1px solid #ddd;
}

.modal th, td {
  padding: 10px;
  text-align: left;
}

.modal button {
  margin-top: 1rem;
  padding: 0.5rem 1rem;
  border: none;
  background: #007bff;
  color: white;
  cursor: pointer;
  border-radius: 4px;
}

.modal button:hover {
  background: #0056b3;
}

/* Ajustes para a responsividade */
@media (max-width: 768px) {
  .filters {
    flex-direction: column;
  }
  .filters select,
  .filters input {
    width: 100%;
    margin-bottom: 0.5rem;
  }
}
</style>
