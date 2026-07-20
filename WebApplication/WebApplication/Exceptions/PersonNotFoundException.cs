namespace WebApplication.Exceptions;

public sealed class PersonNotFoundException(int id)
    : Exception($"Person with id {id} was not found.")
{
    public int Id { get; } = id;
}
