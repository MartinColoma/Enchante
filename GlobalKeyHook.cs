using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class GlobalKeyHook
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private LowLevelKeyboardProc keyboardProc;
    private IntPtr hookId = IntPtr.Zero;

    public event EventHandler<KeyPressedEventArgs> KeyPressed;

    public GlobalKeyHook()
    {
        keyboardProc = HookCallback;
        hookId = SetHook(keyboardProc);
    }

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Keys key = (Keys)vkCode;

            // Check if Ctrl key is pressed and if your application's main window is in the foreground
            if (Control.ModifierKeys == Keys.Control && IsMainWindowInForeground())
            {
                // Handle specific key combinations
                if (key == Keys.D1)
                {
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(Keys.Control | Keys.D1));
                }
                else if (key == Keys.D2)
                {
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(Keys.Control | Keys.D2));
                }
                else if (key == Keys.D3)
                {
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(Keys.Control | Keys.D3));
                }
                else if (key == Keys.D4)
                {
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(Keys.Control | Keys.D4));
                }
                else if (key == Keys.D5)
                {
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(Keys.Control | Keys.D5));
                }
            }
        }

        return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    private bool IsMainWindowInForeground()
    {
        IntPtr foregroundWindowHandle = GetForegroundWindow();
        if (foregroundWindowHandle != IntPtr.Zero)
        {
            uint foregroundProcessId;
            GetWindowThreadProcessId(foregroundWindowHandle, out foregroundProcessId);

            if (foregroundProcessId != 0 && foregroundProcessId == Process.GetCurrentProcess().Id)
            {
                // The main window of your application is in the foreground
                return true;
            }
        }

        return false;
    }

    public void Dispose()
    {
        UnhookWindowsHookEx(hookId);
    }

    // Import required WinAPI functions
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}

public class KeyPressedEventArgs : EventArgs
{
    public Keys KeyPressed { get; private set; }

    public KeyPressedEventArgs(Keys keyPressed)
    {
        KeyPressed = keyPressed;
    }
}
