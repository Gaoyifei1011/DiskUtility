using System;
using System.Drawing;
using System.Runtime.InteropServices;

// 抑制 CA1401 警告
#pragma warning disable CA1401

namespace DiskUtility.WindowsAPI.PInvoke.User32
{
    /// <summary>
    /// User32.dll 函数库
    /// </summary>
    public static class User32Library
    {
        private const string User32 = "user32.dll";

        /// <summary>
        /// 注册应用程序以接收特定电源设置事件的电源设置通知。
        /// </summary>
        /// <param name="hRecipient">指示电源设置通知的发送位置的句柄。 对于交互式应用程序， dwFlags 参数应为零， hRecipient 参数应为窗口句柄。 对于服务，dwFlags 参数应为 1，hRecipient 参数应为从 RegisterServiceCtrlHandlerEx 返回的SERVICE_STATUS_HANDLE。</param>
        /// <param name="PowerSettingGuid">要为其发送通知的电源设置的 GUID 。</param>
        /// <param name="Flags">
        /// DEVICE_NOTIFY_WINDOW_HANDLE：使用 wParam 参数为 PBT_POWERSETTINGCHANGE 的WM_POWERBROADCAST消息发送通知。
        /// DEVICE_NOTIFY_SERVICE_HANDLE：通知发送到 HandlerEx 回调函数，其中 dwControl 参数 为 SERVICE_CONTROL_POWEREVENT ， dwEventType为 PBT_POWERSETTINGCHANGE。
        /// </param>
        /// <returns>返回用于取消注册电源通知的通知句柄。 如果函数失败，则返回值为 NULL。</returns>
        [DllImport(User32, CharSet = CharSet.Unicode, EntryPoint = "RegisterPowerSettingNotification", PreserveSig = true, SetLastError = false)]
        public static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, in Guid PowerSettingGuid, uint Flags);

        /// <summary>
        /// 取消注册电源设置通知。
        /// </summary>
        /// <param name="handle">从 RegisterPowerSettingNotification 函数返回的句柄。</param>
        /// <returns>如果该函数成功，则返回值为非零值。如果函数失败，则返回值为零。</returns>
        [DllImport(User32, CharSet = CharSet.Unicode, EntryPoint = "UnregisterPowerSettingNotification", PreserveSig = true, SetLastError = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterPowerSettingNotification(IntPtr handle);
    }
}
