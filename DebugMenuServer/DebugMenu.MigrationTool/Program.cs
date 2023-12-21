using System.Diagnostics;
using System.Reflection;
using DebugMenu.Silo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DebugMenu.MigrationTool;

internal static class Program {
    private const string MigrationProject = "DebugMenu.Silo";
    private const string StartupProject = "DebugMenu.Silo";

    private static void Main(string[] _) {
        while (true) {
            Console.WriteLine("1. Add migration");
            Console.WriteLine("2. Remove migration");
            Console.WriteLine("3. Update database");
            Console.WriteLine();

            var id = GetSelection(1, 3);
            switch (id) {
                case 1:
                    HandleAddMigration();
                    break;
                case 2:
                    HandleRemoveMigration();
                    break;
                case 3:
                    HandleUpdateDatabase();
                    break;
                default:
                    return;
            }
        }
    }

    private static void HandleRemoveMigration() {
        var dbContext = SelectDbContext();
        var args = CreateRemoveMigrationArgs(dbContext.Name);

        ExecuteCommand("dotnet", args);
    }

    private static void HandleAddMigration() {
        var dbContext = SelectDbContext();

        Console.Write("Migration name: ");
        var migrationName = Console.ReadLine();
        if (string.IsNullOrEmpty(migrationName)) {
            throw new Exception();
        }

        var args = CreateAddMigrationArgs(dbContext.Name, migrationName);

        ExecuteCommand("dotnet", args);
    }

    private static void ExecuteCommand(string command, string args) {
        var startInfo = new ProcessStartInfo {
            FileName = command,
            Arguments = args,
            CreateNoWindow = true,
            UseShellExecute = false,
            WorkingDirectory = GetSolutionDir(),
            RedirectStandardOutput = true
        };
        Console.WriteLine($"Executing: {command} {args}");
        Console.WriteLine($"Working directory: {startInfo.WorkingDirectory}");
        Console.WriteLine();

        using var p = Process.Start(startInfo)!;
        p.OutputDataReceived += (_, e) => { Console.WriteLine(e.Data); };
        p.BeginOutputReadLine();
        p.WaitForExit();
    }

    private static string GetSolutionDir() {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (dir != null) {
            if (dir.GetFiles().Any(f => f.Extension == ".sln")) {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new Exception("No solution dir found");
    }

    private static string CreateUpdateDatabaseArgs(string dbContextName) {
        return
            $"ef database update --context {dbContextName} --project ./{MigrationProject} --verbose --startup-project ./{StartupProject}";
    }

    private static string CreateRemoveMigrationArgs(string dbContextName) {
        return
            $"ef migrations remove --context {dbContextName} --project ./{MigrationProject} --verbose --startup-project ./{StartupProject}";
    }

    private static string CreateAddMigrationArgs(string dbContextName, string migrationName) {
        return
            $"ef migrations add --context {dbContextName} --project ./{MigrationProject} --verbose --startup-project ./{StartupProject} {dbContextName}_{migrationName}";
    }

    private static Type SelectDbContext() {
        var migrationAssembly = typeof(DebugMenuDbContext).Assembly;
        var types = GetReferencedAssemblies(migrationAssembly)
            .SelectMany(a => a.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(DbContext)))
            )
            .ToList();

        for (var i = 0; i < types.Count; i++) {
            Console.WriteLine($"{i + 1}. {types[i].Name}");
        }

        var idx = GetSelection(1, types.Count) - 1;
        Console.WriteLine();
        return types[idx];
    }

    private static IEnumerable<Assembly> GetReferencedAssemblies(Assembly assembly) {
        yield return assembly;
        foreach (var referencedAssemblyName in assembly.GetReferencedAssemblies()) {
            if (!referencedAssemblyName.Name!.Contains("Knowingly")) {
                continue;
            }

            var referencedAssembly = Assembly.Load(referencedAssemblyName);
            yield return referencedAssembly;
        }
    }

    private static void HandleUpdateDatabase() {
        var dbContext = SelectDbContext();
        var args = CreateUpdateDatabaseArgs(dbContext.Name);

        ExecuteCommand("dotnet", args);
    }

    private static int GetSelection(int lowInclusive, int highInclusive) {
        var i = -1;
        while (i < lowInclusive || i > highInclusive) {
            Console.Write($"Input ({lowInclusive}-{highInclusive}): ");
            var line = Console.ReadLine();
            int.TryParse(line, out i);
        }

        return i;
    }
}