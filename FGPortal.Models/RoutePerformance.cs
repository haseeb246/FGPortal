﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
    public partial class RoutePerformance
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int LocationId { get; set; }
        public int? SumOTP { get; set; }
        public int? SumStops { get; set; }
        public int? SumSevenDayOTP { get; set; }
        public int? SumSevenDayStops { get; set; }
        public decimal? OTP { get; set; }
        public decimal? SevenDayOTP { get; set; }

        public virtual Location Location { get; set; }
        public virtual Route Route { get; set; }
    }
}