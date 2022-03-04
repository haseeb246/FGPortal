﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
    public partial class Courier
    {
        public Courier()
        {
            CourierCoverage = new HashSet<CourierCoverage>();
            Fleet = new HashSet<Fleet>();
            InternetUserMapping = new HashSet<InternetUserMapping>();
            InternetUserViewable = new HashSet<InternetUserViewable>();
            Location = new HashSet<Location>();
            MessageLog = new HashSet<MessageLog>();
            Order = new HashSet<Order>();
            ParcelType = new HashSet<ParcelType>();
            Route = new HashSet<Route>();
            RouteStopType = new HashSet<RouteStopType>();
            ServiceLevel = new HashSet<ServiceLevel>();
            ServiceType = new HashSet<ServiceType>();
            Signature = new HashSet<Signature>();
            TransactionLog = new HashSet<TransactionLog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Plus4 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedWhen { get; set; }

        public virtual ICollection<CourierCoverage> CourierCoverage { get; set; }
        public virtual ICollection<Fleet> Fleet { get; set; }
        public virtual ICollection<InternetUserMapping> InternetUserMapping { get; set; }
        public virtual ICollection<InternetUserViewable> InternetUserViewable { get; set; }
        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<MessageLog> MessageLog { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<ParcelType> ParcelType { get; set; }
        public virtual ICollection<Route> Route { get; set; }
        public virtual ICollection<RouteStopType> RouteStopType { get; set; }
        public virtual ICollection<ServiceLevel> ServiceLevel { get; set; }
        public virtual ICollection<ServiceType> ServiceType { get; set; }
        public virtual ICollection<Signature> Signature { get; set; }
        public virtual ICollection<TransactionLog> TransactionLog { get; set; }
    }
}