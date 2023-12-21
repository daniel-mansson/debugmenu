using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.ValidateUserAccess; 

public class ValidateUserAccessRequest : IRequest<bool> {
    public string InstanceId { get; set; }
    public int UserId { get; set; }
}