using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class SlideData
    {
        public SlideData()
        {
            SlideModelCreate = new SlideModelCreate();
            SlideModelEdit = new SlideModelEdit();
            Object = new object();
        }

        public SlideModelCreate SlideModelCreate { get; set;}
        public SlideModelEdit SlideModelEdit { get; set; }
        public object Object { get; set; }
    }
}
