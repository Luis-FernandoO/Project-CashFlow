﻿using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Exception;
using Microsoft.Extensions.Options;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{

    private readonly ILoggedUser _loggedUser;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUserReadOnlyRepository userReadOnlyRepository, 
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;   
        _userReadOnlyRepository = userReadOnlyRepository;

    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.Get();
        
        await Validate(request, loggedUser.Email);

        var user = await _repository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (currentEmail.Equals(request.Email) == false)
        {
            var userExist = await _userReadOnlyRepository.ExisteActiveUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new ValidationFailure(string.Empty, "Email ja está registrado"));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
