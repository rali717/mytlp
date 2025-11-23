
using System.Diagnostics;
using mytlp.Models;

// using mytlp.Models;


namespace mytlp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;

public partial class BatteryProfile(string name, int min, int max) : ObservableObject
{
    [ObservableProperty] private int _min = min; // the name field
    [ObservableProperty] private int _max = max; // the name field

    public string Name // the Name property
    {
        get;
        set;
    } = name;
}

public partial class MainWindowViewModel : ViewModelBase
{
// #pragma warning disable CA1822 // Mark members as static
//     public string Greeting => "Welcome to Avalonia!";
// #pragma warning restore CA1822 // Mark members as static

    [ObservableProperty] private string _logText = "";

    [ObservableProperty] private ObservableCollection<BatteryProfile> _batteryProfiles;

    [ObservableProperty] private BatteryProfile? _selectedBatteryProfile;


    [RelayCommand]
    public void AddProfileCommand()
    {
        BatteryProfiles.Add(new BatteryProfile("New Profile", 95, 100));
        LogText = LogText + "\n- Add new charging profile.\n";
    }

    [RelayCommand]
    public void OnDeleteProfile()
    {
        Debug.WriteLine("OnDeleteprofile");
        if ((SelectedBatteryProfile != null) && (BatteryProfiles.Count > 1))
        {
            BatteryProfiles.Remove(SelectedBatteryProfile);
            LogText = LogText + "\n- Charging profile deleted.\n";
        }
    }


    [RelayCommand]
    public void OnMoin()
    {
        Debug.WriteLine("onMoin");
        LogText = LogText + "\nMoin\n";
    }


    [RelayCommand]
    public void OnSaveProfiles()
    {
        Debug.WriteLine("OnSaveProfiles");
        SaveBatteryProfiles.Save_BatteryProfiles("BatteryProfiles.json", BatteryProfiles);
        LogText = LogText + "\n- All Charging profiles saved\n";
        
    }

    [RelayCommand]
    public async Task OnLoadProfiles()
    {
        Debug.WriteLine("OnLoadProfiles");

        var loaded = await Load_BatteryProfiles.LoadBatteryProfiles_Async("BatteryProfiles.json");
        BatteryProfiles = new ObservableCollection<BatteryProfile>(loaded);
        LogText = LogText + "\n- All stored charging profiles reloaded.\n";
    }

    [RelayCommand]
    public void OnShowCurrentThresholds()
    {
        LogText = LogText + "\n- Current Charging Thresholds:.\n";
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pkexec",
                Arguments = "cat /sys/class/power_supply/BAT0/charge_start_threshold ; cat /sys/class/power_supply/BAT0/charge_stop_threshold",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine(output);

        LogText = LogText + output + error;
 
    }
    
    
    [RelayCommand]
    public void OnFullCharge()
    {
        Debug.WriteLine("OnFullCharge");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pkexec",
                Arguments = "sudo tlp fullcharge",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine(output);

        LogText = LogText + "\n\ntlp fullcharge\n\n" + output + error;
        LogText = LogText + "After reaching the 100%, the battery stays on 100%.";
    }

    [RelayCommand]
    public void OnChargeFullOnce()
    {
        Debug.WriteLine("OnChargeFullOnce");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pkexec",
                Arguments = "sudo tlp chargeonce",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine(output);
        LogText = LogText + "\n\ntlp chargeonce\n\n" + output + error;
        LogText = LogText + "After reaching the 100% once, the normal charging threshold were used.";
    }


    [RelayCommand]
    public void OnSetChargeLimits()
    {
        Debug.WriteLine("OnSetChargeLimits");

        if (SelectedBatteryProfile != null)
        {
            LogText = LogText + "\n- Set new thresholds.\n";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "pkexec",
                    Arguments = "tlp setcharge " + SelectedBatteryProfile.Min + " " + SelectedBatteryProfile.Max +
                                " BAT0",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine(output);
            LogText = LogText + "\n\ntlp setcharge " + SelectedBatteryProfile.Min + " " + SelectedBatteryProfile.Max +
                      " BAT0\n\n" + output + error;
        }
    }

    
    public void MoveItem(BatteryProfile source, BatteryProfile target)
    {
        int oldIndex = BatteryProfiles.IndexOf(source);
        int newIndex = BatteryProfiles.IndexOf(target);

        if (oldIndex != -1 && newIndex != -1 && oldIndex != newIndex)
            BatteryProfiles.Move(oldIndex, newIndex);
    }
   


    public MainWindowViewModel()
    {
        OnShowCurrentThresholds();
        
        OnLoadProfiles();

        if ((_batteryProfiles == null) || (BatteryProfiles.Count == 0))
        {
            _batteryProfiles =
            [
                new BatteryProfile("Mobile", 97, 100),
                new BatteryProfile("Home", 65, 70)  
            ];
            OnSaveProfiles();
            //_batteryProfiles.Add(new BatteryProfile("Other", 75, 80));
        }
    }
    
    
}