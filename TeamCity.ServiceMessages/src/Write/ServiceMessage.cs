/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write
{
  /// <summary>
  /// Helper class to create complex service message. Use object initilizer to simplify code, i.e.
  /// <code>
  ///   new ServiceMessage("buildProblem") { { "identity", identity}, {"description", message}}
  /// </code>
  /// </summary>
  public class ServiceMessage : IServiceMessage, IEnumerable<KeyValuePair<string, string>> 
  {
    private readonly string myName;    
    private readonly Dictionary<string, string> myArguments = new Dictionary<string, string>();

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="message">service message to copy from</param>
    public ServiceMessage(IServiceMessage message) : this(message.Name)
    {
      AddRange(message.Keys.ToDictionary(x => x, message.GetValue));
    }

    /// <summary>
    /// Simple constructor
    /// </summary>
    /// <param name="name">service message name</param>
    public ServiceMessage([NotNull] string name) 
    {
      myName = name;
    }

    public void Add(string key, string value)
    {
      myArguments[key] = value;
    }

    public void AddRange(IEnumerable<KeyValuePair<string, string>> values)
    {
      foreach (var e in values)
      {
        myArguments[e.Key] = e.Value;
      }      
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
      return myArguments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public string Name
    {
      get { return myName; }
    }

    public string DefaultValue
    {
      get { return null; }
    }

    public IEnumerable<string> Keys
    {
      get { return myArguments.Keys; }
    }

    public string GetValue(string key)
    {
      string s;
      return myArguments.TryGetValue(key, out s) ? s : null;
    }
  }
}
