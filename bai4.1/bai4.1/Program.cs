
using System;
using System.Windows.Forms;

namespace bai4._1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Khoi tao giao dien Windows Forms
            ApplicationConfiguration.Initialize();

            // [THÊM MỚI] Khoi dong Form1
            Application.Run(new Form1());
        }
    }
}
