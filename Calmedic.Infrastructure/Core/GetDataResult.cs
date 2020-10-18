using System.Collections.Generic;

namespace Calmedic.Utils
{
    public class GetDataResult
    {
        public GetDataResult()
        {
            ItemList = new List<dynamic>();
        }

        public List<dynamic> ItemList { get; set; }
        public long TotalCount { get; set; }
    }

    public class GetDataResult<TEntity>
    {
        public GetDataResult()
        {
            ItemList = new List<TEntity>();
        }

        public List<TEntity> ItemList { get; set; }
        public long TotalCount { get; set; }
    }
}
