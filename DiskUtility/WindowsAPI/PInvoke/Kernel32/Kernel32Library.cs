﻿using System.Runtime.InteropServices;
using System.Text;

// 抑制 CA1401 警告
#pragma warning disable CA1401

namespace DiskUtility.WindowsAPI.PInvoke.Kernel32
{
    /// <summary>
    /// Kernel32.dll 函数库
    /// </summary>
    public static class Kernel32Library
    {
        private const string Kernel32 = "kernel32.dll";

        public const long APPMODEL_ERROR_NO_PACKAGE = 15700L;

        /// <summary>
        /// 获取当前进程的 应用程序用户模型 ID 。
        /// </summary>
        /// <param name="applicationUserModelIdLength">输入时， applicationUserModelId 缓冲区的大小（以宽字符为单位）。 成功时，使用的缓冲区大小，包括 null 终止符。</param>
        /// <param name="applicationUserModelId">指向接收应用程序用户模型 ID 的缓冲区的指针。</param>
        /// <returns>如果该函数成功，则返回 ERROR_SUCCESS。 否则，该函数将返回错误代码。</returns>
        [DllImport(Kernel32, CharSet = CharSet.Unicode, EntryPoint = "GetCurrentApplicationUserModelId", PreserveSig = true, SetLastError = false)]
        public static extern int GetCurrentApplicationUserModelId(ref uint applicationUserModelIdLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder applicationUserModelId);

        /// <summary>
        /// 获取调用进程的包系列名称。
        /// </summary>
        /// <param name="packageFamilyNameLength">输入时， packageFamilyName 缓冲区的大小（以字符为单位），包括 null 终止符。 输出时，返回的包系列名称的大小（以字符为单位），包括 null 终止符。</param>
        /// <param name="packageFamilyName">包系列名称。</param>
        /// <returns>如果函数成功，则返回 ERROR_SUCCESS。 否则，函数将返回错误代码。</returns>
        [DllImport(Kernel32, CharSet = CharSet.Unicode, EntryPoint = "GetCurrentPackageFamilyName", PreserveSig = true, SetLastError = false)]
        public static extern int GetCurrentPackageFamilyName(ref int packageFamilyNameLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder packageFamilyName);

        /// <summary>
        /// 检索系统的电源状态。 状态指示系统是使用交流还是直流电源运行，电池当前是否正在充电，剩余的电池使用时间，以及节电模式是打开还是关闭。
        /// </summary>
        /// <param name="systemPowerStatus">指向接收状态信息的 SYSTEM_POWER_STATUS 结构的指针。</param>
        [DllImport(Kernel32, CharSet = CharSet.Unicode, EntryPoint = "GetSystemPowerStatus", PreserveSig = true, SetLastError = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS lpSystemPowerStatus);
    }
}
