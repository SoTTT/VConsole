using System.ComponentModel.DataAnnotations.Schema;

namespace VConsole.Entity;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[Table(name: "VideoDetailRecords")]
public class VideoDetailRecord
{
    [Key] public int Id { get; set; }
    public string VideoId { get; set; }
    public DateOnly VideoReleaseDate { get; set; }
    public string VideoTimeSpan { get; set; }
    public List<Genre> VideoGenres { get; set; }
    public List<Actor> VideoActor { get; set; }
    public string Description { get; set; } = "";
}