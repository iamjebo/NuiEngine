using System;
using System.Windows.Forms;
using NuiEngine.NuiControl.Window32.Const;

namespace NuiEngine.NuiControl.Window32
{
    internal class Helper
    {
        private Helper()
        {
        }

        public static bool LeftKeyPressed()
        {
            if (SystemInformation.MouseButtonsSwapped)
            {
                return (NativeMethods.GetKeyState(VK.VK_RBUTTON) < 0);
            }
            else
            {
                return (NativeMethods.GetKeyState(VK.VK_LBUTTON) < 0);
            }
        }
    }
}
