using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Users;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Scitalis.Analytics.FileWriter
{
    public struct PlayerPositionRecord
    {
        public string TimeStamp;
        public string PlayerName;
        public string PlayerID;
        public Vector3 PlayerPosition;
    }
    
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
    public class CsvWriterService : IWriterService
    {
        private const string HeaderLine = "Timestamp,PlayerName,PlayerID,PositionX,PositionY,PositionZ";
        private const string FileName = "player_position.csv";

        private static async Task AppendRecordsToFileAsync(PlayerPositionRecord[] records, string fileName = "PlayerPositions.csv")
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
                
                // Write header if this is a new file
                if (!fileExists)
                    await writer.WriteLineAsync(HeaderLine);

                // Write all records
                foreach (var record in records)
                {
                    string line = $"{EscapeCsv(record.TimeStamp)}," +
                                  $"{EscapeCsv(record.PlayerName)}," +
                                  $"{EscapeCsv(record.PlayerID)}," +
                                  $"{record.PlayerPosition.X}," +
                                  $"{record.PlayerPosition.Y}," +
                                  $"{record.PlayerPosition.Z}";
                    
                    await writer.WriteLineAsync(line);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error writing to file: {ex.Message}");
                throw; // Re-throw to allow caller to handle the exception
            }
        }
        
        private static string EscapeCsv(string input)
        {
            if (string.IsNullOrEmpty(input)) 
                return input;
        
            if (input.Contains(",") || input.Contains("\""))
                return $"\"{input.Replace("\"", "\"\"")}\"";
            
            return input;
        }

        
        public async Task AppendPlayerPositionToFile(ICollection<UnturnedUser> users)
        {
            PlayerPositionRecord[] records = PlayerPositionRecords(users);
            try
            {
                await AppendRecordsToFileAsync(records, FileName);
                Debug.Log("Records written successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to write records: {ex.Message}");
            }
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