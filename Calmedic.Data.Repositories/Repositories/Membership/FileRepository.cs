using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Data
{
    public class FileRepository : Repository<File, MainDatabaseContext>, IFileRepository
    {
        public FileRepository(MainDatabaseContext context) : base(context)
        { }

       
    }
}
