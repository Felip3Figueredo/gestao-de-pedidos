<template>
    <div>
      <h1>Pedidos</h1>
      <button @click="exportarExcel" class="export-button">Exportar para Excel</button>
      <TableComponent ref="tableComponent" :pedidos="pedidos" :clientes="clientes" :produtos="produtos" />
    </div>
  </template>
  
  <script>
  import api from "@/services/api";
  import TableComponent from "@/components/shared/TableComponent.vue";
  
  export default {
    components: { TableComponent },
    data() {
      return {
        pedidos: [],
        clientes: [],
        produtos: [],
      };
    },
    async created() {
      await this.fetchData();
    },
    methods: {
      async fetchData() {
        try {
          const [clientesRes, produtosRes, pedidosRes] = await Promise.all([
            api.get("ListarClientesCadastrados"),
            api.get("ListarProdutosCadastrados"),
            api.get("Pedido/ListarPedidosCadastrados"),
          ]);
          this.clientes = clientesRes.data;
          this.produtos = produtosRes.data;
          this.pedidos = pedidosRes.data;
        } catch (error) {
          console.error("Erro ao carregar dados:", error);
        }
      },
      async exportarExcel() {
        try {
          const tableComponent = this.$refs.tableComponent;
          const filteredData = tableComponent.getFilteredPedidos();
  
          const response = await api.post("Pedido/ExportarPedidosExcel", filteredData, {
            responseType: "blob", 
          });
  
        console.log(filteredData);
          const blob = new Blob([response.data], { type: "application/vnd.ms-excel" });
          const link = document.createElement("a");
          link.href = URL.createObjectURL(blob);
          link.download = "PedidosFiltrados.xlsx";
          link.click();
          URL.revokeObjectURL(link.href);
        } catch (error) {
          console.error("Erro ao exportar Excel:", error);
        }
      },
    },
  };
  </script>
  
  <style scoped>
  h1 {
    text-align: center;
    margin-bottom: 2rem;
  }
  
  .export-button {
    margin-bottom: 1rem;
    padding: 10px 20px;
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 16px;
  }
  
  .export-button:hover {
    background-color: #0056b3;
  }
  </style>
  