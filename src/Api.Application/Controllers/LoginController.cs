using System;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class LoginController: ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Login([FromBody] LoginDto loginDto, [FromServices] ILoginService service){
            try
            {
                var result = await service.FindByLogin(loginDto);
                if(result != null){
                    return Ok(result);
                }else{
                    return NotFound();
                }
            }
            catch (ArgumentException ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failed {ex.Message}"); 
            }
        }
    }
}
