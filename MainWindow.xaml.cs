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
    /// Interaktionslogik des Fensters für die Hauptfensters.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Konstruktor zur Initialisierung des Hauptfensters. 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Media.Imaging.BitmapSource image = Clipboard.GetImage(); //aktuelles Bild aus dem Clipboard holen

            if (image == null) //kein Bild im Clipboard vorhanden
            {
                showErrorBoxAndClose("Fehler! Es wurde kein Screenshot in der Zwischenablage gefunden. Das Programm wird beendet");
            }

            else
            {
                string path = showSaveFileDialog(); //Speicherdialog anzeigen
                try
                {
                    //Bild speichern
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

            Application.Current.Shutdown(); //Nach Speichern des Bildes Applikation beenden

        }


        /// <summary>
        /// Zeige einen Dialog zum Speichern der Bilddatei. 
        /// </summary>
        /// <returns>Dateinamen den Nutzer gewählt hat</returns>
        public static string showSaveFileDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG-Datei|*.png|JPG-Datei|*.jpg|BMP-Datei|*.bmp |GIF-Datei"
                +    "|*.gif"; //erlaubte Dateiformate setzen
            sfd.InitialDirectory = "C:\\"; //starte im Verzeichnis C
            bool? result = sfd.ShowDialog();
            if (result == true) //etwas gespeichert (Pfad gewaehlt)
            {
                return sfd.FileName;
            }
            else return null; //Dialog geschlossen oder abgebrochen  
        }

        /// <summary>
        /// Zeigt eine ein Dialogfenster für einen Fehler. Dieser besitzt den Knopf "OK" zur Bestätigung. Danach wird die Applikation sofort beendet.
        /// </summary>
        /// <param name="errorText">Text der Fehlermeldung</param>
        public static void showErrorBoxAndClose(string errorText)
        {
            MessageBox.Show(errorText, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown(); //beende Programm
        }
        
        /// <summary>
        /// Zeigt eine Fehlermeldung, dass eine Datei nicht unter einem bestimmten Namen gespeichert werden kann, da sie bereits offen ist.
        /// </summary>
        /// <param name="window">Fenster, aus dem Dialog aufgerufen wurde</param>
        public static void showCannotBeSaveUnderNamedError(Window window)
        {
            showErrorBox("Fehler! Die Datei kann nicht unter diesem Namen gespeichert werden, da eine Datei unter diesem Namen bereits offen ist!", window);


        }

        /// <summary>
        /// Zeigt eine ein Dialogfenster für einen Fehler. Dieser besitzt die Knöpfe "OK" zur Bestätigung und Weiterarbeit und "Beenden" um die Applikation zu schließen.
        /// </summary>
        /// <param name="errorText">Text der Fehlermeldung</param>
        /// <param name="window">Fenster, aus dem Dialog aufgerufen wurde</param>
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
