using Application.DTO;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database.Interface;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Command.Users;

public record AddUserCommand(AddUserRequest User) : BaseRequest<AddUserResponse>;

internal sealed class AddUserCommandHandler: BaseRequestHandler<AddUserCommand, AddUserResponse>
{
    private readonly UserManager<User> _userManager;
    public AddUserCommandHandler(IMapper mapper, IAppDbContext dbContext, IAppDbContext appDbContext,
        UserManager<User> userManager) : base(mapper, dbContext)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public override async Task<AddUserResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
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
            Success = true,
            Data = Mapper.Map<User, UserDTO>(user)
        };
    }
}