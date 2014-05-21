using System;
using System.Runtime.InteropServices;

namespace NuiEngine.NuiControl.Window32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct TT_HITTESTINFO
    {
        internal IntPtr hwnd;
        internal POINT pt;
        internal TOOLINFO ti;
    }
}
