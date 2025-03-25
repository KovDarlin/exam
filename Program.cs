using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Dictionary<string, (string Password, DateTime BirthDate)> users = new Dictionary<string, (string, DateTime)>();
    static Dictionary<string, List<int>> quizResults = new Dictionary<string, List<int>>();
    static Dictionary<string, List<Question>> quizzes = new Dictionary<string, List<Question>>();
    static Random rnd = new Random();

    static void Main()
    {

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("Вітаю у Вікторині!");
            Console.WriteLine("1. Увійти");
            Console.WriteLine("2. Зареєструватися");
            Console.WriteLine("3. Вхід для адміністратора");
            Console.WriteLine("4. Вихід");
            Console.Write("Оберіть опцію: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Login();
            }
            else if (choice == "2")
            {
                Register();
            }
            else if (choice == "3")
            {
                AdminLogin();
            }
            else if (choice == "4")
            {
                Console.WriteLine("Дякую за гру! До побачення!");
                break;
            }
            else
            {
                Console.WriteLine("Невірний вибір. Спробуйте знову.");
            }
        }
    }

    static void Register()
    {
        Console.Write("Введіть логін: ");
        string login = Console.ReadLine();

        if (users.ContainsKey(login))
        {
            Console.WriteLine("Такий логін вже існує!");
            return;
        }

        Console.Write("Введіть пароль: ");
        string password = Console.ReadLine();

        Console.Write("Введіть дату народження (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
        {
            Console.WriteLine("Некоректний формат дати.");
            return;
        }

        users.Add(login, (password, birthDate));
        quizResults[login] = new List<int>();
        Console.WriteLine("Реєстрація успішна! Тепер увійдіть у систему.");
    }

    static void Login()
    {
        Console.Write("Введіть логін: ");
        string login = Console.ReadLine();

        Console.Write("Введіть пароль: ");
        string password = Console.ReadLine();

        if (users.ContainsKey(login) && users[login].Password == password)
        {
            Console.WriteLine("Вхід успішний! Ласкаво просимо, " + login + "!");
            if (!quizResults.ContainsKey(login))
            {
                quizResults[login] = new List<int>();
            }
            UserMenu(login);
        }
        else
        {
            Console.WriteLine("Невірний логін або пароль.");
        }
    }

    static void UserMenu(string login)
    {
        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Стартувати нову вікторину");
            Console.WriteLine("2. Переглянути результати вікторин");
            Console.WriteLine("3. Переглянути Топ-20");
            Console.WriteLine("4. Вийти в головне меню");
            Console.Write("Оберіть опцію: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartQuiz(login);
                    break;
                case "2":
                    ViewResults(login);
                    break;
                case "3":
                    ViewTop20();
                    break;
                case "4":
                    Console.WriteLine("Вихід у головне меню...");
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте знову.");
                    break;
            }
        }
    }

    static void AdminLogin()
    {
        Console.Write("Введіть логін: ");
        string login = Console.ReadLine();

        Console.Write("Введіть пароль: ");
        string password = Console.ReadLine();

        if (login == "admin" && password == "admin")
        {
            Console.WriteLine("Вхід адміністратора успішний!");
            AdminMenu();
        }
        else
        {
            Console.WriteLine("Невірний логін або пароль.");
        }
    }
    static void AdminMenu()
    {
        while (true)
        {
            Console.WriteLine("\nМеню адміністратора:");
            Console.WriteLine("1. Додати вікторину");
            Console.WriteLine("2. Видалити вікторину");
            Console.WriteLine("3. Вийти в головне меню");
            Console.Write("Оберіть опцію: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddQuiz();
                    break;
                case "2":
                    RemoveQuiz();
                    break;
                case "3":
                    Console.WriteLine("Вихід у головне меню...");
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте знову.");
                    break;
            }
        }
    }

    static void RemoveQuiz()
    {
        Console.Write("Введіть назву категорії: ");
        string category = Console.ReadLine();

        if (!quizzes.ContainsKey(category))
        {
            Console.WriteLine("Категорія не знайдена!");
            return;
        }

        Console.Write("Введіть номер питання для видалення (починаючи з 1): ");
        if (int.TryParse(Console.ReadLine(), out int questionIndex) && questionIndex > 0 && questionIndex <= quizzes[category].Count)
        {
            quizzes[category].RemoveAt(questionIndex - 1);
            Console.WriteLine("Питання видалено!");
        }
        else
        {
            Console.WriteLine("Невірний номер питання.");
        }
    }
    static void AddQuiz()
    {
        Console.Write("Введіть назву категорії: ");
        string category = Console.ReadLine();

        if (!quizzes.ContainsKey(category))
        {
            quizzes[category] = new List<Question>();
        }

        Console.Write("Введіть запитання: ");
        string questionText = Console.ReadLine();

        Console.WriteLine("Введіть три варіанти відповіді:");
        string[] options = new string[3];
        for (int i = 0; i < 3; i++)
        {
            Console.Write($"Варіант {i + 1}: ");
            options[i] = Console.ReadLine();
        }

        Console.Write("Введіть номер правильної відповіді (1-3): ");
        int correctOption = int.Parse(Console.ReadLine());

        quizzes[category].Add(new Question(questionText, options, correctOption));
        Console.WriteLine("Питання додано!");
    }

    static void ViewResults(string login)
    {
        if (quizResults[login].Count == 0)
        {
            Console.WriteLine("У вас ще немає результатів вікторин.");
            return;
        }

        Console.WriteLine("Ваші результати вікторин:");
        foreach (int score in quizResults[login])
        {
            Console.WriteLine("- " + score + " правильних відповідей");
        }
    }

    static void ViewTop20()
    {
        Console.WriteLine("Топ-20 результатів вікторини:");
        var allScores = quizResults.SelectMany(q => q.Value)
                                   .OrderByDescending(s => s)
                                   .Take(20)
                                   .ToList();

        if (allScores.Count == 0)
        {
            Console.WriteLine("Поки що немає записаних результатів.");
            return;
        }

        for (int i = 0; i < allScores.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {allScores[i]} правильних відповідей");
        }
    }
    static void StartQuiz(string login)
    {
        Console.WriteLine("Оберіть категорію:");
        Console.WriteLine("1. Історія");
        Console.WriteLine("2. Географія");
        Console.WriteLine("3. Біологія");
        Console.WriteLine("4. Змішана вікторина");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine();
        List<Question> questions = choice switch
        {
            "1" => History.GetQuestions(),
            "2" => Geography.GetQuestions(),
            "3" => Biology.GetQuestions(),
            "4" => GetMixedQuestions(),
            _ => new List<Question>()
        };

        int correctAnswers = 0;
        foreach (var question in questions)
        {
            if (question.Ask())
                correctAnswers++;
        }

        quizResults[login].Add(correctAnswers);
        Console.WriteLine($"Ваша оцінка: {correctAnswers} з {questions.Count}!");
    }

    static List<Question> GetMixedQuestions() => History.GetQuestions().Concat(Geography.GetQuestions()).Concat(Biology.GetQuestions()).OrderBy(x => rnd.Next()).Take(20).ToList();
}

