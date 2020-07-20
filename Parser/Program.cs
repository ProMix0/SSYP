using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Net.Http.Headers;

namespace Parser
{
    class Edgeop
    {
        public readonly string first;
        private readonly bool edgeopIsDirect;
        public readonly string second;

        public Edgeop(string input)
        {
            int index = 0;
            string[] inputArray = input.Split(' ');
            if (inputArray.Contains("->"))
            {
                this.edgeopIsDirect = true;
                while (index < inputArray.Length && inputArray[index] != "->")
                {
                    index++;
                }
            }
            else if (inputArray.Contains("--"))
            {
                this.edgeopIsDirect = false;
                while (index < inputArray.Length - 1 && inputArray[index] != "--")
                {
                    index++;
                }
            }
            else
            {
                throw new ArgumentException("Input don't have direction symbol");
            }
            first = inputArray[index - 1].Trim('\t');
            second = inputArray[index + 1].Trim(';','\t');
        }

        public string Display()
        {
            return $"{first} {(edgeopIsDirect ? "->" : "--")} {second}";
        }
    }

    class Graph
    {
        public readonly string name;
        public List<string> nodesNames = new List<string>();
        public List<Edgeop> edgeops = new List<Edgeop>();
        public List<Graph> subgraphs = new List<Graph>();

        public Graph(string[] inputLines)
        {
            string[] graphHeader = inputLines[0].Split(' ');
            this.name = graphHeader[^2];
            int index = 1;
            while (index < inputLines.Length)
            {
                switch (inputLines[index])
                {
                    // Строка с указанием направления
                    case string input when input.Contains("->") || input.Contains("--"):
                        {
                            edgeops.Add(new Edgeop(input));
                            var temp = edgeops[^1];
                            if (!nodesNames.Contains(temp.first))
                                nodesNames.Add(temp.first);
                            if (!nodesNames.Contains(temp.second))
                                nodesNames.Add(temp.second);
                        }
                        break;

                    // Строка с субграфом
                    case string input when input.Contains('{'):
                        {
                            int closeBracketIndex = inputLines.Length - 2;
                            while (!inputLines[closeBracketIndex].Contains('}'))
                                closeBracketIndex--;
                            subgraphs.Add(new Graph(inputLines.Skip(index).SkipLast(inputLines.Length - closeBracketIndex - 1).ToArray()));
                            index = closeBracketIndex;
                        }
                        break;

                    // Строка с объявлением вершины
                    case string input when !(input.Contains('=') || input.Contains('}')):
                        {
                            nodesNames.Add(input.Trim(' ', '\t', ';'));
                        }
                        break;
                }
                index++;
            }
        }

        public IEnumerable<string> Display()
        {
            nodesNames = nodesNames.OrderBy(s => s).ToList();
            edgeops = edgeops.OrderBy(e => e.first).ThenBy(e => e.second).ToList();

            yield return $"Name: {name}";
            yield return $"Nodes names:";
            foreach (var item in nodesNames)
            {
                yield return $"\t{item}";
            }
            yield return $"Edgeops:";
            foreach (var item in edgeops)
            {
                yield return $"\t{item.Display()}";
            }
            if (subgraphs.Count != 0)
            {
                yield return $"Subgraphes:";
                foreach (var item1 in subgraphs)
                {
                    foreach (var item in item1.Display())
                    {
                        yield return $"\t{item}";
                    }
                }
            }
        }
    }

    /*
    Ревью:
    1. Как я понимаю, у тебя поддерживается только ситуация, когда строка с описанием ребёр и вершин содержит только 1 ребро?
       Скучно как-то. https://gitlab.com/graphviz/graphviz/-/blob/master/graphs/undirected/Petersen.gv
       Посмотри на этот файл-пример, выкинь из него аттрибуты (`[ .. ]`) и добавь поддержку описаний вида:
           "0" -- "1" -- "2" -- "3" -- "4" -- "0"
    2. В целом, код достаточно хорош. Странные вещи:
       #1. internal и видимость по умолчанию различных полей. В некоторых случаях я бы поставил public (то, что действительно является свойством, например, ребра),
          а в некоторых я бы поставил private, т.к. это детали реализации.
          Видимость по умолчанию - тоже обычно не используется, пишется явно private (в случае для полей и прочих членов классов).
       2. Display вместо перегрузки ToString(), вместо IEnumerable в Graph юзай StringBuilder.
    #3. Введение явных scope `{ ... }` (как в самом начале конструктора Graph) обычно считается плохим тоном.
       Обычно в таких случаях создают отдельный метод.
    4. Использование ToArray() для выполнения Select с сторонними эффектами (Console.WriteLine) это ещё один некрасивый момент.
       Исправление 2.2. позволит этого избежать.
    #5. Functions.Contains - это вообще зачем? https://docs.microsoft.com/ru-ru/dotnet/api/system.string.contains?view=netcore-3.1#System_String_Contains_System_String_
       Твоя реализация слишком медленная, и она не требуется, когда есть хорошая реализация в framework'е.

    Советую исправить в первую очередь 2-5, 1 - по желанию. Следующее задание я напишу в Slack.
    */

    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in new Graph(new StreamReader(@"C:\Users\Миша\source\repos\SSYP\Parser\Input.gv")
                .ReadToEnd().Split('\n')).Display())
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
