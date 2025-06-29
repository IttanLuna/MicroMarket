﻿using System.ComponentModel.DataAnnotations;

namespace MicroMarket.Models
{
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


        // Relación uno a muchos con Ventas
        public ICollection<Venta>? Ventas { get; set; }
    }
}
