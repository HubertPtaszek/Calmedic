using Calmedic.Data;

namespace Calmedic.Application
{
    public class DisplaySequenceService : ServiceBase, IDisplaySequenceService
    {
        #region Dependencies

        public IDisplaySequenceRepository DisplaySequenceRepository { get; set; }
        public IFileRepository FileRepository { get; set; }

        #endregion Dependencies
    }
}