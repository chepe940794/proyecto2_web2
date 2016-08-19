namespace APICandy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Estadisticas
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id_estadistíca { get; set; }

        [StringLength(250)]
        public string usuario { get; set; }

        public string puntaje { get; set; }
    }
}
