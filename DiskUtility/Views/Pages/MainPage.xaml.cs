using DiskUtility.Views.Windows;
using DiskUtility.WindowsAPI.PInvoke.User32;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 抑制 CA1822，IDE0060 警告
#pragma warning disable CA1822,IDE0060

namespace DiskUtility.Views.Pages
{
    /// <summary>
    /// 应用主窗口页面
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private CaptionButton? pressedButton;

        // 用于避免重复设置状态
        private bool allInNormal = true;

        private bool isWindowActive = true;

        private ElementTheme _windowTheme;

        public ElementTheme WindowTheme
        {
            get { return _windowTheme; }

            set
            {
                if (!Equals(_windowTheme, value))
                {
                    _windowTheme = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowTheme)));
                }
            }
        }

        private bool _isBackEnabled;

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }

            set
            {
                if (!Equals(_isBackEnabled, value))
                {
                    _isBackEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBackEnabled)));
                }
            }
        }

        private bool _isWindowMinimizeEnabled;

        public bool IsWindowMinimizeEnabled
        {
            get { return _isWindowMinimizeEnabled; }

            set
            {
                if (!Equals(_isWindowMinimizeEnabled, value))
                {
                    _isWindowMinimizeEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsWindowMaximized)));
                }
            }
        }

        private bool _isWindowMaximized;

        public bool IsWindowMaximized
        {
            get { return _isWindowMaximized; }

            set
            {
                if (!Equals(_isWindowMaximized, value))
                {
                    _isWindowMaximized = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsWindowMaximized)));
                }
            }
        }

        private bool _isWindowMaximizeEnabled;

        public bool IsWindowMaximizeEnabled
        {
            get { return _isWindowMaximizeEnabled; }

            set
            {
                if (!Equals(_isWindowMaximizeEnabled, value))
                {
                    _isWindowMaximizeEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsWindowMaximized)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            InitializeComponent();
        }

        #region 第一部分：窗口右键菜单事件

        /// <summary>
        /// 窗口还原
        /// </summary>
        private void OnRestoreClicked(object sender, RoutedEventArgs args)
        {
            MainWindow.Current.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// 窗口移动
        /// </summary>
        private async void OnMoveClicked(object sender, RoutedEventArgs args)
        {
            if (sender is MenuFlyoutItem menuFlyoutItem && menuFlyoutItem.Tag is MenuFlyout menuFlyout)
            {
                menuFlyout.Hide();
                await Task.Delay(10);
                User32Library.SendMessage(MainWindow.Current.Handle, WindowMessage.WM_SYSCOMMAND, new UIntPtr(0xF010), IntPtr.Zero);
            }
        }

        /// <summary>
        /// 窗口大小
        /// </summary>
        private void OnSizeClicked(object sender, RoutedEventArgs args)
        {
            if (sender is MenuFlyoutItem menuFlyoutItem && menuFlyoutItem.Tag is MenuFlyout menuFlyout)
            {
                menuFlyout.Hide();
                User32Library.SendMessage(MainWindow.Current.Handle, WindowMessage.WM_SYSCOMMAND, new UIntPtr(0xF000), IntPtr.Zero);
            }
        }

        /// <summary>
        /// 窗口最小化
        /// </summary>
        private void OnMinimizeClicked(object sender, RoutedEventArgs args)
        {
            MainWindow.Current.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 窗口最大化
        /// </summary>
        private void OnMaximizeClicked(object sender, RoutedEventArgs args)
        {
            MainWindow.Current.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        private void OnCloseClicked(object sender, RoutedEventArgs args)
        {
            MainWindow.Current.Close();
        }

        #endregion 第一部分：窗口右键菜单事件

        #region 第二部分：导航控件及其内容挂载的事件

        /// <summary>
        /// 导航控件加载完成后初始化内容，初始化导航控件属性和应用的背景色
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            MainWindow.Current.SetWindowBackdrop();
        }

        #endregion 第二部分：导航控件及其内容挂载的事件

        /// <summary>
        /// 修改窗口按钮的激活状态
        /// </summary>
        public void ChangeButtonActiveState(bool value)
        {
            isWindowActive = value;
            string activeState = value ? "Normal" : "NotActive";

            VisualStateManager.GoToState(this, value ? "Active" : "NotActive", false);

            if (IsWindowMinimizeEnabled)
            {
                VisualStateManager.GoToState(MinimizeButton, activeState, false);
            }

            if (IsWindowMaximizeEnabled)
            {
                VisualStateManager.GoToState(MaximizeButton, activeState, false);
            }

            VisualStateManager.GoToState(CloseButton, activeState, false);
        }

        /// <summary>
        /// 修改按钮的可用状态
        /// </summary>
        public void ChangeButtonEnabledState(CaptionButton button, bool enabled)
        {
            if (button is CaptionButton.Minimize)
            {
                if (enabled)
                {
                    VisualStateManager.GoToState(MinimizeButton, "Normal", false);
                }
                else
                {
                    VisualStateManager.GoToState(MinimizeButton, "Disabled", false);
                }
            }
            else if (button is CaptionButton.Maximize)
            {
                if (enabled)
                {
                    VisualStateManager.GoToState(MaximizeButton, "Normal", false);
                }
                else
                {
                    VisualStateManager.GoToState(MaximizeButton, "Disabled", false);
                }
            }
        }

        /// <summary>
        /// 修改最大化按钮的图标
        /// </summary>
        public void ChangeMaximizeButtonIcon(bool isMaximized)
        {
            VisualStateManager.GoToState(MaximizeButton, isMaximized ? "WindowStateMaximized" : "WindowStateNormal", false);
        }

        /// <summary>
        /// 鼠标移动到某个按钮上时调用
        /// </summary>
        public void HoverButton(CaptionButton button)
        {
            if (pressedButton is not null && pressedButton.HasValue)
            {
                bool hoveringOnPressedButton = Equals(pressedButton.Value, button);
                allInNormal = !hoveringOnPressedButton;

                if (IsWindowMinimizeEnabled)
                {
                    VisualStateManager.GoToState(MinimizeButton, hoveringOnPressedButton && button is CaptionButton.Minimize ? "Pressed" : "Normal", false);
                }

                if (IsWindowMaximizeEnabled)
                {
                    VisualStateManager.GoToState(MaximizeButton, hoveringOnPressedButton && button is CaptionButton.Maximize ? "Pressed" : "Normal", false);
                }

                VisualStateManager.GoToState(CloseButton, hoveringOnPressedButton && button is CaptionButton.Close ? "Pressed" : "Normal", false);
            }
            else
            {
                allInNormal = false;
                string activeState = isWindowActive ? "Normal" : "NotActive";

                if (IsWindowMinimizeEnabled)
                {
                    VisualStateManager.GoToState(MinimizeButton, button is CaptionButton.Minimize ? "PointerOver" : activeState, false);
                }

                if (IsWindowMaximizeEnabled)
                {
                    VisualStateManager.GoToState(MaximizeButton, button is CaptionButton.Maximize ? "PointerOver" : activeState, false);
                }

                VisualStateManager.GoToState(CloseButton, button is CaptionButton.Close ? "PointerOver" : activeState, false);
            }
        }

        /// <summary>
        /// 在某个按钮上按下鼠标时调用
        /// </summary>
        public void PressButton(CaptionButton button)
        {
            allInNormal = false;
            pressedButton = button;

            if (IsWindowMinimizeEnabled)
            {
                VisualStateManager.GoToState(MinimizeButton, button is CaptionButton.Minimize ? "Pressed" : "Normal", false);
            }

            if (IsWindowMaximizeEnabled)
            {
                VisualStateManager.GoToState(MaximizeButton, button is CaptionButton.Maximize ? "Pressed" : "Normal", false);
            }

            VisualStateManager.GoToState(CloseButton, button is CaptionButton.Close ? "Pressed" : "Normal", false);
        }

        /// <summary>
        /// 在标题栏按钮上释放鼠标时调用
        /// </summary>
        public void ReleaseButton(CaptionButton button)
        {
            // 在某个按钮上按下然后释放视为点击，即使中途离开过也是如此，因为 HoverButton 和
            // LeaveButtons 都不改变 _pressedButton
            bool clicked = pressedButton.HasValue && Equals(pressedButton.Value, button);

            if (clicked)
            {
                // 用户点击了某个按钮
                switch (pressedButton.Value)
                {
                    case CaptionButton.Minimize:
                        {
                            User32Library.PostMessage(MainWindow.Current.Handle, WindowMessage.WM_SYSCOMMAND, new UIntPtr((uint)SYSTEMCOMMAND.SC_MINIMIZE), IntPtr.Zero);
                            break;
                        }
                    case CaptionButton.Maximize:
                        {
                            User32Library.PostMessage(MainWindow.Current.Handle, WindowMessage.WM_SYSCOMMAND, IsWindowMaximized ? new UIntPtr((uint)SYSTEMCOMMAND.SC_RESTORE) : new UIntPtr((uint)SYSTEMCOMMAND.SC_MAXIMIZE), IntPtr.Zero);
                            break;
                        }
                    case CaptionButton.Close:
                        {
                            User32Library.PostMessage(MainWindow.Current.Handle, WindowMessage.WM_SYSCOMMAND, new UIntPtr((uint)SYSTEMCOMMAND.SC_CLOSE), IntPtr.Zero);
                            break;
                        }
                }
            }

            pressedButton = null;

            // 如果点击了某个按钮则清空状态，因为此时窗口状态已改变。如果在某个按钮上按下然后在
            allInNormal = clicked;

            if (IsWindowMinimizeEnabled)
            {
                VisualStateManager.GoToState(MinimizeButton, !clicked && button is CaptionButton.Minimize ? "PointerOver" : "Normal", false);
            }

            if (IsWindowMaximizeEnabled)
            {
                VisualStateManager.GoToState(MaximizeButton, !clicked && button is CaptionButton.Maximize ? "PointerOver" : "Normal", false);
            }

            VisualStateManager.GoToState(CloseButton, !clicked && button is CaptionButton.Close ? "PointerOver" : "Normal", false);
        }

        /// <summary>
        /// 在非标题按钮上释放鼠标时调用
        /// </summary>
        public void ReleaseButtons()
        {
            if (pressedButton is not null || pressedButton.HasValue)
            {
                pressedButton = null;
                LeaveButtons();
            }
        }

        /// <summary>
        /// 离开标题按钮时调用，不更改 PressedButton
        /// </summary>
        public void LeaveButtons()
        {
            if (!allInNormal)
            {
                allInNormal = true;

                string activeState = isWindowActive ? "Normal" : "NotActive";

                if (IsWindowMinimizeEnabled)
                {
                    VisualStateManager.GoToState(MinimizeButton, activeState, true);
                }

                if (IsWindowMaximizeEnabled)
                {
                    VisualStateManager.GoToState(MaximizeButton, activeState, true);
                }

                VisualStateManager.GoToState(CloseButton, activeState, true);
            }
        }

        /// <summary>
        /// 获取控件的文字转向
        /// </summary>
        private global::Windows.UI.Xaml.FlowDirection GetControlDirection(RightToLeft rightToLeft)
        {
            return rightToLeft is RightToLeft.Yes ? global::Windows.UI.Xaml.FlowDirection.RightToLeft : global::Windows.UI.Xaml.FlowDirection.LeftToRight;
        }

        private bool GetWindowMaximizeState(bool isWindowMaximized, bool isWindowMaximizeEnabled, string isReverse)
        {
            return isWindowMaximizeEnabled && (string.Equals(isReverse, nameof(isReverse)) ? Equals(isWindowMaximized, false) : isWindowMaximized);
        }
    }
}
