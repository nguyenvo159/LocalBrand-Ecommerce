using backend.Dto.User;

namespace backend.Services;

public interface IUserService
{
    Task<string> Register(UserRegisterDto userRegisterDto);
    Task<string> Login(UserLoginDto userLoginDto);
    Task<UserDto> GetById(Guid id);
    Task<UserDto> GetByEmail(string email);
    Task<UserDto> Update(UserUpdateDto userUpdateDto);
    Task<bool> Delete(Guid id);
}
