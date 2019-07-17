namespace VTIntranetD.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProfileTagDepto")]
    public partial class ProfileTagDepto
    {
        [Key]
        public int idProfileTagDepto { get; set; }

        public int idProfile { get; set; }

        public int idTag { get; set; }

        public int? idParent { get; set; }

        public int idDepto { get; set; }

        public bool active { get; set; }
    }
}
