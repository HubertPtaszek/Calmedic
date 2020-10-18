using Calmedic.Dictionaries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calmedic.Application
{
    public class FileModel
    {
        public FileModel()
        { }

        public string ContentType { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public Stream Content { get; set; }
    }
}
