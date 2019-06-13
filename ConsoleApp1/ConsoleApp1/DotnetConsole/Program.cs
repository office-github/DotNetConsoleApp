using DotnetConsole.Classes;
using DotnetConsole.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DotnetConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      //UsingRegexStringValidator.RegexStringValidatorMethod();
      //ActionDelegateMethod();
      DataBaseCallMethod();
      //ImplementIEnumerableCall();
      //VariousMethodCall();
      //ExtensionCall();
      //ExpressionTreeCall();
      //ParallelCheckCall();
      Console.ReadKey();
    }

    private static void ParallelCheckCall()
    {
      //ParallelClass.CheckPerformanceArray();
      ParallelClass.CheckPerformanceDbCall();
    }

    private static void ExpressionTreeCall()
    {
      //ExpressionClass.ExpressionTreeBasic();
      //ExpressionClass.ExpressionMethod();
      //ExpressionClass.GetExpression();
      //var expression = ExpressionClass.GetWhereExpression(new List<Student>().AsQueryable());
    }
    private static void ExtensionCall()
    {
      var list = new List<string> { "Nepal", "India" };
      var customList = list.ToDictionary();

      var toEnum = typeof(Student).ToEnum();

      var arrays = typeof(CustomEnum).ToArray();

      foreach(var item in arrays)
      {
        Console.WriteLine(item);
      }

      var toList = typeof(CustomEnum).ToList();

      foreach (var item in toList)
      {
        Console.WriteLine(item);
      }

      var dictionary = typeof(Program).ToDictionary<int, CustomEnum>();
      var dictionaryReverse = typeof(CustomEnum).ToDictionary<CustomEnum, int>();
    }

    private static void CustomEnumeratorCall()
    {
      Person person = new Person(1);
      Person person2 = new Person(2);
      Person[] people = new Person[] { person, person2 };
    }

    private static void ImplementIEnumerableCall()
    {
      var iterator = ImplementIEnumerable<string[]>.str.GetEnumerator();

      while (iterator.MoveNext())
      {
        Console.WriteLine(iterator.Current);

        while (iterator.MoveNext())
        {
          Console.WriteLine(iterator.Current);
          break;
        }
      }
    }

    private static void VariousMethodCall()
    {
      int a = 0;
      int b = 0;
      int c = 0;

      bool result = VariousMethod.DynamicParameter(a, b, c);
      Console.WriteLine(result);
    }

    private static async void DataBaseCallMethod()
    {
      try
      {
        Database.SetInitializer(new MigrateDatabaseToLatestVersion<PracticeContext, Migrations.Configuration>());
        //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PracticeContext>());
        //DataBaseCall.SearchStudentByIQuerable();
        //DataBaseCall.SearchStudentByIEnumerable();
        //DataBaseCall.SearchStudentByToList();
        //DataBaseCall.CreateStudent();
        //DataBaseCall.GetStudents();
        //DataBaseCall.Insert(new Student { Name = "Pratik Hero" });
        //var result = DataBaseCall.InsertAsync(new Student { Name = "Pratik Hero" });
        //Console.WriteLine(result.Result.Name);

        //DataBaseCall.Insert(new Branch { Name = "ME" });
        //DataBaseCall.Insert(new Teacher { Name = "Math Teacher", Salary = 1000, Email = "b@b.com" });
        //DataBaseCall.Insert(new Teacher { Name = "Nepali Teacher", Salary = 2000 });
        //Func<Teacher, bool> func = (teacher) => true;
        //DataBaseCall.GetList(func).ForEach(b => Console.WriteLine($"Teacher Id: {b.ID}, Teacher Name: {b.Name}"));
        //DataBaseCall.GetStudentsByDynamicExpression().ForEach(b => Console.WriteLine($"Student Id: {b.ID}, Student Name: {b.Name}"));
        //DataBaseCall.GetStudentsByDynamicExpression<Student, bool>(DbClause.WhereThenOrderBy, (student) => true).ForEach(b => Console.WriteLine($"Student Id: {b.ID}, Student Name: {b.Name}"));

        //var students = new List<Student>();

        //var results = await DataBaseCall.InsertListAsync(students);
        //Console.WriteLine();
        //Console.WriteLine();
        //DataBaseCall.GetStudents();
        var results = await DataBaseCall.GetListAsync<Student>();
        Console.WriteLine("Student Records.");

        foreach(var result in results)
        {
          Console.WriteLine($"ID: {result.ID}, Name: {result.Name}");
        }
      }
      catch(Exception ex)
      {
        Console.WriteLine("Error occured while performing student operation.", ex.ToString());
      }
    }

    private static void ActionDelegateMethod()
    {
      Console.WriteLine("Enter a:");
      int a = Convert.ToInt32(Console.ReadLine());

      Console.WriteLine("Enter b:");
      int b = Convert.ToInt32(Console.ReadLine());

      Console.WriteLine(ActionDelegate.Add(a, b));

      ActionDelegate.Add((x, y) =>
      {
        Console.WriteLine(x + y);
      });

      bool isSuccess = ActionDelegate.Execute<Object1, Object2, bool>((o1, o2) =>
      {
        if (o1.Get == o2.Get)
          return true;

        return false;
      });

      Console.WriteLine(isSuccess);
    }
  }
}
