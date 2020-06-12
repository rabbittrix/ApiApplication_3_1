using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Repository;
using Api.Domain.Interfaces.Services.User;
using Microsoft.IdentityModel.Tokens;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using Api.Domain.Entities;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace Api.Service.Services
{
  public class LoginService : ILoginService
  {
    private IUserRepository _repository;
    private SigningCredentials _signingCredentials;
    private TokenConfigurations _tokenConfigurations;
    private IConfiguration _configuration {get;}
    public LoginService(IUserRepository repository, 
                        SigningCredentials signingCredentials, 
                        TokenConfigurations tokenConfigurations,
                        IConfiguration configuration)
    {
      _repository = repository;
      _signingCredentials = signingCredentials;
      _tokenConfigurations = tokenConfigurations;
      _configuration = configuration;
    }

    public async Task<object> FindByLogin(LoginDto user)
    {
      var baseUser = new UserEntity();
      if (user != null && !string.IsNullOrWhiteSpace(user.Email))
      {
        baseUser = await _repository.FindByLogin(user.Email);
        if(baseUser == null){
          return new {
            authenticated = false,
            message = "Failed to authenticate."
          };
        }else{
          var identity = new ClaimsIdentity(
            new GenericIdentity(baseUser.Email),
            new[]
            {
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //jti id token
              new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
            }
          );
          DateTime createDate = DateTime.Now;
          DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

          var handler = new JwtSecurityToken();
        }
      }
      else
      {
        return null;
      }
    }
  }
}
