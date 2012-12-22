namespace BBSchedule.Viewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using BBSchedule.Data;

    /// <summary>
    /// Formats data according to the type of requested output.
    /// </summary>
    public class Formatter
    {
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>String representation of the header.</returns>
        public virtual string GetHeader()
        {
            return string.Empty;
        }

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <param name="data">The data to be formatted.</param>
        /// <returns>
        /// String representation of the data.
        /// </returns>
        public virtual string Format(ParsedData data)
        {
            return data.ToString();
        }
    }
}
