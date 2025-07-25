﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace OutletStatusPortal.Models
{
    public class AfterOutletSetup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("BeforeOutletSetUp")]
        public int BeforeOutletSetUpSl { get; set; }
        
        [ValidateNever]
        public BeforeOutletSetUp beforeOutletSetUp { get; set; }
        [Required] public string OfficeITSetup { get; set; }
        public string CourierStatus { get; set; }
        public string NetworkVendor { get; set; }
        public string OutletITSetup { get; set; }
        public string LinkStatus { get; set; }
        public string SapId { get; set; }
        public string MailId { get; set; }
        public string PosId { get; set; }
        public string EPSLive { get; set; }
        public DateTime OperationDate { get; set; } = DateTime.Now;
        public string? AssignedPersons { get; set; }
    }

}
