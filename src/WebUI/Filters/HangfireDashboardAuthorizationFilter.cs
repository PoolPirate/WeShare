using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace WeShare.WebAPI.Filters;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context) 
        => true;
}
