using Calmedic.Domain;
using Calmedic.EntityFramework;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
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
                Address = x.Address.FullAdress,
                Email = x.EmailAddress,
                x.PhoneNumber
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }

        public List<PatientsWidgetDTO> GetNewestPatient()
        {
            List<PatientsWidgetDTO> result = _dbset.OrderByDescending(x => x.CreatedDate)
                .Select(x => new PatientsWidgetDTO()
                {
                    Id = x.Id,
                    PatientNumber = x.PatientNumber,
                    Name = String.Join(" ", x.FirstName, x.LastName),
                    FirstLetter = x.FirstName.Substring(0, 1)
                }).Take(4).ToList();
            return result;
        }
    }
}