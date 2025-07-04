﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MicroMarket.Models
{
    public class Venta
    {
        [Key]
        public int VentaId { get; set; }
        public int NroVenta { get; set; }
        [Column(TypeName = "date")]
        [Required]

        [Display(Name = "Fecha Venta")]
        public DateOnly FechaVenta { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Precision(10, 2)]

        [Display(Name = "Precio Unidad")]
        public decimal PrecioUnidad { get; set; }
        [Precision(10, 2)]
        public decimal Total { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        //Relaciones

        public ICollection<Venta> Detalles { get; set; } = new List<Venta>();
    }
}
