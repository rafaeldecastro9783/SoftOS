import type { TicketSituacao } from 'enums';
import type { Cliente } from './cliente';
import type { Empresa } from './empresa';
import type { OrdemServico } from './ordemServico';
import type { Profissional } from './profissional';

export interface Ticket {
  id: number;
  ativo: boolean;
  clienteId: number;
  descricao: string;
  dataCriacao: string | number;
  dataConclusao: string | number;
  empresaId: number;
  ordemServicoId: number;
  profissionalId: number;
  situacao: TicketSituacao;

  readonly cliente?: Cliente[];
  readonly empresa?: Empresa[];
  readonly ordemServico?: OrdemServico[];
  readonly profissional?: Profissional[];
}
