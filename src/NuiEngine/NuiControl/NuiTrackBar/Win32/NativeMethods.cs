using System;
using System.Runtime.InteropServices;
using System.Drawing;
using NuiEngine.NuiControl.Window32.Struct;

namespace NuiEngine.NuiControl.Window32
{
    internal class NativeMethods
    {
        private NativeMethods()
        {
        }

        #region USER32.DLL

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(
            IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(
            IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll", SetLastError = true,
            CharSet = CharSet.Unicode, BestFitMapping = false)]
        public static extern IntPtr CreateWindowEx(
            int exstyle, 
            string lpClassName,
            string lpWindowName, 
            int dwStyle, 
            int x, 
            int y, 
            int nWidth,
            int nHeight,
            IntPtr hwndParent, 
            IntPtr Menu, 
            IntPtr hInstance, 
            IntPtr lpParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(
            IntPtr hInstance, int lpIconName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(
            IntPtr hWnd, 
            IntPtr hWndAfter,
            int x,
            int y, 
            int cx, 
            int cy, 
            uint flags);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(
            IntPtr hWnd, ref RECT r);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(
            IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public extern static int OffsetRect(
            ref RECT lpRect, int x, int y);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(
            IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(
            IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern bool TrackMouseEvent(
            ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PtInRect(ref RECT lprc, Point pt);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr SetTimer(
            IntPtr hWnd, 
            int nIDEvent, 
            uint uElapse, 
            IntPtr lpTimerFunc);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool KillTimer(
            IntPtr hWnd, uint uIDEvent);

        [DllImport("user32.dll")]
        public static extern int SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, int lParam);
        
        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, ref TOOLINFO lParam);
        
        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        
        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, ref RECT lParam);
        
        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd,
            int msg, 
            IntPtr wParam, 
            [MarshalAs(UnmanagedType.LPTStr)]string lParam);
        
        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, IntPtr wParam, ref NMHDR lParam);
        
        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        public static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

        #endregion

        #region GDI32.DLL

        [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
        public static extern bool AlphaBlend(
            IntPtr hdcDest, 
            int nXOriginDest, 
            int nYOriginDest, 
            int nWidthDest, 
            int nHeightDest,
            IntPtr hdcSrc,
            int nXOriginSrc, 
            int nYOriginSrc, 
            int nWidthSrc, 
            int nHeightSrc, 
            BLENDFUNCTION blendFunction);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool StretchBlt(
            IntPtr hDest, 
            int X, 
            int Y, 
            int nWidth, 
            int nHeight, 
            IntPtr hdcSrc,
            int sX, 
            int sY, 
            int nWidthSrc, 
            int nHeightSrc, 
            int dwRop);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(
            IntPtr hdc,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            int dwRop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCA(
            [MarshalAs(UnmanagedType.LPStr)]string lpszDriver,
            [MarshalAs(UnmanagedType.LPStr)]string lpszDevice, 
            [MarshalAs(UnmanagedType.LPStr)]string lpszOutput, 
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCW(
            [MarshalAs(UnmanagedType.LPWStr)]string lpszDriver,
            [MarshalAs(UnmanagedType.LPWStr)]string lpszDevice, 
            [MarshalAs(UnmanagedType.LPWStr)]string lpszOutput, 
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
            string lpszDriver, 
            string lpszDevice, 
            string lpszOutput, 
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
            IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        #endregion

        #region comctl32.dll

        [DllImport("comctl32.dll", 
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool InitCommonControlsEx(
            ref INITCOMMONCONTROLSEX iccex);

        #endregion

        #region kernel32.dll

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMHDR destination, IntPtr source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMTTDISPINFO destination, IntPtr source, int length);
        
        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            IntPtr destination, ref NMTTDISPINFO Source, int length);
        
        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref POINT destination, ref RECT Source, int length);
        
        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMTTCUSTOMDRAW destination, IntPtr Source, int length);
        
        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMCUSTOMDRAW destination, IntPtr Source, int length);

        #endregion
    }
}
