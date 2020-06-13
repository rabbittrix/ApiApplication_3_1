using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using Api.Domain.Entities;

namespace Api.Application.Controllers
{
    
    [ApiController]
    [Route ("api/[controller]")]
    public class UsersController : ControllerBase {

        private IUserService _service;
        public UsersController (IUserService service) {
            _service = service;
         }

        // GET api/users
        [Authorize("Bearer")]
        [HttpGet("")]
        public async Task<ActionResult> GetAll(){
            try
            {
                return Ok (await _service.GetAll());
            }
            catch (ArgumentException ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failed {ex.Message}"); 
            }
        }

        // GET api/users/5
        [Authorize("Bearer")]
        [HttpGet ("{id}", Name="GetWithId")]
        public async Task<ActionResult> Get(Guid id){
            try
            {
                return Ok (await _service.Get(id));
            }
            catch (ArgumentException ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failed {ex.Message}"); 
            }
        }

        // POST api/users
        [Authorize("Bearer")]
        [HttpPost]
        public async Task<ActionResult> Post ([FromBody] UserEntity user) {
             try
            {
                var result = await _service.Post(user);
                if(result != null){
                    return Created(new Uri(Url.Link("GetWithId", new {id = result.Id})), result);
                }else{
                    return BadRequest();
                }
            }
            catch (ArgumentException ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failed {ex.Message}"); 
            }
         }

        // PUT api/users/5
        [Authorize("Bearer")]
        [HttpPut]
        public async Task<ActionResult> Put ([FromBody] UserEntity user) {
            try
            {
                var result = await _service.Put(user);
                if(result != null){
                    return Ok(result);
                }else{
                    return BadRequest();
                }
            }
            catch (ArgumentException ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failed {ex.Message}"); 
            }
         }

        // DELETE api/users/5
        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id){
            try
            {
                return Ok (await _service.Delete(id));
            }
            catch (ArgumentException ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failed {ex.Message}"); 
            }
        }
    }
}
