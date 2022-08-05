namespace SortingYouTubePlaylist;

internal sealed class PlaylistItem
{
    public PlaylistItemId Id { get; set; }
    public PlaylistItemVideo Video { get; set; }
    public DateTime PublishedAt { get; set; }
    public long Position { get; set; }
}

internal sealed class PlaylistItemId
{
    public string ItemId { get; set; }
    public string PlaylistId { get; set; }
    public string Kind { get; set; }
    public string VideoId { get; set; }
}

internal sealed class PlaylistItemVideo
{
    public string ChannelId { get; set; }
    public int Duration { get; set; }
}
