<script setup lang="ts">
import { type AxiosInstance, HttpStatusCode } from 'axios';
import { inject, onMounted, ref, computed } from 'vue';

import type { Profissional } from 'typings';

const axios = inject<AxiosInstance>('axios');

const jsonProfissionais = ref<Profissional[]>([]);
const dialog = ref(false);
const editedIndex = ref(-1);
const editedItem = ref<Profissional>({
  id: 0,
  nome: '',
  email: '',
  telefone: '',
  cpf: '',
  ativo: true,
});

const formTitle = computed(() =>
  editedIndex.value === -1 ? 'Novo Profissional' : 'Editar Profissional',
);

function resetForm() {
  editedItem.value = {
    id: 0,
    nome: '',
    email: '',
    telefone: '',
    cpf: '',
    ativo: true,
  };
  editedIndex.value = -1;
}

function novoProfissional() {
  resetForm();
  dialog.value = true;
}

function editarProfissional(item: Profissional) {
  editedIndex.value = jsonProfissionais.value.indexOf(item);
  editedItem.value = { ...item }; // Cria uma cópia para evitar editar diretamente
  dialog.value = true;
}

async function salvarProfissional() {
  try {
    if (editedIndex.value > -1) {
      const res = await axios?.put(`Profissional/${editedItem.value.id}`, editedItem.value);
      if (res?.status === HttpStatusCode.Ok) {
        Object.assign(jsonProfissionais.value[editedIndex.value], res.data);
      }
    } else {
      const res = await axios?.post('Profissional', editedItem.value);
      if (res?.status === HttpStatusCode.Created) {
        jsonProfissionais.value.push(res.data);
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

// Exclui
async function excluirProfissional(id: number) {
  if (confirm('Tem certeza que deseja excluir este ticket?')) {
    try {
      await axios?.delete(`Profissional/${id}`);
      jsonProfissionais.value = jsonProfissionais.value.filter(
        (Profissional) => Profissional.id !== id,
      );
    } catch (error) {
      alert('Erro ao excluir o ticket.');
      console.error('Falha ao excluir ticket:', error);
    }
  }
}

// Carrega a lista de tickets
async function carregarProfissionais() {
  try {
    const res = await axios?.get('Profissional');
    if (res?.status === HttpStatusCode.Ok) {
      jsonProfissionais.value = res.data as Profissional[];
    }
  } catch (error) {
    alert('Erro ao carregar tickets.');
    console.error('Falha na requisição:', error);
  }
}

onMounted(() => carregarProfissionais());
</script>
<template>
  <div class="pa-4">
    <div class="d-flex justify-space-between align-center mb-4">
      <h2>Profissionais</h2>
      <v-btn color="primary" @click="novoProfissional">Novo</v-btn>
    </div>

    <v-divider class="mb-4" />

    <v-alert v-if="jsonProfissionais.length === 0" type="info">
      Sem Profissionais para Exibir
    </v-alert>

    <v-table v-else>
      <thead>
        <tr>
          <th>ID</th>
          <th>Nome</th>
          <th>Email</th>
          <th>telefone</th>
          <th>cargo</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="profissional in jsonProfissionais" :key="profissional.id">
          <td>{{ profissional.id }}</td>
          <td>{{ profissional.nome }}</td>
          <td>{{ profissional.email }}</td>
          <th>{{ profissional.telefone }}</th>
          <th>{{ profissional.cargo }}</th>
          <td>
            <v-btn icon color="primary" @click="editarProfissional(profissional)" class="mr-2">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon color="red" @click="excluirProfissional(profissional.id)">
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

      <v-card-text gap="10">
        <v-container>
          <v-form @submit.prevent="salvarProfissional">
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="editedItem.id"
                  label="ID do Profissional"
                  type="number"
                  required
                />
              </v-col>
              <v-col cols="12">
                <v-text-field v-model="editedItem.nome" label="Nome" required />
              </v-col>
              <v-col cols="12">
                <v-text-field v-model="editedItem.email" label="email" required />
              </v-col>
              <v-col cols="12">
                <v-text-field v-model="editedItem.telefone" label="telefone" required />
              </v-col>
              <v-col cols="12">
                <v-text-field v-model="editedItem.cargo" label="cargo" required />
              </v-col>
            </v-row>
          </v-form>
        </v-container>
      </v-card-text>

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="dialog = false">Cancelar</v-btn>
        <v-btn color="blue darken-1" text @click="salvarProfissional">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
