<template>
  <div class="container">
    <h1>Cadastro de Pedidos</h1>
    <form @submit.prevent="handleSubmit" class="form">
      <div class="form-group">
        <label for="cliente">Cliente</label>
        <select v-model="pedido.idCliente" @change="loadEnderecos" required>
          <option v-for="cliente in clientes" :key="cliente.id" :value="cliente.id">
            {{ cliente.nome }}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label for="endereco">Endereço de Entrega</label>
        <select v-model="pedido.idEnderecoEntrega" required>
          <option v-for="endereco in enderecos.filter(e => e.tipoEndereco === 'ENTREGA' || e.tipoEndereco === 'AMBOS')" :key="endereco.id" :value="endereco.id">
            {{ endereco.logradouro }} - {{ endereco.cidade }}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label for="endereco">Endereço de Cobrança</label>
        <select v-model="pedido.idEnderecoCobranca" required>
          <option v-for="endereco in enderecos.filter(e => e.tipoEndereco === 'COBRANCA' || e.tipoEndereco === 'AMBOS')" :key="endereco.id" :value="endereco.id">
            {{ endereco.logradouro }} - {{ endereco.cidade }}
          </option>
        </select>
      </div>
      
      <div class="detalhe-section">
        <h2>Pedidos</h2>

        <div v-for="(detalhe, index) in pedido.detalhes" :key="index" class="detalhe-group">
          <h3>Detalhes do Pedido {{ index + 1 }}</h3>
          
          <div class="form-group">
            <label for="produto">Produto</label>
            <select v-model="detalhe.idProduto" required>
              <option v-for="produto in Produtos" :key="produto.id" :value="produto.id">
                {{ produto.nome }}
              </option>
            </select>
          </div>

          <div class="form-group">
            <FormInput 
              v-model="detalhe.quantidade" 
              label="Quantidade" 
              name="quantidade" 
              required 
              type="number" />
          </div>

          <button 
            type="button" 
            class="remove-button" 
            @click="removerDetalhamento(index)"
          >
            Remover Produto
          </button>
        </div>

        <button 
          type="button" 
          class="add-button" 
          @click="adicionarDetalhamento"
        >
          Adicionar Produto
        </button>
      </div>

      <button type="submit" class="submit-button">Cadastrar Pedido</button>
    </form>

    <!-- Modal -->
    <div v-if="modalVisible" class="modal">
      <div class="modal-content">
        <h2>{{ modalTitle }}</h2>
        <p>{{ modalMessage }}</p>
        <button @click="closeModal">Fechar</button>
      </div>
    </div>
  </div>
</template>

<script>
import FormInput from '../components/shared/FormInput.vue';
import api from '../services/api.js';

