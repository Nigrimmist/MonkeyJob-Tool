using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.Modules.Modules
{
    public class MemoryUsageTrayModule : ModuleTrayBase
    {
        private ITrayClient _client;
        public override void Init(ITrayClient client)
        {
            _client = client;
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromSeconds(1); }
        }

        public override void OnFire(Guid trayModuleToken)
        {
            try
            {
                Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
                decimal percentFree = ((decimal) phav/(decimal) tot)*100;
                decimal percentOccupied = 100 - percentFree;

                Color? borderColor = null;

                if (percentOccupied > 80)
                    borderColor = Color.DarkRed;
                else if (percentOccupied > 70)
                    borderColor = Color.Orange;
                
                _client.UpdateTrayText(trayModuleToken, (int)percentOccupied + "%", Color.White, Color.Black, 6, "Tahoma", borderColor);
            }
            catch (PingException ex)
            {
                _client.UpdateTrayText(trayModuleToken, "ERR", Color.White, Color.Black, 6, "Tahoma", Color.Red);
            }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAABNUlEQVRYhc2WIXIDMQxFHyjoEQoLCwoCA3uIHkBHCAgvDAgILAwsXFgYEBAYuKAgMCCgR2iA7Yk6s+vRdmVvPfPZt/zGsiSDbV2AH6N2xpim9QR8DjjcHeBFBd4D2x7tla8FROnBC0AyPukBaGKMqgA6BW+eAEvgsUdL5fuOEB/eAFad4sELbwDrDRzjvrk3gGR8onxnQmWspwIolgJrHyiWgiYG7FJDhSqQjE+okAIrgE7ByhMgBevSSvlSJ9wQ3ofbLBgyDZ+B9zEHjwWYE/qAK4C1ClIKGkIaqo9jPQ03TFAFxfqAtQrSh+TVG8CqYgCS8QkVUmAFmPwGik3DlnC1XWqVr9iHZPIqkIxP/hOA7oSuANZZkN7AltuD/POaAV8quEUHfn/Z78cAANwRmsuQcWxeV7d3Oo71BEw0AAAAAElFTkSuQmCC"; }
        }

        public override string ModuleTitle
        {
            get { return "Free Memory"; }
        }

        public override string ModuleDescription
        {
            get { return "Выводит в трей количество занятой RAM-памяти.";}
        }

        public override string TrayIconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAABNUlEQVRYhc2WIXIDMQxFHyjoEQoLCwoCA3uIHkBHCAgvDAgILAwsXFgYEBAYuKAgMCCgR2iA7Yk6s+vRdmVvPfPZt/zGsiSDbV2AH6N2xpim9QR8DjjcHeBFBd4D2x7tla8FROnBC0AyPukBaGKMqgA6BW+eAEvgsUdL5fuOEB/eAFad4sELbwDrDRzjvrk3gGR8onxnQmWspwIolgJrHyiWgiYG7FJDhSqQjE+okAIrgE7ByhMgBevSSvlSJ9wQ3ofbLBgyDZ+B9zEHjwWYE/qAK4C1ClIKGkIaqo9jPQ03TFAFxfqAtQrSh+TVG8CqYgCS8QkVUmAFmPwGik3DlnC1XWqVr9iHZPIqkIxP/hOA7oSuANZZkN7AltuD/POaAV8quEUHfn/Z78cAANwRmsuQcWxeV7d3Oo71BEw0AAAAAElFTkSuQmCC"; }
        }

        
    }

    public static class PerformanceInfo
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        public static Int64 GetPhysicalAvailableMemoryInMiB()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }

        }

        public static Int64 GetTotalMemoryInMiB()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }

        }
    }
}
