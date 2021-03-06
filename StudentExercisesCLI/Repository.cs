﻿using StudentExercisesCLI.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace StudentExercisesCLI
{
    class Repository
    {
        //the connection to the database
        public SqlConnection Connection
        {
            get
            {
                //make sure the correct db is listed here!
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=StudentExercises;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }


        /* 
         * EXERCISES
         */
        //returns a List of all exercises in the db
        public List<Exercise> GetExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Title, Language FROM Exercise";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int ExerciseId = reader.GetInt32(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        string language = reader.GetString(reader.GetOrdinal("Language"));

                        Exercise exercise = new Exercise
                        {
                            Id = ExerciseId,
                            Title = title,
                            Language = language
                        };

                        exercises.Add(exercise);
                    }
                    reader.Close();
                    return exercises;
                }
            }
        }
        public List<Exercise> SearchExercisesByLanguage(string searchLanguage)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT Id, Title, Language from Exercise WHERE Language = '{searchLanguage}'";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int ExerciseId = reader.GetInt32(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        string language = reader.GetString(reader.GetOrdinal("Language"));

                        Exercise exercise = new Exercise
                        {
                            Id = ExerciseId,
                            Title = title,
                            Language = language
                        };

                        exercises.Add(exercise);
                    }
                    reader.Close();
                    return exercises;
                }
            }
        }
        //add a new exercise to the db
        public void AddExercise(Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO Exercise (Title, Language) VALUES (@Title, @Language)";
                    cmd.Parameters.Add(new SqlParameter("@Title", exercise.Title));
                    cmd.Parameters.Add(new SqlParameter("@Language", exercise.Language));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /* 
         * INSTRUCTORS
         */
        //returns a List of all instructors in the db
        public List<Instructor> GetInstructors()
        {
            //get from database
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, SlackName, Specialty, CohortId FROM Instructor";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> instructors = new List<Instructor>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int IdValue = reader.GetInt32(idColumnPosition);
                        string FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string SlackName = reader.GetString(reader.GetOrdinal("SlackName"));
                        string Specialty = reader.GetString(reader.GetOrdinal("Specialty"));
                        int CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));

                        Instructor instructor = new Instructor
                        {
                            Id = IdValue,
                            _firstname = FirstName,
                            _lastname = LastName,
                            _handle = SlackName,
                            _specialty = Specialty,
                            _cohortId = CohortId
                        };

                        instructors.Add(instructor);
                    }

                    reader.Close();
                    return instructors;
                }
            }
        }
        public List<Instructor> GetInstructorsWithCohort()
        {
            //get from database
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id, i.FirstName, i.LastName, i.SlackName, i.Specialty, i.cohortId, c.Designation 
                        FROM Instructor i JOIN Cohort c ON i.CohortId = c.Id;";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> instructors = new List<Instructor>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int IdValue = reader.GetInt32(idColumnPosition);
                        string FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string SlackName = reader.GetString(reader.GetOrdinal("SlackName"));
                        string Specialty = reader.GetString(reader.GetOrdinal("Specialty"));
                        int CohortId = reader.GetInt32(reader.GetOrdinal("cohortId"));
                        string Designation = reader.GetString(reader.GetOrdinal("Designation"));

                        Instructor instructor = new Instructor
                        {
                            Id = IdValue,
                            _firstname = FirstName,
                            _lastname = LastName,
                            _handle = SlackName,
                            _specialty = Specialty,
                            _cohortId = CohortId,
                            _cohort = new Cohort
                            {
                                Id = CohortId,
                                Name = Designation
                            }
                        };

                        instructors.Add(instructor);
                    }

                    reader.Close();
                    return instructors;
                }
            }
        }
        //add a new instructor to the db
        public void AddInstructor(Instructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"INSERT INTO Instructor (FirstName, LastName, SlackName, Specialty, CohortId) 
                        VALUES (@firstname, @lastname, @slackname, @specialty, @cohortid)";
                    cmd.Parameters.Add(new SqlParameter("@firstname", instructor._firstname));
                    cmd.Parameters.Add(new SqlParameter("@lastname", instructor._lastname));
                    cmd.Parameters.Add(new SqlParameter("@slackname", instructor._handle));
                    cmd.Parameters.Add(new SqlParameter("@specialty", instructor._specialty));
                    cmd.Parameters.Add(new SqlParameter("@cohortid", instructor._cohortId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /*
         * STUDENTS
         */
        //returns a List of all students in the db
        public List<Student> GetStudents()
        {
            //get from database
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, SlackName, CohortId FROM Student";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int IdValue = reader.GetInt32(idColumnPosition);
                        string FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string SlackName = reader.GetString(reader.GetOrdinal("SlackName"));
                        int CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));

                        Student student = new Student
                        {
                            Id = IdValue,
                            _firstname = FirstName,
                            _lastname = LastName,
                            _handle = SlackName,
                            _cohortId = CohortId
                        };

                        students.Add(student);
                    }

                    reader.Close();
                    return students;
                }
            }
        }

        /*
         * Students AND exercises
         */
        //assign an existing exercise to an existing student
        public void AssignExercise(Student student, Exercise exercise, Instructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"INSERT INTO StudentExercise (StudentId, ExerciseId, InstructorId) 
                        VALUES (@studentid, @exerciseid, @instructorid)";
                    cmd.Parameters.Add(new SqlParameter("@studentid", student.Id));
                    cmd.Parameters.Add(new SqlParameter("@exerciseid", exercise.Id));
                    cmd.Parameters.Add(new SqlParameter("@instructorid", instructor.Id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Student> GetStudentExercises()
        {
            //get from database
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT se.Id, s.Id AS StudentId, s.FirstName, s.LastName, 
                        e.Id AS ExerciseId, e.Title, e.Language, c.Id AS CohortId, c.Designation 
                            FROM StudentExercise se 
                            JOIN Student s ON se.StudentId = s.Id
                            JOIN Instructor i ON se.InstructorId = i.Id
                            JOIN Exercise e ON se.ExerciseId = e.Id
                            JOIN Cohort c ON s.CohortId = c.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int IdValue = reader.GetInt32(idColumnPosition);
                        int StudentId = reader.GetInt32(reader.GetOrdinal("StudentId"));
                        string FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        int ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"));
                        string ExerciseTitle = reader.GetString(reader.GetOrdinal("Title"));
                        string ExerciseLanguage = reader.GetString(reader.GetOrdinal("Language"));
                        int CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));
                        string CohortName = reader.GetString(reader.GetOrdinal("Designation"));

                        Student student = new Student
                        {
                            Id = StudentId,
                            _firstname = FirstName,
                            _lastname = LastName,
                            _cohort = new Cohort
                            {
                                Id = CohortId,
                                Name = CohortName
                            },
                            Exercises = new List<Exercise>()
                        };
                        Exercise exercise = new Exercise
                        {
                            Id = ExerciseId,
                            Language = ExerciseLanguage,
                            Title = ExerciseTitle
                        };

                        if (students.Count != 0)
                        {
                            IEnumerable<Student> currentStudent = students.Where(s => s.Id == StudentId);
                            if (currentStudent.Any() == true)
                            {
                                foreach (Student s in currentStudent)
                                {
                                    s.Exercises.Add(exercise);
                                }
                            }
                            else
                            {
                                student.Exercises.Add(exercise);
                                students.Add(student);
                            }
                        }
                        else
                        {
                            student.Exercises.Add(exercise);
                            students.Add(student);
                        }
                    }

                    reader.Close();

                    return students;
                }
            }
        }

        public void AssignCohortExercise(Cohort cohort, Exercise exercise)
        {
            List<Student> studentsWithExercisesAndCohort = GetStudentExercises();

            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    List<Student> cohortStudents = studentsWithExercisesAndCohort.Where(s => s._cohort.Name == cohort.Name).ToList();

                    //get the db ID of the Exercise
                    cmd.CommandText = $"SELECT Id AS ExerciseId FROM Exercise WHERE {exercise.Title} = Title";
                    SqlDataReader reader = cmd.ExecuteReader();

                    int ExerciseId = 0;

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int IdValue = reader.GetInt32(idColumnPosition);
                        ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"));
                    }

                    reader.Close();

                    //get the db ID of the Cohort
                    cmd.CommandText = $"SELECT c.Id AS CohortId FROM Cohort WHERE {cohort.Name} = Designation";
                    SqlDataReader reader2 = cmd.ExecuteReader();

                    int CohortId = 0;

                    while (reader2.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int IdValue = reader.GetInt32(idColumnPosition);
                        CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));
                    }

                    //get the db ID of the Instructor of the Cohort
                    cmd.CommandText = $"SELECT i.Id AS InstructorId FROM Instructor i WHERE {cohort.Id} = {CohortId}";
                    SqlDataReader reader3 = cmd.ExecuteReader();

                    int InstructorId = 0;

                    while (reader3.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int IdValue = reader.GetInt32(idColumnPosition);
                        InstructorId = reader.GetInt32(reader.GetOrdinal("InstructorId"));
                    }
                    //loop through the list of Students in the Cohort
                    foreach (Student s in cohortStudents)
                    {
                        //pull out the Exercise that matches the variable exercise in the Student's List
                        IEnumerable<Exercise> currentExercise = s.Exercises.Where(e => e.Id == ExerciseId);
                        //check to see if that actually grabbed anything
                        if (currentExercise.Any() == false)
                        {
                            //if not, INSERT exercise
                            cmd.CommandText = $@"INSERT INTO StudentExercise (StudentId, ExerciseId, InstructorId) 
                                        VALUES (@studentid, @exerciseid, @instructorid)";
                            cmd.Parameters.Add(new SqlParameter("@studentid", s.Id));
                            cmd.Parameters.Add(new SqlParameter("@exerciseid", ExerciseId));
                            cmd.Parameters.Add(new SqlParameter("@instructorid", InstructorId));

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

        }
    }
}
