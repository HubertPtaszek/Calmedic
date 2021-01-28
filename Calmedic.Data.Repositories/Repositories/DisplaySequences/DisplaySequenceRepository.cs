using Calmedic.Domain;
using Calmedic.EntityFramework;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Linq;

namespace Calmedic.Data
{
    public class DisplaySequenceRepository : Repository<DisplaySequence, MainDatabaseContext>, IDisplaySequenceRepository
    {
        public DisplaySequenceRepository(MainDatabaseContext context) : base(context)
        { }

        public object GetDisplaySequence(DataSourceLoadOptionsBase loadOptions)
        {
            var query = _dbset.Select(x => new
            {
                x.Id,
                x.MediaType,
                x.DisplayTime,
                FileName = x.File.OriginalName,
                x.Description,
                x.Order,
                IsMin = x.Order == _dbset.Where(y => y.ClinicId == 1).Min(y => y.Order),
                IsMax = x.Order == _dbset.Where(y => y.ClinicId == 1).Max(y => y.Order),
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }

        public DisplaySequence GetLower(int order, int clinicId)
        {
            return _dbset.Where(x => x.ClinicId == clinicId && x.Order < order).OrderByDescending(x => x.Order).FirstOrDefault();
        }

        public DisplaySequence GetHigher(int order, int clinicId)
        {
            return _dbset.Where(x => x.ClinicId == clinicId && x.Order > order).OrderBy(x => x.Order).FirstOrDefault();
        }
    }
}
