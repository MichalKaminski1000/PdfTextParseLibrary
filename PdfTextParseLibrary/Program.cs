using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTextParseLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\CWI Presentation\FV ORANGE.pdf";

            

           
            PdfReader reader = new PdfReader(path);
            bool isScannedPdf = PdfTextRecognizer.IsScannedPdf(reader);
            List<Chunk> chunks = PdfTextRecognizer.GetChunksWithRectangleCoordinates(reader);
            List<RectAndWords> words = PdfTextRecognizer.GetWordsWithRectangleCoordinates(chunks);

      


            Console.WriteLine("Hello World!");
            Console.ReadKey();

          
        }
    }
}
