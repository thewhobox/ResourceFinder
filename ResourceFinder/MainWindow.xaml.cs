using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.IO;

namespace ResourceFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> ResourceList { get; set; } = new ObservableCollection<string>();
        private Assembly ass;

        public MainWindow()
        {
            this.DataContext = this;
        }

        public void DoOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Assembly öffnen";
            if(diag.ShowDialog() != true) return;

            ass = Assembly.LoadFrom(diag.FileName);
            ResourceList.Clear();
            foreach(string resource in ass.GetManifestResourceNames())
                ResourceList.Add(resource);
        }
        
        public void DoExport(object sender, RoutedEventArgs e)
        {
            if(ResourceListBox.SelectedItem == null) {
                MessageBox.Show("Bitte wählen Sie eine Resource aus.");
                return;
            }

            string path = ResourceListBox.SelectedItem.ToString();
            Stream sAssembly = ass.GetManifestResourceStream(path);
            Stream sFile = File.Create(path);
            sAssembly.CopyTo(sFile);
            sAssembly.Flush();
            sAssembly.Dispose();
            sFile.Flush();
            sFile.Dispose();
        }
    }
}