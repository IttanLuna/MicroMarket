using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MicroMarket.Models
{
    public class Proveedor
    {
        [Key]
        public int ProveedorId { get; set; }
        [Required]
        public int? CI { get; set; }
        public string? Nombre { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Telefono { get; set; }
        [Required]
        public string? Direccion { get; set; }

        //atributos computados
        [NotMapped]
        public string? Info { get { return $"{CI} - {Nombre}"; } }

        // Relación uno a muchos con Productos
        public ICollection<Producto>? Productos { get; set; }
    }
}
