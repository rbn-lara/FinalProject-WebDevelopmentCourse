using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_Proyecto2.Models
{
   public class Project
   {
      [DisplayName("Cliente")]
      public string ClientId { get; set; }

      [Key]
      public int ProjectId { get; set; }

      [DisplayName("Nombre de proyecto")]
      [StringLength(60, MinimumLength = 10, ErrorMessage = "La longitud debe estar entre 10 y 60 caracteres")]
      [Required]
      public string ProjectName { get; set; }

      [DisplayName("Habitaciones")]
      [Range(1, 6, ErrorMessage = "Debe ser entre 1 y 6 habitaciones")]
      [Required]
      public int Rooms { get; set; }

      [DisplayName("Baños completos")]
      [Range(1, 5, ErrorMessage = "Debe ser entre 1 y 5 baños")]
      [Required]
      public int Bathrooms { get; set; }

      [DisplayName("Medios baños")]
      [Range(1, 3, ErrorMessage = "Debe ser entre 1 y 3 medios baños")]
      [Required]
      public int HalfBathrooms { get; set; }

      [DisplayName("Sala y cocina juntas")]
      public bool IsLivingRoomKitchen { get; set; }

      [DisplayName("Área de pilas al aire libre")]
      public bool HaveSinkOutdoors { get; set; }

      [DisplayName("Tamaño de terraza")]
      [Required]
      public int Terrace { get; set; }

      [DisplayName("Tipo de piso")]
      [Required]
      public int FloorType { get; set; }

      [DisplayName("Tipo de mueble de cocina")]
      public int KitchenFurnitureType { get; set; }

      [DisplayName("Tamaño de construcción")]
      public int ConstructionSize { get; set; }

      [DisplayName("Precio por metro cuadrado")]
      public int PriceBySquaredMeter { get; set; }

      public ProjectImage ProjectImage { get; set; }

      public Project()
      {

      }

   }
}
