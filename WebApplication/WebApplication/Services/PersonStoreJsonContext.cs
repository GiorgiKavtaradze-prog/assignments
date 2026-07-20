using System.Text.Json.Serialization;
using WebApplication.Models;

namespace WebApplication.Services;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(List<Person>))]
internal sealed partial class PersonStoreJsonContext : JsonSerializerContext;
