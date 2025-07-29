using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpenseReadOnlyRepository, IExpenseWriteOnlyRepository,IExpenseUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context;
    public ExpensesRepository(CashFlowDbContext context)
    {
        _context = context;
    }
    public async Task Add(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
    }

    public async Task Delete(long id)
    {
        var result = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);

        _context.Expenses.Remove(result!);
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _context.Expenses.AsNoTracking().Where(e => e.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpenseReadOnlyRepository.GetById(User user,long id)
    {
        return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }
    async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && user.Id == e.UserId);
    }

    public void Update(long id, Expense expense)
    {
        var result = _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
        if (result is not null)
        {
            //_context.Expenses.Update(result);
        }
    }

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(date.Year,date.Month);

        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth,hour: 23,minute: 59,second: 59);
        return await _context.Expenses
            .AsNoTracking()
            .Where(e => e.UserId == user.Id && e.Date >= startDate && e.Date<=endDate)
            .OrderBy(e => e.Date)
            .ToListAsync();
    }
}
