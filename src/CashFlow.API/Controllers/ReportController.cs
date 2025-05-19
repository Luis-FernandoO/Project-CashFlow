using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Commnication.Enums;
using CashFlow.Commnication.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ADMIN)]
public class ReportController : ControllerBase
{
    [HttpGet("Excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromServices] IGenerateExpenseReportExcelUseCase useCase,[FromHeader] DateOnly month)
    {
        byte[] files = await useCase.Execute(month);

        if(files.Length > 0)
            return File(files, MediaTypeNames.Application.Octet,"report.xlsx");
        
        return NoContent();
    }

    [HttpGet("PDF")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf([FromServices] IGenerateExpensesReportPdfUseCase useCase, [FromHeader] DateOnly month)
    {
        byte[] files = await useCase.Execute(month);
        if (files.Length > 0)
            return File(files, MediaTypeNames.Application.Pdf, "report.pdf");
        return NoContent();
    }

}
