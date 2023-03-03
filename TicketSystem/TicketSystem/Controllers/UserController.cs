using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketSystem.API.ViewModels.Users;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;

namespace TicketSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper)
    {
        _mapper = mapper;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<UserViewModel>> Get(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get all users");
        var usersModel = await _userService.GetUsers(cancellationToken);

        return _mapper.Map<IEnumerable<UserViewModel>>(usersModel);
    }

    [HttpGet("{id}")]
    public async Task<UserViewModel> Get(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get user by id {id}", id);

        var userModel = await _userService.GetUserById(id, cancellationToken);

        return _mapper.Map<UserViewModel>(userModel);
    }

    [HttpPost]
    public async Task<UserViewModel> Post(ShortUserViewModel shortUser, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{JsonConvert.SerializeObject(user)}", JsonConvert.SerializeObject(shortUser));

        var userModel = _mapper.Map<User>(shortUser);
        userModel = await _userService.AddUser(userModel, cancellationToken);

        return _mapper.Map<UserViewModel>(userModel);
    }

    [HttpPut("{id}")]
    public async Task<UserViewModel> Put(int id, ShortUserViewModel shortUser, CancellationToken cancellationToken)
    {
        var userModel = _mapper.Map<User>(shortUser);
        userModel.Id = id;

        _logger.LogInformation("{JsonConvert.SerializeObject(user)}", JsonConvert.SerializeObject(userModel));

        userModel = await _userService.UpdateUser(userModel, cancellationToken);

        return _mapper.Map<UserViewModel>(userModel);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete user by id {id}", id);
        await _userService.DeleteUser(id, cancellationToken);
    }
}