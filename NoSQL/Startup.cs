using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NoSQL.Startup))]
namespace NoSQL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
