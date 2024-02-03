
using TaskManager.Models;
namespace TaskManager.Helpers;
public class EnumHelper
{
    public static StatusOfTask EnumParse(string value, StatusOfTask defaultStatus)
    {
        if (!Enum.TryParse(value, out StatusOfTask taskStatus))
        {
            return defaultStatus;
        }
        return taskStatus;
    }
}
