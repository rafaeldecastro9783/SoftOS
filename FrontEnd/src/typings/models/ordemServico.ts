import type { Profissional } from './profissional';
import type { Empresa } from './empresa';
import type { Ticket } from './ticket';
import type { TipoServico } from './tipoServico';

export interface OrdemServico {
  id: number;
  tipoServicoId: number;
  readonly tipoServico?: TipoServico[];
  profissionalId: number;
  readonly profissional?: Profissional[];
  empresaId: number;
  readonly empresa?: Empresa[];
  ticketId: number;
  readonly ticket?: Ticket[];
  dataCriacao: string | number;
  dataConclusao: string | number;
  historico: Record<string | number, string>;
  ativo: boolean;
  // Additional properties can be added as needed
}
