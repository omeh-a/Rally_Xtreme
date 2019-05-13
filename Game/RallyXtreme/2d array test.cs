using System;

public class Class1
{
	public Class1()
	{
        int[] array1 = new int[16];
        int i = 0;

        array1[0] = new int[16];
    

        while (i < 16)
        {
            array1[0][i] = i;
            System.Console.WriteLine(array1[0][i]);
            i++;
        }

	}
}
