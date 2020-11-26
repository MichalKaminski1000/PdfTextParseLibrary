using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfTextParseLibrary
{
    public class PdfTextRecognizer
    {
       

        public static bool IsScannedPdf(PdfReader reader, double imageRatio = 0.96)
        {
            long imagesSize = 0;


            for (int i = 0; i < reader.XrefSize; i++)
            {
                iTextSharp.text.pdf.PdfObject obj = reader.GetPdfObject(i);
                if (obj != null && obj.IsStream())
                {
                    PdfDictionary dict = (PdfDictionary)obj;
                    var objType = dict.Get(new PdfName("Type"));
                    if (objType != null && objType.ToString().ToLower().Contains("xobject"))
                    {
                        imagesSize += ((PdfNumber)dict.Get(new PdfName("Length"))).IntValue;
                    }
                }
            }

            return ((double)imagesSize / reader.FileLength > imageRatio);




        }

        public static List<Chunk> GetChunksWithRectangleCoordinates(PdfReader reader)
        {
            //Create an instance of PDF text exctraction strategy
            MyLocationTextExtractionStrategy textExtractionStrategy = new MyLocationTextExtractionStrategy();

            //Parse the document
            string text = ExtractTextFromPdf(reader, textExtractionStrategy);

            return textExtractionStrategy.myPoints;
        }

        public static List<RectAndWords> GetWordsWithRectangleCoordinates(List<Chunk> myPoints, int distanceBetweenChunks = 3)
        {
            List<RectAndWords> myWords = new List<RectAndWords>();

            int left = 0;
            int bottom = 0;
            int right = 0;
            int top = 0;
            int wordsCounter = 0;


            StringBuilder text = new StringBuilder();


            for (int i = 0; i < myPoints.Count - 1; i++)
            {
                wordsCounter = i;

                if (Math.Abs((int)(myPoints[i].Rect.Right) - (int)(myPoints[i + 1].Rect.Left)) < distanceBetweenChunks)
                {
                    if (String.IsNullOrEmpty(text.ToString()))
                    {
                        text.Append(myPoints[i].Text).ToString();
                        left = (int)myPoints[i].Rect.Left;
                        bottom = (int)myPoints[i].Rect.Bottom;


                    }

                    text.Append(myPoints[i + 1].Text);




                }

                else
                {
                    if (String.IsNullOrEmpty(text.ToString()))
                    {


                        myWords.Add(new RectAndWords(myPoints[i].Text, (int)myPoints[i].Rect.Left,
                            (int)myPoints[i].Rect.Bottom, (int)myPoints[i].Rect.Right, (int)myPoints[i].Rect.Top,
                            myPoints[i].PageN));

                        if (i == (myPoints.Count - 2))
                        {
                            myWords.Add(new RectAndWords(myPoints[i + 1].Text, (int)myPoints[i + 1].Rect.Left,
                            (int)myPoints[i + 1].Rect.Bottom, (int)myPoints[i + 1].Rect.Right, (int)myPoints[i + 1].Rect.Top,
                            myPoints[i + 1].PageN));
                        }

                    }
                    else
                    {
                        right = (int)myPoints[i].Rect.Right;

                        if (i > 0)
                        {
                            if ((int)myPoints[i - 1].Rect.Top <= (int)myPoints[i].Rect.Top)
                            {
                                top = (int)myPoints[i].Rect.Top;
                            }
                            else
                            {
                                top = (int)myPoints[i - 1].Rect.Top;
                            }

                            if ((int)myPoints[i - 1].Rect.Bottom >= (int)myPoints[i].Rect.Bottom)
                            {
                                bottom = (int)myPoints[i].Rect.Bottom;
                            }
                            else
                            {
                                bottom = (int)myPoints[i - 1].Rect.Bottom;
                            }
                        }
                        else
                        {
                            top = (int)myPoints[i].Rect.Top;
                        }

                        myWords.Add(new RectAndWords(text.ToString(), left, bottom, right, top, myPoints[i].PageN));

                        text = new StringBuilder();
                    }
                }



            }

            if (!String.IsNullOrEmpty(text.ToString()))
            {
                right = (int)myPoints[wordsCounter + 1].Rect.Right;

                if ((int)myPoints[wordsCounter].Rect.Top <= (int)myPoints[wordsCounter + 1].Rect.Top)
                {
                    top = (int)myPoints[wordsCounter + 1].Rect.Top;
                }
                else
                {
                    top = (int)myPoints[wordsCounter].Rect.Top;
                }


                myWords.Add(new RectAndWords(text.ToString(), left, bottom, right, top, myPoints[wordsCounter].PageN));
            }

            myWords = myWords.OrderBy(word => word.PageN).ThenByDescending(word => word.Bottom)
                .ThenBy(word => word.Left).ToList();

            return myWords;
        }

        private static string ExtractTextFromPdf(PdfReader reader, MyLocationTextExtractionStrategy strategy)
        {
            StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++) //reader.NumberOfPages
                {
                    strategy.pageN = i;
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i, strategy));
                }
           
            return text.ToString();
        }

       

    }
}
