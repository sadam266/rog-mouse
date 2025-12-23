namespace RogMouse;

using System.Text.Json;
using Helpers;

public static class AppConfig
{
    private static string configFile;

    private static Dictionary<string, object> config = new Dictionary<string, object>();
    private static System.Timers.Timer timer = new System.Timers.Timer(2000);
    private static long lastWrite;

    static AppConfig()
    {
        string startupPath = Application.StartupPath.Trim('\\');
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RogMouse";
        string configName = "\\config.json";

        if (File.Exists(startupPath + configName))
        {
            configFile = startupPath + configName;
        }
        else
        {
            configFile = appPath + configName;
        }


        if (!System.IO.Directory.Exists(appPath))
            System.IO.Directory.CreateDirectory(appPath);

        if (File.Exists(configFile))
        {
            string text = File.ReadAllText(configFile);
            try
            {
                config = JsonSerializer.Deserialize<Dictionary<string, object>>(text)!;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Broken config: {ex.Message} {text}");
                try
                {
                    text = File.ReadAllText(configFile + ".bak");
                    config = JsonSerializer.Deserialize<Dictionary<string, object>>(text)!;
                }
                catch (Exception exb)
                {
                    Logger.WriteLine($"Broken backup config: {exb.Message} {text}");
                    File.Copy(configFile, configFile + ".old", true);
                    File.Copy(configFile + ".bak", configFile + ".bak.old", true);
                    Init();
                }
            }
        }
        else
        {
            Init();
        }

        timer.Elapsed += Timer_Elapsed;
    }

    private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        timer.Stop();
        string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        var backup = configFile + ".bak";

        try
        {
            File.WriteAllText(configFile, jsonString);
            //Debug.WriteLine($"{DateTime.Now}: Config write");
        }
        catch (Exception)
        {
            Thread.Sleep(1000);
            try
            {
                File.WriteAllText(configFile, jsonString);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }

            return;
        }

        lastWrite = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        Thread.Sleep(5000);

        if (Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastWrite) < 4000) return;

        var backupText = File.ReadAllText(configFile);
        bool isValid =
            !string.IsNullOrWhiteSpace(backupText) &&
            backupText.IndexOf('\0') == -1 &&
            backupText.StartsWith("{") &&
            backupText.Trim().EndsWith("}") &&
            backupText.Length >= 10;

        if (isValid)
        {
            File.Copy(configFile, backup, true);
            //Debug.WriteLine($"{DateTime.Now}: Config backup");
        }
        else
        {
            Logger.WriteLine("Error writing config");
        }
    }

    private static void Init()
    {
        config = new Dictionary<string, object>();
        config["performance_mode"] = 0;
        string jsonString = JsonSerializer.Serialize(config);
        File.WriteAllText(configFile, jsonString);
    }

    public static int Get(string name, int empty = -1)
    {
        if (config.ContainsKey(name))
        {
            //Debug.WriteLine(name);
            return int.Parse(config[name].ToString()!);
        }
        else
        {
            //Debug.WriteLine(name + "E");
            return empty;
        }
    }

    public static bool Is(string name)
    {
        return Get(name) == 1;
    }

    public static string? GetString(string name, string? empty = null) =>
        config.TryGetValue(name, out var value) ? value.ToString() : empty;

    private static void Write()
    {
        timer.Stop();
        timer.Start();
    }

    public static void Set(string name, int value)
    {
        config[name] = value;
        Write();
    }

    public static bool IsBWIcon()
    {
        return Is("bw_icon");
    }
}