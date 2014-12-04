using System.IO;
using System.Reflection;
using ExifProcessLib;

namespace ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var testBaseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filenames = new[]
            {
                testBaseDir + @"\TestImages\L1004220.DNG",
                testBaseDir + @"\TestImages\IMG_5953.CR2",
                testBaseDir + @"\TestImages\img_1771.jpg",
                testBaseDir + @"\TestImages\IMG_5006.JPG"
            };

            foreach (var file in filenames)
            {
                var exifProcessor = new ExifProcessor(file);
                exifProcessor.ProcessImage();
            }
        }
    }
}
