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

        public static void PrintStudentExerciseReport(string title, List<Student> students)
        {
            Console.WriteLine($"{title}");
            foreach (Student s in students)
            {
                Console.WriteLine($"{s._firstname} {s._lastname} is in {s._cohort.Name}. They are working on");
                foreach (Exercise e in s.Exercises)
                {
                    Console.WriteLine($"        {e.Title} in {e.Language}");
                }
            }
        }
    }
}
