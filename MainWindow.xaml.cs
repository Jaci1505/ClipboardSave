using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClipboardSave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Media.Imaging.BitmapSource image = Clipboard.GetImage();

            if (image == null)
            {
                showErrorBoxAndClose("Fehler! Es wurde kein Screenshot in der Zwischenablage gefunden. Das Programm wird beendet");
            }

            else
            {
                string path = showSaveFileDialog();
                try
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(fileStream);
                    }

                }
                catch (System.Runtime.InteropServices.COMException comex)
                {
                    if (path != null) //wenn gar kein Pfad gewählt wurde, erzeugt das auch diese Exception, aber der Nutzer soll diese nicht sehen (dadurch Speicherung abbrechbar)
                    {
                        //Fehler der kommt, falls Datei schon offen und Speichern nicht möglich
                        showCannotBeSaveUnderNamedError(this);
                    }
                }
            }

            Application.Current.Shutdown();

        }



        public static string showSaveFileDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG-Bild(*png)|*.png|JPG-Bild(*jpg)|*.jpg"; //nur Excel-Dateien zulassen
            sfd.InitialDirectory = "C:\\"; //starte im Verzeichnis C
            bool? result = sfd.ShowDialog();
            if (result == true) //etwas gespeichert (Pfad gewaehlt)
            {
                return sfd.FileName;
            }
            else return null; //Dialog geschlossen oder abgebrochen  
        }

        public static void showErrorBoxAndClose(string errorText)
        {
            MessageBox.Show(errorText, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown(); //beende Programm
        }

        public static void showCannotBeSaveUnderNamedError(Window window)
        {
            showErrorBox("Fehler! Die Datei kann nicht unter diesem Namen gespeichert werden, da eine Datei unter diesem Namen bereits offen ist!", window);


        }
        public static void showErrorBox(string errorText, Window window)
        {
            TaskDialog td = new TaskDialog();
            td.Icon = TaskDialogStandardIcon.Error;
            td.Caption = "Fehler!";
            td.InstructionText = "Fehler!";
            td.Text = errorText;
            TaskDialogCommandLink btnOK = new TaskDialogCommandLink("btnOK", "OK");
            TaskDialogCommandLink btnExit = new TaskDialogCommandLink("btnExit", "Beenden"); //Knöpfe definieren
            btnOK.Click += new EventHandler((s, e) => td.Close());  //nur Fenster schließen
            btnExit.Click += new EventHandler(exitApplixcation); //Applikation beenden


            td.Controls.Add(btnOK);
            td.Controls.Add(btnExit);

            WindowInteropHelper winInteropHelper = new WindowInteropHelper(window);
            td.OwnerWindowHandle = winInteropHelper.Handle;
            td.Show();

        }




        /// <summary>
        /// Methode zum Beenden der Appliaktion aus einem Dialogfenster.
        /// </summary>
        /// <param name="sender">Sender-Objekt</param>
        /// <param name="e">Argumente des Events</param>
        private static void exitApplixcation(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
