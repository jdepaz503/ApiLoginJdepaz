using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLoginJdepaz.Infraestructure.Models.Entities
{
    [Table("Productos")]
    public class TblProductos
    {
        [Key, Column("productID")]
        public int idProducto { get; set; }

        [Column("SKU")]
        public string SKU { get; set; }

        [Column("nombre")]
        public string nombre { get; set; }

        [Column("cantidad")]
        public int cantidad { get; set; }

        [Column("precio")]
        public decimal precio { get; set; }

        [Column("descripcion")]
        public string descripcion { get; set; }

        [Column("imagen")]
        public string imagen { get; set; }
    }
}
