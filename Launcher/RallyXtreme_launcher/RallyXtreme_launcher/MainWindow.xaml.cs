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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;

namespace RallyXtreme_launcher
{
    /// <summary>
    /// Launcher for RallyXtreme
    /// </summary>
    /// 
    
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
        public static int selectedMap;
        public static int selectedenemy;
        public static int selectedplayer;
        public static short difficulty;
        public MainWindow()
        {
            InitializeComponent();
            enemies = Retrieval.getEnemies();
            maps = Retrieval.getMaps();
            players = Retrieval.getPlayers();
            selectedenemy = 0;
            selectedMap = 0;
            selectedplayer = 0;
            playerDesc = players[0];
            mapDesc = maps[0];
            enemyDesc = enemies[0];
            PlayerDescBox.Text = playerDesc.typeDesc[0] + "\n" +  playerDesc.typeDesc[1];
            EnemyDescBox.Text = enemyDesc.typeDesc[0] + "\n" + enemyDesc.typeDesc[1];
            MapDescBox.Text = mapDesc.typeDesc[0] + "\n" + mapDesc.typeDesc[1];
        }


        public bool mapCacheWrite()
        {
            bool success = true;
            string directory = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            try
            {
                using (StreamWriter w = new StreamWriter(directory + "\\cache.rxtcache"))
                {
                    w.WriteLine($"{mapDesc.directory}");
                    w.WriteLine($"{playerDesc.directory}");
                    w.WriteLine($"{enemyDesc.directory}");
                    w.WriteLine($"{difficulty}");
                    w.WriteLine($"1120");
                    w.WriteLine($"1050");
                }
                Console.WriteLine($"#cachewrite# map = {mapDesc.directory} #{selectedMap}");
                Console.WriteLine($"#cachewrite# player = {playerDesc.directory} #{selectedplayer}");
                Console.WriteLine($"#cachewrite# enemy  = {enemyDesc.directory} #{selectedenemy}");
                Console.WriteLine($"#cachewrite# difficulty = {difficulty}");
            }
            catch (Exception er)
            {
                Console.WriteLine($"#cachewrite# {er}");
            }


            return success;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            mapCacheWrite();
            string dir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\rallyXtreme.exe";
            Console.WriteLine($"#LAUNCHER# Launching -> {dir}");
            Process newGame = new Process();
            newGame.StartInfo.FileName = dir;
            newGame.EnableRaisingEvents = true;
            newGame.Start();

        }



       

        private void mapButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMap >= 0 && selectedMap < Retrieval.mapNum -1)
            {
                selectedMap += 1;
            }
            mapDesc = maps[selectedMap];
        }

        private void mapButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMap > 0 && selectedMap <= Retrieval.mapNum - 1)
            {
                selectedMap -= 1;
            }
            mapDesc = maps[selectedMap];
        }

        private void pButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedplayer >= 0 && selectedplayer < Retrieval.playerNum - 1)
            {
                selectedplayer += 1;
            }
            playerDesc = players[selectedplayer];
        }

        private void pButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedplayer > 0 && selectedplayer <= Retrieval.playerNum - 1)
            {
                selectedplayer -= 1;
            }
            playerDesc = players[selectedplayer];
        }

        private void aiButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedenemy >= 0 && selectedenemy < Retrieval.enemyNum - 1)
            {
                selectedenemy += 1;
            }
            enemyDesc = enemies[selectedplayer];
        }

        private void aiButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedenemy >= 0 && selectedenemy < Retrieval.enemyNum - 1)
            {
                selectedenemy += 1;
            }
            enemyDesc = enemies[selectedplayer];
        }

        private void dButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            if (difficulty < 4)
            {
                difficulty++;
            }
        }

        private void dButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            if (difficulty > 0)
            {
                difficulty--;
            }
        }
    }
}
