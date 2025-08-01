using SoftOS.Shared.Enums;

namespace SoftOS.Shared.DTOs;

public class JwtSoDTO
{
    public int Aid { get; set; }
    public int Exp { get; set; }
    public int Iat { get; set; }
    public int Nbf { get; set; }
    public string Nom { get; set; } = string.Empty;
    public JwtSoPermissao Pms { get; set; }
}
