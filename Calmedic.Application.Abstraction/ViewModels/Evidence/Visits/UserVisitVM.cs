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
    public class UserVisitVM
    {
        public UserVisitVM()
        {

        }

        public int BranchId { get; set; }
        public string Signature { get; set; }
        public string BranchName { get; set; }
        public string DealerName { get; set; }
        public int OpenFrom { get; set; }
        public int OpenTo { get; set; }
        public List<SelectModelBinder> Counselors { get; set; } = new List<SelectModelBinder>();
        public List<SelectModelBinder> VisitStatuses { get; set; } = new List<SelectModelBinder>();
    }
}
