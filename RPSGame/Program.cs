using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPSGame
{

    class Program
    {
        static string HMACHASH(string str, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            using (var hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(str);
                var bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", string.Empty);
            }
        }
        static int GenerateComputerMove(string[] ch)
        {
            Random rnd = new Random();
            int ComputerMove = rnd.Next(0, ch.Length);
            return ComputerMove;
        }
        static string GenerateSecretKey()
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[16];
            rng.GetBytes(tokenData);
            string secretKey = Convert.ToBase64String(tokenData);
            return secretKey;
        }
        static void Menu(string[] ch)
        {
            Console.WriteLine("Available moves:");
            for (int i = 0; i < ch.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {ch[i]}");
            }
            Console.WriteLine($"0 - Exit");
        }
        static int PlayerMove(string[] ch)
        {
            bool Acceptplayermove = false;
            int playermove = -1;
            do
            {
                Console.Write("Enter your move: ");
                string playermovestr = Console.ReadLine();
                if (int.TryParse(playermovestr, out int number))
                {
                    playermove = number - 1;
                    if (playermove >= 0 && playermove <= ch.Length)
                    {
                        Acceptplayermove = true;
                    }
                    else if (playermove == 0)
                    {
                        Console.WriteLine("Game is over :(");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Menu(ch);
                        Console.WriteLine("Not Found, Retry");
                    }
                }
            }
            while (!Acceptplayermove);
            return playermove;
        }

        static string[] StartParams()
        {
            string gamestr = Console.ReadLine();
            string[] ch = gamestr.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return ch;
        }
        static void Main(string[] args)
        {

            string[] ch = StartParams();
            while (ch.Length < 3  || ch.Length % 2 == 0)
            {
                ch = StartParams();
            }


            string secretKey = GenerateSecretKey();

            int ComputerMove = GenerateComputerMove(ch);
            string HMAC = HMACHASH(ch[ComputerMove], secretKey);


            Console.WriteLine("HMAC :" + HMAC);

            Menu(ch);



            int playermove = PlayerMove(ch);


            Console.WriteLine($"You move: {ch[playermove]}");
            Console.WriteLine($"Computer move: {ch[ComputerMove]}");

            int step = (int)((ch.Length - 1) / 2);

            string winner = null;

            if (playermove != ComputerMove)
            {
                for (int i = playermove % ch.Length + 1; i <= playermove + step; i++)
                {
                    if (i % ch.Length == ComputerMove)
                    {

                        winner = "You win!";
                        break;

                    }
                    winner = "Computer win";
                }
            }

            else
            {
                winner = "no winner";
            }

            Console.WriteLine(winner);
            Console.WriteLine("HMAC key: " + secretKey);

        }
    }
}
