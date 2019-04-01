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

namespace RallyXtreme_launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        public void mapCacheWrite()
        {
            // test XML writer, relocate to map selection window
            Map test= new Map();
            test.name = "thirteenfourteen";
            test.imgPath = "c:/Windows/Users/matth/rallyx";
            test.gridX = 13;
            test.gridY = 14;
            test.hitboxPath = null;
            test.enemyNumber = 2;
            test.difficulty = 1;

            XmlSerializer writer = new XmlSerializer(typeof(Map));
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, test);
            file.Close();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            mapCacheWrite();
        }

        private void AIMenu_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
