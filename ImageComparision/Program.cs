using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageComparision
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("use c to generate base64 from test.png and write to file base64.txt");
            Console.WriteLine("use g to generate test.png from base64.txt");
            Console.WriteLine("Pass the filepath to compare with base64.txt");

            var base64Source = File.ReadAllText("base64.txt");
            if (args.Length == 0)
            {
                Console.WriteLine(AreSame(base64Source, "test.png"));
            }
            else
            {
                if (args[0].Equals("g"))
                {
                    SaveImage(base64Source, "test.png");
                }
                else if (args[0].Equals("c"))
                {
                    File.WriteAllText("base64.txt", GetBase64("test.png"));
                }
                else
                {
                    Console.WriteLine(AreSame(base64Source, args[0]));
                }
            }
        }

        private static string GetBase64(string filePath)
        {
            using (var stream = new MemoryStream())
            {
                Image img = Image.FromFile(filePath);
                img.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);

                return Convert.ToBase64String(stream.ToArray());
            }
        }

        private static void SaveImage(string base64, string filePath)
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(base64)))
            {
                Image img = Image.FromStream(stream);
                img.Save(filePath, ImageFormat.Png);
            }
        }

        private static bool AreSame(Image image1, Image image2)
        {
            byte[] image1Bytes;
            byte[] image2Bytes;

            using (var mstream = new MemoryStream())
            {
                image1.Save(mstream, image1.RawFormat);
                image1Bytes = mstream.ToArray();
            }

            using (var mstream = new MemoryStream())
            {
                image2.Save(mstream, image2.RawFormat);
                image2Bytes = mstream.ToArray();
            }

            var image164 = Convert.ToBase64String(image1Bytes);
            var image264 = Convert.ToBase64String(image2Bytes);

            return string.Equals(image164, image264);
        }

        private static bool AreSame(string base64, string filePath)
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(base64)))
            {
                Bitmap bmp1 = (Bitmap)Image.FromStream(stream);
                Bitmap bmp2 = (Bitmap)Image.FromFile(filePath);

                if (!bmp1.Size.Equals(bmp2.Size))
                {
                    return false;
                }
                for (int x = 0; x < bmp1.Width; ++x)
                {
                    for (int y = 0; y < bmp1.Height; ++y)
                    {
                        if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        private static bool AreEqual(string base641, string base642)
        {
            var bmp1 = (Bitmap)Image.FromStream(new MemoryStream(Convert.FromBase64String(base641)));
            var bmp2 = (Bitmap)Image.FromStream(new MemoryStream(Convert.FromBase64String(base642)));

            if (!bmp1.Size.Equals(bmp2.Size))
            {
                return false;
            }
            for (int x = 0; x < bmp1.Width; ++x)
            {
                for (int y = 0; y < bmp1.Height; ++y)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool AreEqual(Bitmap bmp1, Bitmap bmp2)
        {
            if (!bmp1.Size.Equals(bmp2.Size))
            {
                return false;
            }
            for (int x = 0; x < bmp1.Width; ++x)
            {
                for (int y = 0; y < bmp1.Height; ++y)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
