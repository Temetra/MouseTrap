using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace MouseTrap.Core;

[method: JsonConstructor]
public class ProgramItem(string Title, string ProgramPath, string Executable, string Image, bool IsPinned, bool CanTrap)
{
    public static string GetKey(string path, string exe) =>
        GetKey(Path.Combine(path, exe));

    public static string GetKey(string filename)
    {
        var bytes = Encoding.UTF8.GetBytes(filename.ToLowerInvariant());
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    public string Title { get; internal set; } = Title;
    public string ProgramPath { get; internal set; } = ProgramPath;
    public string Executable { get; internal set; } = Executable;
    public string Image { get; internal set; } = Image;
    public bool CanTrap { get; internal set; } = CanTrap;
    [JsonIgnore] public bool IsPinned { get; internal set; } = IsPinned;

    private string _key;
    [JsonIgnore] public string Key => _key ??= GetKey(ProgramPath, Executable);

    private string _fullPath;
    [JsonIgnore] public string FullPath => _fullPath ??= Path.Combine(ProgramPath, Executable);
}
