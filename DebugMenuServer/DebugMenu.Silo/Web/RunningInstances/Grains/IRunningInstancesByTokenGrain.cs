namespace DebugMenu.Silo.Web.RunningInstances.Grains;

public interface IRunningInstancesByTokenGrain : IGrainWithStringKey {
    Task Register(IRunningInstanceGrain instance);
    Task Unregister(IRunningInstanceGrain instance);
    Task<IReadOnlyList<RunningInstanceDto>> GetAllInstances();
}