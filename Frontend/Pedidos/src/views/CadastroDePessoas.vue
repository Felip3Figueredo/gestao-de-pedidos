<template>
  <div class="container">
    <h1>Cadastro de Pessoas</h1>
    <form @submit.prevent="handleSubmit" class="form">
      <div class="form-group">
        <FormInput v-model="person.nome" label="Nome" name="nome" required />
      </div>

      <div class="form-group">
        <FormInput v-model="person.email" label="E-mail" name="email" required />
      </div>

      <div class="form-group">
        <FormInput v-model="person.cpf" label="CPF" name="cpf" required />
      </div>

      <div class="form-group">
        <FormInput v-model="person.telefone" label="Telefone" name="telefone" required />
      </div>

      <div class="addresses-section">
        <h2>Endereços</h2>
        <div
          v-for="(endereco, index) in person.endereco"
          :key="index"
          class="address-group"
        >
          <h3>Endereço {{ index + 1 }}</h3>

          <div class="form-group">
            <FormInput
              v-model="endereco.logradouro"
              label="Logradouro"
              name="logradouro"
              required
            />
          </div>

          <div class="form-group">
            <FormInput
              v-model="endereco.numero"
              label="Número"
              name="numero"
              required
              type="number"
            />
          </div>

          <div class="form-group">
            <FormInput
              v-model="endereco.bairro"
              label="Bairro"
              name="bairro"
              required
            />
          </div>

          <div class="form-group">
            <FormInput
              v-model="endereco.estado"
              label="Estado"
              name="estado"
              required
            />
          </div>

          <div class="form-group">
            <FormInput
              v-model="endereco.complemento"
              label="Complemento"
              name="complemento"
              required
            />
          </div>

          <div class="form-group">
            <FormInput v-model="endereco.cep" label="CEP" name="cep" required />
          </div>

          <div class="form-group">
            <FormInput
              v-model="endereco.Cidade"
              label="Cidade"
              name="cidade"
              required
            />
          </div>

          <div class="form-group">
            <label for="addressType">Tipo de Endereço</label>
            <select
              v-model="endereco.tipoEndereco"
              name="addressType"
              class="form-control"
            >
              <option
                v-for="option in addressTypes"
                :key="option.value"
                :value="option.value"
              >
                {{ option.text }}
              </option>
            </select>
          </div>

          <button
            type="button"
            class="remove-button"
            @click="removeAddress(index)"
          >
            Remover Endereço
          </button>
        </div>

        <button type="button" class="add-button" @click="addAddress">
          Adicionar Endereço
        </button>
      </div>

      <button type="submit" class="submit-button">Cadastrar</button>
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
      person: {
        nome: '',
        email: '',
        cpf: '',
        telefone: '',
        endereco: [
          { 
            logradouro: '', 
            numero: '', 
            complemento: '', 
            bairro: '', 
            cidade: '', 
            estado: '', 
            cep: '',
            tipoEndereco: '' 
          }
        ],
      },
      addressTypes: [
        { value: 1, text: 'ENTREGA' },
        { value: 2, text: 'COBRANÇA' },
        { value: 3, text: 'AMBOS' }
      ],
      modalVisible: false,
      modalTitle: '',
      modalMessage: ''
    };
  },
  methods: {
    addAddress() {
      this.person.endereco.push({ logradouro: '', numero: '', complemento: '', bairro: '', cidade: '', estado: '', cep: '', tipoEndereco: '' });
    },
    removeAddress(index) {
      this.person.endereco.splice(index, 1);
    },
    async handleSubmit() {
      try {
        this.person.endereco.forEach(endereco => {
          endereco.numero = Number(endereco.numero);  
          endereco.tipoEndereco = Number(endereco.tipoEndereco);  
        });
        console.log("Dados a enviar:", this.person);
        // Envia os dados via API
        await api.post('/AdicionarCliente', this.person);
        
        this.showModal('Sucesso', 'Pessoa cadastrada com sucesso!');

        this.resetForm();
      } catch (error) {
        console.error('Erro ao cadastrar pessoa:', error.response.data);

        if (error.response && error.response.data && Array.isArray(error.response.data)) {
          const errorMessages = error.response.data
            .map(err => err.errorMessage)
            .join('\n');
          this.showModal('Erro', `Erro ao cadastrar pessoa:\n${errorMessages}`);
        } else if (error.response.data){
          this.showModal('Erro', error.response.data);
        } else {
          this.showModal('Erro', 'Erro ao cadastrar pessoa: Não foi possível processar os erros retornados.');
        }
      }
    },
    resetForm() {
      this.person = {
        nome: '',
        email: '',
        cpf: '',
        telefone: '',
        endereco: [
          { 
            logradouro: '', 
            numero: '', 
            complemento: '', 
            bairro: '', 
            cidade: '', 
            estado: '', 
            cep: '',
            tipoEndereco: ''
          }
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
    }
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
  
  .form {
    display: flex;
    flex-direction: column;
    gap: 20px;
  }
  
  .form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
  }
  
  .addresses-section h2 {
    margin-top: 30px;
    color: #555;
    font-size: 1.5rem;
    text-align: center;
  }

  .address-group {
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
  