class Question
{
    public string Text;
    public string[] Options;
    public int CorrectOption;

    public Question(string text, string[] options, int correctOption)
    {
        Text = text;
        Options = options;
        CorrectOption = correctOption;
    }

    public bool Ask()
    {
        Console.WriteLine(Text);
        for (int i = 0; i < Options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Options[i]}");
        }
        Console.Write("Ваш вибір: ");
        return int.TryParse(Console.ReadLine(), out int answer) && answer - 1 == CorrectOption;
    }
}

class History
{
    public static List<Question> GetQuestions() => new List<Question>
    {
        new Question("Хто був першим президентом США?", new[] {"Джордж Вашингтон", "Авраам Лінкольн", "Томас Джефферсон"}, 0),
        new Question("Рік початку Другої світової війни?", new[] {"1939", "1941", "1945"}, 0),
        new Question("Коли було прийнято Декларацію незалежності США?", new[] {"1789", "1812", "1776"}, 2),
        new Question("Хто був першим гетьманом України?", new[] {"Іван Мазепа", "Богдан Хмельницький", "Петро Дорошенко"}, 1),
        new Question("Яка держава збудувала Велику китайську стіну?", new[] {"Японія", "Китай", "Монголія"}, 1),
        new Question("В якому році впала Римська імперія?", new[] {"1453", "476", "1066"}, 1),
        new Question("Хто відкрив Америку в 1492 році?", new[] {"Фернан Магеллан", "Христофор Колумб", "Амеріго Веспуччі"}, 1),
        new Question("Який рік став початком Першої світової війни?", new[] {"1939", "1918", "1914"}, 2),
        new Question("Хто був автором \"Маніфесту комуністичної партії\"?", new[] {"Володимир Ленін", "Фрідріх Енгельс", "Карл Маркс"}, 2),
        new Question("Яке місто було столицею Візантійської імперії?", new[] {"Рим", "Афіни", "Константинополь"}, 2),
        new Question("В якому році відбулася битва під Полтавою?", new[] {"1683", "1709", "1812"}, 1),
        new Question("Хто був першим космонавтом у світі?", new[] {"Ніл Армстронг", "Юрій Гагарін", "Олексій Леонов"}, 1),
        new Question("Який рік вважається роком заснування Києва?", new[] {"988", "1240", "482"}, 2),
        new Question("Хто був останнім імператором Росії?", new[] {"Олександр III", "Петро I", "Микола II"}, 2),
        new Question("Коли відбулася Французька революція?", new[] {"1848", "1789", "1917"}, 1),
        new Question("Яка країна першою надала жінкам право голосу?", new[] {"США", "Франція", "Нова Зеландія"}, 2),
        new Question("Хто був фараоном під час будівництва Великої піраміди у Гізі?", new[] {"Рамзес II", "Тутанхамон", "Хеопс"}, 2),
        new Question("Яке місто стало столицею Польщі після Другої світової війни?", new[] {"Краків", "Варшава", "Познань"}, 1),
        new Question("В якому році Україна здобула незалежність?", new[] {"1991", "1989", "1994"}, 0),
        new Question("Яка імперія існувала найдовше у світовій історії?", new[] {"Османська імперія", "Римська імперія", "Єгипетська імперія"}, 1)

    };
}

