using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Services.Extensions
{
    public static class StringExtensions
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(this string key) =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }
}
