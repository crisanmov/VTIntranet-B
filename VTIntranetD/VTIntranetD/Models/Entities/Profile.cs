namespace VTIntranetD.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Profile")]
    public partial class Profile
    {
        [Key]
        public int idProfile { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        
        [StringLength(50)]
        public string rolName { get; set; }

        public bool profileActive { get; set; }

        [Required]
        public int idUser { get; set; }
    }
}
