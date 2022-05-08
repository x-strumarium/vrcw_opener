using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;
using VRCW_Opener.Properties;


namespace VRCW_Opener
{
    public class FileAssociation
    {
        private static readonly string _ExeFilePath = Assembly.GetExecutingAssembly().Location;
        private static readonly string _ClassName = $"{Path.GetFileNameWithoutExtension(_ExeFilePath)}{Opener.VrcwExtension}";

        private readonly bool _isAdministrator = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="args">コマンドラインオプション</param>
        public FileAssociation(string[] args)
        {
            if (!_isAdministrator)
                try
                {
                    // 管理者権限でアプリケーションを開き直す
                    using (var process = Process.Start(new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = _ExeFilePath,
                        Verb = "runas",
                        Arguments = string.Join(" ", args)
                    })) { };
                }
                catch (Win32Exception) { }
        }


        /// <summary>
        /// ファイルの関連付けを設定
        /// </summary>
        public void CreateRegistryKey()
        {
            if (_isAdministrator)
            {
                using (var hkcr = Registry.ClassesRoot)
                {
                    // 拡張子キー
                    using (var extensionKey = hkcr.CreateSubKey(Opener.VrcwExtension))
                        extensionKey.SetValue(string.Empty, _ClassName);

                    // 拡張子 クラスキー
                    using (var classKey = hkcr.CreateSubKey(_ClassName))
                    {
                        classKey.SetValue("FriendlyTypeName", Resources.FileTypeName);

                        using (var defaultIconKey = classKey.CreateSubKey("DefaultIcon"))
                            defaultIconKey.SetValue(string.Empty, $"{_ExeFilePath},1");

                        using (var shellKey = classKey.CreateSubKey("Shell"))
                        {
                            var command = "Command";
                            var openCommand = $"\"{_ExeFilePath}\" \"%1\"";

                            using (var openKey = shellKey.CreateSubKey("Open"))
                            using (var commandKey = openKey.CreateSubKey(command))
                                commandKey.SetValue(string.Empty, $"{openCommand} --no-vr");

                            using (var vrModeKey = shellKey.CreateSubKey("VRMode"))
                            {
                                vrModeKey.SetValue(string.Empty, Resources.OpenVRModeCommandName);

                                using (var commandKey = vrModeKey.CreateSubKey(command))
                                    commandKey.SetValue(string.Empty, openCommand);
                            }
                        }
                    }
                }
                _ = MessageBox.Show(Resources.FileAssociation, Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// ファイルの関連付けを解除
        /// </summary>
        public void DeleteRegistryKey()
        {
            if (_isAdministrator)
            {
                using (var hkcr = Registry.ClassesRoot)
                {
                    try { hkcr.DeleteSubKeyTree(Opener.VrcwExtension); }
                    catch (ArgumentException) { }
                    try { hkcr.DeleteSubKeyTree(_ClassName); }
                    catch (ArgumentException) { }
                }
                _ = MessageBox.Show(Resources.FileDisassociation, Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}