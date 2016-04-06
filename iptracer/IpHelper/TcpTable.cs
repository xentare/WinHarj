using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public IEnumerable<TcpRow> KnownRows
        {
            get
            {
                List<TcpRow> list = new List<TcpRow>();

                foreach (TcpRow row in tcpRows)
                {
                    Process process = Process.GetProcessById(row.ProcessId);
                    if (process.ProcessName == "System")
                    {
                        list.Add(row);
                    }
                }
                return tcpRows.Except(list);
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
