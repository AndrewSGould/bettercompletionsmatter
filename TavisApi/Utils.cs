using dotenv.net;

namespace TavisApi;

public interface IUtils
{
    string GetEnvVar(string environmentVariable);
}

public class Utils : IUtils
{
    public string GetEnvVar(string environmentVariable)
    {
        var envVars = DotEnv.Read();
        var result = envVars.TryGetValue(environmentVariable, out var key) ? key : null;
        if (result is null || result == "") result = Environment.GetEnvironmentVariable(environmentVariable);

        if (result is null) throw new Exception("Cant retrieve requested environment variable");

        return result;
    }
}
