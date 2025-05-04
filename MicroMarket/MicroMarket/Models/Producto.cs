using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MicroMarket.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal Precio { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public int Stock { get; set; }
        public string? UrlFoto { get; set; }

        //para subir archivos
        [NotMapped]
        [Display(Name = "Cargar Foto")]
        public IFormFile? FotoFile { get; set; }

        [NotMapped]
        public string? Info { get { return $"{Precio}"; } }


        // Relación uno a muchos con Ventas
        public ICollection<Venta> DetallesVentas { get; set; } = new List<Venta>();
    }
}
