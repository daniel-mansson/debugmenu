using Microsoft.AspNetCore.Mvc;

namespace DebugMenu.Silo; 

public static class VersionEndpoints {
    public static Task<IResult> MapVersionEndpoints([FromQuery] int value) {
        return Task.FromResult(Results.Ok($"asdf {value}"));
    }
}