class Geography
{
    public static List<Question> GetQuestions() => new List<Question>
    {
        new Question("Яка найбільша країна світу за площею?", new[] {"Росія", "Канада", "Китай"}, 0),
        new Question("Яка річка найдовша у світі?", new[] {"Ніл", "Амазонка", "Міссісіпі"}, 0),
        new Question("Яка найбільша країна світу за площею?", new[] {"Канада", "Китай", "Америка"}, 2),
        new Question("Яка річка найдовша у світі?", new[] {"Амазонка", "Ніл", "Міссісіпі"}, 1),
        new Question("Де знаходиться найвища гора світу – Еверест?", new[] {"Тибет", "Непал", "Пакистан"}, 1),
        new Question("Який океан є найбільшим на планеті?", new[] {"Атлантичний", "Тихий", "Індійський"}, 1),
        new Question("Столиця Австралії?", new[] {"Канберра", "Сідней", "Мельбурн"}, 0),
        new Question("На якому материку знаходиться пустеля Сахара?", new[] {"Африка", "Азія", "Австралія"}, 0),
        new Question("Яка країна має найбільшу кількість островів?", new[] {"Індонезія", "Філіппіни", "Швеція"}, 2),
        new Question("Де розташоване озеро Байкал?", new[] {"Росія", "Китай", "Монголія"}, 0),
        new Question("Яка столиця Канади?", new[] {"Оттава", "Торонто", "Ванкувер"}, 0),
        new Question("Найменша країна у світі?", new[] {"Монако", "Мальта", "Ватикан"}, 2),
        new Question("Який континент вважається найбільш населеним?", new[] {"Європа", "Азія", "Африка"}, 1),
        new Question("Який з цих островів найбільший?", new[] {"Мадагаскар", "Гренландія", "Нова Гвінея"}, 1),
        new Question("Столиця Бразилії?", new[] {"Ріо-де-Жанейро", "Бразиліа", "Сан-Паулу"}, 1),
        new Question("Який материк повністю вкритий кригою?", new[] {"Антарктида", "Арктика", "Гренландія"}, 0),
        new Question("Де знаходиться Великий Бар'єрний риф?", new[] {"Австралія", "Філіппіни", "Індонезія"}, 0),
        new Question("Яка країна займає весь материк?", new[] {"Австралія", "Антарктида", "Канада"}, 0),
        new Question("Найвища гора в Європі?", new[] {"Ельбрус", "Монблан", "Маттерхорн"}, 0),
        new Question("Яка пустеля є найбільшою у світі?", new[] {"Гобі", "Антарктична", "Сахара"}, 1)

    };
}