export default {
  components: {
    FormInput,
  },
  data() {
    return {
      pedido: {
        idCliente: '',
        idEnderecoEntrega: '',
        idEnderecoCobranca: '',
        detalhes: [
          { 
            idProduto: '', 
            quantidade: ''
          }
        ],
      },
      clientes: [],
      enderecos: [],
      Produtos: [],
      modalVisible: false,
      modalTitle: '',
      modalMessage: '',
    };
  },
  async created() {
    await this.loadClientes();
    await this.laodProdutos();
  },
  methods: {
    adicionarDetalhamento(){
      this.pedido.detalhes.push({ idProduto: '', quantidade: '' });
    },
    removerDetalhamento(index) {
      this.pedido.detalhes.splice(index, 1);
    },
    async loadClientes() {
      try {
        const response = await api.get('/ListarClientesCadastrados');
        this.clientes = response.data;
      } catch (error) {
        console.error('Erro ao carregar clientes:', error);
        this.showModal('Erro', 'Erro ao carregar clientes.');
      }
    },
    async laodProdutos() {
      try {
        const response = await api.get('/ListarProdutosCadastrados')
        this.Produtos = response.data
      } catch (error) {
        console.error('Erro ao carregar produtos:', error);
        this.showModal('Erro', 'Erro ao carregar produtos.');
      }
    },
    async loadEnderecos() {
      const cliente = this.clientes.find(cliente => cliente.id === this.pedido.idCliente);
      if (cliente) {
        this.enderecos = cliente.endereco;
        console.log(this.enderecos)
      }
    },
    async handleSubmit() {
      try {
        this.pedido.detalhes.forEach(detalhe => {
          detalhe.quantidade = Number(detalhe.quantidade);
        });

        console.log("Dados a enviar:", this.pedido);
        await api.post('/Pedido/AdicionarPedido', this.pedido);
        this.showModal('Sucesso', 'Pedido cadastrado com sucesso!');

        this.resetForm();
      } catch (error) {
        console.error('Erro ao cadastrar pedido:', error.response.data);
        if (error.response && error.response.data && Array.isArray(error.response.data)) {
          const errorMessages = error.response.data
            .map(err => err.errorMessage)
            .join('\n');
          this.showModal('Erro', `Erro ao cadastrar pedido:\n${errorMessages}`);
        } else {
          this.showModal('Erro', 'Erro ao cadastrar pedido: Não foi possível processar os erros retornados.');
        }
      }
    },
    resetForm() {
      this.pedido = {
        idCliente: '',
        idEnderecoEntrega: '',
        idEnderecoCobranca: '',
        detalhes: [
          { 
            idProduto: '', 
            quantidade: ''
          }
        ],
      };
      this.enderecos = [];
    },
    showModal(title, message) {
      this.modalTitle = title;
      this.modalMessage = message;
      this.modalVisible = true;
    },
    closeModal() {
      this.modalVisible = false;
    },
  },
};
</script>

<style scoped>

.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5); 
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background-color: #fff;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  max-width: 500px;
  width: 100%;
}

.modal-content p {
  white-space: pre-line;
  line-height: 1.5;
  color: #333;
}

.modal-content button {
  margin-top: 20px;
  padding: 10px 20px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.modal-content button:hover {
  opacity: 0.9;
}

.container {
  width: 100%;
  min-height: 80vh;
  margin: 0 auto;
  padding: 40px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.container {
  width: 100%;
  min-height: 80vh;
  margin: 0 auto;
  padding: 40px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  justify-content: center;
}

h1 {
  text-align: center;
  color: #333;
  font-size: 2rem;
  margin-bottom: 30px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin-bottom: 15px;
}

.form-group label {
  font-size: 1rem;
  color: #333;
  font-weight: bold;
  margin-bottom: 5px;
}

.form-group select {
  padding: 10px;
  font-size: 1rem;
  border-radius: 6px;
  border: 1px solid #ccc;
  background-color: #fff;
  color: #333;
  transition: all 0.3s ease;
  outline: none;
}

.form-group select:focus {
  border-color: #007bff;
  box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
}

.form-group select option {
  padding: 10px;
}

.form-group select:disabled {
  background-color: #f0f0f0;
  color: #aaa;
}

.form-group select::placeholder {
  color: #aaa;
}

  .detalhe-section h2 {
    margin-top: 30px;
    color: #555;
    font-size: 1.5rem;
    text-align: center;
  }
  
  .detalhe-group {
    background-color: #fff;
    padding: 15px;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
    margin-bottom: 20px;
  }
  
  .add-button, .remove-button {
    padding: 8px 15px;
    font-size: 1rem;
    cursor: pointer;
    border-radius: 5px;
    border: none;
  }
  
  .add-button {
    background-color: #4CAF50;
    color: white;
    margin-top: 10px;
    width: 100%;
    align-items: center
  }
  
  .remove-button {
    background-color: #f44336;
    color: white;
    margin-top: 10px;
  }
  
  .submit-button {
    padding: 12px 20px;
    font-size: 1.2rem;
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    width: 100%;
    margin-top: 20px;
  }
  
  button[type="submit"]:hover,
  .add-button:hover,
  .remove-button:hover {
    opacity: 0.9;
  }
</style>
