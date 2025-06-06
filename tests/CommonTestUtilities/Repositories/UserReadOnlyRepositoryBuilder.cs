﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;
    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }
    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userRepository => userRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);
        return this;

    }
    public void ExistActiveUserWithEmail(string email)
    {

        _repository.Setup(userReadOnly => userReadOnly.ExisteActiveUserWithEmail(email)).ReturnsAsync(true);
    }
    public IUserReadOnlyRepository Build() => _repository.Object;
}
