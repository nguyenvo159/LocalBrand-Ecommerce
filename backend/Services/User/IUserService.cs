using backend.Dto.User;

namespace backend.Services;

public interface IUserService
{
    Task<string> Register(UserRegisterDto userRegisterDto);
    Task<string> Login(UserLoginDto userLoginDto);
}
