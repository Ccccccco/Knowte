using Digimezzo.Foundation.Core.IO;
using Digimezzo.Foundation.Core.Logging;
using System;

namespace Knowte.Core.Base
{
    public static class SafeActions
    {
        public static void TryOpenLink(string link)
        {
            try
            {
                Actions.TryOpenLink(link);
            }
            catch (Exception ex)
            {
                LogClient.Error("Could not open the link {0}. Exception: {1}", link, ex.Message);
            }
        }
    }
}
