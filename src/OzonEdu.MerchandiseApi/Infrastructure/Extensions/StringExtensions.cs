using System.Linq;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchandiseApi.Infrastructure.Extensions
{
    internal static class StringExtensions
    {
        internal static string ToString(IHeaderDictionary headers)
        {
            const int headerNameSpace = -30;
            var headersString = headers
                .Select(h => $"\t{h.Key, headerNameSpace}{h.Value}");
            return string.Join("\n", headersString);    
        }
    }
}