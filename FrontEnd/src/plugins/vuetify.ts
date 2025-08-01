import '@mdi/font/css/materialdesignicons.css';
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import 'vuetify/styles';

export const themes = {
  padrao: {
    dark: false,
    colors: {
      background: '#8f998a',
      surface: '#e6f0e9',

      mix: '#bbc5ba',
      'mix-lighten': '#d0dad1',
      'mix-darken': '#a5afa2',

      primary: '#4d7c5b',
      'primary-darken': '#245338',
      'primary-lighten': '#76a47e',
      secondary: '#5c4d7c',
      'secondary-darken': '#392353',
      'secondary-lighten': '#7e76a4',
      tertiary: '#7e5b4b',
      'tertiary-darken': '#553922',
      'tertiary-lighten': '#a67d74',

      error: '#b04e5e',
      info: '#516fb5',
      success: '#469f6b',
      warning: '#c19952',
    },
  },
};

export default createVuetify({
  components: components,
  defaults: {
    VAppBar: {
      density: 'compact',
      color: 'primary',
      elevation: '8',
    },
    VBtn: {
      variant: 'flat',
    },
    VChip: {
      density: 'compact',
      variant: 'flat',
    },
    VDialog: {
      persistent: true,
      opacity: 0.675,
      width: 'auto',
    },
    VDivider: {
      opacity: 0.25,
      thickness: 2,
      color: 'fff',
    },
    VRow: {
      noGutters: true,
    },
    VSelect: {
      density: 'compact',
    },
    VTable: {
      density: 'compact',
      fixedHeader: true,
    },
    VTextField: {
      variant: 'outlined',
      density: 'compact',
      hideDetails: true,
      color: 'fff',
    },
  },
  theme: {
    themes,
  },
});
