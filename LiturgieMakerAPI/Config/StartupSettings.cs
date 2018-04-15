using System.Collections.Generic;

namespace LiturgieMakerAPI.Config
{
    public class StartupSettings
    {
        public string[] CorsOrigins { get; set; }
        public bool UseSwagger { get; set; }
        public bool UseReverseProxyServer { get; set; }
    }
}