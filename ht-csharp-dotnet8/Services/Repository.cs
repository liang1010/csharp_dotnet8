using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;

namespace ht_csharp_dotnet8.Services
{
    //public interface IRepository<T> where T : BaseEntity
    //{
    //    Task<T> GetByIdAsync(int id);
    //    Task<IEnumerable<T>> GetAllAsync();
    //    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
    //    //void AddAsync(T entity);
    //    void Add(T entity);
    //    void Edit(T entity);
    //    void AddRangeAsync(IEnumerable<T> entities);
    //    void Remove(int id);
    //    void RemoveRange(IEnumerable<T> entities);
    //    Task<PagedListingResponse<T>> GetPagedLisitng(PageListingRequest filter, Expression<Func<T, bool>> expression = null);
    //}

    //[ServiceDependencies]
    //public class Repository<T> : IRepository<T> where T : BaseEntity
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly DbSet<T> _entities;
    //    private readonly ILogger<IRepository<T>> _logger;
    //    public Repository(ILogger<IRepository<T>> logger, ApplicationDbContext context)
    //    {
    //        _logger = logger;
    //        _context = context ?? throw new ArgumentNullException(nameof(context));
    //        _entities = context.Set<T>();
    //    }

    //    public void Add(T entity)
    //    {
    //        entity.Status = Status.Active;
    //        entity.LastUpdatedTime = DateTime.Now;
    //        entity.LastUpdatedBy = "System";
    //        _entities.AddAsync(entity);
    //        _context.SaveChanges();

    //        Log.Warning($"Add : {JsonSerializer.Serialize(entity)}");
    //    }
    //    public void Edit(T entity)
    //    {
    //        T temp = GetByIdAsync(entity.Id).Result;
    //        entity.Status = Status.Active;
    //        entity.LastUpdatedTime = DateTime.Now;
    //        entity.LastUpdatedBy = "System";
    //        _entities.Update(entity);
    //        _context.SaveChanges();
    //        T temp2 = GetByIdAsync(entity.Id).Result;
    //        Log.Warning($"Edit From : {JsonSerializer.Serialize(temp)}, To : {JsonSerializer.Serialize(temp2)}");
    //    }

    //    public void AddRange(IEnumerable<T> entity)
    //    {
    //        foreach (var item in entity)
    //        {
    //            item.Status = Status.Active;
    //            item.LastUpdatedTime = DateTime.Now;
    //            item.LastUpdatedBy = "System";
    //        }
    //        _entities.AddRange(entity);
    //        _context.SaveChanges();
    //        Log.Warning($"Add Range : {JsonSerializer.Serialize(entity)}");
    //    }

    //    public async void AddRangeAsync(IEnumerable<T> entities)
    //    {
    //        await _entities.AddRangeAsync(entities);
    //        Log.Warning($"Add Range Async : {JsonSerializer.Serialize(entities)}");
    //    }
    //    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
    //    {
    //        return await _entities.Where(expression).ToListAsync();
    //    }
    //    public async Task<IEnumerable<T>> GetAllAsync()
    //    {
    //        return await _entities.ToListAsync();
    //    }
    //    public async Task<T> GetByIdAsync(int id)
    //    {
    //        return await _entities.Where(s => s.Id == id).AsNoTracking().FirstOrDefaultAsync();
    //    }
    //    public void Remove(int id)
    //    {
    //        var entity = GetByIdAsync(id).Result;
    //        _entities.Remove(entity);
    //        _context.SaveChanges();
    //        Log.Warning($"Remove : {JsonSerializer.Serialize(entity)}");
    //    }
    //    public void RemoveRange(IEnumerable<T> entities)
    //    {
    //        _entities.RemoveRange(entities);
    //        Log.Warning($"Remove Range : {JsonSerializer.Serialize(entities)}");
    //    }

    //    public async Task<PagedListingResponse<T>> GetPagedLisitng(PageListingRequest filter, Expression<Func<T, bool>> expression = null)
    //    {
    //        var validFilter = new PageListingRequest(filter.PageNumber, filter.PageSize);
    //        List<T> pagedData;
    //        int totalRecords;
    //        if (expression != null)
    //        {
    //            pagedData = await _entities
    //            .Where(expression)
    //           .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
    //           .Take(validFilter.PageSize)
    //           .ToListAsync();
    //            totalRecords = await _entities
    //                .Where(expression)
    //                .CountAsync();
    //        }
    //        else
    //        {
    //            pagedData = await _entities
    //           .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
    //           .Take(validFilter.PageSize)
    //           .ToListAsync();
    //            totalRecords = await _entities
    //                .CountAsync();

    //        }

