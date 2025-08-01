import { defineStore } from 'pinia';
import router, { routes } from 'plugins/router';
import type { JwtSoDTO } from 'typings';
import { parse } from 'utils/jwtUtils';
import { localRef } from 'utils/refUtils';

export const useConfigStore = defineStore('config', {
  state: () => ({
    emTelaReduzida: false,
    jwtSo: localRef<JwtSoDTO | undefined>('jwt-s-o'),
    login: localRef<string | undefined>('login'),
    token: localRef<string | undefined>('token'),
    senha: localRef<string | undefined>('senha'),
  }),
  actions: {
    deslogar() {
      this.token = undefined;
      router.push(routes.login.path);
    },
    logar(jwt: string) {
      this.tokenAtualizar(jwt);
      router.push(routes.dashboard.path);
    },
    tokenAtualizar(val: string) {
      useConfigStore().jwtSo = parse(val);
      this.token = val;
    },
    validarSessao() {
      if (this.jwtSo) {
        if (this.jwtSo.exp < Date.now() / 1000) {
          this.jwtSo = undefined;
          this.token = undefined;
          router.replace(routes.login);
        }
      }
    },
  },
});
