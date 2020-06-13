using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Dtos;

namespace Api.Domain.Interfaces.Services.User
{
    public interface IUserService
    {
         Task<UserDto> Get (Guid id);
         Task<UserDtoCreateResult> Post (UserDtoCreate user);
         Task<UserDtoUpdateResult> Put (UserDtoUpdate user);
         Task<IEnumerable<UserDto>> GetAll();
         Task<bool> Delete (Guid id);
    }
}
