import axios, { type AxiosInstance } from 'axios';
import { useConfigStore } from 'stores';
import * as CallHandlerUtil from 'utils/callHandlerUtils';
import type { Plugin } from 'vue';
const plugin: Plugin = {
  install(app) {
    const instance: AxiosInstance = axios.create({
      timeout: import.meta.env.DEV ? 300000 : 30000,
      baseURL: import.meta.env.VITE_BASE_URL,
      paramsSerializer: { indexes: null },
    });

    instance.interceptors.request.use((config) => {
      config.headers.Authorization = useConfigStore().token;
      return config;
    });

    instance.interceptors.response.use(
      async (response) => {
        await new Promise((resolve) => setTimeout(resolve, 125));
        CallHandlerUtil.handleAxiosResponse(response);
        return response;
      },
      async (error) => {
        await new Promise((resolve) => setTimeout(resolve, 125));
        return CallHandlerUtil.handleAxiosError(error);
      },
    );

    app.provide('axios', instance);
  },
};

export default plugin;
