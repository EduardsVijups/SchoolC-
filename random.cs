// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using System;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        int x = 1;
        int y = 1;
        string[] m = {
            "##########",
            "#@       #",
            "#        #",
            "#        #",
            "#        #",
            "#        #",
            "##########",
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
        run(x, y);
    }
    public static void draw(string[] m)
    {

        for (int i = 0; i < m.Length; i++)
        {
            Console.WriteLine(m[i]);
        }
    }
    public static void run(int a, int b)
    {
        int x = a;
        int y = b;
        string[] m = {
            "##########",
            "#        #",
            "#        #",
            "#        #",
            "#        #",
            "#        #",
            "##########",
        };
        string input;
        input = Console.ReadLine();
        if (input == "a") {
            if (x > 1) {
                x -= 1;
            } else {
                x = 1;
            }
        }
        if (input == "d") {
            if (x < 8) {
                x += 1;
            } else {
                x = 8;
            }
        }
        if (input == "w") {
            if (y > 1) {
                y -= 1;
            } else {
                y = 1;
            }
        }
        if (input == "s") {
            if (y < 5) {
                y += 1;
            } else {
                y = 5;
            }
        }
        for (int i = 0; i < m.Length; i++)
        {
            if (i == y){
                var pp = m[i].ToCharArray();
                pp[x] = '@';

                var result = new string(pp);
                m[i] = result;
                
                draw(m);
                
            }
            
        }
        run(x, y);
    }
}
