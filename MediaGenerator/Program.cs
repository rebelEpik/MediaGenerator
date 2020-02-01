using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MediaGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> movieList = new List<string>();

            Console.WriteLine("Provide path to directory...");
            var input = Console.ReadLine();
            if (!Directory.Exists(input))
            {
                Console.WriteLine("Directory does not exist, try again");
                Environment.Exit(0);
            }
            Console.WriteLine("Output Path...");
            var output = Console.ReadLine();



            Console.WriteLine("Now generating list");
            var allFiles = Directory.EnumerateFiles(input, "*.*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".mp4") || s.EndsWith(".mkv") || s.EndsWith(".avi") && !s.ToLower().Contains("sample"));
            foreach (var file in allFiles)
            {

                FileInfo info = new FileInfo(file);

                var name = info.Name.Replace(".", " ");

                name = name.Remove((name.Length - 4));

                var filter = name.Split(' ');
                var t = filter[0] + " ";

                foreach (var n in filter.Skip(1))
                {
                    if (n.Contains("20") || n.Contains("19") || n.Contains("(") || n.ToLower().Contains("sample") || n.ToLower().Contains("etrg"))
                    {
                        break;
                    }
                    else
                    {
                        t += n + " ";
                    }


                }
                if (t.Length > 0)
                {
                    movieList.Add(t);
                }


            }

            CreateFiles(output);

            GenerateOutput(movieList, output);
        }

        private static void GenerateOutput(List<string> movieList, string output)
        {

            movieList.Sort();
            List<string> final = movieList.Distinct().ToList();
            string openHtml = "<html><head><title>Movie List</title><link rel='stylesheet' type='text/css' href='MovieList.css'></head><body><input type='text' id='myInput' onkeyup='myFunction()' placeholder='Movie?'> <ul id='myUL'>";
            string closeHtml = @"</ul><script src='script.js'></script></body></html>";
            using (FileStream fs = new FileStream(output + "MovieList.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(openHtml);
                    foreach (var n in final)
                    {
                        w.WriteLine(@"<li><a href='#'>" + n + "</a></li>");
                    }
                    w.WriteLine(closeHtml);
                }
            }


        }

        public static void CreateFiles(string output)
        {
            string js = AppDomain.CurrentDomain.BaseDirectory + @"HTML\script.js";
            string css = AppDomain.CurrentDomain.BaseDirectory + @"HTML\MovieList.css";

            if (File.Exists(output + "script.js"))
            {
                File.Delete(output + "script.js");
                File.Copy(js, output + "script.js");
            }
            else
            {
                File.Copy(js, output + "script.js");
            }

            if (File.Exists(output + "MovieList.css"))
            {
                File.Delete(output + "MovieList.css");
                File.Copy(css, output + "MovieList.css");
            }
            else
            {
                File.Copy(css, output + "MovieList.css");
            }

        }


    }
}