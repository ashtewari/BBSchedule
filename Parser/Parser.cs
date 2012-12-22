namespace BBSchedule.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Parses the text file
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// List of months
        /// </summary>
        private readonly IList<string> months = new List<string>() { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

        /// <summary>
        /// The days of the week
        /// </summary>
        private readonly IList<string> daysOfWeek = new List<string>() { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };

        /// <summary>
        /// Split line using these delimiters
        /// </summary>
        private readonly char[] delimiters = new char[] { '\t', ' ' };

        /// <summary>
        /// The current section
        /// </summary>
        private Section currentSection = Section.Header;

        /// <summary>
        /// Parses the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>content as list of strings</returns>
        public IList<ParsedData> Parse(string fileName)
        {
             var list = new List<ParsedData>();

            string fileToRead = System.IO.Path.GetFullPath(fileName);
            Console.WriteLine(string.Format("Reading {0}", fileToRead));

            if (System.IO.File.Exists(fileToRead) == false)
            {
                Console.WriteLine("File not found.");
                return list;
            }
           
            using (System.IO.TextReader reader = new StreamReader(fileToRead))
            {
                bool capture = false;                
                string line = string.Empty, lineIn = string.Empty;
                string currentMonth = string.Empty, currentDate = string.Empty, currentDayOfWeek = string.Empty;
                string currentTime = "12:00AM", currentLocation = "Salem Middle School";
                string eventLabel = string.Empty;
                
                while ((lineIn = reader.ReadLine()) != null)
                {                    
                    line = lineIn.Trim();

                    if (line.Length == 0)
                    {
                        continue;
                    }    
                
                    IList<string> teams = new List<string>();

                    if (line.ToUpperInvariant().Contains("PARKS, RECREATION & CULTURAL RESOURCES"))
                    {
                        this.currentSection = Section.Header;
                        capture = false;
                    }

                    if (line.StartsWith("GAME SCHEDULE"))
                    {
                        this.currentSection = Section.GameSchedule;
                        capture = false;
                    }

                    if (line.StartsWith("PRACTICE SCHEDULE"))
                    {
                        this.currentSection = Section.PracticeSchedule;
                        capture = false;
                    }

                    string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    var firstWord = words[0];

                    if (this.months.Contains(firstWord.ToUpperInvariant()))
                    {
                        currentMonth = firstWord;
                        currentDate = words[1];
                        currentTime = words[3];

                        string teamName = string.Empty;
                        for (int i = 4; i < words.Length; i++)
                        {
                            string tmp = words[i].Trim();
                            if (tmp.Length == 0)
                            { 
                                continue;
                            }

                            if (tmp == "VS" || (tmp == "/" && tmp.Length == 1))
                            {
                                teams.Add(teamName);
                                teamName = string.Empty;
                            }
                            else
                            {
                                if (tmp.StartsWith("/"))
                                {
                                    //record the current teamName
                                    teams.Add(teamName);

                                    //extract new teamName
                                    teamName = tmp.Substring(1, tmp.Length - 1);
                                    teams.Add(teamName);
                                }
                                else
                                {
                                    teamName = string.Format("{0} {1}", teamName, tmp);
                                }
                            }

                            if (i == words.Length - 1)
                            {
                                teams.Add(teamName);
                            }
                        }                        

                        capture = true;
                    }
                    else
                    {
                        if (this.currentSection == Section.GameSchedule || this.currentSection == Section.PracticeSchedule)
                        {
                            int parsedChar;
                            if (int.TryParse(firstWord[0].ToString(CultureInfo.InvariantCulture), out parsedChar))
                            {
                                int nextIndex = 1;
                                if (int.TryParse(words[0], out parsedChar))
                                {
                                    currentDate = words[0];
                                    currentTime = words[2];
                                    nextIndex = 3;
                                }
                                else
                                {
                                    currentTime = words[0];
                                }                                

                                string teamName = string.Empty;
                                for (int i = nextIndex; i < words.Length; i++)
                                {
                                    string tmp = words[i].Trim();
                                    if (tmp.Length == 0)
                                    {
                                        continue;
                                    }

                                    if (tmp == "VS" || (tmp == "/" && tmp.Length == 1))
                                    {
                                        teams.Add(teamName);
                                        teamName = string.Empty;
                                    }
                                    else
                                    {
                                        if (tmp.StartsWith("/"))
                                        {
                                            //record the current teamName
                                            teams.Add(teamName);
                                            
                                            //extract new teamName
                                            teamName = tmp.Substring(1, tmp.Length - 1);                                            
                                        }
                                        else
                                        {
                                            teamName = string.Format("{0} {1}", teamName, tmp);
                                        }
                                    }

                                    if (i == words.Length - 1)
                                    {
                                        teams.Add(teamName);
                                    }
                                }

                                capture = true;

                            }
                            else
                            {                                
                                capture = false;
                            }
                        }
                        else
                        {
                            capture = false;
                        }
                    }

                    if (this.currentSection == Section.PracticeSchedule && line.EndsWith("GAMES"))
                    {
                        capture = false;
                    }

                    if (capture)
                    {
                        switch (currentSection)
                        {
                            case Section.Header:
                                eventLabel = "Header";
                                break;
                            case Section.GameSchedule:
                                eventLabel = "Game";
                                break;
                            case Section.PracticeSchedule:
                                eventLabel = "Practice";
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        string output = string.Format("{0}, {1}", eventLabel, line);

                        if (eventLabel.Length == 0)
                        {
                            output = string.Format("{0}", line);
                        }

                        if (output.EndsWith("GAMES"))
                        {
                            Console.WriteLine(string.Format("Capturing bad output : {0}", output));
                        }

                        // Subject, Start Date, Start Time, End Date, End Time, Private, All Day Event, Location

                        string eventDescription = string.Empty;
                        if (this.currentSection == Section.GameSchedule)
                        {
                            for (int index = 0; index < teams.Count; index++)
                            {
                                var team = teams[index];
                                eventDescription += string.Format("{0}{1}", index != 0 ? " VS " : string.Empty, team);
                            }
                        }
                        else if (this.currentSection == Section.PracticeSchedule)
                        {
                            for (int index = 0; index < teams.Count; index++)
                            {
                                var team = teams[index];
                                eventDescription += string.Format("{0}{1}", index != 0 ? " / " : string.Empty, team);
                            }
                        }

                        foreach (var team in teams)
                        {
                            if (team == "VS")
                            {
                                continue;
                            }

                            bool allDay = false;
                            double? duration = 1.0;

                            var ts = DateTime.Now;
                            var month = this.months.IndexOf(currentMonth) + 1;
                            int year = month < ts.Month ? ts.Year + 1 : ts.Year;
                            int day = Convert.ToInt32(currentDate);
                            int hour = 0;
                            int minute = 0;

                            if (currentTime == "12NOON")
                            {
                                currentTime = "12:00PM";
                            }

                            if ((currentTime.EndsWith("AM") || currentTime.EndsWith("PM")) && currentTime.Contains(":"))
                            {
                                var timeAmPm = currentTime.Substring(currentTime.Length - 2, 2);
                                var tmpTime = currentTime.Substring(0, currentTime.Length - 2);

                                var strings = tmpTime.Split(new char[] { ':' });

                                if (strings.Length == 2)
                                {
                                    if (timeAmPm == "AM")
                                    {
                                        hour = Convert.ToInt32(strings[0] == "12" ? "0" : strings[0]);
                                    }
                                    else if (timeAmPm == "PM")
                                    {
                                        hour = Convert.ToInt32(strings[0] == "12" ? "0" : strings[0]) + 12;
                                    }
                                    else
                                    {
                                        throw new Exception(
                                            string.Format("Bad date time : {0} in '{1}'", currentTime, line));
                                    }

                                    minute = Convert.ToInt32(strings[1]);
                                }

                                ts = new DateTime(year, month, day, hour, minute, 0);
                            }
                            else
                            {
                                continue;
                            }

                            var data = new ParsedData()
                                {
                                    Subject = string.Format("Basketball {0}", eventLabel),
                                    EndOn = duration == null ? ts : ts.AddHours(1),
                                    EventDescription = eventDescription,
                                    EventType = this.currentSection,
                                    IsAllDayEvent = allDay,
                                    IsPrivate = false,
                                    Location = currentLocation,
                                    StartOn = ts,
                                    TeamName = team
                                };

                            list.Add(data);                           
                        }
                    }
                }
            }

            return list;
        }
    }
}
