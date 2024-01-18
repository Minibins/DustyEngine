using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public static class FileReader
{
    static readonly char[] mergeMarkers = { '}', '{', ','};
    static readonly char regionStarter = '{', regionEnder = '}', regionContinuer = ',';
    public static List<string> Read(string file)
    {
        List<string> lines = File.ReadAllLines(file).ToList();
        List<string> lines2 = new List<string>();
        foreach(string line in lines)
        {
            if(line.Length == 0) continue;
            if(mergeMarkers.Any<char>(c => line.StartsWith(c))&&lines2.Count!=0)
            {
                lines2[lines2.Count-1]+=line;
            }
            else
            {
                lines2.Add(line);
            }
        }
        lines.Clear();
        foreach(string line in lines2)
            Console.WriteLine(line);
        return lines2;
    }
}