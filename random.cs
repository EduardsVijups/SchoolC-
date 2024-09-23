using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Project1
{
    public static void Main(string[] args)
    {
        int x = 2;
        int y = 1;
        int score = 0;
        string[] m = {
            "# # # # # # # # # #",
            "# @               #",
            "#                 #",
            "#                 #",
            "#                 #",
            "#                 #",
            "# # # # # # # # # #",
        };
        /*for (int j = 0; j < 10; j++) {
            Console.WriteLine(" ");
            for (int i = 0; i < 10; i++) {
                Console.Write(a);
            }
        }*/
        /*for (int i = 0; i < m.Length; i++)
        {
            if (i == y){
                var pp = m[i].ToCharArray();
                pp[x] = '@';

                var result = new string(pp);
                m[i] = result;
                
                draw(m);
                
            }
            
        }*/
        draw(m);
        Random rnd = new Random();
        int randX = rnd.Next(1, 8)*2;
        int randY = rnd.Next(1, 5);
        run(x, y, randX, randY, score);
    }
    public static void draw(string[] m)
    {

        for (int i = 0; i < m.Length; i++)
        {
            Console.WriteLine(m[i]);
        }
    }

    public static void run(int a, int b, int c, int d, int e)
    {
        int x = a;
        int y = b;
        int randX = c;
        int randY = d;
        int score = e;
        string[] m = {
            "# # # # # # # # # #",
            "#                 #",
            "#                 #",
            "#                 #",
            "#                 #",
            "#                 #",
            "# # # # # # # # # #",
        };
        string input;
        input = Console.ReadLine();
        if (input == "a")
        {
            if (x > 1)
            {
                x -= 2;
            }
            else
            {
                x = 1;
            }
        }
        if (input == "d")
        {
            if (x < 17)
            {
                x += 2;
            }
            else
            {
                x = 17;
            }
        }
        if (input == "w")
        {
            if (y > 1)
            {
                y -= 1;
            }
            else
            {
                y = 1;
            }
        }
        if (input == "s")
        {
            if (y < 5)
            {
                y += 1;
            }
            else
            {
                y = 5;
            }
        }
        if (y == randY && x == randX)
        {
            score += 1;
            Random rnd = new Random();
            randX = rnd.Next(2, 8)*2;
            randY = rnd.Next(1, 5);
        }
        for (int i = 0; i < m.Length; i++)
        {
            if (i == y)
            {
                var pp = m[i].ToCharArray();
                pp[x] = '@';

                var result = new string(pp);
                m[i] = result;

            }
            if (i == randY)
            {
                var pp = m[i].ToCharArray();
                pp[randX] = 'X';

                var result = new string(pp);
                m[i] = result;


            }


        }
        draw(m);
        Console.WriteLine("Score = " + score);
        run(x, y, randX, randY, score);
    }
}
