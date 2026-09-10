using System;
using System.Runtime.InteropServices;

namespace Celestite.Utils.Interop.Windows
{
    public static partial class Carleen
    {
        private const string DLL_NAME = "Carleen";

        private static partial class Native
        {
            [LibraryImport(DLL_NAME, SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
            public static partial int GetAvailableVersion(
                [MarshalAs(UnmanagedType.LPWStr)] string browserExecutableFolder,
                [MarshalAs(UnmanagedType.LPWStr)] out string versionInfo);

            [LibraryImport(DLL_NAME, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool VersionCheck();

            // HWND parentHandle, PCWSTR browserExecutableFolder, PCWSTR userDataFolder, PCWSTR additionalBrowserArguments
            [LibraryImport(DLL_NAME, SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
            public static partial int CreateWebView2Environment(
                [MarshalAs(UnmanagedType.LPWStr)] string browserExecutableFolder,
                [MarshalAs(UnmanagedType.LPWStr)] string userDataFolder,
                [MarshalAs(UnmanagedType.LPWStr)] string additionalBrowserArguments);

            [LibraryImport(DLL_NAME)]
            public static partial void InitTabHistoryUpdateCallback(TabHistoryUpdateCallback callback);
            [LibraryImport(DLL_NAME)]
            public static partial void InitTabNavigationStatusChangedCallback(TabNavigatingStatusChangedCallback callback);
            [LibraryImport(DLL_NAME)]
            public static partial void InitLogCallback(LogCallback logCallback);
            [LibraryImport(DLL_NAME)]
            public static partial void InitTabDocumentTitleChangedCallback(TabDocumentTitleChangedCallback logCallback);
            [LibraryImport(DLL_NAME)]
            public static partial void InitNewWindowRequestedCallback(NewWindowRequestedCallback logCallback);
            [LibraryImport(DLL_NAME)]
            public static partial void InitUriProcessedCallback(UriProcessedCallback logCallback);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.SysInt)]
            public static partial nint CreateNativeWindow(HWND hWnd);

            [LibraryImport(DLL_NAME)]
            public static partial void DestroyNativeWindow(nint pNativeWindow);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool OnNativeControlSizeChanged(nint pNativeWindow, RECT rect);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool AvaCreateTab(nint pNativeWindow, long tabId, [MarshalAs(UnmanagedType.LPWStr)] string navigateUrl, [MarshalAs(UnmanagedType.Bool)] bool shouldBeActive, nint pCallerTab);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool AvaSwitchTab(nint pNativeWindow, long tabId);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool AvaRemoveTab(nint pNativeWindow, long tabId);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool AvaGoForward(nint pNativeWindow);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool AvaGoBack(nint pNativeWindow);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool AvaRefresh(nint pNativeWindow);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool AvaNavigate(nint pNativeWindow, [MarshalAs(UnmanagedType.LPWStr)] string uri);

            [LibraryImport(DLL_NAME)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool AvaDropTabOutside(nint pNativeWindow, long tabId, nint pTargetNativeWindow);

            [LibraryImport(DLL_NAME)]
            public static partial void UpdateCurrentUser([MarshalAs(UnmanagedType.LPWStr)] string email, [MarshalAs(UnmanagedType.LPWStr)] string loginSecureId, [MarshalAs(UnmanagedType.LPWStr)] string loginSessionId);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TabHistoryUpdateCallback(nint pNativeWindow, int tabId, bool canGoForward, bool canGoBack);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TabNavigatingStatusChangedCallback(nint pNativeWindow, int tabId, [MarshalAs(UnmanagedType.LPWStr)] string uri, bool toReload);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LogCallback(int level, [MarshalAs(UnmanagedType.LPWStr)] string logString);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TabDocumentTitleChangedCallback(nint pNativeWindow, int tabId, [MarshalAs(UnmanagedType.LPWStr)] string documentTitle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void NewWindowRequestedCallback(nint pNativeWindow, [MarshalAs(UnmanagedType.LPWStr)] string uri, nint pCallerTab);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void UriProcessedCallback([MarshalAs(UnmanagedType.LPWStr)] string uri);

        public static int GetAvailableVersion(string browserExecutableFolder, out string versionInfo)
        {
            try
            {
                return Native.GetAvailableVersion(browserExecutableFolder, out versionInfo);
            }
            catch
            {
                versionInfo = string.Empty;
                return -1;
            }
        }

        public static bool VersionCheck()
        {
            try
            {
                return Native.VersionCheck();
            }
            catch
            {
                return false;
            }
        }

        public static int CreateWebView2Environment(string browserExecutableFolder, string userDataFolder, string additionalBrowserArguments)
        {
            try
            {
                return Native.CreateWebView2Environment(browserExecutableFolder, userDataFolder, additionalBrowserArguments);
            }
            catch
            {
                return -1;
            }
        }

        public static void InitTabHistoryUpdateCallback(TabHistoryUpdateCallback callback)
        {
            try { Native.InitTabHistoryUpdateCallback(callback); } catch { }
        }

        public static void InitTabNavigationStatusChangedCallback(TabNavigatingStatusChangedCallback callback)
        {
            try { Native.InitTabNavigationStatusChangedCallback(callback); } catch { }
        }

        public static void InitLogCallback(LogCallback logCallback)
        {
            try { Native.InitLogCallback(logCallback); } catch { }
        }

        public static void InitTabDocumentTitleChangedCallback(TabDocumentTitleChangedCallback logCallback)
        {
            try { Native.InitTabDocumentTitleChangedCallback(logCallback); } catch { }
        }

        public static void InitNewWindowRequestedCallback(NewWindowRequestedCallback logCallback)
        {
            try { Native.InitNewWindowRequestedCallback(logCallback); } catch { }
        }

        public static void InitUriProcessedCallback(UriProcessedCallback logCallback)
        {
            try { Native.InitUriProcessedCallback(logCallback); } catch { }
        }

        public static nint CreateNativeWindow(HWND hWnd)
        {
            try
            {
                return Native.CreateNativeWindow(hWnd);
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        public static void DestroyNativeWindow(nint pNativeWindow)
        {
            try
            {
                Native.DestroyNativeWindow(pNativeWindow);
            }
            catch { }
        }

        public static bool OnNativeControlSizeChanged(nint pNativeWindow, RECT rect)
        {
            try
            {
                return Native.OnNativeControlSizeChanged(pNativeWindow, rect);
            }
            catch
            {
                return false;
            }
        }

        public static bool AvaCreateTab(nint pNativeWindow, long tabId, string navigateUrl, bool shouldBeActive, nint pCallerTab)
        {
            try
            {
                return Native.AvaCreateTab(pNativeWindow, tabId, navigateUrl, shouldBeActive, pCallerTab);
            }
            catch
            {
                return false;
            }
        }

        public static bool AvaSwitchTab(nint pNativeWindow, long tabId)
        {
            try
            {
                return Native.AvaSwitchTab(pNativeWindow, tabId);
            }
            catch
            {
                return false;
            }
        }

        public static bool AvaRemoveTab(nint pNativeWindow, long tabId)
        {
            try
            {
                return Native.AvaRemoveTab(pNativeWindow, tabId);
            }
            catch
            {
                return false;
            }
        }

        public static bool AvaGoForward(nint pNativeWindow)
        {
            try
            {
                return Native.AvaGoForward(pNativeWindow);
            }
            catch
            {
                return false;
            }
        }

        public static bool AvaGoBack(nint pNativeWindow)
        {
            try
            {
                return Native.AvaGoBack(pNativeWindow);
            }
            catch
            {
                return false;
            }
        }

        public static bool AvaRefresh(nint pNativeWindow)
        {
            try
            {
                return Native.AvaRefresh(pNativeWindow);
            }
            catch
            {
                return false;
            }
        }

        public static bool AvaNavigate(nint pNativeWindow, string uri)
        {
            try
            {
                return Native.AvaNavigate(pNativeWindow, uri);
            }
            catch
            {
                return false;
            }
        }

        public static bool AvaDropTabOutside(nint pNativeWindow, long tabId, nint pTargetNativeWindow)
        {
            try
            {
                return Native.AvaDropTabOutside(pNativeWindow, tabId, pTargetNativeWindow);
            }
            catch
            {
                return false;
            }
        }

        public static void UpdateCurrentUser(string email, string loginSecureId, string loginSessionId)
        {
            try
            {
                Native.UpdateCurrentUser(email, loginSecureId, loginSessionId);
            }
            catch
            {
            }
        }
    }
}
