using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace password_generator
{
    static class Program
    {
        private static readonly IEnumerable<char> LowerCase =
            Enumerable.Range('a', 'z' - 'a' + 1)
                .Select(c => (char) c);

        private static readonly IEnumerable<char> UpperCase =
            Enumerable.Range('A', 'Z' - 'A' + 1)
                .Select(c => (char) c);

        private static readonly IEnumerable<char> Numbers =
            Enumerable.Range('0', '9' - '0' + 1)
                .Select(c => (char) c);

        private static readonly IEnumerable<char> Symbols =
            Enumerable.Range('!', '/' - '!' + 1)
                .Select(c => (char) c);
        
        static void Main(string[] args)
        {   
            Random random = new Random();
            var app = new CommandLineApplication();
            app.Name = "Password Generator";
            app.HelpOption("-h|--help");

            var includeLowerOption = app.Option(
                "-L", 
                "Include lowercase characters", 
                CommandOptionType.NoValue);

            var includeUpperOption = app.Option(
                "-U",
                "Include uppercase characters",
                CommandOptionType.NoValue);

            var includeNumbersOption = app.Option(
                "-N",
                "Include numbers",
                CommandOptionType.NoValue);

            var includeSymbolsOption = app.Option(
                "-S",
                "Include symbols",
                CommandOptionType.NoValue);
            
            var passwordLengthOption = app.Option(
                "-n",
                "Password length",
                CommandOptionType.SingleValue);
            
            app.OnExecute(() =>
            {
                List<char> characters = new List<char>();
                
                if (includeLowerOption.HasValue())
                    characters = characters.Concat(LowerCase).ToList();
                
                if (includeUpperOption.HasValue())
                    characters = characters.Concat(UpperCase).ToList();
                
                if (includeNumbersOption.HasValue())
                    characters = characters.Concat(Numbers).ToList();
                
                if (includeSymbolsOption.HasValue())
                    characters = characters.Concat(Symbols).ToList();

                if (!characters.Any())
                    throw new Exception("You should include at least one character group.");

                var passwordLength = 16;
                if (passwordLengthOption.HasValue())
                {
                    var success = int.TryParse(
                        passwordLengthOption.Value(), 
                        out passwordLength);
                    
                    if (!success) 
                        throw new Exception("Password length has to be a integer.");
                }
                
                var password = "";
                for (int i = 0; i < passwordLength; i++)
                {
                    var charIndex = random.Next(0, characters.Count);
                    password = password + characters[charIndex];
                }
                
                Console.WriteLine(password);
                
                return 0;
            });

            try
            {
                app.Execute(args);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
