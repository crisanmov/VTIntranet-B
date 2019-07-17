namespace VTIntranetD.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Event")]
    public partial class Event
    {
        [Key]
        public int IdEvent { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }

        [Required]
        [StringLength(100)]
        public string Path { get; set; }

        [StringLength(150)]
        public string Url { get; set; }

        public int IdActivity { get; set; }
    }
}
