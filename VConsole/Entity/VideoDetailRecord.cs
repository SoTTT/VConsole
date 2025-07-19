namespace VConsole.Entity;

public record VideoDetailRecord(
    string VideoId,
    DateOnly VideoReleaseDate,
    string VideoTimeSpan,
    IList<string> VideoGenres,
    string VideoActor,
    string Description = "");