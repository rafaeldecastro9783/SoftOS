import type { Cliente } from './cliente';
import type { OrdemServico } from './ordemServico';
import type { Profissional } from './profissional';
import type { Ticket } from './ticket';

export interface Empresa {
  Id: number;
  nome: string;
  email: string;
  telefone: string;
  cpf: string;
  endereco: string;
  Cidade: string;
  estado: string;
  cep: string;
  pais: string;
  cnpj?: string;
  profissionalId: number;
  ativo: boolean;
  readonly profissional?: Profissional[];
  clienteId: number;
  readonly cliente?: Cliente[];
  ticketId: number;
  readonly ticket?: Ticket[];
  ordemServicoId: number;
  readonly ordemServico?: OrdemServico[];
}
