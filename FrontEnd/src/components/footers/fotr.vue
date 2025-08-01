<script lang="ts">
const links = ['Home', 'About Us', 'Team', 'Services', 'Blog', 'Contact Us'];
</script>

<template>
  <v-footer
    class="d-flex align-center justify-center ga-1 flex-wrap flex-grow-1 py-3"
    color="surface-light"
  >
    <v-btn v-for="link in links" :key="link" :text="link" variant="text" rounded></v-btn>

    <div class="flex-1-0-100 text-center mt-2">
      {{ new Date().getFullYear() }} — <strong>Assinatura</strong>
    </div>
  </v-footer>
</template>
