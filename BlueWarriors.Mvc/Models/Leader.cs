using System;

namespace Bluewarriors.Mvc.Models
{
    public class Leader
    {
        public int LeaderId { get; set; }
        public string Name { get; set; }
        public int Msisdn { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int LeaderTypeId { get; set; }
        public string LeaderType { get; set; }
        public string Status { get; set; }
        public DateTime? DeactivationDate { get; set; }
    }
}