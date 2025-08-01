namespace SoftOS.Shared.Enums;

[Flags]
public enum JwtSoPermissao
{
    None = 0,
    Ticket = 1 << 0, // 1
    OrdemServico = 1 << 1, // 2
    Profissional = 1 << 2, // 4
    Empresa = 1 << 3, // 8
    Cliente = 1 << 4, // 16
}
