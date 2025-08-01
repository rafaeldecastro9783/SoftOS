import { defineStore } from 'pinia';
import { TemaModal } from '../enums/temaModal';
import type { OverlayMensagemProps } from 'typings';

function wait(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

export const useOverlayStore = defineStore('orverlay', {
  state: () => ({
    dlgCarregamento: false,
    dlgErroServidor: false,
    dlgMensagem: undefined as OverlayMensagemProps | undefined,
    dlgSessaoExpirada: false,
    snbExpirou: false,
    snbTempo: false,
  }),
  actions: {
    async hideLoadingDialog() {
      await wait(125);
      this.dlgCarregamento = false;
    },
    hideMessageDialog() {
      this._hideDialog(this.dlgMensagem);
    },
    hideServerErrorWindow() {
      this.dlgErroServidor = false;
    },
    hideSessionExpiredDialog() {
      this.dlgSessaoExpirada = false;
    },
    async showErrorMessageDialog(title: string, text: string, delay?: number) {
      await this.showMessageDialog(TemaModal.Erro, title, text, delay ? delay : 125);
    },
    async showInfoMessageDialog(title: string, text: string, delay?: number) {
      await this.showMessageDialog(TemaModal.Info, title, text, delay ? delay : 125);
    },
    showLoadingDialog() {
      this.dlgCarregamento = true;
    },
    async showMessageDialog(tema: TemaModal, title: string, text: string, delay: number = 125) {
      const map: Record<
        TemaModal,
        { icon: OverlayMensagemProps['icon']; color: OverlayMensagemProps['color'] }
      > = {
        [TemaModal.Ignore]: { icon: 'mdi-dots-horizontal-circle', color: 'grey' },
        [TemaModal.Aviso]: { icon: 'mdi-alert', color: 'warning' },
        [TemaModal.Erro]: { icon: 'mdi-close-octagon', color: 'error' },
        [TemaModal.Info]: { icon: 'mdi-information-box', color: 'info' },
        [TemaModal.Sucesso]: { icon: 'mdi-check-decagram', color: 'success' },
      };
      const { icon, color } = map[tema];
      this.dlgMensagem = {
        visible: false,
        color,
        title,
        icon,
        text,
      };
      await wait(delay);
      if (this.dlgMensagem) {
        this.dlgMensagem.visible = true;
      }
    },
    async showServerErrorWindow(delay: number = 0) {
      await wait(delay);
      this.dlgErroServidor = true;
    },
    showSessionExpiredDialog() {
      this.dlgSessaoExpirada = true;
    },
    async showSuccessMessageDialog(title: string, text: string, delay?: number) {
      await this.showMessageDialog(TemaModal.Sucesso, title, text, delay ? delay : 125);
    },
    async showWarningMessageDialog(title: string, text: string, delay?: number) {
      await this.showMessageDialog(TemaModal.Aviso, title, text, delay ? delay : 125);
    },
    async _hideDialog(dialog?: OverlayMensagemProps) {
      if (dialog?.visible) {
        dialog.visible = false;
        await wait(250);
        if (this.dlgMensagem === dialog) {
          this.dlgMensagem = undefined;
        }
      }
    },
  },
});
