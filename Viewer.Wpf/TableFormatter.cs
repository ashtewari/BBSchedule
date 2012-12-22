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
    public class TableFormatter : Formatter
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
            return string.Format("{0},{1},{2},{3},{4},{5}", data.Subject.Trim(), data.TeamName.Trim(), string.Format("{0:MM/dd/yy}", data.StartOn), string.Format("{0:h:mm tt}", data.StartOn), data.Location.Trim(), data.EventDescription.Trim());
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>
        /// String representation of the header.
        /// </returns>
        public override string GetHeader()
        {
            return "Subject,Team,Date,Time,Location,Description";
        }
    }
}
