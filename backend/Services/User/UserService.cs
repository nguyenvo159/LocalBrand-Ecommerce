using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using backend.Dto.User;
using backend.Entity;
using backend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository,
            IConfiguration configuration,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }



    public async Task<string> Login(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
        if (user == null || !await _userRepository.CheckPasswordAsync(user, userLoginDto.Password))
        {
            throw new ApplicationException("Invalid email or password.");
        }

        return GenerateToken(user);
    }

    public async Task<string> Register(UserRegisterDto userRegisterDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(userRegisterDto.Email);
        if (existingUser != null)
        {
            throw new ApplicationException("User already exists");
        }

        var user = _mapper.Map<User>(userRegisterDto);
        user.Role = "User";
        user.PasswordHash = _passwordHasher.HashPassword(user, userRegisterDto.Password);

        await _userRepository.CreateUserAsync(user);

        return GenerateToken(user);
    }

    public async Task<UserDto> GetById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new ApplicationException("User not found");
        }
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new ApplicationException("User not found");
        }
        return _mapper.Map<UserDto>(user);
    }

    private string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<UserDto> Update(UserUpdateDto userUpdateDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(userUpdateDto.Id);
        if (existingUser == null)
        {
            throw new ApplicationException("User not found");
        }

        _mapper.Map(userUpdateDto, existingUser);

        if (!string.IsNullOrEmpty(userUpdateDto.Password))
        {
            existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, userUpdateDto.Password);
        }
        if (!string.IsNullOrEmpty(userUpdateDto.Role))
        {
            existingUser.Role = userUpdateDto.Role;
        }

        await _userRepository.UpdateUserAsync(existingUser);
        return _mapper.Map<UserDto>(existingUser);
    }
    public async Task<bool> Delete(Guid id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            throw new ApplicationException("User not found");
        }
        return await _userRepository.DeleteUserAsync(id);
    }


}
