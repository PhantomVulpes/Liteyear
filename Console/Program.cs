// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Vulpes.Liteyear.Domain.Messaging;
using Vulpes.Liteyear.External.Rabbit;

Console.Title = "Liteyear - Console";
var running = true;


while (running)
{
    WriteLine("Liteyear Console - Type 'exit' to quit.");
    Write("Command> ", ConsoleColor.Cyan);
    var input = Console.ReadLine()?.Trim().ToLowerInvariant();

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    switch (input)
    {
        case "exit":
            running = false;
            WriteLine("Exiting Liteyear Console...");
            break;

        case "help":
            WriteLine("Available commands: exit, help");
            break;

        default:
            WriteError($"Unknown command: '{input}'. Type 'help' for available commands.");
            break;
    }
}


static void WriteError(string error) => WriteLine(error, ConsoleColor.Red);
static void WriteSuccess(string success) => WriteLine(success, ConsoleColor.DarkGreen);

static void WriteLine(string line = "", ConsoleColor color = ConsoleColor.Gray)
{
    Console.ForegroundColor = color;
    Console.WriteLine(line);
    Console.ResetColor();
}

static void Write(string line, ConsoleColor color = ConsoleColor.Gray)
{
    Console.ForegroundColor = color;
    Console.Write(line);
    Console.ResetColor();
}