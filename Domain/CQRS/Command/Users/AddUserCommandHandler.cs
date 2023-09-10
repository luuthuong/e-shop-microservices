using AutoMapper;
using Domain.Core;
using Domain.Database.Interface;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.CQRS.Command.Users;

public record AddUserCommand(AddUserRequest User) : IRequest<AddUserResponse>;

internal sealed class AddUserCommandHandler: IRequestHandler<AddUserCommand, AddUserResponse>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public AddUserCommandHandler(IAppDbContext appDbContext, IMapper mapper, UserManager<User> userManager)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<AddUserResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var userRequest = request.User;
        var isExisted = await _userManager.FindByNameAsync(userRequest.UserName);
        if (isExisted is not null)
            throw new Exception($"{userRequest.UserName} has been created.");
        if (!string.IsNullOrEmpty(userRequest.Email))
        {
            var isExistedByEmail = await _userManager.FindByEmailAsync(userRequest.Email);
            if (isExistedByEmail is not null)
                throw new Exception($"{userRequest.Email} has been created.");
        }
        var user = User.Create(userRequest.UserName, userRequest.Email, userRequest.Password, userRequest.DisplayName);
        var result = await _userManager.CreateAsync(user, userRequest.Password);
        return new AddUserResponse()
        {
            Data = _mapper.Map<User, UserDTO>(user),
            Success = result.Succeeded,
            ErrorMsg = string.Join('|', result.Errors.Select(err => err.Description))
        };
    }
}