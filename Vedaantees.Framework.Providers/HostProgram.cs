using Microsoft.AspNetCore.Hosting;

namespace Vedaantees.Framework.Providers
{
    public class HostProgram
    {
        public string Url { get; }

        public HostProgram(string url)
        {
            Url = url;
        }
        
        public void Main<T>() where T:class
        {            
            var host = new WebHostBuilder()
                            .UseKestrel()
                            .UseIISIntegration()
                            .UseStartup<T>();

            if (!string.IsNullOrEmpty(Url))
                host.UseUrls(Url);

            host.Build().Run();
        }
    }
}
