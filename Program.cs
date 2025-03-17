using System;
using System.Collections.Generic;
using System.IO;

class Word_Book
{
    static Dictionary<string, List<string>> dictionary = new();
    static string filePath = "vocabulary.txt";
    static string dictionaryType = "undefined";

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Loaded();
        while (true)
        {
            Console.WriteLine("\nMENU:");
            Console.WriteLine("1. Create dictionary");
            Console.WriteLine("2. Add word");
            Console.WriteLine("3. Replace word or translation");
            Console.WriteLine("4. Delete word or translation");
            Console.WriteLine("5. Search translation");
            Console.WriteLine("6. Export word");
            Console.WriteLine("7. Exit");
            Console.Write("Choose action: ");

            string action = Console.ReadLine();
            switch (action)
            {
                case "1": Created(); break;
                case "2": Added(); break;
                case "3": ReplaceMenu(); break;
                case "4": Deleted(); break;
                case "5": Searched(); break;
                case "6": Exported(); break;
                case "7": Exited(); return;
                default: Console.WriteLine("I don’t know this action"); break;
            }
        }
    }

    static void Loaded()
    {
        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    dictionary[parts[0]] = new List<string>(parts[1].Split(","));
                }
            }
            Console.WriteLine("Dictionary loaded successfully!");
        }
    }

    static void Saved()
    {
        using StreamWriter writer = new(filePath);
        foreach (var entry in dictionary)
        {
            writer.WriteLine($"{entry.Key}={string.Join(",", entry.Value)}");
        }
        Console.WriteLine("Dictionary saved successfully!");
    }

    static void Created()
    {
        dictionary.Clear();
        Console.Write("Enter dictionary type (English-Ukrainian): ");
        dictionaryType = Console.ReadLine();
        Console.WriteLine($"New {dictionaryType} dictionary was created!");
    }

    static void Added()
    {
        Console.Write("Enter the word: ");
        string word = Console.ReadLine();

        Console.Write("Enter translation: ");
        string[] translations = Console.ReadLine().Split(',');

        if (!dictionary.ContainsKey(word))
        {
            dictionary[word] = new List<string>();
        }
        dictionary[word].AddRange(translations);
        Console.WriteLine("Added!");
    }

    static void ReplaceMenu()
    {
        Console.WriteLine("1. Replace word\n2. Replace translation\n3. Back");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1": ReplacedWord(); break;
            case "2": ReplacedTranslation(); break;
            case "3": return;
            default: Console.WriteLine("Invalid option"); break;
        }
    }

    static void ReplacedWord()
    {
        Console.Write("Enter word to replace: ");
        string word = Console.ReadLine();
        if (dictionary.ContainsKey(word))
        {
            Console.Write("Enter new word: ");
            string newWord = Console.ReadLine();

            dictionary[newWord] = dictionary[word];
            dictionary.Remove(word);
            Console.WriteLine("Word was updated!");
        }
        else
        {
            Console.WriteLine("I can’t find the word!");
        }
    }

    static void ReplacedTranslation()
    {
        Console.Write("Enter the word: ");
        string word = Console.ReadLine();
        if (dictionary.ContainsKey(word))
        {
            Console.Write("Enter the old translation: ");
            string oldTranslation = Console.ReadLine();

            if (dictionary[word].Contains(oldTranslation))
            {
                Console.Write("Enter the new translation: ");
                string newTranslation = Console.ReadLine();
                dictionary[word].Remove(oldTranslation);
                dictionary[word].Add(newTranslation);
                Console.WriteLine("Translation updated!");
            }
            else
            {
                Console.WriteLine("Old translation not found!");
            }
        }
        else
        {
            Console.WriteLine("I can’t find this word");
        }
    }

    static void Deleted()
    {
        Console.Write("Enter the word to delete: ");
        string word = Console.ReadLine();
        if (dictionary.ContainsKey(word))
        {
            Console.Write("Delete word or translation? ");
            string action = Console.ReadLine();
            if (action == "word")
            {
                dictionary.Remove(word);
                Console.WriteLine("Word was deleted");
            }
            else if (action == "translation")
            {
                Console.Write("Enter translation to delete: ");
                string translation = Console.ReadLine();
                dictionary[word].Remove(translation);
                if (dictionary[word].Count == 0)
                {
                    dictionary.Remove(word);
                    Console.WriteLine("The last translation was deleted, word removed.");
                }
                else
                {
                    Console.WriteLine("Translation was deleted");
                }
            }
        }
        else
        {
            Console.WriteLine("I can’t find this word");
        }
    }

    static void Searched()
    {
        Console.Write("Enter the word: ");
        string word = Console.ReadLine();
        if (dictionary.ContainsKey(word))
        {
            Console.WriteLine("Translation: " + string.Join(", ", dictionary[word]));
        }
        else
        {
            Console.WriteLine("This word is not in the dictionary");
        }
    }

    static void Exported()
    {
        Console.Write("Enter word to export: ");
        string word = Console.ReadLine();
        if (dictionary.ContainsKey(word))
        {
            string exportPath = word + ".txt";
            File.WriteAllLines(exportPath, dictionary[word]);
            Console.WriteLine($"Exported to {exportPath}");
        }
        else
        {
            Console.WriteLine("I can’t find this word");
        }
    }

    static void Exited()
    {
        Saved();
        Console.WriteLine("All saved! Exit");
    }
}
