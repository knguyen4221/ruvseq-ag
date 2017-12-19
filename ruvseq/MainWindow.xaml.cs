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



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string fullPath;

            ofd.Filter = "csv files (*.csv)|*.txt|All files (*.*)|*.*";
            ofd.RestoreDirectory = true;
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fullPath = ofd.FileName;
                string[] cell_types = DataProcessing.DataProcessing.ReadCSV(fullPath);
                for(int i = 1; i < cell_types.Length; ++i)
                {
                    group1.Items.Add(cell_types[i]);
                }
            }
        }

        private void group1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (group1.Items.Count == 0)
                return;
            int index = System.Windows.Forms.IndexFromPoint(group1, e.X, e.Y);

        }
    }

}

namespace DataProcessing
{
    public class DataProcessing
    {
        public DataProcessing() { }
        public static string[] ReadCSV(string fileName)
        {
            IEnumerable<string> result = File.ReadLines(System.IO.Path.ChangeExtension(fileName,".csv"));
            string toSplit = result.First();
            return toSplit.Split(',');
        }
    }
}
