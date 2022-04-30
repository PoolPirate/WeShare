using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WeShare.Infrastructure.Options;
public class StorageOptions : Option
{
    [Required]
    public string PostStoragePath { get; set; }
}

