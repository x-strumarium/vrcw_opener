using System;


namespace VRCW_Opener
{
    public static class Program
    {
        /// <summary>
        /// エントリポイント
        /// </summary>
        /// <param name="args">コマンドラインオプション</param>
        public static int Main(string[] args)
        {
            try
            {
                _ = new Opener(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                return 1;
            }
#if DEBUG
            Console.ReadKey();
#endif
            return 0;
        }
    }
}