class Biology
{
    public static List<Question> GetQuestions() => new List<Question>
    {
        new Question("Який орган відповідає за кровообіг?", new[] {"Серце", "Легені", "Печінка"}, 0),
        new Question("Скільки хромосом має людина?", new[] {"46", "48", "44"}, 0),
        new Question("Який орган відповідає за перекачування крові в організмі?", new[] {"Легені", "Печінка", "Серце"}, 2),
        new Question("Скільки хромосом має людина?", new[] {"48", "46", "44"}, 1),
        new Question("Яка молекула несе генетичну інформацію?", new[] {"РНК", "ДНК", "Білок"}, 1),
        new Question("Який організм вважається найпростішим?", new[] {"Амеба", "Бактерія", "Гриб"}, 1),
        new Question("Як називається процес утворення глюкози в рослинах?", new[] {"Дихання", "Фотосинтез", "Бродіння"}, 1),
        new Question("Який газ виділяють рослини під час фотосинтезу?", new[] {"Кисень", "Вуглекислий газ", "Азот"}, 0),
        new Question("Який орган відповідає за фільтрацію крові та утворення сечі?", new[] {"Серце", "Легені", "Нирки"}, 2),
        new Question("Скільки кісток у дорослої людини?", new[] {"206", "208", "210"}, 0),
        new Question("Як називається наука про рослини?", new[] {"Зоологія", "Ботаніка", "Екологія"}, 1),
        new Question("Який тип кровообігу є у людей?", new[] {"Закритий", "Відкритий", "Змішаний"}, 0),
        new Question("Яка група тварин відкладає ікру у воду?", new[] {"Птахи", "Земноводні", "Ссавці"}, 1),
        new Question("Який елемент є основою органічних сполук?", new[] {"Кисень", "Вуглець", "Азот"}, 1),
        new Question("Що таке мітохондрії?", new[] {"Органели клітини", "Вид бактерій", "Частина ДНК"}, 0),
        new Question("Яка речовина надає крові червоний колір?", new[] {"Гемоглобін", "Кератин", "Міозин"}, 0),
        new Question("Як називається процес, під час якого організми пристосовуються до навколишнього середовища?", new[] {"Еволюція", "Мутація", "Дифузія"}, 0),
        new Question("Яка частина нервової системи відповідає за свідомі дії?", new[] {"Головний мозок", "Спинний мозок", "Автономна нервова система"}, 0),
        new Question("Що таке хлорофіл?", new[] {"Пігмент у рослинах", "Фермент", "Вид бактерій"}, 0),
        new Question("Який організм є хижаком?", new[] {"Корова", "Вовк", "Кролик"}, 1),
        new Question("Який орган людини відповідає за вироблення інсуліну?", new[] {"Печінка", "Підшлункова залоза", "Нирки"}, 1),
        new Question("Що таке симбіоз?", new[] {"Взаємовигідне співіснування організмів", "Процес поділу клітин", "Розмноження грибів"}, 0)

    };
}
