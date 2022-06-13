using enki.dict.Commom.http;
using System;
using System.Management;

namespace FishWork.Commom
{
    public class SelfInfo
    {    /// <summary>
         /// 取程序版本号
         /// 1.0.0.1
         /// </summary>
         /// <returns></returns>
        public static string GetAppVersion()
        {
            string str = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return str;
        }



        /// <summary>
        /// 取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string GetDiskID()
        {
            try
            {
                //获取硬盘ID 
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HttpClient.MD5Entry(HDid);
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        /// <summary>
        /// 取操作系统名称
        /// </summary>
        /// <returns></returns>
        public static string GetSystemName()
        {
            try
            {
                string _operatingSystem = "";
                string _osArchitecture = "";
                using (ManagementObjectSearcher win32OperatingSystem = new ManagementObjectSearcher("select * from Win32_OperatingSystem"))
                {
                    foreach (ManagementObject obj in win32OperatingSystem.Get())
                    {
                        _operatingSystem = obj["Caption"].ToString();
                        _osArchitecture = obj["OSArchitecture"].ToString();
                        break;
                    }
                }
                return _operatingSystem + _osArchitecture;
            }
            catch
            {
                return Environment.OSVersion.VersionString;
            }
        }

        /// <summary>
        /// 获取计算机名称
        /// </summary>
        /// <returns></returns>
        public static string GetMachineName()
        {
            return Environment.MachineName;
        }
    }
}
