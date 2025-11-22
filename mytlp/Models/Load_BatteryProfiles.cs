using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using mytlp.ViewModels;

namespace mytlp.Models;

public static class Load_BatteryProfiles
{
    static Load_BatteryProfiles()
    {
        
    }
    
    public static async Task<List<BatteryProfile>> LoadBatteryProfiles_Async(string filePath)
    {
        if (!File.Exists(filePath))
            return new List<BatteryProfile>();

        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<BatteryProfile>>(json);
    }
} 



