using System.Reflection;
using System.Text;
using System.Web;
namespace DTARServer.Helpers;

public static class HttpRequestExtensions
{
    //create in a static class
    static public object GetPropertyValue(this object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
    }



    public static Uri GetCompleteUri(this HttpRequest request)
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port.GetValueOrDefault(80),
            Path = request.Path.ToString(),
            Query = request.QueryString.ToString()
        };
        return uriBuilder.Uri;
    }

    public static Uri GetServerUri(this HttpRequest request)
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port.GetValueOrDefault(80),
        };
        return uriBuilder.Uri;
    }

    public static Uri GetRefUri(this HttpRequest request)
    {
        var uriBuilder = new UriBuilder
        {
            //Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port.GetValueOrDefault(80),
        };
        return uriBuilder.Uri;
    }

}
public static partial class Extensions
{

    public static String RemoveExtension(this String str)
    {
        return str.Split('.')[0];
    }
    public static String RemoveVersion(this String str)
    {
        return str.Split('_')[0];
    }

    /// <summary>
    ///     Encodes a URL string.
    /// </summary>
    /// <param name="str">The text to encode.</param>
    /// <returns>An encoded string.</returns>
    public static String UrlEncode(this String str)
    {
        return HttpUtility.UrlEncode(str);
    }

    /// <summary>
    ///     Encodes a URL string using the specified encoding object.
    /// </summary>
    /// <param name="str">The text to encode.</param>
    /// <param name="e">The  object that specifies the encoding scheme.</param>
    /// <returns>An encoded string.</returns>
    public static String UrlEncode(this String str, Encoding e)
    {
        return HttpUtility.UrlEncode(str, e);
    }

    public static void ForEach<T>(this IEnumerable<T> source,Action<T> action)
    {
        foreach (T element in source) 
            action(element);
    }


    public static void CopyProperties<T>(this T source, T dest)
    {
        var plist = from prop in typeof(T).GetProperties() where prop.CanRead && prop.CanWrite select prop;

        foreach (PropertyInfo prop in plist)
        {
            var value = prop.GetValue(source, null);
            prop.SetValue(dest, value, null);
        }
    }

     public static void CopyPropertiesTo<T, U>(this T source, U dest)
    {
        var plistsource = from prop1 in typeof(T).GetProperties() where prop1.CanRead select prop1;
        var plistdest = from prop2 in typeof(U).GetProperties() where prop2.CanWrite select prop2;

        foreach (PropertyInfo destprop in plistdest)
        {
            var sourceprops = plistsource.Where((p) => p.Name == destprop.Name &&
              destprop.PropertyType.IsAssignableFrom(p.GetType()));
            foreach (PropertyInfo sourceprop in sourceprops)
            { // should only be one
                var value = sourceprop.GetValue(source, null);
                destprop.SetValue(dest, value, null);
            }
        }
    }
}