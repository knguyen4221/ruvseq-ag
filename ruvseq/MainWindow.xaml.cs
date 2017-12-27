using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Forms;
using RProcesses;

namespace ruvseq
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

        string fullPath;

        private void open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            ofd.RestoreDirectory = true;
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fullPath = ofd.FileName;
                string[] cell_types = DataProcessing.ReadCSV(fullPath);
                for(int i = 1; i < cell_types.Length; ++i)
                {
                    group1.Items.Add(cell_types[i]);
                }
            }
            e.Handled = true;
        }

        private void ruvseq_Click(object sender, RoutedEventArgs e)
        {
            List<string> g1 = new List<string>();
            for (int i = 0; i < group1.Items.Count; ++i)
                g1.Add((string)group1.Items.GetItemAt(i));
            List<string> g2 = new List<string>();
            for (int i = 0; i < group2.Items.Count; ++i)
                g2.Add((string)group2.Items.GetItemAt(i));
            string outFile = outputFileName.Text;
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDirectory + "\\ruvseq_output"))
            {
                Directory.CreateDirectory(currentDirectory + "\\ruvseq_output");
            }
            RUVseq process = new RUVseq(fullPath,g1, g2, outFile, currentDirectory+"\\ruvseq_output");
        }
    }

    public class DataProcessing
    {
        public DataProcessing() { }
        public static string[] ReadCSV(string fileName)
        {
            IEnumerable<string> result = File.ReadLines(System.IO.Path.ChangeExtension(fileName, ".csv"));
            string toSplit = result.First();
            return toSplit.Split(',');
        }
    }
}

