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
using System.IO;

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


    public struct description
    {
        public char type;
        public string[] typeDesc;
        public string directory;
    }

    



    public partial class MainWindow : Window
    {
        public static description playerDesc;
        public static description enemyDesc;
        public static description mapDesc;
        public static description[] maps;
        public static description[] players;
        public static description[] enemies;
        public MainWindow()
        {
            InitializeComponent();
            enemyDesc = Retrieval.getEnemies();
            mapRetrieval.getMaps();
            Retrieval.getPlayers();
            PlayerDescBox.Text = playerDesc.typeDesc[0] + playerDesc.typeDesc[1];
            EnemyDescBox.Text = enemyDesc.typeDesc[0] + enemyDesc.typeDesc[1];
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
            string directory = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            try
            {
                using (StreamWriter w = new StreamWriter(directory))
                {

                }
            }
            catch (Exception er)
            {
                Console.WriteLine($"#cachewrite# {er}");
            }


            return success;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
                
            Process newGame = new Process();
            newGame.StartInfo.FileName = getDirectory.getExecutable();
            newGame.EnableRaisingEvents = true;
            newGame.Start();
            Close();

        }



       

        private void mapButtonPlus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mapButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void pButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            Retrieval.selectPlayer(1);
        }

        private void pButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            Retrieval.selectPlayer(-1);
        }

        private void aiButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            Retrieval.selectEnemy(1);
        }

        private void aiButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            Retrieval.selectEnemy(-1);
        }

        private void dButtonPlus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dButtonMinus_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