    //        var respose = new PagedListingResponse<T>()
    //        {
    //            Data = pagedData,
    //            PageNumber = validFilter.PageNumber,
    //            PageSize = validFilter.PageSize,
    //        };
    //        var totalPages = (double)totalRecords / (double)validFilter.PageSize;
    //        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
    //        respose.TotalPages = roundedTotalPages;
    //        respose.TotalRecords = totalRecords;
    //        return respose;
    //    }
    //}
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> Find(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task EditAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task RemoveAsync(Guid id);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task<PagedListingResponse<T>> GetPagedListing(
    PageListingRequest filter,
    Expression<Func<T, bool>> expression = null,
    Func<IQueryable<T>, IQueryable<T>> include = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    }

    [ServiceDependencies]
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;
        private readonly ILogger<IRepository<T>> _logger;

        public Repository(ILogger<IRepository<T>> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            SetAuditFields(entity);
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            _logger.LogWarning($"Add : {JsonSerializer.Serialize(entity)}");
        }

        public async Task EditAsync(T entity)
        {
            var temp = await GetByIdAsync(entity.Id);
            entity.Status = Status.Active;
            entity.LastUpdatedTime = DateTime.Now;
            entity.LastUpdatedBy = "System";

            _entities.Update(entity);
            await _context.SaveChangesAsync();

            var temp2 = await GetByIdAsync(entity.Id);
            _logger.LogWarning($"Edit From : {JsonSerializer.Serialize(temp)}, To : {JsonSerializer.Serialize(temp2)}");
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                item.Status = Status.Active;
                item.LastUpdatedTime = DateTime.Now;
                item.LastUpdatedBy = "System";
            }

            await _entities.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            _logger.LogWarning($"Add Range Async : {JsonSerializer.Serialize(entities)}");
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            _entities.Remove(entity);
            await _context.SaveChangesAsync();

            _logger.LogWarning($"Remove : {JsonSerializer.Serialize(entity)}");
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
            await _context.SaveChangesAsync();

            _logger.LogWarning($"Remove Range : {JsonSerializer.Serialize(entities)}");
        }

        public async Task<List<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await _entities.Where(expression).AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _entities.Where(s => s.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        //public async Task<PagedListingResponse<T>> GetPagedLisitng(PageListingRequest filter, Expression<Func<T, bool>> expression = null)
        //{
        //    var validFilter = new PageListingRequest(filter.PageNumber, filter.PageSize);
        //    List<T> pagedData;
        //    int totalRecords;

        //    if (expression != null)
        //    {
        //        pagedData = await _entities.Where(expression)
        //            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        //            .Take(validFilter.PageSize)
        //            .ToListAsync();

        //        totalRecords = await _entities.Where(expression).CountAsync();
        //    }
        //    else
        //    {
        //        pagedData = await _entities
        //            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        //            .Take(validFilter.PageSize)
        //            .ToListAsync();

        //        totalRecords = await _entities.CountAsync();
        //    }

        //    return new PagedListingResponse<T>
        //    {
        //        Result = pagedData,
        //        PageNumber = validFilter.PageNumber,
        //        PageSize = validFilter.PageSize,
        //        TotalRecords = totalRecords,
        //        TotalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize)
        //    };
        //}

        public async Task<PagedListingResponse<T>> GetPagedListing(
    PageListingRequest filter,
    Expression<Func<T, bool>> expression = null,
    Func<IQueryable<T>, IQueryable<T>> include = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

            IQueryable<T> query = _entities;

            if (include != null)
                query = include(query);

            if (expression != null)
                query = query.Where(expression);

            // ✅ Ordering
            if (orderBy != null)
                query = orderBy(query);

            var totalRecords = await query.AsNoTracking().CountAsync();

            var pagedData = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedListingResponse<T>
            {
                Result = pagedData,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
            };
        }




        private void SetAuditFields(object entity, string user = "System")
        {
            if (entity is not BaseEntity baseEntity) return;

            var now = DateTime.Now;

            if (baseEntity.LastUpdatedTime == default)
            {
                baseEntity.LastUpdatedTime = now;
                baseEntity.LastUpdatedBy = user;
            }

            baseEntity.LastUpdatedTime = now;
            baseEntity.LastUpdatedBy = user;
            baseEntity.Status = Status.Active;

            var type = entity.GetType();
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                // 处理 IEnumerable<BaseEntity> 类型的集合属性
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                    prop.PropertyType.IsGenericType &&
                    typeof(BaseEntity).IsAssignableFrom(prop.PropertyType.GetGenericArguments()[0]))
                {
                    var children = prop.GetValue(entity) as System.Collections.IEnumerable;
                    if (children == null) continue;

                    foreach (var child in children)
                    {
                        SetAuditFields(child, user);
                    }
                }
            }
        }

    }

}
