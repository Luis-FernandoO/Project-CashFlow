using CashFlow.Application.UseCases.Expenses;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Commnication.Enums;
using CashFlow.Commnication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validator.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();    
        //Act
        var result = validator.Validate(request);

        //Assert    
        result.IsValid.Should().BeTrue();   
    }

    [Theory]
    [InlineData(null)]
    [InlineData("               ")]
    [InlineData("")]
    public void Error_Title_Empty(string title)
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = title ; 
        //Act
        var result = validator.Validate(request);
        //Assert    
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorsMessages.TITULO_OBRIGATORIO));
    }

    [Fact]
    public void Error_Date_Future()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);  
        //Act
        var result = validator.Validate(request);
        //Assert    
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorsMessages.DIVIDA_NAO_PODE_SER_DO_FUTURO));
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)100;

        //Act
        var result = validator.Validate(request);
        //Assert    
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorsMessages.TIPO_PAGAMENTO_INVALIDO));
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-12)]
    [InlineData(-8)]
    public void Error_Amount_Invalid(decimal amount)
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Amount = amount;

        //Act
        var result = validator.Validate(request);
        //Assert    
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorsMessages.VALOR_MAIOR_QUE_ZERO));
    }


}
