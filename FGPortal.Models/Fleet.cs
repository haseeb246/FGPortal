// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
    public partial class Fleet
    {
        public Fleet()
        {
            Order = new HashSet<Order>();
            Route = new HashSet<Route>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? CourierId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Plus4 { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedWhen { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedWhen { get; set; }
        public int? CourierFleetId { get; set; }

        public virtual Courier Courier { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<Route> Route { get; set; }
    }
}