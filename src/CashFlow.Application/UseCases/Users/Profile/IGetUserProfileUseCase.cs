﻿using CashFlow.Commnication.Responses;

namespace CashFlow.Application.UseCases.Users.Profile;
public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
