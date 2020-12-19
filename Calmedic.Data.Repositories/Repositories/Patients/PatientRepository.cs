using Calmedic.Domain;
using Calmedic.EntityFramework;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Linq;

namespace Calmedic.Data
{
    public class PatientRepository : Repository<Patient, MainDatabaseContext>, IPatientRepository
    {
        public PatientRepository(MainDatabaseContext context) : base(context)
        { }

        public object GetPatients(DataSourceLoadOptionsBase loadOptions)
        {
            var query = _dbset.Select(x => new
            {
                x.Id,
                x.PatientNumber,
                x.FirstName,
                x.LastName,
                x.DateOfBirth,
                x.Sex,
                x.Address.FullAdress,
                x.EmailAddress,
                x.PhoneNumber
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }
    }
}