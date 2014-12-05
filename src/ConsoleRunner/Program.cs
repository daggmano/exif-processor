using System;
using System.IO;
using System.Reflection;
using ExifProcessLib;
using ExifProcessLib.Models;

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
                var tags = exifProcessor.ProcessImage();

                if (tags != null)
                {
                    Console.WriteLine(file);
                    Console.WriteLine("==============================================================================");
                    foreach (var tag in tags)
                    {
                        if (tag.IFDType == IFDType.IFD_GPS)
                        {
                            Console.WriteLine(tag.GPSTag + ": " + tag.ToDisplayString());
                        }
                        else
                        {
                            Console.WriteLine(tag.ExifTag + ": " + tag.ToDisplayString());
                        }
                    }

                    Console.WriteLine();
                }
            }

            Console.ReadLine();
        }
    }
}
