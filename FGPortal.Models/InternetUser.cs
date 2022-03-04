﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
    public partial class InternetUser
    {
        public InternetUser()
        {
            InternetUserMapping = new HashSet<InternetUserMapping>();
            InternetUserViewable = new HashSet<InternetUserViewable>();
            SupportTicket = new HashSet<SupportTicket>();
            UserPreference = new HashSet<UserPreference>();
            UserRestriction = new HashSet<UserRestriction>();
        }

        public int Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedWhen { get; set; }
        public bool? UpdateStop { get; set; }
        public bool Admin { get; set; }

        public virtual ICollection<InternetUserMapping> InternetUserMapping { get; set; }
        public virtual ICollection<InternetUserViewable> InternetUserViewable { get; set; }
        public virtual ICollection<SupportTicket> SupportTicket { get; set; }
        public virtual ICollection<UserPreference> UserPreference { get; set; }
        public virtual ICollection<UserRestriction> UserRestriction { get; set; }
    }
}