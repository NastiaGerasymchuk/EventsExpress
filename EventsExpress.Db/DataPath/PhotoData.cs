namespace EventsExpress.Db.DataPath
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using EventsExpress.Db.Entities;

    public class PhotoData
    {
        private const string PhotoPath = Path.PhotoPath;

        public const string FilePathMongo = Path.FilePathMongoPhoto;

        public Photo GetPhoto(string filePath)
        {
            return new Photo
            {
                Id = Guid.NewGuid(),
                Img = ReadFile(filePath),
                Thumb = ReadFile(filePath),
            };
        }

        private byte[] ReadFile(string sPath)
        {
            byte[] data = null;
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            data = br.ReadBytes((int)numBytes);
            return data;
        }

        public IEnumerable<Photo> GetPhotossFromFile()
        {
            List<Photo> photos = new List<Photo>();
            var csvTable = new DataTable();
            using (var csvReader = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(System.IO.File.OpenRead(PhotoPath)), true))
            {
                csvTable.Load(csvReader);
                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    photos.Add(new Photo
                    {
                        Id = new Guid(csvTable.Rows[i][0].ToString()),
                        Img = ReadFile(FilePathMongo),
                        Thumb = ReadFile(FilePathMongo),
                    });
                }
            }

            return photos;
        }
    }
}
