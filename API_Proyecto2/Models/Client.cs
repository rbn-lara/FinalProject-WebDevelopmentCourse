using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_Proyecto2.Models
{
   public class Client
   {
      [Key]
      [StringLength(12)]
      [RegularExpression("(0[0-9]-[0-9]{4}-[0-9]{4})|([0-9]-[0-9]{3}-[0-9]{6})", ErrorMessage = "Debe seguir el formato indicado")]
      [Required(ErrorMessage = "Debe llenar este campo")]
      [DisplayName("Identificación")]
      public string ClientId { get; set; }

      [StringLength(60, MinimumLength = 10, ErrorMessage = "La longitud debe estar entre 10 y 60 caracteres")]
      [Required(ErrorMessage = "Debe llenar este campo")]
      [DisplayName("Nombre Completo")]
      public string ClientName { get; set; }
      [StringLength(9)]
      [RegularExpression("[0-9]{4}-[0-9]{4}", ErrorMessage = "Debe seguir el formato ####-####")]
      [Required(ErrorMessage = "Debe llenar este campo")]
      [DisplayName("Número telefónico")]
      public string Phone { get; set; }

      public List<Project> Projects { get; set; }
      public Client()
      {

      }

   }
}
