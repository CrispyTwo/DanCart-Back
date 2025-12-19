using DanCart.DataAccess.Models;
using DanCart.DataAccess.Models.Utility;
using DanCart.Models.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DanCart.DataAccess.Extensions;

public static class IQueryableExtension
{
    public static async Task<IEnumerable<T>> GetPageAsync<T>(this IQueryable<T> query, Page page, 
        IEnumerable<(string Name, bool Desc)>? sortings = null)
    {
        return await query.ApplySorting(sortings)
            .Paginate(page)
            .ToListAsync();
    }

    public static async Task<IEnumerable<T>> GetPageAsync<T>(this IQueryable<T> query, Page page, 
        IEnumerable<(string Name, bool Desc)>? sortings, string? search) where T : IFullTextSearchable
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.ApplyFullTextSearch(search)
                .OrderByDescending(r => r.Rank)
                .Select(r => r.Entity);
        }
        else
        {
            query = query.ApplySorting(sortings);
        }

        query = query.Paginate(page);
        return await query.ToListAsync();
    }

    public async static Task<IEnumerable<T>> GetAllAsync<T>(this IQueryable<T> query)
    {
        return await query.ToListAsync();
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, IEnumerable<(string Name, bool Desc)>? sortings)
    {
        if (sortings != null && sortings.Any())
        {
            var parts = new List<string>();
            foreach (var (name, desc) in sortings)
            {
                if (string.IsNullOrWhiteSpace(name)) continue;
                parts.Add(name + (desc ? " DESC" : ""));
            }

            var sortString = parts.Count == 0 ? null : string.Join(", ", parts);
            if (!string.IsNullOrWhiteSpace(sortString))
                query = query.OrderBy(new() { RestrictOrderByToPropertyOrField = true }, sortString);
        }

        return query;
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, Page? page)
    {
        if (page == null) return query;
        return query.Skip((Math.Max(1, page.Value.Number) - 1) * page.Value.Size).Take(page.Value.Size);
    }

    public static IQueryable<FullTextResult<T>> ApplyFullTextSearch<T>(this IQueryable<T> query, string search, string language = "english") where T : IFullTextSearchable
    {
        return query.Where(x => x.SearchVector.Matches(EF.Functions.PhraseToTsQuery(language, search)))
            .Select(x => new FullTextResult<T>
            {
                Entity = x,
                Rank = x.SearchVector.Rank(EF.Functions.PhraseToTsQuery(language, search))
            });
    }
}
