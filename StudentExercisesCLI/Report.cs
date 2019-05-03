using System;
using StudentExercisesCLI.Models;
using System.Collections.Generic;

namespace StudentExercisesCLI
{
    public class Report
    {
        public static void PrintExerciseReport(string title, List<Exercise> exercises)
        {
            Console.WriteLine($"{title}");
            foreach (Exercise e in exercises)
            {
                Console.WriteLine($"{e.Title} is an exercise in {e.Language}");
            }
        }

        public static void PrintInstructorReport(string title, List<Instructor> instructors)
        {
            Console.WriteLine($"{title}");
            foreach (Instructor i in instructors)
            {
                if (i._cohort == null)
                {
                    Console.WriteLine($"{i._firstname} {i._lastname} is an instructor with slack handle @{i._handle}.");
                } else
                {
                    Console.WriteLine($"{i._firstname} {i._lastname} is an instructor for {i._cohort.Name} with slack handle @{i._handle}.");
                }
            }
        }
    }
}
