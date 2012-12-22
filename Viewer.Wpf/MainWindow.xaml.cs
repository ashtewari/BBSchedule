using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BBScheduleWpfViewer
{
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    using BBSchedule.Viewer;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.txtFile.Text = "Boys-Mites.txt";
            this.chkGoogle.IsChecked = true;
            this.chkTable.IsChecked = false;
        }

        /// <summary>
        /// Handles the Click event of the btnOk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var parser = new BBSchedule.Data.Parser();
            var list = parser.Parse(this.txtFile.Text);

            txtLines.Document.Blocks.Clear();

            Formatter formatter = null;

            if (chkGoogle.IsChecked.HasValue && chkGoogle.IsChecked.Value)
            {
                formatter = new GoogleCalenderFormatter();
            }

            if (chkTable.IsChecked.HasValue && chkTable.IsChecked.Value)
            {
                formatter = new TableFormatter();
            }

            if (formatter == null)
            {
                txtLines.AppendText("Please specify the output.");
                return;
            }

            bool firstLine = true;
            foreach (var line in list)
            {
                if (firstLine)
                {
                    txtLines.AppendText(string.Format("{0}\n\n", formatter.GetHeader()));
                    firstLine = false;
                }

                txtLines.AppendText(string.Format("{0}\n", formatter.Format(line)));
            }

            if (chkXml.IsChecked.HasValue && chkXml.IsChecked.Value)
            {
                this.ExportXml();
            }
        }

        /// <summary>
        /// Exports the XML.
        /// </summary>
        private void ExportXml()
        {
            string content = new TextRange(txtLines.Document.ContentStart, txtLines.Document.ContentEnd).Text;
            var lines = content.Split("\n".ToCharArray());

            var xml = new XElement(
                "output",
                lines.Select(
                    line =>
                    new XElement(
                        "Item", line.Split(',').Select((column, index) => new XElement("Column" + index, column)))));

            var builder = new StringBuilder();
            xml.WriteTo(new XmlTextWriter(new IndentedTextWriter(new StringWriter(builder))));         

            txtLines.Document.Blocks.Clear();
            txtLines.AppendText(builder.ToString());
        }
    }
}
