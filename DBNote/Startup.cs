using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBNote.Startup))]
namespace DBNote
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
