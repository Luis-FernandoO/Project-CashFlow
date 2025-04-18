﻿using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.IdentityModel.Logging;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private readonly IExpensesReadOnlyRepository _repository;
    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);
        if (expenses.Count == 0)
        {
            return [];
        }
        var document = CreateDocument(month);
        var page = CreatePage(document);
        var totalExpenses = expenses.Sum(x => x.Amount);
        
        CreateHeaderWithProfilePhotoAndName(page);
        CreateTotalSpentSection(page, month, totalExpenses);
        foreach (var item in expenses)
        {
            var table = CreateExpenseTable(page);
            var row = table.AddRow();
            row.Height = 25;
            
            row.Cells[0].AddParagraph(item.Title);
            row.Cells[0].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorHelper.BLACK};
            row.Cells[0].Shading.Color = ColorHelper.RED_LIGHT;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].MergeRight = 2;
            row.Cells[0].Format.LeftIndent = 10;

            row.Cells[3].AddParagraph(ResourceReportGenerationMessages.AMOUNT);
            row.Cells[3].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorHelper.WHITE };
            row.Cells[3].Shading.Color = ColorHelper.RED_DARK;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;


            row = table.AddRow();
            row.Height = 25;
            row.Cells[0].AddParagraph(item.DateExpense.ToString("D"));
            row.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorHelper.BLACK };
            row.Cells[0].Shading.Color = ColorHelper.GREEN_DARK;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].Format.LeftIndent = 10;

            row.Cells[1].AddParagraph(item.DateExpense.ToString("t"));
            row.Cells[1].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorHelper.BLACK };
            row.Cells[1].Shading.Color = ColorHelper.GREEN_DARK;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[3].AddParagraph($"{CURRENCY_SYMBOL} -{item.Amount}");
            row.Cells[3].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorHelper.BLACK };
            row.Cells[3].Shading.Color = ColorHelper.WHITE;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;


            //ESPAÇAMENTO ENTRE LINHAS DA TABELA
            row = table.AddRow();
            row.Height = 30;
            row.Borders.Visible = false;

        }
        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSE_FOR} {month:Y}";
        document.Info.Author = "Luís Fernando";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;
        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;

    }

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");
        

        var row = table.AddRow();
        var assembly = Assembly.GetExecutingAssembly();

        var directoryName = Path.GetDirectoryName(assembly.Location);

        var pathFile = Path.Combine(directoryName!, "Images", "img_eagle.png");

        row.Cells[0].AddImage(pathFile);
        row.Cells[1].AddParagraph("Olá, Luís Fernando");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }
    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });

        paragraph.AddLineBreak(); //QUEBRA DE LINHA ENTRE UM PARAGRAFO E OUTRO

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }
    private byte[] RenderDocument(Document document)
    {
        var render = new PdfDocumentRenderer
        {
            Document = document,
        };
        render.RenderDocument();
        using var file = new MemoryStream();
        render.PdfDocument.Save(file);
        return file.ToArray();
    }
}
