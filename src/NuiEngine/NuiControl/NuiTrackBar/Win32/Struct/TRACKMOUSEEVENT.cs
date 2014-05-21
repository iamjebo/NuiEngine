using System;
using System.Runtime.InteropServices;
using NuiEngine.NuiControl.Window32.Enum;

namespace NuiEngine.NuiControl.Window32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct TRACKMOUSEEVENT
    {
        internal uint cbSize;
        internal TRACKMOUSEEVENT_FLAGS dwFlags;
        internal IntPtr hwndTrack;
        internal uint dwHoverTime;
    }
}
