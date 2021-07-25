using Application.Service;
using Application.Settlement.Command;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SettlementApi
{
    public static class IocExtensions
    {
        public static void AddDenpendencies(this IServiceCollection services)
        {
            services.AddScoped<IBookingService, BookingService>();
            services.AddSingleton<IBookingStoreProvider, BookingStoreProvider>();
            services.AddMediatR(typeof(BookSettlementCommand).Assembly);
        }
    }
}
