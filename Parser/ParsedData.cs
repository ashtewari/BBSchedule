// -----------------------------------------------------------------------
// <copyright file="ParsedData.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BBSchedule.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Container for the parsed data
    /// </summary>
    public class ParsedData
    {
        // Subject, Start Date, Start Time, End Date, End Time, Private, All Day Event, Location

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the start on.
        /// </summary>
        /// <value>
        /// The start on.
        /// </value>
        public DateTime? StartOn { get; set; }

        /// <summary>
        /// Gets or sets the end on.
        /// </summary>
        /// <value>
        /// The end on.
        /// </value>
        public DateTime? EndOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is private.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is private; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is all day event.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is all day event; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllDayEvent { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public Section EventType { get; set; }

        /// <summary>
        /// Gets or sets the event label.
        /// </summary>
        /// <value>
        /// The event label.
        /// </value>
        public string EventDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        /// <value>
        /// The name of the team.
        /// </value>
        public string TeamName { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}", this.Subject, this.TeamName, this.StartOn, this.EndOn, this.EventDescription);
        }
    }
}
