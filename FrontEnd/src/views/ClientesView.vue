<script setup lang="ts">
import { type AxiosInstance, HttpStatusCode } from 'axios';
import { inject, onMounted, ref, computed } from 'vue';

// Definição do tipo Ticket, incluindo propriedades opcionais (null) para chaves estrangeiras
import type { Cliente } from 'typings';

const axios = inject<AxiosInstance>('axios');

// Estado da aplicação
const jsonClientes = ref<Cliente[]>([]);
const dialog = ref(false); // Controla a visibilidade do modal
const editedIndex = ref(-1); // -1 = novo, >-1 = editando
const editedItem = ref<Cliente>({
  Id: 0,
  nome: '',
  login: '',
  senha: '',
  email: '',
  endereco: '',
  cidade: '',
  estado: '',
  cep: '',
  telefone: '',
  tipo: 0,
  cnpj: '',
  isActive: true,
});

// Título do modal baseado se é um novo item ou edição
const formTitle = computed(() => (editedIndex.value === -1 ? 'Novo Cliente' : 'Editar Cliente'));

// Reseta o formulário
function resetForm() {
  editedItem.value = {
    Id: 0,
    nome: '',
    login: '',
    senha: '',
    email: '',
    endereco: '',
    cidade: '',
    estado: '',
    cep: '',
    telefone: '',
    tipo: 0,
    cnpj: '',
    isActive: true,
  };
  editedIndex.value = -1;
}

// Abre o modal para criar um novo ticket
function novoCliente() {
  resetForm();
  dialog.value = true;
}

// Abre o modal para editar um ticket existente
function editarCliente(item: Cliente) {
  editedIndex.value = jsonClientes.value.indexOf(item);
  editedItem.value = { ...item }; // Cria uma cópia para evitar editar diretamente
  dialog.value = true;
}

// Salva o ticket (novo ou editado)
async function salvarCliente() {
  try {
    if (editedIndex.value > -1) {
      // Lógica de Edição (UPDATE)
      const res = await axios?.put(`Cliente/${editedItem.value.id}`, editedItem.value);
      if (res?.status === HttpStatusCode.Ok) {
        Object.assign(jsonClientes.value[editedIndex.value], res.data);
      }
    } else {
      // Lógica de Criação (CREATE)
      const res = await axios?.post('Cliente', editedItem.value);
      if (res?.status === HttpStatusCode.Created) {
        jsonClientes.value.push(res.data);
      }
    }
  } catch (error) {
    alert('Erro ao salvar o cliente. Verifique os dados ou se as chaves estrangeiras existem.');
    console.error('Falha ao salvar cliente:', error);
  } finally {
    dialog.value = false;
    resetForm();
  }
}

// Exclui um ticket
async function excluirCliente(id: number) {
  if (confirm('Tem certeza que deseja excluir este cliente?')) {
    try {
      await axios?.delete(`Cliente/${id}`);
      jsonClientes.value = jsonClientes.value.filter((cliente) => cliente.Id !== id);
    } catch (error) {
      alert('Erro ao excluir o cliente.');
      console.error('Falha ao excluir o cliente:', error);
    }
  }
}

// Carrega a lista de tickets
async function carregarClientes() {
  try {
    const res = await axios?.get('Cliente');
    if (res?.status === HttpStatusCode.Ok) {
      jsonClientes.value = res.data as Cliente[];
    }
  } catch (error) {
    alert('Erro ao carregar Clientes.');
    console.error('Falha na requisição:', error);
  }
}

onMounted(() => carregarClientes());
</script>

<template>
  <div class="pa-4">
    <div class="d-flex justify-space-between align-center mb-4">
      <h2>Clientes</h2>
      <v-btn color="primary" @click="novoCliente">Novo</v-btn>
    </div>

    <v-divider class="mb-4" />

    <v-alert v-if="jsonClientes.length === 0" type="info"> Sem Clientes para Exibir </v-alert>

    <v-table v-else>
      <thead>
        <tr>
          <th>ID</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="Cliente in jsonClientes" :key="Cliente.id">
          <td>{{ Cliente.Id }}</td>
          <td>
            <v-btn icon color="primary" @click="editarCliente(Cliente)" class="mr-2">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon color="red" @click="excluirCliente(Cliente.Id)">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </td>
        </tr>
      </tbody>
    </v-table>
  </div>

  <v-dialog v-model="dialog" max-width="600px">
    <v-card>
      <v-card-title>
        <span class="text-h5">{{ formTitle }}</span>
      </v-card-title>

      <v-card-text>
        <v-container>
          <v-form @submit.prevent="salvarCliente">
            <v-row>
              <v-col cols="12">
                <v-text-field v-model="editedItem.nome" label="Nome do Cliente" required />
              </v-col>
              <v-col cols="12">
                <v-text-field v-model="editedItem.cnpj" label="Cnpj" required />
              </v-col>
            </v-row>
          </v-form>
        </v-container>
      </v-card-text>

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="dialog = false">Cancelar</v-btn>
        <v-btn color="blue darken-1" text @click="salvarCliente">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
