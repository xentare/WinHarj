using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace iptracer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPProperties Properties;
        private Program Program;
        private bool showAdministratorDialog = true;
        private TcpRow selectedItem;

        public MainWindow()
        {
            InitializeComponent();
            InitIpTable();

            Properties = new IPProperties();
            Program = new Program();

            /*webBrowser.Source = new Uri(
                $"https://maps.googleapis.com/maps/api/staticmap?center=0,0&zoom=1&size=550x400&markers=color:red%7Clabel:C%7C50,50");*/

            webBrowser.NavigateToString($"<iframe Width=\"580\" Height=\"400\" frameborder=\"0\" style=\"border:0\"" +
                                        "src=\"https://www.google.com/maps/embed/v1/search?key=AIzaSyCOoohbl-ISim2qr12dni48B7uUNfsJVS8&q=0,0&center=0,0\"</iframe>");

            //ipListBox.DisplayMemberPath = Program.
        }

        public void InitIpTable()
        {
            TcpTable table = ManagedIpHelper.GetExtendedTcpTable(true);
            IpDataGrid.ItemsSource = table.KnownRows;
            SetFileNamesToIpDataGrid(table);

        }



        private void ipListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (IpDataGrid.SelectedItem != selectedItem)
                {
                    selectedItem = (TcpRow)IpDataGrid.SelectedItem;
                    var row = (TcpRow) IpDataGrid.CurrentItem;
                    Process exe = Process.GetProcessById(row.ProcessId);
                    tbExePath.Text = exe.MainModule.FileName;
                    tbStatus.Text = exe.MainModule.ModuleName;
                    lbDllPaths.ItemsSource = GetProcessDlls();
                    txtbHttp.Text = HttpRequest(row.RemoteEndPoint.ToString());
                    ResolvedIp resolvedIp =
                        JsonConvert.DeserializeObject<ResolvedIp>(HttpRequest(row.RemoteEndPoint.ToString()));
                    SetMap(resolvedIp.Latitude,resolvedIp.Longitude);
                }
            }
            catch(Win32Exception ex)
            {
                tbStatus.Text = ex.Message;
            }
            catch (Exception ex)
            {
                tbStatus.Text = ex.Message;
                if (showAdministratorDialog)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "You have to run this application in administrator mode to have access to certain resources",
                        "Admin privileges needed", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                    {
                        showAdministratorDialog = false;
                    }
                }
            }

        }

        public void SetFileNamesToIpDataGrid(TcpTable table)
        {
            try
            {
                foreach (TcpRow row in table.KnownRows)
                {
                    Process process = Process.GetProcessById(row.ProcessId);
                    row.FileName = process.MainModule.FileName;
                    Console.WriteLine(row.ProcessId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SetMap(string latitude, string longitude)
        {
            /*webBrowser.Source = new Uri(
               $"https://maps.googleapis.com/maps/api/staticmap?center=0,0&zoom=1&size=500x450&markers=color:red%7Clabel:C%7C{latitude},{longitude}");*/

            webBrowser.NavigateToString("<iframe Width=\"580\" Height=\"400\" frameborder=\"0\" style=\"border:0\"" +
                                        $"src=\"https://www.google.com/maps/embed/v1/place?key=AIzaSyCOoohbl-ISim2qr12dni48B7uUNfsJVS8&q={latitude},{longitude}&center=0,0\"</iframe>");

        }

        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser)
                .GetField("_axIWebBrowser2",
                          BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember(
                "Silent", BindingFlags.SetProperty, null, objComWebBrowser,
                new object[] { Hide });
        }

        public string HttpRequest(string ip)
        {
            string apiKey = iptracer.Properties.Settings.Default.ipDbApiKey;
            //Remove port from ip
            string newIp = ip.Substring(0, ip.IndexOf(':'));

            WebRequest request = WebRequest.Create(string.Format("http://api.ipinfodb.com/v3/ip-city/?key={0}&ip={1}&format=json",apiKey, newIp));

            WebResponse response = request.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            return reader.ReadToEnd();
        }

        private List<string> GetProcessDlls()
        {
            List<string> list = new List<string>();
            try
            {
                var row = (TcpRow)IpDataGrid.CurrentItem;
                Process process = Process.GetProcessById(row.ProcessId);
                string msg = "";
                foreach (ProcessModule mod in process.Modules)
                {
                    list.Add(mod.FileName);
                }
                list.Remove(process.MainModule.FileName);
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            InitIpTable();
        }

        private void WebBrowser_OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            HideScriptErrors((WebBrowser)sender, true);
        }
    }
}
