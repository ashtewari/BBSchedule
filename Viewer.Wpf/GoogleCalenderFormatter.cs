namespace BBSchedule.Viewer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using BBSchedule.Data;

    /// <summary>
    /// Formats data according to the type of requested output.
    /// </summary>
    public class GoogleCalenderFormatter : Formatter
    {
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="data">The data to be formatted.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string Format(ParsedData data)
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", string.Format("{0} ({1})", data.Subject, data.TeamName), string.Format("{0:MM/dd/yy}", data.StartOn), string.Format("{0:hh:mm tt}", data.StartOn), string.Format("{0:MM/dd/yy}", data.EndOn), string.Format("{0:hh:mm tt}", data.EndOn), data.IsPrivate.ToString(CultureInfo.InvariantCulture).ToUpperInvariant(), data.IsAllDayEvent.ToString(CultureInfo.InvariantCulture).ToUpperInvariant(), data.Location, data.EventDescription);
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>
        /// String representation of the header.
        /// </returns>
        public override string GetHeader()
        {
            return "Subject, Start Date, Start Time, End Date, End Time, Private, All Day Event, Location, Description";
        }
    }
}
