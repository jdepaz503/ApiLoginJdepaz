using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLoginJdepaz.Infraestructure.Models.Entities
{
    [Table("Usuarios")]
    public class TblUsuarios
    {
        [Key, Column("UserId")]
        public int idUsuario { get; set; }

        [Column("nombre_user")]
        public string nombre_user { get; set; }

        [Column("username")]
        public string username { get; set; }

        [Column("email_user")]
        public string email_user { get; set; }

        [Column("pass_user")]
        public string pass_user { get; set; }

        [Column("telefono_user")]
        public string telefono_user { get; set; }

        [Column("fnac_user")]
        public DateTime fnac_user { get; set; }

        [Column("Estado")]
        public int Estado { get; set; }

    }
}
