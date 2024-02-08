namespace DebugMenu.Silo.Persistence;

public class EntityWithIntId : EntityWithId<int>{
}

public class EntityWithId<T> {
    public T Id { get; set; }
}
