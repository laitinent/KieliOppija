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

namespace KieliOppija
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dao dao=null;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            var dropped = (string)e.Data.GetData(DataFormats.Text);
            MessageBox.Show(dropped);
        }

        private void CanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            // Call a method to determine if there is a file open.
            // If there is a file open, then set CanExecute to true.
            e.CanExecute = Clipboard.ContainsText();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dao == null || !textBox.Text.EndsWith(".")) return;
            List<AnalyzedSentence> sentences = TextAnalyzer.AnalyzeSentences(textBox.Text);
            dao.saveSentences(sentences);
            //string[] words = TextAnalyzer.AnalyzeWords(sentences);
            //dao.saveWords(words);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dao = new Dao();
        }
    }
}
