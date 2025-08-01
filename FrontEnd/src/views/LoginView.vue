<script setup lang="ts">
import { HttpStatusCode, type AxiosInstance } from 'axios';
import { useConfigStore } from 'stores';
import type { AuthRequestDTO } from 'typings';
import { computed, inject, onMounted, ref } from 'vue';
//import { useRouter } from 'vue-router';

const axios = inject<AxiosInstance>('axios');
const configStore = useConfigStore();
//const router = useRouter();

const btnAcessarLoading = ref<boolean>(false);
const ckbCredenciais = ref<boolean>(false);
const txfLogin = ref<string>('');
const txfSenha = ref<string>('');

const btnAcessarDisabled = computed(() => {
  const login = txfLogin.value.trim();
  const senha = txfSenha.value.trim();
  return login.length < 3 || senha.length < 8 || senha.length > 64;
});

async function frmLoginSubmit(e: Event) {
  const data: AuthRequestDTO = {
    login: txfLogin.value,
    senha: txfSenha.value,
  };

  btnAcessarLoading.value = true;
  e.preventDefault();

  try {
    const res = await axios?.post('Auth/login/profissional', data);
    if (res?.status === HttpStatusCode.Ok) {
      if (ckbCredenciais.value) {
        configStore.login = txfLogin.value;
        configStore.senha = txfSenha.value;
      } else {
        configStore.login = undefined;
        configStore.senha = undefined;
      }
      configStore.logar(res.data as string);
    }
  } finally {
    btnAcessarLoading.value = false;
  }
}

onMounted(async () => {
  if (!configStore.senha || !configStore.login) return;

  await new Promise((resolve) => setTimeout(resolve, 250));

  txfLogin.value = configStore.login;
  txfSenha.value = configStore.senha;
  ckbCredenciais.value = true;
});
</script>

<template>
  <v-sheet class="mx-auto mt-10 pa-6" width="380" elevation="3">
    <v-card-title class="text-center mb-4">
      <h3>LOGIN DO PROFISSIONAL</h3>
    </v-card-title>
    <v-divider class="mb-4" />

    <form @submit="frmLoginSubmit" class="d-flex flex-column ga-4">
      <v-text-field v-model="txfLogin" label="Login" prepend-inner-icon="mdi-account" required />
      <v-text-field
        v-model="txfSenha"
        label="Senha"
        type="password"
        prepend-inner-icon="mdi-lock"
        required
      />
      <v-divider class="mb-4" />
      <v-btn
        :disabled="btnAcessarDisabled"
        :loading="btnAcessarLoading"
        color="primary"
        type="submit"
        class="mt-2"
        block
      >
        Entrar
      </v-btn>
    </form>
  </v-sheet>
</template>
