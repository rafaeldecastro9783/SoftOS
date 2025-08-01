import { AxiosError, HttpStatusCode, type AxiosInstance, type AxiosResponse } from 'axios';
import router, { routes } from 'plugins/router';
import { useConfigStore, useOverlayStore } from 'stores';
import type { ServiceException } from 'typings';
import { inject, type Ref } from 'vue';

export function handleAxiosError(error: unknown) {
  if (!(error instanceof AxiosError) || !error.config) {
    console.error(error);
    return;
  }

  const res = error.response;

  if (!res) {
    useOverlayStore().showServerErrorWindow(125);
    return;
  }

  const isBlob = error.config.responseType === 'blob';
  const modalHandled = isBlob ? handleBlobModalResponse(res) : handleJsonModalResponse(res);

  if (!modalHandled) handleHttpError(res.status);
}

export function handleAxiosResponse(response: AxiosResponse) {
  const token = response.headers['authorization'];
  if (token) useConfigStore().tokenAtualizar(token);

  filterJsonModal(response.data);
  return response;
}

export async function onMountedHandleAxiosGet(
  endpoints: string[],
  properties: Ref[],
  params: unknown[] = [],
) {
  try {
    const axios = inject<AxiosInstance>('axios');
    const requests = endpoints.map((url, i) => axios?.get(url, { params: params[i] || {} }));

    const responses = await Promise.all(requests);
    responses.forEach((res, i) => {
      if (!res || res.status !== HttpStatusCode.Ok)
        throw new Error(`Failed to fetch ${endpoints[i]}`);
      properties[i].value = res.data;
    });
  } catch (error) {
    handleAxiosError(error);
  }
}

export function warnEndUser(text: string) {
  console.warn(text);
}

async function handleHttpError(status: number) {
  const overlay = useOverlayStore();
  const routeName = router.currentRoute.value.name;

  switch (status) {
    case HttpStatusCode.Unauthorized:
      if (routeName === routes.login.name) {
        if (!overlay.snbExpirou) overlay.showSessionExpiredDialog();
      } else {
        overlay.snbExpirou = false;
        overlay.showSessionExpiredDialog();
      }
      break;
    case HttpStatusCode.Forbidden:
      await overlay.showInfoMessageDialog(
        'Você não pode acessar este conteúdo',
        'Por questões de segurança você foi redirecionado',
      );
      break;
    case HttpStatusCode.InternalServerError:
    default:
      await overlay.showServerErrorWindow(125);
      break;
  }
}

async function handleBlobModalResponse(response: AxiosResponse): Promise<boolean> {
  const text = (await response.data.text?.()) || (await readBlobAsText(response.data));
  const parsed = JSON.parse(text);

  if (isModalPayload(parsed)) {
    showModal(parsed);
    return true;
  }

  return false;
}

function handleJsonModalResponse(response: AxiosResponse): boolean {
  if (isModalPayload(response.data)) {
    showModal(response.data);
    return true;
  }

  return false;
}

function filterJsonModal(data: unknown): boolean {
  if (isModalPayload(data)) {
    showModal(data as ServiceException);
    return true;
  }

  return false;
}

function isModalPayload(data: unknown): boolean {
  if (data instanceof Object && 'tema' in data && 'texto' in data && 'titulo' in data) {
    const serviceException = data as ServiceException;
    if (!!serviceException.tema && !!serviceException.texto && !!serviceException.titulo) {
      useOverlayStore().showMessageDialog(
        serviceException.tema,
        serviceException.titulo,
        serviceException.texto,
      );
      return true;
    }
  }

  return false;
}

function showModal(payload: ServiceException) {
  useOverlayStore().showMessageDialog(payload.tema, payload.titulo, payload.texto);
}

function readBlobAsText(blob: Blob): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => resolve(String(reader.result));
    reader.onerror = reject;
    reader.readAsText(blob);
  });
}
