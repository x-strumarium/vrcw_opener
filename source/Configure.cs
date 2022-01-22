using Newtonsoft.Json;
using System.IO;
using System.Reflection;


namespace VRCW_Opener
{
    [JsonObject("VRCW Opener Data")]
    public class Configure
    {
        private static readonly string _Location = Assembly.GetExecutingAssembly().Location;
        private static readonly string _confFilePath = Path.Combine(Path.GetDirectoryName(_Location), $"{Path.GetFileNameWithoutExtension(_Location)}.conf.json");

        private ConfigureData _data = null;
        public ConfigureData Data => _data;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Configure()
        {
            if (!Load())
            {
                _data = new ConfigureData();
                Save();
            }
        }


        /// <summary>
        /// 設定データの読み込み
        /// </summary>
        public bool Load()
        {
            try
            {
                _data = JsonConvert.DeserializeObject<ConfigureData>(File.ReadAllText(_confFilePath));
                return _data != null && !string.IsNullOrWhiteSpace(_data.VrcExeFilePath);
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }


        /// <summary>
        /// 設定データの保存
        /// </summary>
        public void Save() => File.WriteAllText(_confFilePath, JsonConvert.SerializeObject(_data, Formatting.Indented));
    }
}