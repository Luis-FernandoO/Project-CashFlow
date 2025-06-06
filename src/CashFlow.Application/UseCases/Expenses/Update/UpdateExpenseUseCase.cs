﻿using AutoMapper;
using CashFlow.Commnication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase 
{

    private readonly IMapper _mapper;
    private readonly IExpensesUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    public UpdateExpenseUseCase(IMapper mapper, IExpensesUpdateOnlyRepository repository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var expense = await _repository.GetById(loggedUser,id);
        
        if(expense is null)
        {
            throw new NotFoundException(ResourceErrorsMessages.DESPESA_NAO_ENCONTRADA);
        }

        _mapper.Map(request, expense);
        _repository.Update(expense);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
