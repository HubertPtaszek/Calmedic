using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Data
{
    public class ClinicRepository : Repository<Clinic, MainDatabaseContext>, IClinicRepository
    {
        public ClinicRepository(MainDatabaseContext context) : base(context)
        { }

        public List<SelectModelBinder<int>> GetClinicsToSelect()
        {
            List<SelectModelBinder<int>> result = _dbset.Select(x => new SelectModelBinder<int>()
            {
                Value = x.Id,
                Text = x.Name
            }).OrderBy(x => x.Text).ToList();
            return result;
        }

        public object GetClinics(DataSourceLoadOptionsBase loadOptions)
        {
            var query = _dbset.Select(x => new
            {
                x.Id,
                x.Name,
                x.Type,
                OpenFrom = x.OpenFrom,
                OpenTo = x.OpenTo,
                Address = x.Address.FullAdress,
                Email = x.Email,
                x.PhoneNumber
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }

        public object GetClinicDocotrs(DataSourceLoadOptionsBase loadOptions, int clinicId)
        {
            var query = Context.Doctors.Where(x => x.Person.Clinics.Any(y => y.ClinicId == clinicId))
            .Select(x => new
            {
                Id = x.Id,
                FirstName = x.Person.FirstName,
                LastName = x.Person.LastName,
                Title = x.Title,
                SpecializationId = x.SpecializationId,
                DocumentNumber = x.DocumentNumber,
                Email = x.Person.Email,
                PhoneNumber = x.Person.PhoneNumber
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }

        public object GetClinicsForUser(DataSourceLoadOptionsBase loadOptions, ClinicUser clinicUser)
        {
            var query = _dbset.Where(x => x.Users.Contains(clinicUser)).Select(x => new
            {
                x.Id,
                x.Name,
                x.Type,
                x.OpenFrom,
                x.OpenTo,
                Address = x.Address.FullAdress,
                Email = x.Email,
                x.PhoneNumber
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }
    }
}