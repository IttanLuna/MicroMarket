using System.ComponentModel.DataAnnotations;

namespace MicroMarket.Models
{
    public enum TipoRol
    {
        Administrador = 1,
        Vendedor = 2
    }

    public class Vendedor
    {
        [Key]
        public int VendedorId { get; set; }

        public string? Nombre { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Contraseña { get; set; }

        [Required]
        public string? Telefono { get; set; }

        [Required]
        public string? Direccion { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "El rol debe ser 1 (Administrador) o 2 (Vendedor)")]
        public TipoRol Rol { get; set; }  // Rol con valores 1 o 2

        // Relación uno a muchos con Ventas
        public ICollection<Venta>? Ventas { get; set; }
    }

}
