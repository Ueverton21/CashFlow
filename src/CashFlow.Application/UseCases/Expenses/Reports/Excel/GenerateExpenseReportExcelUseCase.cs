using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private readonly IExpenseReadOnlyRepository _repository;
    public GenerateExpenseReportExcelUseCase(IExpenseReadOnlyRepository repository)
    {
        _repository = repository;
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);

        if (expenses.Count == 0) {
            return [];
        }

        using var workbook = new XLWorkbook();

        workbook.Author = "Ueverton Carmo";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var name = month.ToString("Y");

        var worksheet = workbook.Worksheets.Add(name);

        InsertHeader(worksheet);

        int i = 2;
        foreach (var expense in expenses) {
            worksheet.Cell($"A{i}").Value = expense.Title;
            worksheet.Cell($"B{i}").Value = expense.Date;
            worksheet.Cell($"C{i}").Value = expense.PaymentType.ConvertPaymentType();
            worksheet.Cell($"D{i}").Value = expense.Amount;
            worksheet.Cell($"D{i}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";
            worksheet.Cell($"E{i}").Value = expense.Description;
            i++;
        }
        worksheet.Columns().AdjustToContents();
        var file = new MemoryStream();

        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = "Título";
        worksheet.Cell("B1").Value = "Data";
        worksheet.Cell("C1").Value = "Tipo de pagamento";
        worksheet.Cell("D1").Value = "Preço";
        worksheet.Cell("E1").Value = "Descrição";

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#f5c2b6");

        worksheet.Cells("A1:C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cells("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    } 
}
