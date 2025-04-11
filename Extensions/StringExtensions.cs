using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Extensions
{
    public static class StringExtensions
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(this string key) =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }
}
