using System;

namespace BlueWarriors.Mvc.ViewModels
{
    public class AgentViewModel
    {
        public int AgentId { get; set; }
        public string Name { get; set; }
        public int Msisdn { get; set; }
        public int LeaderId { get; set; }
        public int AgentTypeId { get; set; }
        public string Status{ get; set; }
        public byte[] RowVersion { get; set; }
        public string AuthPassword { get; set; }
        public DateTime? DeactivationDate { get; set; }
    }
}