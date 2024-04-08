using MediatR;

namespace DebugMenu.Silo.Web.Startup.Requests.ProcessStartupEvents;

public class ProcessStartupEventsRequest : IRequest<StartupEventsDto> {
    public string UserId { get; set; }
}
