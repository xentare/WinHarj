using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace iptracer
{
    class IPProperties
    {

        IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

        public IPProperties()
        {
            
        }

        public void GetOpenPorts()
        {
            try
            {
                using (Process p = new Process())
                {
                    ProcessStartInfo ps = new ProcessStartInfo();
                    ps.Arguments = "-a -n -o";
                    ps.FileName = "netstat.exe";
                    ps.UseShellExecute = false;
                    ps.WindowStyle = ProcessWindowStyle.Hidden;
                    ps.RedirectStandardInput = true;
                    ps.RedirectStandardError = true;
                    ps.RedirectStandardOutput = true;

                    p.StartInfo = ps;
                    p.Start();

                    StreamReader stdOutput = p.StandardOutput;
                    StreamReader stdError = p.StandardError;

                    string content = stdOutput.ReadToEnd() + stdError.ReadToEnd();
                    string exitStatus = p.ExitCode.ToString();

                    if (exitStatus != "0")
                    {
                        throw new Exception("Error on opening netstat");
                    }

                    Console.WriteLine(content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<string> GetOpenConnections()
        {
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            List<string> list = new List<string>();

/*            foreach (TcpConnectionInformation c in connections)
            {
                if (c.State == TcpState.Established)
                {
                    var connection = c.LocalEndPoint + "<=>" + c.RemoteEndPoint;
                    list.Add(connection);
                }

            }*/

            return list;
        }

    }

}
