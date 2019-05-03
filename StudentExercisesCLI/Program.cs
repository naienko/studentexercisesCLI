using StudentExercisesCLI.Models;
using System;
using System.Collections.Generic;

namespace StudentExercisesCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialize the db connector class
            Repository repository = new Repository();

            //Query the database for all the Exercises.
            List<Exercise> exercises = repository.GetExercises();
            Report.PrintExerciseReport("All Exercises", exercises);
            Pause();

            //Find all the exercises in the database where the language is JavaScript.
            List<Exercise> JSexercises = repository.SearchExercisesByLanguage("JavaScript");
            Report.PrintExerciseReport("JavaScript Exercises", JSexercises);
            Pause();

            //Insert a new exercise into the database.
            Exercise bangazonERD = new Exercise
            {
                Title = "Bangazon ERD",
                Language = "SQL"
            };
            repository.AddExercise(bangazonERD);
            Console.WriteLine("Adding new exercise ...");
            exercises = repository.GetExercises();
            Report.PrintExerciseReport("All exercises after adding new one",exercises);
            Pause();

            //Find all instructors in the database. Include each instructor's cohort.
            List<Instructor> instructors = repository.GetInstructorsWithCohort();
            Report.PrintInstructorReport("All Instructors with Cohort", instructors);
            Pause();

            //Insert a new instructor into the database.Assign the instructor to an existing cohort.
            Instructor vaughn = new Instructor
            {
                _firstname = "Vaughn",
                _lastname = "Snow",
                _handle = "starman",
                _specialty = "PHP",
                _cohortId = 3
            };
            repository.AddInstructor(vaughn);
            Console.WriteLine("Adding new instructor ...");
            instructors = repository.GetInstructorsWithCohort();
            Report.PrintInstructorReport("All Instructors after new instructor added",instructors);
            Pause();

            //Assign an existing exercise to an existing student.
            List<Student> students = repository.GetStudents();
            instructors = repository.GetInstructors();
            exercises = repository.GetExercises();

            repository.AssignExercise(students[1], exercises[6], instructors[1]);
            Pause();

            //Find all the students in the database. Include each student's cohort AND each student's list of exercises.
            List<Student> studentsWithExercisesAndCohort = repository.GetStudentExercises();
            Report.PrintStudentExerciseReport("All Students with Cohort and Exercises", studentsWithExercisesAndCohort);
            Pause();
        }
        //steve's stop-to-read-output code
        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
