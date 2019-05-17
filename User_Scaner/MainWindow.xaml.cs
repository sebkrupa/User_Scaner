using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;

namespace User_Scaner
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> users = new List<User>();
        Thread thread;
        public MainWindow()
        {
            InitializeComponent();
            Rozpocznij();
        }

        private void Rozpocznij()
        {
            users.Clear();
            stack.Children.Clear();

            utworzListe();

            Action onCompleted = () =>
            {
                Koniec();
            };

            thread = new Thread(
                () =>
                {
                    try
                    {
                        Skanuj();
                    }
                    finally
                    {
                        onCompleted();
                    }
                });
            thread.Start();
        }

        private void utworzListe()
        {
            users.Add(new User() { hostName = "zywlap001", nazwa = "Paweł Berdychowski" });
            users.Add(new User() { hostName = "zywlap012", nazwa = "Marcin Wójcik" });
            users.Add(new User() { hostName = "zywlap016", nazwa = "Szymon Łęcki" });
            users.Add(new User() { hostName = "zywlap030", nazwa = "Ola Wyszomirska" });
            users.Add(new User() { hostName = "zywlap028", nazwa = "Patryk Łuczak" });
            users.Add(new User() { hostName = "zywlap032", nazwa = "Szymon Jeż" });
            users.Add(new User() { hostName = "zywlap015", nazwa = "Waldemar Stando" });
            users.Add(new User() { hostName = "zywlap035", nazwa = "Jan Salamon" });
            users.Add(new User() { hostName = "zywlap044", nazwa = "Tomek Harężlak" });
            users.Add(new User() { hostName = "zywlap034", nazwa = "Dawid Holisz" });
            users.Add(new User() { hostName = "zywwks035", nazwa = "KIOSK TIPS" });

        }

        private void Skanuj()
        {
            foreach(User user in users.OrderBy(x=>x.hostName))
             utworzKontrolke(PingHost(user.hostName), user);

        }

        private void Koniec()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                Label label = new Label();
                label.Content = "--------KONIEC--------";
                label.HorizontalAlignment = HorizontalAlignment.Center;
                stack.Children.Add(label);

                Button button = new Button();
                button.Content = "Refresh";
                button.Width = 100;
                button.Height = 40;
                button.Background = Brushes.Black;
                button.Foreground = Brushes.White;
                button.Click += Button_Click;

                stack.Children.Add(button);
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Rozpocznij();
        }

        public static bool PingHost(string hostName)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(hostName,3000);
                pingable = reply.Status == IPStatus.Success;
            }
            catch(PingException ee)
            {
                //MessageBox.Show(ee.Message);
            }
            finally
            {
                if(pinger!= null)
                {
                    pinger.Dispose();
                }
            }
            return pingable;
        }

        
        private void utworzKontrolke(bool status,User user)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBlock block = new TextBlock();
                block.Width = Double.NaN;
                block.Height = 20;
                block.Margin = new Thickness(10);
                if (!status)
                {
                    block.Text = user.ToString();
                    block.Background = Brushes.IndianRed;
                }
                else
                {
                    IPHostEntry ipHost = System.Net.Dns.GetHostEntry(user.hostName);
                    IPAddress[] addr = ipHost.AddressList;
                    string ip = addr[addr.Length - 1].ToString();
                    string zaklad = ktoryZaklad(addr[addr.Length - 1].ToString());


                    block.Text = user.ToString() + "______" + zaklad + "--- " + ip ;
                    if (zaklad != "Wieprz")
                        block.Background = Brushes.LightBlue;
                    else
                        block.Background = Brushes.LightGreen;

                }



                stack.Children.Add(block);
            }));
        }

        private string ktoryZaklad(string ip)
        {
            string[] adres = ip.Split('.');
            int fragment = Convert.ToInt16(adres[2]);
            string zaklad = string.Empty;
            if (fragment >= 80 && fragment <= 90)
                zaklad = "Wieprz";
            else if (fragment >= 48 && fragment <= 60)
                zaklad = "Wyszków";
            else if (fragment >= 16 && fragment <= 28)
                zaklad = "Bielsko";
            else if (fragment >= 0 && fragment <= 12)
                zaklad = "Wapienica";
            else
                zaklad = "NIEZNANY";

            return zaklad;
        }

    }

    public class User
    {
        public string hostName { get; set; }
        public string nazwa { get; set; }

        public override string ToString()
        {
            return $"{hostName}" + " - " + $"{nazwa}";
        }
    }
}
