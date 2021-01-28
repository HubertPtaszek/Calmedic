using Calmedic.Dictionaries;
using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class DisplaySequenceListVM
    {
        public DisplaySequenceListVM()
        {
        }

        public List<EnumModelBinder> MediaTypes { get; set; } = EnumHelpers.GetEnumBinderList<MediaType>();
    }
}