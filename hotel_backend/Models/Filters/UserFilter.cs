namespace hotel_backend.Models.Filters;

public class UserFilter
{
    private UserFilter(string? partOfName, string? partOfEmail, Guid? role)
    {
        PartOfName = partOfName;
        PartOfEmail = partOfEmail;
        Role = role;
    }

    public string? PartOfName { get; }
    public string? PartOfEmail { get; }
    public Guid? Role { get; }

    private static string BasicChecks()
    {
        var error = string.Empty;

        return error;
    }

    public static (UserFilter UserFilter, string Error) Create(string? partOfName, string? partOfEmail, Guid? role)
    {
        // if (partOfName == string.Empty)
        //     partOfName = null;
        //
        // if (partOfEmail == string.Empty)
        //     partOfEmail = null;
        //
        // if (role == string.Empty)
        //     role = null;

        var error = BasicChecks();

        var userFilter = new UserFilter(partOfName, partOfEmail, role);

        return (userFilter, error);
    }
}