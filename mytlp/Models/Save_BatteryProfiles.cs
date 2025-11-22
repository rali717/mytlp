using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using mytlp.ViewModels;

namespace mytlp.Models;

public static class SaveBatteryProfiles
{
    public static async Task     Save_BatteryProfiles (string filePath, ObservableCollection<BatteryProfile> people)
    {
    
        var options = new JsonSerializerOptions
        {
            WriteIndented = true // für schön formatiertes JSON
        };

        string json = JsonSerializer.Serialize(people, options);
        await File.WriteAllTextAsync(filePath, json);
    }
}
