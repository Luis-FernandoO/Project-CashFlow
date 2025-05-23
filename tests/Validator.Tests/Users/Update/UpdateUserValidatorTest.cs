using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Xml.XPath;

namespace Validator.Tests.Users.Update;
public class UpdateUserValidatorTest
{
    [Fact]
    public void Success() 
    {
        
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();   
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name) 
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);   
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Nome não pode ser vazio"));
    }


    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email) 
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build(); 
        request.Email = email;
        var result = validator.Validate(request);   

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Email não pode ser vazio"));

    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "email.comInvalido";
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Email inválido"));
    }
}
