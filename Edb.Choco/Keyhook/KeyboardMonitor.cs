using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edb.Environment.Keyhook
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class KeyboardMonitor
    {
        /// <summary>
        /// Необходимое свойство для того, чтобы хранить ссылку на делегат обработчика нажатий клавишь.
        /// В противном случае, если сразу же пережать делегат во внешний компонент, ссылка на него будет собрана сборщиком мусора и он перестанет работать.
        /// </summary>
        private static KeyHookExternal.HookProc _keyboardHookDelegate;

        /// <summary>
        /// Stores the handle to the Keyboard hook procedure.
        /// </summary>
        private static int _keyboardHookHandle;

        private static int _exclusiveIndexer;

        private static int _handlerIndexer;

        /// <summary>
        /// Список обработчиков нажатий клавиш
        /// </summary>
        private static Dictionary<int, Action<KeyboardEventArgs>> _keyPressHandlers;

        /// <summary>
        /// Список обработчиков нажатий клавишь клавиатуры, обрабатываемых эксклюзивно
        /// </summary>
        private static Dictionary<int, Action<KeyboardEventArgs>> _exclusiveKeyboardHandlers;

        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            var keyBoardHookInfo = (KeyHookExternal.KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyHookExternal.KeyboardHookStruct));
            var handled = false;
            if (wParam == KeyHookExternal.WM_KEYDOWN || wParam == KeyHookExternal.WM_SYSKEYDOWN || wParam == KeyHookExternal.WM_KEYUP || wParam == KeyHookExternal.WM_SYSKEYUP)
            {
                var keyType = wParam == KeyHookExternal.WM_SYSKEYDOWN || wParam == KeyHookExternal.WM_SYSKEYUP ? EKeyType.System : EKeyType.Common;
                var state = wParam == KeyHookExternal.WM_KEYDOWN || wParam == KeyHookExternal.WM_SYSKEYDOWN ? EState.Down : EState.Up;
                var keyData = (Keys)keyBoardHookInfo.VirtualKeyCode;
                var e = new KeyboardEventArgs(state, keyType, keyData);

                if (_exclusiveKeyboardHandlers.Any())
                {
                    foreach (var kvp in _exclusiveKeyboardHandlers)
                    {
                        if (kvp.Value != null)
                        {
                            kvp.Value.Invoke(e);
                            handled = nCode > 0 && e.Handled;
                        }
                    }
                }
                else
                {
                    foreach (var kvp in _keyPressHandlers)
                    {
                        if (kvp.Value != null)
                        {
                            kvp.Value.Invoke(e);
                        }
                    }
                }
            }

            if (handled)
            {
                return -1;
            }

            return KeyHookExternal.CallNextHookEx(_keyboardHookHandle, nCode, wParam, lParam);
        }

        static KeyboardMonitor()
        {
            _exclusiveKeyboardHandlers = new Dictionary<int, Action<KeyboardEventArgs>>();
            _keyPressHandlers = new Dictionary<int, Action<KeyboardEventArgs>>();
            InitializeMonitor();
        }

        /// <summary>
        /// Add common keyhook 
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public static int SubscribeCommonHandler(Action<KeyboardEventArgs> act)
        {
            _keyPressHandlers.Add(_handlerIndexer, act);
            _handlerIndexer--;
            return _handlerIndexer + 1;
        }

        /// <summary>
        /// Remove common keyhook
        /// </summary>
        /// <param name="key"></param>
        public static bool UnsubscribeCommonHandler(int key)
        {
            if (_keyPressHandlers.ContainsKey(key))
            {
                _keyPressHandlers.Remove(key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add common keyhook 
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public static int SubscribeExclusiveHandler(Action<KeyboardEventArgs> act)
        {
            _exclusiveKeyboardHandlers.Add(_exclusiveIndexer, act);
            _exclusiveIndexer++;
            return _exclusiveIndexer - 1;
        }

        /// <summary>
        /// Add common keyhook 
        /// </summary>
        /// <returns></returns>
        public static bool UnsubscribeExclusiveHandler(int key)
        {
            if (_exclusiveKeyboardHandlers.ContainsKey(key))
            {
                _exclusiveKeyboardHandlers.Remove(key);
                return true;
            }

            return false;
        }

        private static void InitializeMonitor()
        {
            if (_keyboardHookHandle == 0)
            {
                _keyboardHookDelegate = KeyboardHookProc;

                _keyboardHookHandle = KeyHookExternal.SetWindowsHookEx(
                    KeyHookExternal.WH_KEYBOARD_LL,
                    _keyboardHookDelegate,
                    KeyHookExternal.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);

                //If SetWindowsHookEx fails.
                if (_keyboardHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }
    }
}
