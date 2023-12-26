﻿
using CHDesktop.Models.Enums;
using CHDesktop.Services.Helpers;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CHDesktop.Services.Helpers;

public static class ConfigurationHelper
{
    public class Config
    {
        public string? Ip { get; set; }
        public string? Port { get; set; }
        public string? Token { get; set; }
    }

    private static readonly string FilePath = Directory.GetCurrentDirectory();
    private const string FileName = "config.json";
    public static Config ConfigApp { get; private set; } = LoadAuthData().Result;

    public static async Task<LoggingHelper> SaveConfig(string? ip = null, string? port = null, string? token = null)
    {
        Config newConfig = new()
        {
            Ip = ip ?? ConfigApp?.Ip ?? "000.000.000.000",
            Port = port ?? ConfigApp?.Port ?? "0000",
            Token = token ?? ConfigApp?.Token ?? "",
        };
        try
        {
            string jsonParam = JsonSerializer.Serialize(newConfig);
            await File.WriteAllTextAsync($"{FilePath}\\{FileName}", jsonParam);
            return new(TypeMessage.Ordinary, DangerLevel.Oke, "Ok");
        }
        catch (Exception ex)
        {
            return new(TypeMessage.Problem, DangerLevel.Error, ex.Message);
        }
    }

    private static async Task<Config> LoadAuthData()
    {
        try
        {
            string textFromFile = File.ReadAllText($"{FilePath}\\{FileName}");
            Config configApp = JsonSerializer.Deserialize<Config>(textFromFile)
                ?? throw new EncoderFallbackException();
            if (configApp.Ip == null || configApp.Token == null || configApp.Port == null)
                throw new EncoderFallbackException();
            return configApp;
        }
        catch
        {
            await SaveConfig("000.000.000.000", "0000");
            return await LoadAuthData();
        }
    }
}
