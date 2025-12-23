using System.Management;
using System.Text.Json;
using GHelper.Helpers;

public static class AppConfig
{
    private static string configFile;

    private static string? _model;
    private static string? _modelShort;
    private static string? _bios;

    private static Dictionary<string, object> config = new Dictionary<string, object>();
    private static System.Timers.Timer timer = new System.Timers.Timer(2000);
    private static long lastWrite;

    static AppConfig()
    {
        string startupPath = Application.StartupPath.Trim('\\');
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GHelper";
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
                config = JsonSerializer.Deserialize<Dictionary<string, object>>(text);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Broken config: {ex.Message} {text}");
                try
                {
                    text = File.ReadAllText(configFile + ".bak");
                    config = JsonSerializer.Deserialize<Dictionary<string, object>>(text);
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

    public static string GetModel()
    {
        if (_model is null)
        {
            _model = "";
            try
            {
                using (var searcher = new ManagementObjectSearcher(@"Select * from Win32_ComputerSystem"))
                {
                    foreach (var process in searcher.Get())
                    {
                        _model = process["Model"].ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }
        }

        //if (_model.Contains("GA402RK")) _model = "ROG Flow Z13 GZ302EA"; // Debug Purposes

        return _model;
    }

    public static (string, string) GetBiosAndModel()
    {
        if (_bios is not null && _modelShort is not null) return (_bios, _modelShort);

        using (ManagementObjectSearcher objSearcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_BIOS"))
        {
            using (ManagementObjectCollection objCollection = objSearcher.Get())
            {
                foreach (ManagementObject obj in objCollection)
                    if (obj["SMBIOSBIOSVersion"] is not null)
                    {
                        string[] results = obj["SMBIOSBIOSVersion"].ToString().Split(".");
                        if (results.Length > 1)
                        {
                            _modelShort = results[0];
                            _bios = results[1];
                        }
                        else
                        {
                            _modelShort = obj["SMBIOSBIOSVersion"].ToString();
                        }
                    }

                return (_bios, _modelShort);
            }
        }
    }

    public static string GetModelShort()
    {
        string model = GetModel();
        int trim = model.LastIndexOf("_");
        if (trim > 0) model = model.Substring(0, trim);
        return model;
    }

    private static void Init()
    {
        config = new Dictionary<string, object>();
        config["performance_mode"] = 0;
        string jsonString = JsonSerializer.Serialize(config);
        File.WriteAllText(configFile, jsonString);
    }

    public static bool Exists(string name)
    {
        return config.ContainsKey(name);
    }

    public static int Get(string name, int empty = -1)
    {
        if (config.ContainsKey(name))
        {
            //Debug.WriteLine(name);
            return int.Parse(config[name].ToString());
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

    public static bool IsNotFalse(string name)
    {
        return Get(name) != 0;
    }

    public static bool IsOnBattery(string zone)
    {
        return Get(zone + "_bat", Get(zone)) != 0;
    }

    public static string GetString(string name, string empty = null)
    {
        if (config.ContainsKey(name))
            return config[name].ToString();
        else return empty;
    }

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

    public static void Set(string name, string value)
    {
        config[name] = value;
        Write();
    }

    public static void Remove(string name)
    {
        config.Remove(name);
        Write();
    }

    public static byte[] StringToBytes(string str)
    {
        String[] arr = str.Split('-');
        byte[] array = new byte[arr.Length];
        for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);
        return array;
    }

    public static bool IsNoOverdrive()
    {
        return Is("no_overdrive");
    }

    public static bool IsNVPlatform()
    {
        return Is("nv_platform");
    }

    public static bool IsBWIcon()
    {
        return Is("bw_icon");
    }

    public static bool SaveDimming()
    {
        return Is("save_dimming");
    }

    public static bool IsAutoStatusLed()
    {
        return Is("auto_status_led");
    }
}