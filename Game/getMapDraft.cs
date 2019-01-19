using System;
using System.IO;


/// <summary>
/// 
/// </summary>
public class getMap
{
    struct map
    {
        int gridSize;
        int y_size;
        int x_size;

        string imageDir;
    }


    public map getMap()
    {
        map search = new map();

        string directory = null;
        // get directory from wherever data for session is being stored

        string line;
        int i = 0;
        int e = 0;
        int l = 0;

        // fetching data from mapdata.rxtm
        try
        {
            using (StreamReader r = new StreamReader($"{directory}/mapdata.rxtm"))
            {
                while ((line = r.ReadLine()) != null)
                {
                    e = 0;
                    i = 0;
                    Console.Write(line + '\n');
                    if (line[0] != '~')
                    {
                        while (line[i] != '$')
                        {
                            if (line[i] == '#')
                            {
                                e++;
                            }
                            else if (e == 0)
                            {
                                d[l].name += line[i];
                            }
                            else if (e == 1 && line[i] == '1')
                            {
                                d[l].mon = true;
                            }
                            else if (e == 2 && line[i] == '1')
                            {
                                d[l].tue = true;
                            }
                            else if (e == 3 && line[i] == '1')
                            {
                                d[l].wed = true;
                            }
                            else if (e == 4 && line[i] == '1')
                            {
                                d[l].thu = true;
                            }
                            else if (e == 5 && line[i] == '1')
                            {
                                d[l].fri = true;
                            }
                            else if (e == 6 && line[i] == '1')
                            {
                                d[l].sat = true;
                            }
                            else if (e == 7 && line[i] == '1')
                            {
                                d[l].sun = true;
                            }
                            i++;
                        }
                        l++;
                    }
                }
            }
        }

        // Shows an error message if something happens
        catch (Exception er)
        {
            MessageBox.Show(er.Message);
        }

        return search;
    }
}
