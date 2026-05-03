using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Wsm.Aplication.DTOs.Common;

namespace net_maui_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected Guid ObterUsuarioId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }


    protected IActionResult Success<T>(T data, string message = "Operação realizada com sucesso")
    {
        var response = ApiResponse<T>.Success(data, message);
        return Ok(response);
    }

    protected IActionResult Created<T>(T data, string message = "Recurso criado com sucesso")
    {
        var response = ApiResponse<T>.Success(data, message);
        return StatusCode(201, response);
    }

    protected IActionResult NoContent(string message = "Operação realizada com sucesso")
    {
        var response = ApiResponse<object>.Success(null, message);
        return Ok(response);
    }

    protected IActionResult NotFound(string message = "Recurso não encontrado")
    {
        var response = ApiResponse<object>.Failure(message, "O recurso solicitado não foi encontrado");
        return StatusCode(404, response);
    }

    protected IActionResult BadRequest(string message, string erro)
    {
        var response = ApiResponse<object>.Failure(message, erro);
        return StatusCode(400, response);
    }

    protected IActionResult BadRequest(string message, List<string> erros)
    {
        var response = ApiResponse<object>.Failure(message, erros);
        return StatusCode(400, response);
    }
}
