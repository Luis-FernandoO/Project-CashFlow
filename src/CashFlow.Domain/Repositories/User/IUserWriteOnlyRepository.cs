﻿
namespace CashFlow.Domain.Repositories;
public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user);
    Task Delete(Entities.User user);
}
