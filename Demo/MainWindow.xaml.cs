using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.BZip2;
using Microsoft.Win32;
using OpenStreetMapParser;
using OpenStreetMapParser.Models;

namespace Demo
{
    public partial class MainWindow : Window
    {
        DateTime startTime;
        int nodeCount = 0;
        int relationCount = 0;
        int wayCount = 0;

        CancellationTokenSource cancelTokenSource;

        public MainWindow()
        {
            InitializeComponent();
            panelControls.Visibility = Visibility.Visible;
            panelProgress.Visibility = Visibility.Hidden;
        }

        private async void BtnParse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "osm data bz2|*.bz2",
            };

            if (dlg.ShowDialog() != true)
            {
                return;
            }

            using (Stream fs = dlg.OpenFile())
            {
                await ProcessDump(fs);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource?.Cancel();
        }

        private async Task ProcessDump(Stream inputStream)
        {
            cancelTokenSource?.Cancel();
            cancelTokenSource = new CancellationTokenSource();

            panelControls.Visibility = Visibility.Hidden;
            panelProgress.Visibility = Visibility.Visible;

            try
            {
                using (BZip2InputStream stream = new BZip2InputStream(inputStream))
                {
                    Parser parser = new  MyParser(stream, NodeRead, RelationRead, WayRead);

                    CancellationToken token = cancelTokenSource.Token;

                    await Task.Run(() =>
                    {
                        startTime = DateTime.UtcNow;
                        nodeCount = 0;
                        relationCount = 0;
                        wayCount = 0;

                        parser.Read(token);
                    }, token);

                    UpdateStatus();
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                panelControls.Visibility = Visibility.Visible;
                panelProgress.Visibility = Visibility.Hidden;
            }
        }

        private void NodeRead(OsmNode node)
        {
            nodeCount++;

            if (nodeCount % 100 == 0)
            {
                UpdateStatus();
            }
        }
    
        private void RelationRead(OsmRelation relation)
        {
            relationCount++;
            
            if (relationCount % 100 == 0)
            {
                UpdateStatus();
            }
        }

        private void WayRead(OsmWay way)
        {
            wayCount++;

            if (wayCount % 100 == 0)
            {
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            Dispatcher.BeginInvoke(new Action(() => 
            {
                txtProgress.Text = $"Nodes: {nodeCount}, Ways: {wayCount}, Relations: {relationCount}";
            }));
        }

    }
}
