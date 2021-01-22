using Calmedic.Dictionaries;
using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class AppRoleDetailsVM
    {
        public int Id { get; set; }

        public AppRoleType AppRoleType { get; set; }

        [Display(ResourceType = typeof(SharedResource), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(SharedResource), Name = "Description")]
        public string Description { get; set; }
    }
}