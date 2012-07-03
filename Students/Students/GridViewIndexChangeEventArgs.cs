using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Students
{
    public class GridViewIndexChangeEventArgs:EventArgs
    {
        public int PreviousIndex;
        public GridViewIndexChangeEventArgs(int prev)
        {
            PreviousIndex=prev;
        }
    }
    
}