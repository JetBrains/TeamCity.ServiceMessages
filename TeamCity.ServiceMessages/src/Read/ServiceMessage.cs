using System.Collections.Generic;
using JetBrains.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Read
{
  public class ServiceMessage
  {
    private readonly Dictionary<string, string> myProperties;

    [NotNull]
    public string Name { get; private set; }

    [CanBeNull]
    public string DefaultValue { get; private set; }

    public ServiceMessage([NotNull] string name, [CanBeNull] string defaultValue = null, [NotNull] Dictionary<string, string> properties = null)
    {
      Name = name;
      DefaultValue = properties != null ? null : defaultValue;
      myProperties = properties ?? new Dictionary<string, string>();
    }

    [CanBeNull]
    public string this[[NotNull] string key]
    {
      get
      {
        string s;
        return myProperties.TryGetValue(key, out s) ? s : null;
      }
    }

    public IEnumerable<string> Keys { get { return myProperties.Keys; } }
  }
}