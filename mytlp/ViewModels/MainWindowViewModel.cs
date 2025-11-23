using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;
using Avalonia.Rendering.Composition.Animations;
using mytlp.Models;

// using mytlp.Models;


namespace mytlp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
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
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static


    [ObservableProperty] private ObservableCollection<BatteryProfile> _batteryProfiles;

    [ObservableProperty] private BatteryProfile? _selectedBatteryProfile;


    [RelayCommand]
    public void AddProfileCommand()
    {
        BatteryProfiles.Add(new BatteryProfile("New Profile", 95, 100));
    }

    [RelayCommand]
    public void OnDeleteProfile()
    {
        Debug.WriteLine("OnDeleteprofile");
        if ((SelectedBatteryProfile != null) && (BatteryProfiles.Count > 1))
        {
            BatteryProfiles.Remove(SelectedBatteryProfile);
        }
    }


    // [RelayCommand]
    // public void OnSave_Exit()
    // {
    //     Debug.WriteLine("Save_Exit");
    //     SaveBatteryProfiles.Save_BatteryProfiles("BatteryProfiles.json", BatteryProfiles);
    // }


    [RelayCommand]
    public void OnSaveProfiles()
    {
        Debug.WriteLine("OnSaveProfiles");
        SaveBatteryProfiles.Save_BatteryProfiles("BatteryProfiles.json", BatteryProfiles);
    }

    [RelayCommand]
    public async Task OnLoadProfiles()
    {
        Debug.WriteLine("OnLoadProfiles");
        //Load_BatteryProfiles.LoadBatteryProfiles_Async("BatteryProfiles.json");

        var loaded = await Load_BatteryProfiles.LoadBatteryProfiles_Async("BatteryProfiles.json");
        BatteryProfiles = new ObservableCollection<BatteryProfile>(loaded);
    }


    [RelayCommand]
    public void OnSetChargeLimits()
    {
        Debug.WriteLine("OnSetChargeLimits");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pkexec",
                Arguments = "tlp setcharge 65 75 BAT0",
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
    }


    public MainWindowViewModel()
    {
        _batteryProfiles =
        [
            new BatteryProfile("Mobile", 95, 100),
            new BatteryProfile("Home", 60, 65)
        ];

        _batteryProfiles.Add(new BatteryProfile("test", 10, 20));
    }
}