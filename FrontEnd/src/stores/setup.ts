import { HttpStatusCode, type AxiosInstance } from 'axios';
import { defineStore } from 'pinia';
import type { SetupDTO } from 'typings';
import { inject } from 'vue';

export const useSetupStore = defineStore('setup', {
  state: (): SetupDTO => {
    return {
      delay: {
        modal: 1000,
        response: 500,
      },
      ready: false,
    };
  },
  actions: {
    async fetchData() {
      const res = await inject<AxiosInstance>('axios')?.get('Setup');
      if (res?.status === HttpStatusCode.Ok) {
        const setup = <SetupDTO>(<unknown>{ setupDTO: res.data });
        this.delay = setup.delay;
        this.ready = setup.ready;
      }
    },
  },
});
