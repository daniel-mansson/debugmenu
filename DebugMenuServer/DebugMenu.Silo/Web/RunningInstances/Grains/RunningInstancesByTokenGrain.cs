using Orleans.Providers;

namespace DebugMenu.Silo.Web.RunningInstances.Grains;

[StorageProvider(ProviderName = Constants.DefaultStore)]
public class RunningInstancesByTokenGrain : Grain<RunningInstancesByTokenState>, IRunningInstancesByTokenGrain {

    public override Task OnActivateAsync(CancellationToken cancellationToken) {
        RegisterTimer(OnValidateTimerCallback, this, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(30));
        return Task.CompletedTask;
    }
    
    public async Task Register(IRunningInstanceGrain instance) {
        if(!State.ActiveInstances.Contains(instance)) {
            State.ActiveInstances.Add(instance);
            await WriteStateAsync();
        }
    }

    public async Task Unregister(IRunningInstanceGrain instance) {
        if (State.ActiveInstances.Remove(instance)) {
            await WriteStateAsync();
        }
    }

    public async Task<IReadOnlyList<RunningInstanceDto>> GetAllInstances() {
        var tasks = State.ActiveInstances
            .Select(instance => instance.GetDetails())
            .ToList();

        await Task.WhenAll(tasks);

        return tasks
            .Select(t => t.Result)
            .ToList();
    }

    private Task OnValidateTimerCallback(object _) {
        return ValidateReferences();
    }

    private async Task ValidateReferences() {
        var work = State.ActiveInstances
            .Select(instance => (task: instance.IsValid(this.GetPrimaryKeyString()), instance))
            .ToList();

        await Task.WhenAll(work.Select(t => t.task));

        bool anyRemoved = false;
        foreach (var outcome in work) {
            if (outcome.task.Result != false) {
                continue;
            }

            if (State.ActiveInstances.Remove(outcome.instance)) {
                anyRemoved = true;
            }
        }

        if (anyRemoved) {
            await WriteStateAsync();
        }
    }
}