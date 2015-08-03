using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace JeSCommandLine
{
    class TextBlock
    {
        public Color? Color { get; set; }
        public string Text { get; set; }

        public TextBlock()
        {
            this.Color = null;
        }
    }
}
