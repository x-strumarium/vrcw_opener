using System;
using System.Windows.Forms;
using VRCW_Opener.Properties;


namespace VRCW_Opener
{
    public static class Program
    {
        /// <summary>
        /// エントリポイント
        /// </summary>
        /// <param name="args">コマンドラインオプション</param>
        [STAThread]
        public static int Main(string[] args)
        {
            try { _ = new CommandLine(args); }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.ToString(), Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
#if DEBUG
                throw;
#else
                return 1;
#endif
            }
            return 0;
        }
    }
}