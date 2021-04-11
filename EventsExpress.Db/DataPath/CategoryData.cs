using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EventsExpress.Db.Entities;
using LumenWorks.Framework.IO.Csv;

namespace EventsExpress.Db.DataPath
{
    public class CategoryData
    {
        private const string CategoryPath = Path.CATEGORYPATH;

        public static Category FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            Category category = new Category();
            category.Name = values[0];
            category.Id = new Guid(values[1]);
            return category;
        }

        public static IEnumerable<Category> GetCategoriesFromFile()
        {
            List<Category> categories = File.ReadAllLines(CategoryPath)
                                           .Skip(1)
                                           .Select(v => FromCsv(v))
                                           .ToList();

            // var csvTable = new DataTable();
            // using (var csvReader = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(System.IO.File.OpenRead(CategoryPath)), true))
            // {
            //    csvTable.Load(csvReader);
            //    for (int i = 0; i < csvTable.Rows.Count; i++)
            //    {
            //        categories.Add(new Category { Id = new Guid(csvTable.Rows[i][1].ToString()), Name = csvTable.Rows[i][0].ToString() });
            //    }
            // }
            return categories;
        }
    }
}
