using backend.Dto.User;
using backend.Dtos.User;

namespace backend.Services;

public interface IUserService
{
    Task<string> Register(UserRegisterDto userRegisterDto);
    Task<string> Login(UserLoginDto userLoginDto);
    Task ChangePassword(Guid id, UserChangePassDto userChangePassDto);
    Task<List<UserDto>> GetAll();
    Task<UserDto> GetById(Guid id);
    Task<UserDto> GetByEmail(string email);
    Task<UserDto> Update(UserUpdateDto userUpdateDto);
    Task<bool> Delete(Guid id);
}
