using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetConsole
{
  public class Repository<T>: IDisposable where T: class
  {
    private PracticeContext context =  null;

    public DbSet<T> Entities => this.context.Set<T>();
    public IQueryable<T> Table => this.Entities;

    public Repository(PracticeContext context)
    {
      this.context = context;
    }

    public T Insert(T entity)
    {
      try
      {
        T t = this.Entities.Add(entity);
        this.context.SaveChanges();
        return t;
      }
      catch (DbEntityValidationException dbEx)
      {
        var msg = string.Empty;

        foreach (var validationErrors in dbEx.EntityValidationErrors)
        {
          foreach (var validationError in validationErrors.ValidationErrors)
          {
            msg += string.Format("Property: {0} Error: {1}{2}", validationError.PropertyName, validationError.ErrorMessage, Environment.NewLine);
          }
        }

        var fail = new Exception(msg, dbEx);
        //Debug.WriteLine(fail.Message, fail);
        throw fail;
      }
    }

    public List<T> InsertList(List<T> entites)
    {
      try
      {
        List<T> t = this.Entities.AddRange(entites).ToList();
        this.context.SaveChanges();
        return t;
      }
      catch (DbEntityValidationException dbEx)
      {
        var msg = string.Empty;

        foreach (var validationErrors in dbEx.EntityValidationErrors)
        {
          foreach (var validationError in validationErrors.ValidationErrors)
          {
            msg += string.Format("Property: {0} Error: {1}{2}", validationError.PropertyName, validationError.ErrorMessage, Environment.NewLine);
          }
        }

        var fail = new Exception(msg, dbEx);
        //Debug.WriteLine(fail.Message, fail);
        throw fail;
      }
    }

    public async Task<T> InsertAsync(T entity)
    {
      try
      {
        T t = this.Entities.Add(entity);
        await this.context.SaveChangesAsync();
        return t;
      }
      catch (DbEntityValidationException dbEx)
      {
        var msg = string.Empty;

        foreach (var validationErrors in dbEx.EntityValidationErrors)
        {
          foreach (var validationError in validationErrors.ValidationErrors)
          {
            msg += string.Format("Property: {0} Error: {1}{2}", validationError.PropertyName, validationError.ErrorMessage, Environment.NewLine);
          }
        }

        var fail = new Exception(msg, dbEx);
        //Debug.WriteLine(fail.Message, fail);
        throw fail;
      }
    }

    public async Task<List<T>> InsertListAsync(List<T> entites)
    {
      try
      {
        List<T> t = this.Entities.AddRange(entites).ToList();
        await this.context.SaveChangesAsync();
        return t;
      }
      catch (DbEntityValidationException dbEx)
      {
        var msg = string.Empty;

        foreach (var validationErrors in dbEx.EntityValidationErrors)
        {
          foreach (var validationError in validationErrors.ValidationErrors)
          {
            msg += string.Format("Property: {0} Error: {1}{2}", validationError.PropertyName, validationError.ErrorMessage, Environment.NewLine);
          }
        }

        var fail = new Exception(msg, dbEx);
        //Debug.WriteLine(fail.Message, fail);
        throw fail;
      }
    }

    public void Dispose()
    {
      GC.SuppressFinalize(this);
    }
  }
}
