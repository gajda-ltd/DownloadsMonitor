namespace DownloadsMonitor.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public sealed class FileEntry
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(256)]
        public string FileName { get; set; }
        [Required]
        public long Length { get; set; }
        [Required]
        [StringLength(48)]
        public string MD5 { get; set; }
    }
}
