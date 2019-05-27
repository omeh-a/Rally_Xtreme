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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

namespace RallyXtreme_launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public struct prefs
    {
        public string mapDirectory;
        public string playerDirectory;
        public string aiDirectory;
        public int difficulty;
    }



    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        public class Map
            {
            public string name;
            public string imgPath;
            public ushort gridX;
            public ushort gridY;
            public string hitboxPath;
            public uint enemyNumber;
            public ushort difficulty;

            }

        public bool mapCacheWrite()
        {
            bool success = true;



            return success;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            
                Process newGame = new Process();
                newGame.StartInfo.FileName = "C:\\Users\\matth\\Documents\\GitHub\\Rally_Xtreme\\Game\\RallyXtreme\\RallyXtreme\\bin\\DesktopGL\\AnyCPU\\Debug\\rallyxtreme.exe";
                newGame.EnableRaisingEvents = true;
                newGame.Start();    
            


        }

        private void AIMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ControlsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
