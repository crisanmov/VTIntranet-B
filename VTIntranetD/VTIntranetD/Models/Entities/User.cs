namespace VTIntranetD.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Profile = new HashSet<Profile>();
        }

        [Key]
        public int idUser { get; set; }

        [Required]
        [StringLength(100)]
        public string username { get; set; }

        [Required]
        [StringLength(100)]
        public string password { get; set; }

      
        [StringLength(100)]
        public string name { get; set; }

   
        [StringLength(100)]
        public string lastNameP { get; set; }

 
        [StringLength(100)]
        public string lastNameM { get; set; }

  
        [StringLength(100)]
        public string userActive { get; set; }

       
        [StringLength(100)]
        public string skype { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profile> Profile { get; set; }
    }
}
