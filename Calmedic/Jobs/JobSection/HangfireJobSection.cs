using System.Collections.Generic;

namespace Calmedic.Jobs.JobSection
{
    public class HangfireJobSection
    {
        public string ServerName { get; set; }
        public bool IsEnabled { get; set; }
        public List<HangfireJobSectionItem> Jobs { get; set; } = new List<HangfireJobSectionItem>();
    }

    public class HangfireJobSectionItem
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public bool IsEnabled { get; set; }
        public string CronExpression { get; set; }
        public string JobId { get; set; }
        public string Description { get; set; }
        public string Queue { get; set; }
    }
}