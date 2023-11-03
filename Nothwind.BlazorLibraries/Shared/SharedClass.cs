/* Shared classes can be referenced by both the Client and Server */
namespace Packt.Shared;

public static class NorthwindExtensionMethods
{
    public static string ConvertToBase64Jpeg(this byte[] picture)
    {
        return $"data:image/jpg;base64,{Convert.ToBase64String(picture)}";
    }
}