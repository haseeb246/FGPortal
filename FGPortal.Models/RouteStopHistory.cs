// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
    public partial class RouteStopHistory
    {
        public int Id { get; set; }
        public int RouteStopTypeId { get; set; }
        public int RouteId { get; set; }
        public int LocationId { get; set; }
        public int? DriverId { get; set; }
        public DateTime PostDate { get; set; }
        public int Sequence { get; set; }
        public DateTime? TimeMin { get; set; }
        public DateTime? TimePreferred { get; set; }
        public DateTime? TimeMax { get; set; }
        public DateTime? TimeArrived { get; set; }
        public DateTime? TimeCompleted { get; set; }
        public string Status { get; set; }
        public string Podtext { get; set; }
        public string Podsignature { get; set; }
        public string Remarks { get; set; }
        public string Reference { get; set; }
        public string CcpmsgId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedWhen { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedWhen { get; set; }
    }
}