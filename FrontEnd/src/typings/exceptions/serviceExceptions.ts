import type { TemaModal } from '../../enums/temaModal';

export interface ServiceException {
  tema: TemaModal;
  titulo: string;
  texto: string;
}
