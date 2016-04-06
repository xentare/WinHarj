using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace iptracer
{
    class Program
    {

        public static IEnumerable<TcpRow> GetTcpTable()
        {
            return ManagedIpHelper.GetExtendedTcpTable(true);
        }

        public static void GetTcpTableConsole()
        {
            Console.WriteLine("Active Connections");
            Console.WriteLine();
            Console.WriteLine("  Proto  Local Address          Foreign Address        State         PID");
            foreach (TcpRow tcpRow in ManagedIpHelper.GetExtendedTcpTable(true))
            {
                Console.WriteLine("  {0,-7}{1,-23}{2, -23}{3,-14}{4}", "TCP", tcpRow.LocalEndPoint, tcpRow.RemoteEndPoint, tcpRow.State, tcpRow.ProcessId);
                try
                {
                    Process process = Process.GetProcessById(tcpRow.ProcessId);
                    if (process.ProcessName != "System")
                    {
                        foreach (ProcessModule processModule in process.Modules)
                        {
                            Console.WriteLine("  {0}", processModule.FileName);
                        }

                        Console.WriteLine("  [{0}]", Path.GetFileName(process.MainModule.FileName));
                    }
                    else
                    {
                        Console.WriteLine("  -- unknown component(s) --");
                        Console.WriteLine("  [{0}]", "System");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine();
            }

            Console.Write("{0}Press any key to continue...", Environment.NewLine);
            Console.ReadKey();
        }
    }
}
