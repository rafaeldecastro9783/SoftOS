using Microsoft.AspNetCore.Mvc;
using SoftOS.Shared.Exceptions;

namespace SoftOS.API.Controllers;

[ApiController]
[Route("/Ajax/[controller]")]
public class AjaxController() : ControllerBase
{
    protected IActionResult Execute(Func<IActionResult> func)
    {
        try
        {
            return func();
        }
        catch (ServiceException ex)
        {
            //Log.Debug(ex, "Erro esperado ao processar requisição");
            return ex.ToObjectResult();
        }
        catch (Exception ex)
        {
            //Log.Error(ex, "Erro inesperado ao processar requisição");
            return StatusCode(500);
        }
    }

    protected async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> func)
    {
        try
        {
            return await func();
        }
        catch (ServiceException ex)
        {
            //Log.Debug(ex, "Erro esperado ao processar requisição");
            return ex.ToObjectResult();
        }
        catch (Exception ex)
        {
            //Log.Error(ex, "Erro inesperado ao processar requisição");
            return StatusCode(500);
        }
    }
}
