using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public static class FileReader
{
    private static readonly char[] s_mergeMarkers = {'}', '{', ','};
    private static readonly char s_regionStarter = '{', _regionEnder = '}', s_regionContinuer = ',';

    public static List<string> Read(string file)
    {
        List<string> _lines = File.ReadAllLines(file).ToList();
        List<string> _lines2 = new List<string>();
        foreach (string line in _lines)
        {
            if (line.Length == 0) continue;
            if (s_mergeMarkers.Any<char>(c => line.StartsWith(c)) && _lines2.Count != 0)
            {
                _lines2[_lines2.Count - 1] += line;
            }
            else
            {
                _lines2.Add(line);
            }
        }

        _lines.Clear();
        foreach (string line in _lines2)
            Console.WriteLine(line);
        return _lines2;
    }
}