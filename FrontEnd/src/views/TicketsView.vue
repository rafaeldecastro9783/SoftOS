<script setup lang="ts">
import { type AxiosInstance, HttpStatusCode } from 'axios';
import type { Ticket } from 'typings';
import { inject, onMounted, ref } from 'vue';

const axios = inject<AxiosInstance>('axios');

const jsonTickets = ref<Ticket[]>([]);

async function carregarTickets() {
  try {
    const res = await axios?.get('Ajax/Tickets');
    if (res?.status === HttpStatusCode.Ok) {
      const tickets = res.data as Ticket[];
      jsonTickets.value = tickets;
    }
  } finally {
    alert('');
  }
}

//async function criarTicket() {
// const data: Ticket = {
// ativo:
//};
//}

onMounted(() => carregarTickets());
</script>

<template>
  <div class="pa-4">
    <div class="d-flex justify-space-between align-center mb-4">
      <h2>Tickets</h2>
      <v-btn color="primary">Novo</v-btn>
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
          <td>{{ ticket.dataCriacao }}</td>
          <td>{{ ticket.descricao }}</td>
          <td>
            <v-btn icon text="ticket">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon color="red" text="{{ticket.id}}">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </td>
        </tr>
      </tbody>
    </v-table>
  </div>
</template>
