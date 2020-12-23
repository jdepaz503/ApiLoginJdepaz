using System;
using System.ComponentModel.DataAnnotations;

namespace ApiLoginJdepaz.Core.Domains.Productos
{
    public class ProductoRequest
    {
        [Required]
        [StringLength(12, MinimumLength = 12)]
        public string SKU { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string nombre { get; set; }
        [Required]
        [Range(0, Int64.MaxValue, ErrorMessage = "Por favor especifique solo números.")]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Debe especificar como mínimo un dígito.")]
        public int cantidad { get; set; }
        [Required]
        public double precio { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string descripcion { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string imagen { get; set; }
        
    }
}
