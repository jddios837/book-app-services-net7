using System.Text;

namespace SynchronizingResourceAccess;

static class SharedObjects
{
    public static StringBuilder Message = new StringBuilder();
    public static object Conch = new();
    public static int Counter;
}