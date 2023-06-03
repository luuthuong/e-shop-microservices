using AutoMapper;
using Domain.Database.Interface;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.CQRS.Command.Users;

public record AddUserCommand(AddUserRequest User) : IRequest<UserDTO>;

internal sealed class AddUserCommandHandler: IRequestHandler<AddUserCommand, UserDTO>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public AddUserCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var userRequest = request.User;
        var user = User.Create(userRequest.UserName, userRequest.Email, userRequest.Password, userRequest.DisplayName);
        var isExisted = await _appDbContext.User.AnyAsync(x => x.UserName == user.UserName || x.Email == user.Email, cancellationToken);
        if (isExisted)
            throw new Exception("existed user");
        _appDbContext.User.Add(user);
        await _appDbContext.SaveChangeAsync(cancellationToken);
        return _mapper.Map<User, UserDTO>(user);
    }
}