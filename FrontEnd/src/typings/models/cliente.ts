import type { TipoCliente } from 'enums';
import type { Ticket } from './ticket';
import type { ILoginModel } from './iLoginModel';
import type { OrdemServico } from './ordemServico';
import type { Empresa } from './empresa';

export interface Cliente extends ILoginModel {
  Id: number;
  tipo: TipoCliente;
  nome: string;
  login: string;
  senha: string;
  email: string;
  telefone: string;
  endereco: string;
  cidade: string;
  estado: string;
  cep: string;
  oais: string;
  cnpj?: string;
  isActive: boolean;
  empresaId: number;
  ordemServicolId: number;
  ticketid: number;
  readonly empresa?: Empresa[];
  readonly OrdemServico?: OrdemServico[];
  readonly ticket?: Ticket[];
}
