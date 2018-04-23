using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using DevExpress.XtraScheduler.iCalendar;

namespace WpfApplication1 {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        #region #Import_Button
        private void Import_Button_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "iCalendar files (*.ics)|*.ics";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() != true)
                return;

            using (Stream stream = dialog.OpenFile()) {
                ImportAppointments(stream);
            }
        }
        #endregion #Import_Button

        #region #Import_Drop
        private void schedulerControl1_Drop(object sender, DragEventArgs e) {
            string[] fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileNames == null || fileNames.Length == 0)
                return;

            foreach (string fileName in fileNames) {
                if (File.Exists(fileName)) {
                    using (Stream stream = File.Open(fileName, FileMode.Open)) {
                        ImportAppointments(stream);
                    }
                }
            }
        }
        #endregion #Import_Drop

        #region #Import
        private void ImportAppointments(Stream stream) {
            if (stream == null)
                return;
            schedulerControl1.Storage.AppointmentStorage.Clear();
            iCalendarImporter importer = new iCalendarImporter(schedulerControl1.GetCoreStorage());
            importer.Import(stream);
        }
        #endregion #Import

        #region #Export
        private void Export_Button_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "iCalendar files (*.ics)|*.ics";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == true) {
                using (Stream stream = dialog.OpenFile()) {
                    ExportAppointments(stream);
                }
            }
        }
        void ExportAppointments(Stream stream) {
            if (stream == null)
                return;
            iCalendarExporter exporter = new iCalendarExporter(schedulerControl1.GetCoreStorage());
            exporter.ProductIdentifier = "-//Developer Express Inc.//DXScheduler iCalendarExchange Example//EN";
            exporter.Export(stream);
        }
        #endregion #Export


    }
}