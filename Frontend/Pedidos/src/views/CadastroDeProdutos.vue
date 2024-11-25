<template>
  <div class="container">
    <h1>Cadastro de Produtos</h1>
    <form @submit.prevent="handleSubmit" class="form">
      <div class="form-group">
        <FormInput v-model="product.nome" label="Nome do Produto" name="nome" required />
      </div>

      <div class="form-group">
        <FormInput v-model="product.descricao" label="Descrição" name="descricao" required />
      </div>

      <div class="form-group">
        <FormInput 
          v-model="product.preco" 
          label="Preço" 
          name="preco" 
          required 
          type="number" 
          step="0.01" 
          min="0"
        />
      </div> 

      <button type="submit" class="submit-button">Cadastrar Produto</button>
    </form>
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
      product: {
        nome: '',
        descricao: '',
        preco: '',
      },
      modalVisible: false,
      modalTitle: '',
      modalMessage: '',
    };
  },
  methods: {
    async handleSubmit() {
      try {
        if (!this.product.preco) {
          this.product.preco = 0;
        }

        console.log("Dados a enviar:", this.product);
        await api.post('/AdicionarProduto', this.product);
        
        this.showModal('Sucesso', 'Produto cadastrado com sucesso!');

        this.resetForm();
      } catch (error) {
        console.error('Erro ao cadastrar produto:', error.response.data);

        if (error.response && error.response.data && Array.isArray(error.response.data)) {
          const errorMessages = error.response.data
            .map(err => err.errorMessage)
            .join('\n');
          this.showModal('Erro', `Erro ao cadastrar produto:\n${errorMessages}`);
        } else {
          this.showModal('Erro', 'Erro ao cadastrar produto.');
        }
      }
    },
    resetForm() {
      this.product = {
        nome: '',
        descricao: '',
        preco: '',
        categorias: [
          {
            nome: '',
            descricao: '',
          },
        ],
      };
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

.form-control {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
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
}

.categories-section h2 {
  margin-top: 30px;
  color: #555;
  font-size: 1.5rem;
  text-align: center;
}

.category-group {
  background-color: #fff;
  padding: 15px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
  margin-bottom: 20px;
}

.add-button,
.remove-button {
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
  align-items: center;
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
