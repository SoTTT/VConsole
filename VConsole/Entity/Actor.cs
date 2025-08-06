using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VConsole.Entity;

public class Actor
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public int VideoDetailRecordId { get; set; }
    public List<VideoDetailRecord> VideoDetailRecord { get; set; }
}