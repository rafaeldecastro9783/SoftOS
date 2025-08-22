<script setup lang="ts">
import { type AxiosInstance, HttpStatusCode } from 'axios';
import { inject, onMounted, ref, computed } from 'vue';

// Definição do tipo Ticket, incluindo propriedades opcionais (null) para chaves estrangeiras
import type { OrdemServico } from 'typings';

const axios = inject<AxiosInstance>('axios');

// Estado da aplicação
const jsonOrdemServicos = ref<OrdemServico[]>([]);
const dialog = ref(false); // Controla a visibilidade do modal
const editedIndex = ref(-1); // -1 = novo, >-1 = editando
const editedItem = ref<OrdemServico>({
  id: 0,
  historico: '',
  dataCriacao: new Date().toISOString(),
  dataConclusao: null,
  ticketId: '',
  profissionalId: null,
  empresaId: null,
  tipoServicoId: '',
  ativo: true,
  // Propriedades de navegação também são nulas por padrão
});

// Título do modal baseado se é um novo item ou edição
const formTitle = computed(() =>
  editedIndex.value === -1 ? 'Novo OrdemServico' : 'Editar OrdemServico',
);

// Reseta o formulário
function resetForm() {
  editedItem.value = {
    id: 0,
    historico: '',
    dataCriacao: new Date().toISOString(),
    dataConclusao: null,
    ticketId: '',
    profissionalId: null,
    empresaId: null,
    tipoServicoId: '',
    ativo: true,
  };
  editedIndex.value = -1;
}

// Abre o modal para criar um novo ticket
function novoOrdemServico() {
  resetForm();
  dialog.value = true;
}

// Abre o modal para editar um ticket existente
function editarOrdemServico(item: OrdemServico) {
  editedIndex.value = jsonOrdemServicos.value.indexOf(item);
  editedItem.value = { ...item }; // Cria uma cópia para evitar editar diretamente
  dialog.value = true;
}

// Salva o ticket (novo ou editado)
async function salvarOrdemServico() {
  try {
    if (editedIndex.value > -1) {
      // Lógica de Edição (UPDATE)
      const res = await axios?.put(`OrdemServico/${editedItem.value.id}`, editedItem.value);
      if (res?.status === HttpStatusCode.Ok) {
        Object.assign(jsonOrdemServicos.value[editedIndex.value], res.data);
      }
    } else {
      // Lógica de Criação (CREATE)
      const res = await axios?.post('OrdemServico', editedItem.value);
      if (res?.status === HttpStatusCode.Created) {
        jsonOrdemServicos.value.push(res.data);
      }
    }
  } catch (error) {
    alert(
      'Erro ao salvar a Ordem de Serviço. Verifique os dados ou se as chaves estrangeiras existem.',
    );
    console.error('Falha ao salvar Ordem de Serviço:', error);
  } finally {
    dialog.value = false;
    resetForm();
  }
}

// Exclui um ticket
async function excluirOrdemServico(id: number) {
  if (confirm('Tem certeza que deseja excluir esta Ordem de Serviço?')) {
    try {
      await axios?.delete(`OrdemServico/${id}`);
      jsonOrdemServicos.value = jsonOrdemServicos.value.filter((ticket) => ticket.id !== id);
    } catch (error) {
      alert('Erro ao excluir a ordem de serviço.');
      console.error('Falha ao excluir a ordem de servço:', error);
    }
  }
}

// Carrega a lista de tickets
async function carregarOrdemServicos() {
  try {
    const res = await axios?.get('OrdemServico');
    if (res?.status === HttpStatusCode.Ok) {
      jsonOrdemServicos.value = res.data as OrdemServico[];
    }
  } catch (error) {
    alert('Erro ao carregar ordens de serviços.');
    console.error('Falha na requisição:', error);
  }
}

onMounted(() => carregarOrdemServicos());
</script>

<template>
  <div class="pa-4">
    <div class="d-flex justify-space-between align-center mb-4">
      <h2>Ordens de Serviço</h2>
      <v-btn color="primary" @click="novoOrdemServico">Novo</v-btn>
    </div>

    <v-divider class="mb-4" />

    <v-alert v-if="jsonOrdemServicos.length === 0" type="info">
      Sem Ordens de Serviços para Exibir
    </v-alert>

    <v-table v-else>
      <thead>
        <tr>
          <th>ID</th>
          <th>Cliente</th>
          <th>Ticket ID</th>
          <th>Cliente ID</th>
          <th>Profssional</th>
          <th>Data de Criação</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="OrdemServico in jsonOrdemServicos" :key="OrdemServico.id">
          <td>{{ OrdemServico.id }}</td>
          <th>{{ OrdemServico.ticketId }}</th>
          <td>{{ OrdemServico.empresaId }}</td>
          <th>{{ OrdemServico.profissionalId }}</th>
          <td>{{ new Date(OrdemServico.dataCriacao).toLocaleDateString() }}</td>
          <td>
            <v-btn icon color="primary" @click="editarOrdemServico(OrdemServico)" class="mr-2">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon color="red" @click="excluirOrdemServico(OrdemServico.id)">
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
          <v-form @submit.prevent="salvarOrdemServico">
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="editedItem.empresaId"
                  label="ID do Cliente"
                  type="number"
                  required
                />
              </v-col>
              <v-col cols="12">
                <v-textarea v-model="editedItem.historico" label="Descrição" required />
              </v-col>
            </v-row>
          </v-form>
        </v-container>
      </v-card-text>

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="dialog = false">Cancelar</v-btn>
        <v-btn color="blue darken-1" text @click="salvarOrdemServico">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
