using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTextParseLibrary
{
    public class RectAndWords
    {
        public int Left { get; set; }
        public int Bottom { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public string Text { get; set; }
        public int PageN { get; set; }
        public int Line { get; set; }

        
        public RectAndWords(string text, int left, int bottom, int right, int top, int pageN)
        {
            this.Text = text;
            this.Left = left;
            this.Bottom = bottom;
            this.Right = right;
            this.Top = top;
            this.PageN = pageN;
        }
    }
}
