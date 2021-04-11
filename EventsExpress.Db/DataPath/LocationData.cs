namespace EventsExpress.Db.DataPath
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using CsvHelper;
    using EventsExpress.Db.Entities;

    public class LocationData
    {
        private const string EventLocationPath = Path.LOCATIONPATH;

        public static IEnumerable<EventLocation> GetEventsLocationFromFile()
        {
           List<EventLocation> eventsLocation = new List<EventLocation>();
           DataTable csvTable = new DataTable();
           using (var csvReader = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(System.IO.File.OpenRead(EventLocationPath)), true))
            {
                csvTable.Load(csvReader);
                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    eventsLocation.Add(new EventLocation { Id = Guid.NewGuid(), OnlineMeeting = new Uri(csvTable.Rows[i][0].ToString()), Type = Enums.LocationType.Online });
                }
            }

           return eventsLocation;
        }
    }
}
