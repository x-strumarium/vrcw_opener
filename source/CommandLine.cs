using System.Linq;
using System.Windows.Forms;
using VRCW_Opener.Properties;


namespace VRCW_Opener
{
    public class CommandLine
    {
        private const string _CommandLineHead = "--vrcw_opener";
        private static readonly string[] _Associate = new string[] { $"{_CommandLineHead}-a", $"{_CommandLineHead}-associate" };
        private static readonly string[] _Disassociate = new string[] { $"{_CommandLineHead}-d", $"{_CommandLineHead}-disassociate" };
        private static readonly string[] _EditExeFilePath = new string[] { $"{_CommandLineHead}-e", $"{_CommandLineHead}-edit" };


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="args">コマンドラインオプション</param>
        public CommandLine(string[] args)
        {
            if (args != null && args.Length > 0 && args[0].ToLower().IndexOf(_CommandLineHead) == 0)
            {
                var option = args[0].ToLower();

                if (_Associate.Contains(option))
                    new FileAssociation(args).CreateRegistryKey();
                else if (_Disassociate.Contains(option))
                    new FileAssociation(args).DeleteRegistryKey();
                else if (_EditExeFilePath.Contains(option))
                    new Opener().EditExePath();
                else
                    _ = MessageBox.Show(string.Format(Resources.IncorrectOptions, option), Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                new Opener().Start(args);
        }
    }
}