using Calmedic.Dictionaries;
using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class PatientListVM
    {
        public PatientListVM()
        { }
        public List<EnumModelBinder> SexDictionary { get; set; } = EnumHelpers.GetEnumBinderList<SexDictionary>();
    }
}