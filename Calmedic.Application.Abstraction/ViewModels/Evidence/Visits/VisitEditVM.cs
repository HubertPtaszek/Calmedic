using Calmedic.Dictionaries;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calmedic.Application
{
    public class VisitEditVM
    {
        public VisitEditVM()
        {

        }

        public int Id { get; set; }
        public int CounselorId { get; set; }
        public string CarRegistrationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        public int BranchId { get; set; }
    }
}
