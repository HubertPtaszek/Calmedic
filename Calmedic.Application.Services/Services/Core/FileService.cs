using Calmedic.Data;

namespace Calmedic.Application
{
    public class FileService : ServiceBase, IFileService
    {
        #region Dependencies

        public IFileRepository FileRepository { get; set; }

        #endregion Dependencies
    }
}