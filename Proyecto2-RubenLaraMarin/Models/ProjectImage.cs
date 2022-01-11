using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto2_RubenLaraMarin.Models
{
   public class ProjectImage
   {
      [Key]
      public int ProjectId { get; set; }
      public byte[] ImageData { get; set; }

      public Project Project { get; set; }
   }
}
