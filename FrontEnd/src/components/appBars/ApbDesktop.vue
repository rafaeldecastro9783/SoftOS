<script setup lang="ts">
import { useConfigStore } from 'stores';

const configStore = useConfigStore();
async function logout() {
  configStore.deslogar();
}
</script>

<template>
  <v-app-bar :elevation="2" rounded>
    <v-app-bar-title text="Dashboard"></v-app-bar-title>
    <v-slot:extension> </v-slot:extension>
    <v-tabs>
      <v-tab text="Tickets" :to="{ name: 'tickets' }"></v-tab>
      <v-tab text="Ordens de Serviço" :to="{ name: 'ordensdeservico' }"></v-tab>
      <v-tab text="Clientes" :to="{ name: 'clientes' }"></v-tab>
      <v-tab text="Profisionais" :to="{ name: 'profissionais' }"></v-tab>
      <v-btn label="logout" color="warning" @click="logout">Sair</v-btn>
    </v-tabs>
  </v-app-bar>
</template>
