using System.ComponentModel.DataAnnotations;

namespace API_Proyecto2.Models
{
   public class ProjectImage
   {
      [Key]
      public int ProjectId { get; set; }
      public byte[] ImageData { get; set; }

      public Project Project { get; set; }
   }
}
