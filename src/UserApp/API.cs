using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp
{
    public class API : ClientProxy
    {
        private static API Instance = null;

        public API(string appId, object options = null)
            : base(appId, options) {}

        public API(string appId, string token, object options = null)
            : base(appId, token, options) {}

        public API(object options)
            : base(options) {}

        public static API GetInstance(dynamic initialOptions = null)
        {
            if (Instance != null && initialOptions != null)
            {
                Instance = new API(initialOptions);
            }
            return Instance;
        }

        public static void SetGlobalOptions(dynamic options)
        {
            ClientOptions.GetGlobal().Set(options);
        }
    }
}
