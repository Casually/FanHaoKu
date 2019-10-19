using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bgimage
{
    public class Class1
    {
        public Class1()
        {
            GetType().Assembly.GetManifestResourceStream("Bgimage.Resources.086.jpg");
        }

        public Stream ReturnStream()
        {
            return GetType().Assembly.GetManifestResourceStream("Bgimage.Resources.086.jpg");
        }
    }

}
