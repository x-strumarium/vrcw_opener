using Newtonsoft.Json;
using System;
using System.IO;


namespace VRCW_Opener
{
    public class ConfigureData
    {
        [JsonProperty("VRChat.exe file path")]
        private string _vrcExeFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\steamapps\common\VRChat\VRChat.exe");
        [JsonIgnore]
        public string VrcExeFilePath { get => _vrcExeFilePath; set { _vrcExeFilePath = value; } }
    }
}