namespace DotnetConsole
{
  public class Teacher
  {
    public int ID { get; set; }
    public string Name { get; set; }
    public decimal Salary { get; set; }
    public string Email { get; set; }
  }

  public class Student
  {
    public int ID { get; set; }
    public string Name { get; set; }
  }

  public class Branch
  {
    public int ID { get; set; }
    public string Name { get; set; }
  }
}