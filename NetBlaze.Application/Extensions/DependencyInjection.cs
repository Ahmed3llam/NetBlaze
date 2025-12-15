using Fido2NetLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Services;

namespace NetBlaze.Application.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISampleService, SampleService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAttendService, AttendService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVacationService, VacationService>();
            builder.Services.AddScoped<IPolicyService, PolicyService>();
            builder.Services.AddScoped<IRandomCheckService, RandomCheckService>();
            builder.Services.Configure<Fido2Configuration>(
                builder.Configuration.GetSection("Fido2"));
                builder.Services.AddSingleton<Fido2>(sp =>
                {
                    var config = sp.GetRequiredService<IOptions<Fido2Configuration>>().Value;
                    return new Fido2(config);
                });
        }
    }
}