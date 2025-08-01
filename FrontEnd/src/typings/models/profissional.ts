import type { ProfissionalCargo } from 'enums';
import type { Ticket } from './ticket';
import type { Empresa } from './empresa';
import type { OrdemServico } from './ordemServico';

export interface Profissional {
  id: number;
  guid: string;
  nome: string;
  login: string;
  senha: string;
  email: string;
  telefone: string;
  cpf: string;
  cargo: ProfissionalCargo;
  ativo: boolean;

  readonly empresa?: Empresa[];
  readonly ordemServico?: OrdemServico[];
  readonly tickets?: Ticket[];
}
