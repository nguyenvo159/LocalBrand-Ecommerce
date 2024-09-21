using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using backend.Dto.User;
using backend.Dtos.User;
using backend.Entity;
using backend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IRepository<User> userRepository,
            IConfiguration configuration,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }


    public async Task<List<UserDto>> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
        if (users == null)
        {
            throw new ApplicationException("No users found");
        }
        return _mapper.Map<List<UserDto>>(users);
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
        var user = await _userRepository.FindAsync(u => u.Email == email);
        if (user == null)
        {
            throw new ApplicationException("User not found");
        }
        return _mapper.Map<UserDto>(user);
    }

    public async Task<string> Login(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.FindAsync(u => u.Email == userLoginDto.Email);

        if (user == null || !await CheckPasswordAsync(user, userLoginDto.Password))
        {
            throw new ApplicationException("Invalid email or password.");
        }

        return GenerateToken(user);
    }

    public Task<bool> CheckPasswordAsync(User user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        return Task.FromResult(result == PasswordVerificationResult.Success);
    }

    public async Task<string> Register(UserRegisterDto userRegisterDto)
    {
        var existingUser = await _userRepository.FindAsync(u => u.Email == userRegisterDto.Email);
        if (existingUser != null)
        {
            throw new ApplicationException("User already exists");
        }

        var user = _mapper.Map<User>(userRegisterDto);
        if (userRegisterDto.Role.IsNullOrEmpty())
        {
            user.Role = "User";
        }
        user.PasswordHash = _passwordHasher.HashPassword(user, userRegisterDto.Password);

        await _userRepository.AddAsync(user);

        return GenerateToken(user);
    }

    public async Task ChangePassword(Guid id, UserChangePassDto userChangePassDto)
    {
        var user = await _userRepository.FindAsync(u => u.Id == id);
        if (user == null)
        {
            throw new ApplicationException("User not found");
        }
        if (!await CheckPasswordAsync(user, userChangePassDto.OldPassword))
        {
            throw new ApplicationException("Invalid old password");
        }
        user.PasswordHash = _passwordHasher.HashPassword(user, userChangePassDto.NewPassword);

        await _userRepository.UpdateAsync(user);
    }

    public async Task<UserDto> Update(UserUpdateDto userUpdateDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(userUpdateDto.Id);
        if (existingUser == null)
        {
            throw new ApplicationException("User not found");
        }
        if (existingUser.Email != userUpdateDto.Email)
        {
            var existingEmail = await _userRepository.FindAllAsync(u => u.Email == userUpdateDto.Email);
            if (existingEmail.Count > 0)
            {
                throw new ApplicationException("Email already exists");
            }
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

        await _userRepository.UpdateAsync(existingUser);
        return _mapper.Map<UserDto>(existingUser);
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
        var secretKey = jwtSettings["SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ApplicationException("SecretKey is not configured in JwtSettings");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
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


    public async Task<bool> Delete(Guid id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            throw new ApplicationException("User not found");
        }
        return await _userRepository.DeleteAsync(id);
    }


}
