export interface OverlayMensagemProps {
  visible: boolean;
  color: 'error' | 'grey' | 'info' | 'success' | 'warning';
  icon:
    | 'mdi-close-octagon'
    | 'mdi-dots-horizontal-circle'
    | 'mdi-information-box'
    | 'mdi-check-decagram'
    | 'mdi-alert';
  title: string;
  text: string;
}
