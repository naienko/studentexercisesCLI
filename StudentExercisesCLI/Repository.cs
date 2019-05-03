using StudentExercisesCLI.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

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
    }
}
