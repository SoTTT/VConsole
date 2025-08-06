using System.ComponentModel.DataAnnotations;

namespace VConsole.Entity;

public class Genre
{
    [Key] public int Id { get; set; }
    public string TagName { get; set; }

    public int VideoDetailRecordId { get; set; }
    public List<VideoDetailRecord> VideoDetailRecord { get; set; }
}