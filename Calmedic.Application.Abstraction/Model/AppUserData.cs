using Calmedic.Dictionaries;
using System;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class AppUserData
    {
        public AppUserData()
        {
            ValidDate = DateTime.Now.AddMinutes(5);
            Roles = new List<AppRoleType>();
        }
     
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime ValidDate { get; set; }
        public string AppIdentityUserId { get; set; }
        public int? ClinicId { get; set; }
        public List<AppRoleType> Roles { get; set; }
    }
}
