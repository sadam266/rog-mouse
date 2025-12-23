using System.Diagnostics;
using System.Management;
using System.Text.Json;

public static class AppConfig
{
    private static string configFile;
    private static Dictionary<string, object> config = new Dictionary<string, object>();
    private static System.Timers.Timer timer = new System.Timers.Timer(2000);

    static AppConfig()
    {
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ROGMouseHelper";
        string configName = "\\config.json";
        configFile = appPath + configName;

        if (!System.IO.Directory.Exists(appPath))
            System.IO.Directory.CreateDirectory(appPath);

        if (File.Exists(configFile))
        {
            try
            {
                string text = File.ReadAllText(configFile);
                config = JsonSerializer.Deserialize<Dictionary<string, object>>(text);
            }
            catch { }
        }
        timer.Elapsed += (s, e) => Save();
    }

    public static int Get(string name)
    {
        if (config.ContainsKey(name))
            return int.Parse(config[name].ToString());
        return 0;
    }

    public static string GetString(string name)
    {
        if (config.ContainsKey(name))
            return config[name].ToString();
        return null;
    }

    public static void Set(string name, int value)
    {
        config[name] = value;
        timer.Stop();
        timer.Start();
    }

    public static void Set(string name, string value)
    {
        config[name] = value;
        timer.Stop();
        timer.Start();
    }

    public static bool Is(string name)
    {
        return Get(name) == 1;
    }

    private static void Save()
    {
        timer.Stop();
        try
        {
            string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFile, jsonString);
        }
        catch { }
    }

    public static string GetModel() => "ROG Mouse";
    public static bool IsASUS() => true;
    public static bool IsAlly() => false;
    public static bool IsDynamicLightingInit() => false;
    public static bool IsROG() => true;
    public static bool IsS17() => false;
    public static bool IsZ13() => false;
}
