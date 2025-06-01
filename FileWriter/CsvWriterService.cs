using Microsoft.Extensions.DependencyInjection;
using OpenMod.Unturned.Players.Life.Events;
using Vector3 = System.Numerics.Vector3;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Scitalis.Analytics.Models;
using OpenMod.Unturned.Users;
using System.Threading.Tasks;
using OpenMod.API.Ioc;
using System.Text;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Reflection;

namespace Scitalis.Analytics.FileWriter
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
    public class CsvWriterService : IWriterService
    {
        private readonly ILogger<CsvWriterService> _logger;
        private const string PositionFileName = "player_position.csv";
        private const string CombatFileName = "player_combat.csv";

        public CsvWriterService(ILogger<CsvWriterService> logger)
        {
            _logger = logger;
        }

        public async Task AppendToPlayerPositionFile(ICollection<UnturnedUser> users)
        {
            PlayerPositionRecord[] records = PlayerPositionRecords(users);
            try
            {
                await AppendRecordsToFileAsync(records, PositionFileName);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error occured while trying to write position: {e}");
            }
        }
        
        public async Task AppendToDamageFile(UnturnedPlayerDamagedEvent @event)
        {
            KillFeedRecord record = new KillFeedRecord(killerID: @event.Killer, victimID: @event.Player.SteamId,
                hitLimb: @event.Limb, damageSource: @event.DamageSource, damageAmount: @event.DamageAmount,
                cause: @event.Cause);
            try
            {
                await AppendRecordsToFileAsync(record, CombatFileName);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error occured while trying to write kill feed: {e}");
            }
        }
        
        private async Task AppendRecordsToFileAsync<T>(T[] records, string fileName) where T : struct
        {
            try
            {
                string pluginPath = Path.Combine(Application.dataPath, "Plugins");
                string logsFolder = Path.Combine(pluginPath, "Logs");
                string filePath = Path.Combine(logsFolder, fileName);
            
                // Create directory if it doesn't exist
                string? directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) 
                    Directory.CreateDirectory(directory);

                // Check if file exists to determine if we need headers
                bool fileExists = File.Exists(filePath);

                await using var writer = new StreamWriter(filePath, true, Encoding.UTF8);
            
                // Get all public fields and properties of the struct
                var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
                // Write header if this is a new file
                if (!fileExists)
                {
                    var headers = fields.Select(f => f.Name)
                        .Concat(properties.Select(p => p.Name));
                    await writer.WriteLineAsync(string.Join(",", headers));
                }

                // Write all records
                foreach (T record in records)
                {
                    List<string> values = new List<string>();
                
                    // Process fields
                    values.AddRange(fields.Select(field => field.GetValue(record)).Select(FormatValue).ToList());
                
                    // Process properties
                    values.AddRange(properties.Select(property => property.GetValue(record)).Select(FormatValue));

                    await writer.WriteLineAsync(string.Join(",", values));
                }    
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new IOException("Access denied to file or directory", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new IOException("File path is too long", ex);
            }
            catch (IOException ex)
            {
                // Re-throw IOExceptions as they're already relevant
                throw new IOException($"Failed to write records to file '{fileName}'", ex);
            }
            catch (Exception ex) when (ex is ArgumentException || 
                                       ex is NotSupportedException || 
                                       ex is InvalidOperationException)
            {
                // Wrap other expected exceptions in IOException for consistent handling
                throw new IOException($"Failed to process records for file '{fileName}'", ex);
            }
            catch (Exception ex)
            {
                // Wrap unexpected exceptions
                throw new IOException($"Unexpected error while writing records to file '{fileName}'", ex);
            }
        }
        
        private async Task AppendRecordsToFileAsync<T>(T record, string fileName) where T : struct
        {
            string pluginPath = Path.Combine(Application.dataPath, "Plugins");
            string logsFolder = Path.Combine(pluginPath, "Logs");
            string filePath = Path.Combine(logsFolder, fileName);
            
            try
            {
                // Create directory if it doesn't exist
                string? directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) 
                    Directory.CreateDirectory(directory);

                // Check if file exists to determine if we need headers
                bool fileExists = File.Exists(filePath);

                await using var writer = new StreamWriter(filePath, true, Encoding.UTF8);
                
                // Get all public fields and properties of the struct
                var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                
                // Write header if this is a new file
                if (!fileExists)
                {
                    var headers = fields.Select(f => f.Name)
                                        .Concat(properties.Select(p => p.Name));
                    await writer.WriteLineAsync(string.Join(",", headers));
                }

                List<string> values = new List<string>();
                
                values.AddRange(fields.Select(field => field.GetValue(record)).Select(FormatValue).ToList());
                values.AddRange(properties.Select(property => property.GetValue(record)).Select(FormatValue));

                await writer.WriteLineAsync(string.Join(",", values));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error writing to file: {ex.Message}");
                throw;
            }
        }
        
        private static string FormatValue(object? value)
        {
            return value switch
            {
                null => string.Empty,
                // Handle special cases for complex types
                Vector3 vector3 => $"({vector3.X};{vector3.Y};{vector3.Z})",
                Dictionary<string, object> dictionary => EscapeCsv(JsonUtility.ToJson(dictionary)),
                Enum enumValue => enumValue.ToString(),
                _ => EscapeCsv(value.ToString())
            };
        }
        
        private static string EscapeCsv(string input)
        {
            if (string.IsNullOrEmpty(input)) 
                return input;
        
            if (input.Contains(",") || input.Contains("\""))
                return $"\"{input.Replace("\"", "\"\"")}\"";
            
            return input;
        }

        private static PlayerPositionRecord[] PlayerPositionRecords(ICollection<UnturnedUser> users)
        {
            PlayerPositionRecord[] records = new PlayerPositionRecord[users.Count];
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int i = 0;
            foreach (UnturnedUser user in users)
            {
                Vector3 playerPosition = user.Player.Transform.Position;
                string playerName = user.DisplayName;
                string playerId = user.Id;

                PlayerPositionRecord record = new PlayerPositionRecord
                {
                    TimeStamp = timeStamp,
                    PlayerName = playerName,
                    PlayerID = playerId,
                    PlayerPosition = playerPosition
                };
                records[i] = record;
                i++;
            }

            return records;
        }
    }
}