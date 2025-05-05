using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Web;

public static class EnumEx
{
    public static string? GetDisplayName(this Enum enumValue)
    {
        return enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()?
            .GetCustomAttribute<DisplayAttribute>()?
            .Name;
    }
    
}