using System;
using System.Runtime.InteropServices;

namespace NuiEngine.NuiControl.Window32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct INITCOMMONCONTROLSEX
    {
        internal INITCOMMONCONTROLSEX(int flags)
        {
            this.dwSize = Marshal.SizeOf(typeof(INITCOMMONCONTROLSEX));
            this.dwICC = flags;
        }

        internal int dwSize;
        internal int dwICC;
    }
}
