namespace Core.Image.Dto
{
    /// <summary>Makaleye bağlı olmayan, yazı içeriğine gömülen görsel.</summary>
    public record ContentImageDto(string FileName, string OriginalFileName, string Path, string ContentType, long FileSize);
}
