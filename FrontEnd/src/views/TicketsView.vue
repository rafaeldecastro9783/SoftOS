<script setup lang="ts">
import { type AxiosInstance, HttpStatusCode } from 'axios';
import { inject, onMounted, ref, computed } from 'vue';

// Definição do tipo Ticket, incluindo propriedades opcionais (null) para chaves estrangeiras
import type { Ticket } from 'typings';

const axios = inject<AxiosInstance>('axios');

// Estado da aplicação
const jsonTickets = ref<Ticket[]>([]);
const dialog = ref(false); // Controla a visibilidade do modal
const editedIndex = ref(-1); // -1 = novo, >-1 = editando
const editedItem = ref<Ticket>({
  id: 0,
  descricao: '',
  dataCriacao: new Date().toISOString(),
  dataConclusao: null,
  ativo: true,
  situacao: 0,
  // Propriedades de navegação também são nulas por padrão
});

// Título do modal baseado se é um novo item ou edição
const formTitle = computed(() => (editedIndex.value === -1 ? 'Novo Ticket' : 'Editar Ticket'));

// Reseta o formulário
function resetForm() {
  editedItem.value = {
    id: 0,
    descricao: '',
    dataCriacao: new Date().toISOString(),
    dataConclusao: null,
    ativo: true,
    situacao: 0,
  };
  editedIndex.value = -1;
}

// Abre o modal para criar um novo ticket
function novoTicket() {
  resetForm();
  dialog.value = true;
}

// Abre o modal para editar um ticket existente
function editarTicket(item: Ticket) {
  editedIndex.value = jsonTickets.value.indexOf(item);
  editedItem.value = { ...item }; // Cria uma cópia para evitar editar diretamente
  dialog.value = true;
}

// Salva o ticket (novo ou editado)
async function salvarTicket() {
  try {
    if (editedIndex.value > -1) {
      // Lógica de Edição (UPDATE)
      const res = await axios?.put(`Ticket/${editedItem.value.id}`, editedItem.value);
      if (res?.status === HttpStatusCode.Ok) {
        Object.assign(jsonTickets.value[editedIndex.value], res.data);
      }
    } else {
      // Lógica de Criação (CREATE)
      const res = await axios?.post('Ticket', editedItem.value);
      if (res?.status === HttpStatusCode.Created) {
        jsonTickets.value.push(res.data);
      }
    }
  } catch (error) {
    alert('Erro ao salvar o ticket. Verifique os dados ou se as chaves estrangeiras existem.');
    console.error('Falha ao salvar ticket:', error);
  } finally {
    dialog.value = false;
    resetForm();
  }
}

// Exclui um ticket
async function excluirTicket(id: number) {
  if (confirm('Tem certeza que deseja excluir este ticket?')) {
    try {
      await axios?.delete(`Ticket/${id}`);
      jsonTickets.value = jsonTickets.value.filter((ticket) => ticket.id !== id);
    } catch (error) {
      alert('Erro ao excluir o ticket.');
      console.error('Falha ao excluir ticket:', error);
    }
  }
}

// Carrega a lista de tickets
async function carregarTickets() {
  try {
    const res = await axios?.get('Ticket');
    if (res?.status === HttpStatusCode.Ok) {
      jsonTickets.value = res.data as Ticket[];
    }
  } catch (error) {
    alert('Erro ao carregar tickets.');
    console.error('Falha na requisição:', error);
  }
}

onMounted(() => carregarTickets());
</script>

<template>
  <div class="pa-4">
    <div class="d-flex justify-space-between align-center mb-4">
      <h2>Tickets</h2>
      <v-btn color="primary" @click="novoTicket">Novo</v-btn>
    </div>

    <v-divider class="mb-4" />

    <v-alert v-if="jsonTickets.length === 0" type="info"> Sem Tickets para Exibir </v-alert>

    <v-table v-else>
      <thead>
        <tr>
          <th>ID</th>
          <th>Cliente</th>
          <th>Data de Criação</th>
          <th>Descrição</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="ticket in jsonTickets" :key="ticket.id">
          <td>{{ ticket.id }}</td>
          <td>{{ ticket.cliente }}</td>
          <td>{{ new Date(ticket.dataCriacao).toLocaleDateString() }}</td>
          <td>{{ ticket.descricao }}</td>
          <td>
            <v-btn icon color="primary" @click="editarTicket(ticket)" class="mr-2">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon color="red" @click="excluirTicket(ticket.id)">
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
          <v-form @submit.prevent="salvarTicket">
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="editedItem.clienteId"
                  label="ID do Cliente"
                  type="number"
                  required
                />
              </v-col>
              <v-col cols="12">
                <v-textarea v-model="editedItem.descricao" label="Descrição" required />
              </v-col>
            </v-row>
          </v-form>
        </v-container>
      </v-card-text>

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="dialog = false">Cancelar</v-btn>
        <v-btn color="blue darken-1" text @click="salvarTicket">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
