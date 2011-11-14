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

using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityMessageWriter : BaseWriter, ITeamCityMessageWriter
  {
    public TeamCityMessageWriter(IServiceMessageProcessor target) : base(target)
    {
    }

    private void Write([NotNull] string text, string details, [NotNull] string status)
    {
      var msg = new SimpleServiceMessage("message"){{"text", text}, {"status", status}};
      if (!string.IsNullOrEmpty(details))
        msg.Add("errorDetails", details);

      PostMessage(msg);
    }


    public void WriteMessage(string text)
    {
      Write(text, null, "NORMAL");
    }

    public void WriteWarning(string text)
    {
      Write(text, null, "WARNING");
    }

    public void WriteError(string text, string errorDetails)
    {
      Write(text, errorDetails, "ERROR");
    }
  }
}