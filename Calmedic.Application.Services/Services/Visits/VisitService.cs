using Calmedic.Data;

namespace Calmedic.Application
{
    public class VisitService : ServiceBase, IVisitService
    {
        #region Dependencies
        public IVisitRepository VisitRepository { get; set; }
        public VisitConverter VisitConverter { get; set; }
        #endregion
    }
}
