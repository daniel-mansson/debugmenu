namespace DebugMenu.Silo.Web.RunningInstances.Grains;

public interface IRunningInstanceGrain : IGrainWithStringKey {
    Task<RunningInstanceDto> GetDetails();
    Task<RunningInstanceDto> StartInstance(StartInstanceRequestDto request);
    Task<bool> IsValid(string token);
    Task<RunningInstanceMetadata> GetMetadata();
}