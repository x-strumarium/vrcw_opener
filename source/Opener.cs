using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VRCW_Opener.Properties;


namespace VRCW_Opener
{
    public class Opener
    {
        private const string _Arguments = "--url=create?url=file:///";
        private static readonly string _OutputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\LocalLow\VRChat\VRChat\Worlds");
        public const string VrcwExtension = ".vrcw";
        private static readonly string _VrcwFilter = $"{Resources.FileTypeName} (*{VrcwExtension})|*{VrcwExtension}";
        private const string _ExeFileName = "VRChat.exe";

        private readonly Configure _conf = new Configure();


        /// <summary>
        /// VRChatを起動
        /// </summary>
        /// <param name="args">コマンドラインオプション</param>
        public void Start(string[] args)
        {
            string filePath;

            if (args != null && args.Length > 0)
                filePath = args[0];
            else
            {
                filePath = string.Empty;
                args = new string[1];
            }

            try { _ = File.GetAttributes(filePath); }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is ArgumentException)
                {
                    if (!Path.IsPathRooted(filePath))
                    {
                        using (var ofd = new OpenFileDialog()
                        {
                            Title = string.Format(Resources.OpenFileDialogTitle, Resources.FileTypeName),
                            InitialDirectory = _OutputDirectory,
                            RestoreDirectory = true,
                            Filter = _VrcwFilter
                        })
                        {
                            if (ofd.ShowDialog() == DialogResult.OK)
                                filePath = ofd.FileName;
                            else
                                return;
                        }

                        var list = args.ToList();
                        list.Insert(0, string.Empty);
                        args = list.ToArray();
                    }
                }
                else
                    throw;
            }

            args[0] = $"{_Arguments}{Uri.EscapeDataString(filePath)}";

            do
            {
                try
                {
                    using (Process.Start(_conf.Data.VrcExeFilePath, string.Join(" ", args))) { };
                    _conf.Save();
                    return;
                }
                catch (Win32Exception)
                {
                    if (MessageBox.Show(string.Format(Resources.IncorrectExeFilePath, _ExeFileName), Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                        EditExePath();
                    else
                        return;
                }
            }
            while (true);
        }


        /// <summary>
        /// EXEファイルのパスを変更する
        /// </summary>
        public void EditExePath()
        {
            using (var ofd = new OpenFileDialog()
            {
                Title = string.Format(Resources.SelectExeFileDialogTitle, _ExeFileName),
                InitialDirectory = Path.GetDirectoryName(_conf.Data.VrcExeFilePath),
                Filter = $"|{_ExeFileName}",
                FileName = _conf.Data.VrcExeFilePath
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _conf.Data.VrcExeFilePath = ofd.FileName;
                    _conf.Save();
                }
            }
        }
    }
}