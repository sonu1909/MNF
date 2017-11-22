using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MnfPic
{
    public static class MnfScan
    {
        public static bool IsScaning = false;
        public static void ScanPPL(TcpClient TC,int a, int b)
        {
            IsScaning = true;
            for (int i = a; i < b; i++)
            {
                string s = "<data avatar_details=\"1\" id=\"" + i + "\" />";
                NetworkStream ns = TC.GetStream();
                ns.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                Thread.Sleep(20);
            }
            IsScaning = false;
        }
    }
}
