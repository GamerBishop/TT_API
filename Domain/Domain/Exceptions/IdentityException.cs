using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Domain.Exceptions;

public class IdentityException(string message, IEnumerable<IdentityError> errors) : Exception(IdentityException.Transcript(message, errors))
{
    public static string Transcript(string message, IEnumerable<IdentityError> errors)
    {
        var sb = new StringBuilder();
        sb.AppendLine(message);
        sb.AppendLine("Identity Errors:");
        foreach (var error in errors)
        {
            sb.AppendLine($"Code: {error.Code}, Description: {error.Description}");
        }
        return sb.ToString();
    }
}
