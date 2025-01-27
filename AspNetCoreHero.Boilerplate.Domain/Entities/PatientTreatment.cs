﻿using AspNetCoreHero.Abstractions.Domain;
using AspNetCoreHero.Boilerplate.Domain.Common;
using Domain.Entities;
using System;

namespace AspNetCoreHero.Boilerplate.Domain.Entities
{
    public class PatientTreatment : BaseAuditableEntity
    {
        public int Tooth { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime DateOfIntervention { get; set; }
        public string Payment { get; set; }
        public byte[] Picture { get; set; } 

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int? StaffUserId { get; set; }
        public StaffUser StaffUser{ get; set; } 
    }
}
