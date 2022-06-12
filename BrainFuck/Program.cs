using BrainFuck.BF;
using BrainFuck.Interfaces.Menus;
using BrainFuck.IO;
using BrainFuck.Menus;

namespace BrainFuck;

using System.Text;  //убрать позже

public static class Program
{
    public static void Main()
    {
        var repository = new Repository("++++++++10--------20++++++++30--------40++++++++50--------60++++++++70--------80++++++++90-------100+++++++110-------120+++++++130-------140+++++++150-------160+++++++170-------180+++++++190-------200");
        var bfDebugger = new BfDebuggerMenu(repository);
        bfDebugger.DebugMenu();

        //var menu = BuildMenu();
        //menu.RunMenu();
    }

    private static IMenu BuildMenu()
    {
        var consoleCursorWrapper = new ConsoleCursorWrapper();
        var inputOutput = new InputOutput(Console.In, Console.Out, consoleCursorWrapper);
        var menuBuilder = new MenuBuilder(inputOutput);
        var bfInterpretation = new BfInterpretation(inputOutput);
        menuBuilder
            .AddNewMenuLine(
                new MenuLine("Запустить стандартную программу",
                new DefaultBrainFuckCommand(bfInterpretation)))
            .AddNewMenuLine(
                new MenuLine("Ввести программу вручную",
                new HandelInputBfCodeAndRunCommand(bfInterpretation)))
            .AddNewMenuLine(
                new MenuLine("Дебаг меню",
                new ExitCommand(menuBuilder.ExitToken)))
            .AddNewMenuLine(
                new MenuLine("Выйти",
                new ExitCommand(menuBuilder.ExitToken)));

        return menuBuilder.Build();
    }

    public class BfDebuggerMenu
    {
        private readonly Repository _dataFromRepository;
        private char _cursor = '^';
        private string _BFCodeString;
        private int _cursorIndex = 0;

        public BfDebuggerMenu(Repository dataFromRepository)
        {
            _dataFromRepository = dataFromRepository;
            _BFCodeString = dataFromRepository.Program;
        }

        public void PrintBFCode(string programString)
        {
            int consoleWidth = Console.WindowWidth;
            if (programString.Length > consoleWidth) 
            {
                for (int i = 0; i < consoleWidth; i++) 
                {
                    Console.Write(programString[i]);
                }
            }
            else
            {
                Console.Write(programString);
            }
        }

        public string DeleteFirstCharInString(string programString)
        {
            return _BFCodeString = programString.Remove(0, 1);
        }

        public void PrintCursorAndBFCode() 
        {
            var consoleCursorWrapper = new ConsoleCursorWrapper();
            int consoleWidthHalf = Console.WindowWidth / 2;
            consoleCursorWrapper.SetCursorPosition(0, 1);
            Console.Write(_cursor);
            while (true)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.N)
                {
                    if (_cursorIndex < consoleWidthHalf)
                    {
                        Console.Write("\b ");
                        Console.Write(_cursor);
                    }
                    else if(_cursorIndex >= consoleWidthHalf && _cursorIndex < _dataFromRepository.Program.Length - consoleWidthHalf)
                    {
                        consoleCursorWrapper.SetCursorPosition(0, 0);
                        PrintBFCode(DeleteFirstCharInString(_BFCodeString));
                    }
                    else if (_cursorIndex == _dataFromRepository.Program.Length - consoleWidthHalf)
                    {
                        consoleCursorWrapper.SetCursorPosition(consoleWidthHalf, 1);
                        Console.Write("\b ");
                        Console.Write(_cursor);
                    }
                    else if (_cursorIndex >= _dataFromRepository.Program.Length - consoleWidthHalf)
                    {
                        Console.Write("\b ");
                        Console.Write(_cursor);
                    }
                    _cursorIndex += 1;
                }
            }
        }

        public void DebugMenu() 
        {
            PrintBFCode(_dataFromRepository.Program);
            while (true) 
            {
                PrintCursorAndBFCode();
            }
        }
    }
}