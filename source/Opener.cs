using System;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace VRCW_Opener
{
    public class Opener
    {
        private const string _Arguments = "--url=create?url=file:///";
        private static readonly string _DefaultVrcwFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\LocalLow\VRChat\VRChat\Worlds\scene-StandaloneWindows64-01.vrcw");

        private readonly Configure _conf = new Configure();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="args">コマンドラインオプション</param>
        public Opener(string[] args)
        {
            string filePath;

            if (args != null && args.Length > 0)
                filePath = args[0];
            else
            {
                filePath = string.Empty;
                args = new string[1];
            }

            try
            {
                _ = File.GetAttributes(filePath);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is ArgumentException)
                {
                    if (!Path.IsPathRooted(filePath))
                    {
                        filePath = _DefaultVrcwFilePath;
                        var list = args.ToList();
                        list.Insert(0, string.Empty);
                        args = list.ToArray();
                    }
                }
                else throw;
            }

            args[0] = $"{_Arguments}{Uri.EscapeDataString(filePath)}";
            _ = Process.Start(_conf.Data.VrcExeFilePath, string.Join(" ", args));
        }
    }
}