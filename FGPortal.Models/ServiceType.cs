﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int CourierId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Courier Courier { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}