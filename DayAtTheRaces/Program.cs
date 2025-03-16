// See https://aka.ms/new-console-template for more information
using System;
using System.Threading;

class Greyhound
{
    public string Name { get; set; }
    private static Random random = new Random();
    private int Position = 0;

    public bool Run()
    {
        int distance = random.Next(1, 5); // Move 1-5 steps
        Position += distance;

        if (Position >= 50) // Finish line
        {
            return true;
        }

        return false;
    }

    public void Reset()
    {
        Position = 0;
    }

    public int GetPosition()
    {
        return Position;
    }
}

class Player
{
    public string Name { get; }
    public int Cash { get; private set; }
    public int BetAmount { get; private set; }
    public int BetDogIndex { get; private set; }

    public Player(string name, int cash)
    {
        Name = name;
        Cash = cash;
    }

    public void PlaceBet(int amount, int dogIndex)
    {
        if (amount > Cash)
        {
            Console.WriteLine($"{Name} does not have enough money to place this bet.");
        }
        else
        {
            BetAmount = amount;
            BetDogIndex = dogIndex;
            Console.WriteLine($"{Name} bet ${amount} on Dog #{dogIndex + 1}.");
        }
    }

    public void CollectWinnings(int winner)
    {
        if (BetDogIndex == winner)
        {
            Cash += BetAmount * 2;
            Console.WriteLine($"{Name} won! New balance: ${Cash}");
        }
        else
        {
            Cash -= BetAmount;
            Console.WriteLine($"{Name} lost! New balance: ${Cash}");
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("🏁 Welcome to 'A Day at the Races'! 🏁\n");

        Greyhound[] dogs = {
            new Greyhound { Name = "Speedy" },
            new Greyhound { Name = "Lightning" },
            new Greyhound { Name = "Rocket" }
        };

        Player[] players = {
            new Player("Joe", 100),
            new Player("Bob", 100),
            new Player("Al", 100)
        };

        while (true)
        {
            // Display players' balances
            Console.WriteLine("\n--- Players ---");
            foreach (var player in players)
                Console.WriteLine($"{player.Name} has ${player.Cash}");

            // Players place bets
            foreach (var player in players)
            {
                Console.Write($"\n{player.Name}, enter your bet amount: ");
                int amount = int.Parse(Console.ReadLine());

                Console.Write($"{player.Name}, choose a dog (1-{dogs.Length}): ");
                int dogIndex = int.Parse(Console.ReadLine()) - 1;

                player.PlaceBet(amount, dogIndex);
            }

            Console.WriteLine("\n🏇 The race is starting!\n");
            Thread.Sleep(1000);

            // Start race
            bool raceOver = false;
            int winner = -1;

            while (!raceOver)
            {
                Console.Clear();
                for (int i = 0; i < dogs.Length; i++)
                {
                    if (dogs[i].Run())
                    {
                        raceOver = true;
                        winner = i;
                        break;
                    }
                }

                // Display race progress
                for (int i = 0; i < dogs.Length; i++)
                {
                    Console.WriteLine($"Dog {i + 1}: " + new string('-', dogs[i].GetPosition()) + "🐶");
                }

                Thread.Sleep(300);
            }

            Console.WriteLine($"\n🏆 Dog #{winner + 1} wins! 🏆");

            // Pay out bets
            foreach (var player in players)
            {
                player.CollectWinnings(winner);
            }

            // Reset dogs
            foreach (var dog in dogs)
            {
                dog.Reset();
            }

            // Ask to continue
            Console.Write("\nPlay again? (y/n): ");
            if (Console.ReadLine().ToLower() != "y")
            {
                Console.WriteLine("Thanks for playing! 🏁");
                break;
            }
        }
    }
}
