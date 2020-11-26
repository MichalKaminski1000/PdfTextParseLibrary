using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfTextParseLibrary
{
    public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        //Hold each coordinate
        public List<Chunk> myPoints = new List<Chunk>();
        public int pageN;

        //Automatically called for each chunk of text in the PDF
        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);

            //Get the bounding box for the chunk of text
            Vector bottomLeft = renderInfo.GetDescentLine().GetStartPoint();
            Vector topRight = renderInfo.GetAscentLine().GetEndPoint();

            //Create a rectangle from it
            var rect = new iTextSharp.text.Rectangle(
                bottomLeft[Vector.I1],
                bottomLeft[Vector.I2],
                topRight[Vector.I1],
                topRight[Vector.I2]
            );


            //Add this to our main collection
            this.myPoints.Add(new Chunk(rect, renderInfo.GetText(), pageN));
        }
    }

    public class Chunk
    {
        public iTextSharp.text.Rectangle Rect;
        public String Text;
        public int PageN;

        public Chunk(iTextSharp.text.Rectangle rect, String text, int PageN)
        {
            this.Rect = rect;
            this.Text = text;
            this.PageN = PageN;
        }
    }
}

