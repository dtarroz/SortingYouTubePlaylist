namespace SortingYouTubePlaylist;

internal sealed class PlaylistItem
{
    public PlaylistItemId Id { get; set; } = null!;
    public PlaylistItemVideo Video { get; set; } = null!;
    public DateTime PublishedAt { get; set; }
    public long Position { get; set; }
    public string? Note { get; set; }
}

internal sealed class PlaylistItemId
{
    public string ItemId { get; set; } = null!;
    public string PlaylistId { get; set; } = null!;
    public string Kind { get; set; } = null!;
    public string VideoId { get; set; } = null!;
}

internal sealed class PlaylistItemVideo
{
    public string Title { get; set; } = null!;
    public string Channel { get; set; } = null!;
    public string ChannelId { get; set; } = null!;
    public int Duration { get; set; }
}
