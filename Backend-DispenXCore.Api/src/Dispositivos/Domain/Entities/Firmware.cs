namespace Backend_DispenXCore.Api.src.Dispositivos.Domain.Entities;

public class Firmware
{
    public string Id { get; private set; }
    public string Version { get; private set; }
    public DateTime ReleasedAt { get; private set; }
    public string Changelog { get; private set; }
    public bool IsLatest { get; private set; }
    public string DownloadUrl { get; private set; }

    private Firmware() { }
    public Firmware(string id, string version, string changelog, string downloadUrl, bool isLatest = false)
    {
        Id = id;
        Version = version;
        ReleasedAt = DateTime.UtcNow;
        Changelog = changelog;
        DownloadUrl = downloadUrl;
        IsLatest = isLatest;
    }

    public void SetLatest(bool latest) => IsLatest = latest;
}