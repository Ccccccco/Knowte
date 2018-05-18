using System;
using System.Collections.Generic;
using System.Linq;

namespace Knowte.Data.Utils
{
    public static class QueryUtils
    {
        public static string GetInClause(IList<string> items)
        {
            return String.Join(",", items.Select(x => $"'{x}'").ToArray());
        }
    }
}
