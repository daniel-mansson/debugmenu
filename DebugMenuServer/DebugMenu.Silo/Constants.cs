public static class Constants {
    public const string DefaultStore = "DefaultStore";
    public const string InMemoryStream = "wsstream";
    public const string WebSocketNamespace = "wsns";

#if DEBUG
    public const string TempDatabaseString =
        "Host=localhost;Database=debugmenu2;Username=postgres;Password=postgres;Include Error Detail=true;";
#else
    public const string TempDatabaseString =
        "User Id=postgres.zzafmbznfltbgtwxefmr;Password=d31784b370294f87b017506329b806b1;Server=aws-0-eu-central-1.pooler.supabase.com;Port=5432;Database=postgres;";
#endif
}
