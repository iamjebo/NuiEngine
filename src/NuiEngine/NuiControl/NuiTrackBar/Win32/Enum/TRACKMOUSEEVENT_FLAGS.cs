using System;

namespace NuiEngine.NuiControl.Window32.Enum
{
    internal enum TRACKMOUSEEVENT_FLAGS : uint
    {
        TME_HOVER = 1,
        TME_LEAVE = 2,
        TME_QUERY = 0x40000000,
        TME_CANCEL = 0x80000000
    }
}
