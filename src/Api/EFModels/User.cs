using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.EFModels
{
  [Table("User")] 
  public partial class User
  {
    [Key]
    public string guid { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public DateTime created { get; set; }
  }
}