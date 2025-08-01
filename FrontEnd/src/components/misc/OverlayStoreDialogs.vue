<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { useConfigStore, useOverlayStore } from 'stores';

const { hideMessageDialog, hideSessionExpiredDialog } = useOverlayStore();
const { dlgCarregamento, dlgMensagem, dlgSessaoExpirada } = storeToRefs(useOverlayStore());
const { emTelaReduzida } = storeToRefs(useConfigStore());

function btnOkClick() {
  hideSessionExpiredDialog();
  useConfigStore().deslogar();
}
</script>

<template>
  <v-dialog v-model="dlgCarregamento" class="text-center">
    <div class="div-radial" />
    <h2 class="h2-carregando-animation">{{ 'CARREGANDO' }}</h2>
    <v-progress-circular class="mx-auto" indeterminate size="128" width="8">
      <template v-slot:default>
        <v-icon class="icn-timer-sand-complete-animation">
          {{ 'mdi-timer-sand-complete' }}
        </v-icon>
      </template>
    </v-progress-circular>
  </v-dialog>

  <v-dialog
    v-if="dlgMensagem"
    v-model="dlgMensagem.visible"
    :persistent="false"
    max-width="560"
    width="100%"
  >
    <v-card :color="dlgMensagem!.color" class="bg-surface text-center">
      <h2 class="py-2">{{ dlgMensagem.title! }}</h2>
      <v-divider />
      <b class="overflow-y-auto text-center mx-4 py-2">
        {{ dlgMensagem!.text }}
      </b>
      <template v-if="!emTelaReduzida">
        <v-icon :icon="dlgMensagem!.icon" class="icn-mensagem-left" />
        <v-icon :icon="dlgMensagem!.icon" class="icn-mensagem-right" />
      </template>
      <v-btn
        @click="hideMessageDialog"
        :class="`text-${dlgMensagem!.color} mx-auto bg-white my-2`"
        density="compact"
        width="5rem"
        color="fff"
        text="ok"
      />
    </v-card>
  </v-dialog>

  <v-dialog v-model="dlgSessaoExpirada">
    <v-card class="mx-auto">
      <v-icon class="mx-auto mt-2" size="64">{{ 'mdi-alarm' }}</v-icon>
      <div class="text-center my-auto mx-4">
        <h2>A sessão expirou</h2>
        <h4>Por favor, faça login novamente</h4>
        <v-btn
          @click="btnOkClick"
          class="ml-auto my-2"
          variant="outlined"
          density="compact"
          text="ok"
        />
      </div>
    </v-card>
  </v-dialog>
</template>

<style scoped>
.h2-carregando-animation {
  animation: h2-carregando-animation 1.5s infinite ease-in-out;
}

@keyframes h2-carregando-animation {
  0% {
    transform: scaleX(1);
    opacity: 1;
  }
  50% {
    transform: scaleX(1.125);
    opacity: 0.5;
  }
  100% {
    transform: scaleX(1);
    opacity: 1;
  }
}

.icn-timer-sand-complete-animation {
  animation: icn-timer-sand-complete-animation 1s infinite ease-out;
  font-size: 96px;
}

@keyframes icn-timer-sand-complete-animation {
  0% {
    transform: rotateZ(0) scale(0.875);
  }
  15%,
  30% {
    transform: rotateZ(0) scale(1);
  }
  75%,
  100% {
    transform: rotateZ(180deg) scale(1);
  }
}

.div-radial {
  background: radial-gradient(ellipse at center, #ffffff40 0%, transparent 66%);
  transform: translate(-50%, -50%);
  pointer-events: none;
  position: absolute;
  height: 100dvh;
  width: 100dvw;
  left: 50%;
  top: 50%;
}

.icn-mensagem-left {
  position: absolute;
  font-size: 48px;
  opacity: 0.5;
  bottom: 16px;
  left: 16px;
}
.icn-mensagem-right {
  position: absolute;
  font-size: 48px;
  opacity: 0.5;
  bottom: 16px;
  right: 16px;
}
</style>
