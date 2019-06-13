using DotnetConsole.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotnetConsole
{
  public class DataBaseCall
  {
    public static List<T> GetList<T>(Func<T, bool> func) where T: class
    {
      try
      {
        return Execute<T, List<T>>((repository) => {
          return repository.Table.Where(func).ToList();
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        return null;
      }
    }

    public static async Task<List<T>> GetListAsync<T>(Func<T, bool> func) where T : class
    {
      try
      {
        return await ExecuteAsync<T, List<T>>(async (repository) => {
          return await Task.Run(() => repository.Table.Where(func).ToList());
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        return null;
      }
    }

    public static async Task<IEnumerable<T>> GetListAsync<T>() where T : class
    {
      try
      {
        return await ExecuteAsync<T, IEnumerable<T>>(async (repository) => {
          return await repository.Table.ToListAsync();
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        return null;
      }
    }

    public static T Insert<T>(T value) where T : class
    {
      return Execute<T, T>((repository) => {
        return repository.Insert(value);
      });
    }

    public static async Task<T> InsertAsync<T>(T value) where T: class
    {
      try
      {
        return await ExecuteAsync<T, T>(async (repository) =>
        {
          return await repository.InsertAsync(value);
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        return null;
      }
    }

    public static async Task<List<T>> InsertListAsync<T>(List<T> list) where T: class
    {
      try
      {
        return await ExecuteAsync<T, List<T>>(async (repository) =>
        {
          return await repository.InsertListAsync(list);
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        return null;
      }
    }

    public static List<Student> GetStudentsByDynamicExpression<T, K>(string clause, Expression<Func<T, K>> expression)
    {
      try
      {
        return Execute<Student, List<Student>>((repository) => {
          IQueryable<Student> queryableData = repository.Table;
          MethodCallExpression callExpression = null;

          switch (clause)
          {
            case DbClause.Where:
              callExpression = Expression.Call(
                typeof(Queryable),
                DbClause.Where,
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                expression);
              break;

            case DbClause.OrderBy:
              break;

            case DbClause.WhereThenOrderBy:
              var whereCallExpression = Expression.Call(
                typeof(Queryable),
                DbClause.Where,
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                expression);

              ParameterExpression argParam = Expression.Parameter(typeof(Student), "s");
              Expression nameProperty = Expression.Property(argParam, "ID");

              var exp = Expression.Lambda<Func<Student, int>>(nameProperty, argParam);
              var type = queryableData.ElementType;

              callExpression = Expression.Call(
              typeof(Queryable),
              DbClause.OrderBy,
              new Type[] { queryableData.ElementType, typeof(int) },
              whereCallExpression,
              exp);
              break;

            case DbClause.FirstOrDefault:
              break;

            default:
              break;
          }
          IQueryable<Student> results = queryableData.Provider.CreateQuery<Student>(callExpression);
          return results.ToList();
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        return null;
      }
    }


    public static List<Student> GetStudentsByDynamicExpression()
    {
      try
      {
        return Execute<Student, List<Student>>((repository) => {
          IQueryable<Student> queryableData = repository.Table;

          ParameterExpression pe = Expression.Parameter(typeof(Student), "s");

          Expression left = Expression.Property(pe, "ID");
          Expression right = Expression.Constant(1);
          Expression e1 = Expression.Equal(left, right);
 
          left = Expression.Property(pe, "Name");
          right = Expression.Constant("Bijay");
          Expression e2 = Expression.Equal(left, right);

          Expression predicateBody = Expression.OrElse(e1, e2);

          MethodCallExpression whereCallExpression = Expression.Call(
              typeof(Queryable),
              "Where",
              new Type[] { queryableData.ElementType },
              queryableData.Expression,
              Expression.Lambda<Func<Student, bool>>(predicateBody, new ParameterExpression[] { pe }));

          IQueryable<Student> results = queryableData.Provider.CreateQuery<Student>(whereCallExpression);
          return results.ToList();
        });
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex);
        return null;
      }
    }

    public static List<Student> GetStudents()
    {
      return Execute<Student, List<Student>>((repository) =>
      {
        var students = repository.Table.OrderBy(s => s.Name).ToList();

        if (students != null && students.Count() > 0)
        {
          Console.WriteLine($"Student ID : Student Name");
          foreach (Student student in students)
          {
            Console.WriteLine($"{student.ID} : {student.Name}");
          }
        }

        return students;
      });
    }

    public static List<Student> GetStudentsOrderByAsParallel()
    {
      return Execute<Student, List<Student>>((repository) =>
      {
        var students = repository.Table.OrderBy(s => s.Name).AsParallel().ToList();

        if (students != null && students.Count() > 0)
        {
          Console.WriteLine($"Student ID : Student Name");
          foreach (Student student in students)
          {
            Console.WriteLine($"{student.ID} : {student.Name}");
          }
        }

        return students;
      });
    }

    public static List<Student> GetStudentsAsParallelOrderBy()
    {
      return Execute<Student, List<Student>>((repository) =>
      {
        var students = repository.Table.AsParallel().OrderBy(s => s.Name).ToList();

        if (students != null && students.Count() > 0)
        {
          Console.WriteLine($"Student ID : Student Name");
          foreach (Student student in students)
          {
            Console.WriteLine($"{student.ID} : {student.Name}");
          }
        }

        return students;
      });
    }

    public static Student SearchStudentByIEnumerable()
    {
      Console.WriteLine("Sure to Search Student? Yes or No");
      string yesOrNo = Console.ReadLine().ToLower();

      if (yesOrNo != "yes")
        return null;

      return Execute<Student, Student>((repository) =>
      {
        var students = repository.Table.AsEnumerable();
        Console.WriteLine("Student Id?");
        int id = Convert.ToInt32(Console.ReadLine());
        Student student = students.FirstOrDefault(s => s.ID == id);
        Console.WriteLine($"{student?.ID} : {student?.Name}");
        return student;
      });
    }

    public static Student SearchStudentByIQuerable()
    {
      Console.WriteLine("Sure to Search Student? Yes or No");
      string yesOrNo = Console.ReadLine().ToLower();

      if (yesOrNo != "yes")
        return null;

      return Execute<Student, Student>((repository) =>
      {
        var students = repository.Table.AsQueryable();
        Console.WriteLine("Student Id?");
        int id = Convert.ToInt32(Console.ReadLine());
        Student student = students.FirstOrDefault(s => s.ID == id);
        Console.WriteLine($"{student.ID} : {student.Name}");
        return student;
      });
    }

    public static Student SearchStudentByToList()
    {
      Console.WriteLine("Sure to Search Student? Yes or No");
      string yesOrNo = Console.ReadLine().ToLower();

      if (yesOrNo != "yes")
        return null;

      return Execute<Student, Student>((repository) =>
      {
        var students = repository.Table.ToList();
        Console.WriteLine("Student Id?");
        int id = Convert.ToInt32(Console.ReadLine());
        Student student = students.FirstOrDefault(s => s.ID == id);
        Console.WriteLine($"{student.ID} : {student.Name}");
        return student;
      });
    }

    public static List<Student> CreateStudent()
    {
      return Execute<Student, List<Student>>((repository) => {
        string yesOrNo = "No";
        Console.WriteLine("Sure to Create Student? Yes or No");
        yesOrNo = Console.ReadLine().ToLower();

        if (yesOrNo != "yes")
          return null;

        List<Student> students = new List<Student>();
        do
        {
          Console.WriteLine("Student Name?");
          students.Add(new Student
          {
            Name = Console.ReadLine()
          });

          Console.WriteLine("Add More Student? Yes or No");
          yesOrNo = Console.ReadLine().ToLower();
        }
        while (yesOrNo == "yes");

        var results = repository.InsertList(students);
        return results;
      });
    }

    private static void Execute<T>(Action<Repository<T>> action) where T: class
    {
      using (PracticeContext context = new PracticeContext())
      {
        try
        {
          action(new Repository<T>(context));
        }
        catch (SqlException ex)
        {
          Console.WriteLine(ex);
          throw new Exception("Error Occured.");
        }
      }
    }

    private static TResult Execute<T, TResult>(Func<Repository<T>, TResult> func) where T : class
    {
      using (PracticeContext context = new PracticeContext())
      {
        try
        {
          return func(new Repository<T>(context));
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
          throw new Exception("Error Occured.");
        }
      }
    }

    private async static Task<TResult> ExecuteAsync<T, TResult>(Func<Repository<T>, Task<TResult>> func) where T : class
    {
      using (PracticeContext context = new PracticeContext())
      {
        try
        {
          return await func(new Repository<T>(context));
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
          throw new Exception("Error Occured.");
        }
      }
    }
  }

  public class PracticeContext: DbContext
  {
    public PracticeContext(): base(){
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        modelBuilder.Configurations.AddFromAssembly(assembly);
      }
    }
  }
}
