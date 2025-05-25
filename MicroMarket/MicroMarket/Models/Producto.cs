using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MicroMarket.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }

        [Required]
        [Precision(10, 2)]
        [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
        public decimal Precio { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo.")]
        public int StockMinimo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock máximo no puede ser negativo.")]
        public int StockMaximo { get; set; }

        
        public string TipoProducto { get; set; } = string.Empty;

        public DateOnly? FechaVencimiento { get; set; }

        public string? UrlFoto { get; set; }

        // Para subir archivos
        [NotMapped]
        [Display(Name = "Cargar Foto")]
        public IFormFile? FotoFile { get; set; }

        [NotMapped]
        public string? Info => $"{Descripcion}";

        // Relación uno a muchos con Ventas
        public ICollection<Venta> DetallesVentas { get; set; } = new List<Venta>();
    }

}
