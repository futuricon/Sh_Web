using System;

namespace Sh.Domain.Entities.Base
{
    public static class GeneratorId
    {
        public static string GenerateLong()
        {
            return $"{DateTime.Now:yyyyMMddHHmmssffffff}";
        }

        public static string GenerateComplex()
        {
            return Guid.NewGuid().ToString().ToLower().Replace("-", "");
        }
    }
}
