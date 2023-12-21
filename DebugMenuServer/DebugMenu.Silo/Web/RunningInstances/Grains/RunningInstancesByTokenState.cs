namespace DebugMenu.Silo.Web.RunningInstances.Grains;

public class RunningInstancesByTokenState {
    public List<IRunningInstanceGrain> ActiveInstances { get; set; } = new();

}