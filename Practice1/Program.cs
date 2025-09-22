using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdaptiveGamePrototype
{
    public enum Difficulty { Easy = 1, Medium = 2, Hard = 3 }

    public class Player
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<string> Achievements { get; } = new List<string>();
    }

    public class Question
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Text { get; set; }
        public string Answer { get; set; }
        public Difficulty Difficulty { get; set; }

        public bool Evaluate(string ans) => string.Equals(ans?.Trim(), Answer?.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public class Level
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }

    public class GameSession
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Player Player { get; }
        public Level Level { get; }
        public int Score { get; private set; } = 0;
        public int CorrectCount { get; private set; } = 0;
        public int Asked { get; private set; } = 0;

        public event Action<GameSession, string> OnStatsUpdated;

        public GameSession(Player p, Level l)
        {
            Player = p;
            Level = l;
        }

        public void SubmitResult(bool correct, int points)
        {
            Asked++;
            if (correct) { CorrectCount++; Score += points; }
            OnStatsUpdated(this, $"Score={Score}, Correct={CorrectCount}/{Asked}");
        }
    }

    public class AdaptationEngine
    {
        public Difficulty SuggestDifficulty(GameSession session, Difficulty current)
        {
            if (session.Asked == 0) return current;
            double acc = session.CorrectCount / (double)session.Asked;
            if (acc >= 0.8 && current != Difficulty.Hard) return current + 1;
            if (acc < 0.5 && current != Difficulty.Easy) return current - 1;
            return current;
        }
    }

    public class AchievementManager
    {
        public List<(string id, Func<GameSession,bool> rule)> Rules = new List<(string, Func<GameSession,bool>)>();

        public AchievementManager()
        {
            Rules.Add(("FirstCorrect", s => s.CorrectCount >= 1));
            Rules.Add(("FiveCorrect", s => s.CorrectCount >= 5));
            Rules.Add(("PerfectRound", s => s.Asked > 0 && s.CorrectCount == s.Asked));
        }

        public IEnumerable<string> Check(GameSession session)
        {
            foreach (var r in Rules)
            {
                if (r.rule(session))
                    yield return r.id;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Підготовка demo data
            var player = new Player { Name = "Ivan" };
            var level = CreateDemoLevel();
            var session = new GameSession(player, level);
            var adapt = new AdaptationEngine();
            var achMgr = new AchievementManager();

            // Підписка на "реальне-часове" оновлення статистики
            session.OnStatsUpdated += (s, payload) =>
            {
                Console.WriteLine($"[RealtimeStats] {payload}");
            };

            Console.WriteLine($"Player {player.Name} starts level '{level.Title}'");
            Difficulty current = Difficulty.Medium; // стартова складність

            // Імітуємо ітеративне завдання: беремо 7 питань (по черзі — адаптуємо)
            for (int i = 0; i < 7; i++)
            {
                // Отримати питання з потрібною складністю (простий пошук)
                var q = level.Questions.FirstOrDefault(x => x.Difficulty == current);
                if (q == null)
                {
                    // fallback: будь-яке питання
                    q = level.Questions[new Random().Next(level.Questions.Count)];
                }

                // Present question
                Console.WriteLine($"\nQuestion #{i+1} (Difficulty: {current}): {q.Text}");
                Console.Write("Your answer: ");
                string answer = Console.ReadLine();

                bool correct = q.Evaluate(answer);
                int points = correct ? (int)current * 10 : 0; // просте правило
                session.SubmitResult(correct, points);

                Console.WriteLine(correct ? $"Correct! +{points} pts" : $"Wrong. Correct answer: {q.Answer}");

                // Перевіряємо досягнення
                var newAch = achMgr.Check(session).Except(player.Achievements).ToList();
                foreach (var a in newAch)
                {
                    player.Achievements.Add(a);
                    Console.WriteLine($"[Achievement Unlocked] {a}");
                }

                // Коригуємо складність
                current = adapt.SuggestDifficulty(session, current);
                Console.WriteLine($"(AdaptiveEngine) Next difficulty: {current}");
            }

            Console.WriteLine("\n--- Level finished ---");
            Console.WriteLine($"Final score: {session.Score}");
            Console.WriteLine("Achievements: " + (player.Achievements.Any() ? string.Join(", ", player.Achievements) : "none"));
        }

        static Level CreateDemoLevel()
        {
            return new Level
            {
                Title = "Math Basics",
                Questions = new List<Question>
                {
                    new Question{ Text="2+2?", Answer="4", Difficulty=Difficulty.Easy },
                    new Question{ Text="5-3?", Answer="2", Difficulty=Difficulty.Easy },
                    new Question{ Text="6+4?", Answer="10", Difficulty=Difficulty.Easy },
                    new Question{ Text="12/3?", Answer="4", Difficulty=Difficulty.Medium },
                    new Question{ Text="7*6?", Answer="42", Difficulty=Difficulty.Medium },
                    new Question{ Text="sqrt(81)?", Answer="9", Difficulty=Difficulty.Medium },
                    new Question{ Text="Derivative of x^2 ?", Answer="2x", Difficulty=Difficulty.Hard },
                    new Question{ Text="Integral of 2x dx ?", Answer="x^2 + C", Difficulty=Difficulty.Hard }
                }
            };
        }
    }
}