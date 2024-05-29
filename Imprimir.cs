using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Microsoft.Win32;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;

namespace NoCocinoMas
{
    static class Imprimir
    {
        static public void Imprime(string archivo, string impresora, string sufijo)
        {
            PdfDocument doc = new PdfDocument();
            try
            {                
                doc.LoadFromFile(archivo);
                doc.PrintSettings.PrinterName = impresora;

                //SEUR
                if (sufijo == "2")
                {

                    Image img = doc.SaveAsImage(0);

                    Bitmap m = new Bitmap(img);
                    Bitmap nb = new Bitmap(360, 650);
                    using (Graphics g = Graphics.FromImage(nb))
                    {
                        g.DrawImage(m, -170, -140);
                    }                    

                    //PdfBitmap pdfBitmap = new PdfBitmap(nb);
                    //pdfBitmap.Draw(doc.Pages[0], PointF.Empty);

                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += (object o, PrintPageEventArgs e) => {
                        e.Graphics.DrawImage(nb, 20, 0, 324, 585);
                    };
                    pd.PrinterSettings.PrinterName = impresora;
                    pd.Print();
                    pd.Dispose();

                    //doc.PrintSettings.SelectSinglePageLayout(Spire.Pdf.Print.PdfSinglePageScalingMode.CustomScale, false, 160f);
                    //doc.Print();
                }
                else
                {
                    doc.Print();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(archivo);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                doc.Close();
            }
        }

    }
}
