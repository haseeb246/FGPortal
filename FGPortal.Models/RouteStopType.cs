﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
    public partial class RouteStopType
    {
        public RouteStopType()
        {
            RouteStop = new HashSet<RouteStop>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? CourierId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Courier Courier { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<RouteStop> RouteStop { get; set; }
    }
}