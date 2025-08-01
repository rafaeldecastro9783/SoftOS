using Microsoft.EntityFrameworkCore;
using SoftOS.BLL.Models;
using SoftOS.DAL.Context;
using SoftOS.Shared.DTOs;
using SoftOS.Shared.Exceptions;
using SoftOS.Shared.Utils;
using SoftOS.Shared.Enums;
using System.Net;

namespace SoftOS.BLL.Services
{
    public interface IAuthService
    {
        Task<string> CreateLoginAsync<T>(AuthRequestDTO model) where T : class, ILoginModel;
    }

    public class AuthService(AppDbContext context, IHttpContextAccessor contextAccessor) : IAuthService
    {
        private readonly AppDbContext _context = context;

        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public async Task<string> CreateLoginAsync<T>(AuthRequestDTO model) where T : class, ILoginModel
        {
            if (
                await _context.Set<T>().FirstOrDefaultAsync(p => p.Login == model.Login)
                is not T dbModel
            )
                throw new ServiceException(
                    HttpStatusCode.NotFound,
                    TemaModal.Aviso,
                    "Funcionário não encontrado",
                    "Não existe funcionário com esse login cadastrado no sistema"
                );

            if (dbModel.Senha!= HashUtil.ComputarPBKDF2(model.Senha))
                throw new ServiceException(
                    HttpStatusCode.Unauthorized,
                    TemaModal.Aviso,
                    "Usuário não autenticado",
                    "Por favor, informe a senha corretamente"
                );

            return JwtUtil.TokenGerar(dbModel);
        }
    }
}
