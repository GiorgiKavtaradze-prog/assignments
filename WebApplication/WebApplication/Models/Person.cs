namespace WebApplication.Models;

public class Person
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string JobPosition { get; set; } = string.Empty;
    public double Salary { get; set; }
    public double WorkExperience { get; set; }
    public PersonAddress Address { get; set; } = new PersonAddress();
}
