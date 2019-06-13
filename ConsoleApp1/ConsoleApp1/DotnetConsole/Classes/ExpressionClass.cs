using System;
using System.Linq;
using System.Linq.Expressions;

namespace DotnetConsole.Classes
{
  public class ExpressionClass
  {
    public static void ExpressionTreeBasic()
    {
      Func<string, string, string> func = (str1, str2) => string.Concat(str1, str2);
      string str = func("Bijay", " Koirala");
      Console.WriteLine(str);

      Expression<Func<string, string, string>> expression = (str1, str2) => string.Concat(str1, str2);
      var del = expression.Compile();
      Console.WriteLine(expression.Body);

      var result = del("3", "3");
      Console.WriteLine(result);
    }

    public static MethodCallExpression GetWhereExpression<T>()
    {
      // The IQueryable data to query.  
      IQueryable<T> queryableData = Enumerable.Empty<T>().AsQueryable();

      // Compose the expression tree that represents the parameter to the predicate.  
      ParameterExpression pe = Expression.Parameter(typeof(T), "t");

      // ***** Where(company => (company.ToLower() == "coho winery" || company.Length > 16)) *****  
      // Create an expression tree that represents the expression 'company.ToLower() == "coho winery"'.  
      Expression left = Expression.Property(pe, "ID");
      Expression right = Expression.Constant(1);
      Expression e1 = Expression.Equal(left, right);

      // Create an expression tree that represents the expression 'company.Length > 16'.  
      left = Expression.Property(pe, "Name");
      right = Expression.Constant("Bijay");
      Expression e2 = Expression.Equal(left, right);

      // Combine the expression trees to create an expression tree that represents the  
      // expression '(company.ToLower() == "coho winery" || company.Length > 16)'.  
      Expression predicateBody = Expression.OrElse(e1, e2);

      // Create an expression tree that represents the expression  
      // 'queryableData.Where(company => (company.ToLower() == "coho winery" || company.Length > 16))'  
      MethodCallExpression whereCallExpression = Expression.Call(
          typeof(Queryable),
          "Where",
          new Type[] { queryableData.ElementType },
          queryableData.Expression,
          Expression.Lambda<Func<T, bool>>(predicateBody, new ParameterExpression[] { pe }));
      return whereCallExpression;
    }

    public static void GetExpression()
    {
      ParameterExpression argParam = Expression.Parameter(typeof(Student), "s");
      Expression nameProperty = Expression.Property(argParam, "ID");
      Expression namespaceProperty = Expression.Property(argParam, "Name");

      var val1 = Expression.Constant(2);
      var val2 = Expression.Constant("Namespace");

      Expression e1 = Expression.Equal(nameProperty, val1);
      Expression e2 = Expression.Equal(namespaceProperty, val2);
      var andExp = Expression.AndAlso(e1, e2);

      var lambda = Expression.Lambda<Func<Student, bool>>(andExp, argParam);
    }

    public static void ExpressionMethod()
    {
      // Add a using directive for System.Linq.Expressions.  

      string[] companies = { "Consolidated Messenger", "Alpine Ski House", "Southridge Video", "City Power & Light",
                   "Coho Winery", "Wide World Importers", "Graphic Design Institute", "Adventure Works",
                   "Humongous Insurance", "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
                   "Blue Yonder Airlines", "Trey Research", "The Phone Company",
                   "Wingtip Toys", "Lucerne Publishing", "Fourth Coffee" };

      // The IQueryable data to query.  
      IQueryable<String> queryableData = companies.AsQueryable<string>();

      // Compose the expression tree that represents the parameter to the predicate.  
      ParameterExpression pe = Expression.Parameter(typeof(string), "company");

      // ***** Where(company => (company.ToLower() == "coho winery" || company.Length > 16)) *****  
      // Create an expression tree that represents the expression 'company.ToLower() == "coho winery"'.  
      Expression left = Expression.Call(pe, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
      Expression right = Expression.Constant("coho winery");
      Expression e1 = Expression.Equal(left, right);

      // Create an expression tree that represents the expression 'company.Length > 16'.  
      left = Expression.Property(pe, typeof(string).GetProperty("Length"));
      right = Expression.Constant(16, typeof(int));
      Expression e2 = Expression.GreaterThan(left, right);

      // Combine the expression trees to create an expression tree that represents the  
      // expression '(company.ToLower() == "coho winery" || company.Length > 16)'.  
      Expression predicateBody = Expression.OrElse(e1, e2);

      // Create an expression tree that represents the expression  
      // 'queryableData.Where(company => (company.ToLower() == "coho winery" || company.Length > 16))'  
      MethodCallExpression whereCallExpression = Expression.Call(
          typeof(Queryable),
          "Where",
          new Type[] { queryableData.ElementType },
          queryableData.Expression,
          Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { pe }));
      // ***** End Where *****  

      // ***** OrderBy(company => company) *****  
      // Create an expression tree that represents the expression  
      // 'whereCallExpression.OrderBy(company => company)'  
      MethodCallExpression orderByCallExpression = Expression.Call(
          typeof(Queryable),
          "OrderBy",
          new Type[] { queryableData.ElementType, queryableData.ElementType },
          whereCallExpression,
          Expression.Lambda<Func<string, string>>(pe, new ParameterExpression[] { pe }));
      // ***** End OrderBy *****  

      // Create an executable query from the expression tree.  
      IQueryable<string> results = queryableData.Provider.CreateQuery<string>(orderByCallExpression);

      // Enumerate the results.  
      foreach (string company in results)
        Console.WriteLine(company);

      /*  This code produces the following output:  

          Blue Yonder Airlines  
          City Power & Light  
          Coho Winery  
          Consolidated Messenger  
          Graphic Design Institute  
          Humongous Insurance  
          Lucerne Publishing  
          Northwind Traders  
          The Phone Company  
          Wide World Importers  
      */
    }
  }
}
