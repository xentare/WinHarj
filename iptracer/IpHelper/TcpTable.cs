using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace iptracer
{
    public class TcpTable : IEnumerable<TcpRow>
    {
        #region Private Fields

        private IEnumerable<TcpRow> tcpRows;

        #endregion

        #region Constructors

        public TcpTable(IEnumerable<TcpRow> tcpRows)
        {
            this.tcpRows = tcpRows;
        }

        #endregion

        #region Public Properties

        public IEnumerable<TcpRow> Rows
        {
            get { return this.tcpRows; }
            set { this.tcpRows = value; }
        }

        public IEnumerable<TcpRow> KnownRows
        {
            get
            {
                List<TcpRow> list = new List<TcpRow>();
                try {
                    foreach (TcpRow row in tcpRows)
                    {
                        Process process = Process.GetProcessById(row.ProcessId);
                        if (process.ProcessName == "System")
                        {
                            list.Add(row);
                        }
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return tcpRows.Except(list);
            }
        } 

        public IEnumerable<TcpRow> RemoteRows
        {
            get
            {
                return from row in Rows
                       where row.RemoteEndPoint.ToString().Substring(0, 3) != "127" && row.RemoteEndPoint.ToString().Substring(0, 1) != "0"
                       select row;
            }
        }

        public IEnumerable<TcpRow> KnownRowsWithoutIpTracerRows
        {
            get
            {
                List<TcpRow> list = new List<TcpRow>();
                try
                {
                    foreach (TcpRow row in tcpRows)
                    {
                        Process process = Process.GetProcessById(row.ProcessId);
                        Process current = Process.GetCurrentProcess();
                        if (process.Id == current.Id)
                        {
                            list.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return KnownRows.Except(list);
            }
        }

        public IEnumerable<TcpRow> RemoteRowsWithoutIpTracerRows
        {
            get
            {
                List<TcpRow> list = new List<TcpRow>();
                try
                {
                    foreach (TcpRow row in tcpRows)
                    {
                        Process process = Process.GetProcessById(row.ProcessId);
                        Process current = Process.GetCurrentProcess();
                        if (process.Id == current.Id)
                        {
                            list.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return RemoteRows.Except(list);
            }
        }

        #endregion

        #region IEnumerable<TcpRow> Members

        public IEnumerator<TcpRow> GetEnumerator()
        {
            return this.tcpRows.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.tcpRows.GetEnumerator();
        }

        #endregion
    }
}
