namespace MouseTrap.Core;

// Used for serializing to and from JSON
internal class DataStoreModel
{
    public Settings Settings { get; set; }
    public IEnumerable<ProgramItem> PinnedPrograms { get; set; } = [];
}
