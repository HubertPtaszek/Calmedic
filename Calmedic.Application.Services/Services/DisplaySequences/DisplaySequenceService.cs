using Calmedic.Data;
using Calmedic.Domain;
using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public class DisplaySequenceService : ServiceBase, IDisplaySequenceService
    {
        #region Dependencies

        public IDisplaySequenceRepository DisplaySequenceRepository { get; set; }
        public IFileRepository FileRepository { get; set; }

        #endregion Dependencies

        public virtual object GetDisplaySequence(DataSourceLoadOptionsBase loadOptions)
        {
            return DisplaySequenceRepository.GetDisplaySequence(loadOptions);
        }

        public virtual void SetElementLower(int id)
        {
            DisplaySequence element = DisplaySequenceRepository.GetSingle(x => x.Id == id);
            if (element == null)
            {
                return;
            }
            DisplaySequence lowerElement = DisplaySequenceRepository.GetLower(element.Order, element.ClinicId);
            if (lowerElement == null)
            {
                return;
            }
            int order = element.Order;
            element.Order = lowerElement.Order;
            lowerElement.Order = order;
            DisplaySequenceRepository.Edit(lowerElement);
            DisplaySequenceRepository.Edit(element);
            DisplaySequenceRepository.Save();
        }

        public virtual void SetElementHigher(int id)
        {
            DisplaySequence element = DisplaySequenceRepository.GetSingle(x => x.Id == id);
            if (element == null)
            {
                return;
            }
            DisplaySequence higherElement = DisplaySequenceRepository.GetHigher(element.Order, element.ClinicId);
            if (higherElement == null)
            {
                return;
            }
            int order = element.Order;
            element.Order = higherElement.Order;
            higherElement.Order = order;
            DisplaySequenceRepository.Edit(higherElement);
            DisplaySequenceRepository.Edit(element);
            DisplaySequenceRepository.Save();
        }
    }
}