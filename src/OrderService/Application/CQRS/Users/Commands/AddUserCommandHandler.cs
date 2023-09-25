using Application.DTO;
using Application.Helpers;
using AutoMapper;
using Domain.DomainEvents.Users;
using Domain.Entities;
using Infrastructure.Database.Interface;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Users.Commands;

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
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user));
        var result = await _userManager.CreateAsync(user, userRequest.Password);
        // await DBContext.SaveChangeAsync(cancellationToken);
        if (!result.Succeeded)
            return new()
            {
                Success = false,
                ErrorMsg = result.Errors.First().Description
            };
        return new()
        {
            Success = true,
            Data = Mapper.Map<User, UserDTO>(user)
        };
    }
}