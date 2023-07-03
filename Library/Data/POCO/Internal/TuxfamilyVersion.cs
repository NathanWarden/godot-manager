#nullable enable
using System;
using GodotManager.Library.Utility;
using LiteDB;

namespace GodotManager.Library.Data.POCO.Internal;

public class TuxfamilyVersion
{
    [BsonId]
    public Guid Id { get; set; }
    public string TagName { get; set; }
    public VersionUrls Standard { get; set; }
    public VersionUrls CSharp { get; set; }
    public VersionUrl Sha512Sums { get; set; }
    public VersionUrl Source { get; set; }
    public VersionUrl AndroidEditor { get; set; }
    public VersionUrl AndroidLibs { get; set; }
    public string ReleaseStage { get; set; }

    private SemanticVersion _semVersion = null;

    [BsonIgnore]
    public SemanticVersion SemVersion => _semVersion ??=
        SemanticVersion.Parse(ReleaseStage == "Stable" ? TagName : $"{TagName}-{ReleaseStage}");

    public VersionUrl? this[string key] => key switch
    {
        "Sha512Sums" => Sha512Sums,
        "Source" => Source,
        "AndroidEditor" => AndroidEditor,
        "AndroidLibs" => AndroidLibs,
        _ => null
    };

    public VersionUrl? this[string key, string subkey] => key switch
    {
        "Standard" => Standard[subkey],
        "CSharp" => CSharp[subkey],
        _ => null
    };

    [BsonIgnore]
    public string StandardDownloadUrl => Platform.Get() switch
    {
        PlatformType.Windows => Platform.Is64Bit ? Standard.Win64.Url : Standard.Win32.Url,
        PlatformType.LinuxBSD => Platform.Is64Bit ? Standard.Linux64.Url : Standard.Linux32.Url,
        PlatformType.MacOS => Standard.OSX.Url,
        _ => string.Empty
    };

    [BsonIgnore]
    public string CSharpDownloadUrl => Platform.Get() switch
    {
        PlatformType.Windows => Platform.Is64Bit ? CSharp.Win64.Url : CSharp.Win32.Url,
        PlatformType.LinuxBSD => Platform.Is64Bit ? CSharp.Linux64.Url : CSharp.Linux32.Url,
        PlatformType.MacOS => CSharp.OSX.Url,
        _ => string.Empty
    };

    [BsonIgnore]
    public int StandardDownloadSize => Platform.Get() switch
    {
        PlatformType.Windows => Platform.Is64Bit ? Standard.Win64.Size : Standard.Win32.Size,
        PlatformType.LinuxBSD => Platform.Is64Bit ? Standard.Linux64.Size : Standard.Linux32.Size,
        PlatformType.MacOS => Standard.OSX.Size,
        _ => -1
    };
    
    [BsonIgnore]
    public int CSharpDownloadSize => Platform.Get() switch
    {
        PlatformType.Windows => Platform.Is64Bit ? CSharp.Win64.Size : CSharp.Win32.Size,
        PlatformType.LinuxBSD => Platform.Is64Bit ? CSharp.Linux64.Size : CSharp.Linux32.Size,
        PlatformType.MacOS => CSharp.OSX.Size,
        _ => -1
    };
}