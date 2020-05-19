using cw_5.DTOs.Requests;
using System;
using System.Data.SqlClient;
using System.Transactions;
using cw_5.DTOs.Responses;

namespace cw_5.Services
{
    public class SqlServerStudentService : IStudentService
    {
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequests newStudent)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16578;Integrated Security=True"))
            using (var command = connection.CreateCommand())
            using( var transaction = new TransactionScope())
            {
                connection.Open();

                command.CommandText = "SELECT * FROM Student WHERE IndexNumber = @index";
                command.Parameters.AddWithValue("@index", newStudent.Index);
                using (var dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        throw new InvalidOperationException("Student with this index already exists");
                    }
                }

                int idStudy;
                int idEnrollment;
                command.CommandText = "SELECT IdStudy FROM Studies WHERE name = @name";
                command.Parameters.AddWithValue("@name", newStudent.Studies);
                using (var dataReader = command.ExecuteReader())
                {
                    if (!dataReader.Read())
                    {
                        throw new InvalidOperationException("Studies does not exist");
                    }
                    idStudy = (int)dataReader["IdStudy"];
                }


                command.CommandText = "IF NOT EXISTS(SELECT * FROM Enrollment WHERE Semester = 1 AND IdStudy = @IdStudy) BEGIN " +
                                        "DECLARE @MaxId INT = (SELECT MAX(IdEnrollment)+1 FROM Enrollment)+1 " +
                                        "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) " +
                                        "VALUES ( @MaxId, 1, 1, CURRENT_TIMESTAMP) END " +
                                        "SELECT IdEnrollment FROM Enrollment WHERE Semester = 1 AND IdStudy = @IdStudy";
                command.Parameters.AddWithValue("@IdStudy", idStudy);

                using (var dataReader = command.ExecuteReader())
                {
                    dataReader.Read();
                    idEnrollment = (int)dataReader.GetInt32(0);
                }
                
                command.CommandText = "INSERT INTO Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment)" +
                                        "VALUES (@index, @firstname, @lastname, @birthDate, @IdEnrollment)";
                command.Parameters.AddWithValue("@firstname", newStudent.FirstName);
                command.Parameters.AddWithValue("@lastname", newStudent.LastName);
                command.Parameters.AddWithValue("@birthDate", newStudent.BirthDate);
                command.Parameters.AddWithValue("@IdEnrollment", idEnrollment);

                command.ExecuteNonQuery();

                var insertedData = new EnrollStudentResponse();
                command.CommandText =
                    "SELECT IdEnrollment, Semester, IdStudy, StartDate FROM Enrollment WHERE IdEnrollment = @IdEnrollment";
                using (var dataReader = command.ExecuteReader())
                {
                    if(dataReader.Read())
                    {
                        insertedData.IdEnrollment = dataReader.GetInt32(0);
                        insertedData.IdStudy = dataReader.GetInt32(1);
                        insertedData.Semester = dataReader.GetInt32(2);
                        insertedData.StartDate = dataReader.GetDateTime(3);
                    }
                    else
                    {
                        // nie wczytuje
                    }
                }
                transaction.Complete();
                return insertedData;
            }
        }
        public PromotionStudentRepsonse EnrollStudentPromotions(EnrollStudentPromotions promotion)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16578;Integrated Security=True"))
            using (var command = connection.CreateCommand())
            using (var transaction = new TransactionScope())
            {
                var insertedData = new PromotionStudentRepsonse();
                try
                {
                    connection.Open();
                command.CommandText = "EXEC PromoteStudents @Studies = @studies, @Semester = @semester";
                command.Parameters.AddWithValue("@studies", promotion.Studies);
                command.Parameters.AddWithValue("@semester", promotion.Semester);

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            insertedData.IdEnrollment = dataReader.GetInt32(0);
                            insertedData.Semester = dataReader.GetInt32(2);
                            insertedData.IdStudy = dataReader.GetInt32(1);
                            insertedData.StartDate = dataReader.GetDateTime(3);
                        }
                    }
                }
                catch (SqlException sqlerror)
                {
                    throw new InvalidOperationException(sqlerror.Message);
                }
                return insertedData;
            }
        }
        public IndexStudentResponse GetStudent(string index)
        {
            var indexResponse = new IndexStudentResponse();
            using (var connection = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16578;Integrated Security=True"))
            using (var command = connection.CreateCommand())
            using (var transaction = new TransactionScope())
            {
                connection.Open();
                command.CommandText = "SELECT IndexNumber FROM Student WHERE IndexNumber = @index";
                command.Parameters.AddWithValue("@index", index);

                var dataReader = command.ExecuteReader();

                if (!dataReader.Read())
                {
                    throw new UnauthorizedAccessException("No access for this user");
                }

                indexResponse.Index = (string)dataReader.GetSqlString(0);
                connection.Close();
            }
            return indexResponse;
        }

        public StudentResponse Login(LoginRequest login)
        {
            var student = new StudentResponse();

            using(var connection = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16578;Integrated Security=True"))
            using(var command = connection.CreateCommand())
            using(var transaction = new TransactionScope())
            {
                connection.Open();
                command.CommandText = "SELECT FirstName FROM Students WHERE IndexNumber = @login AND Password = @password";
                command.Parameters.AddWithValue("@login", login.Index);
                command.Parameters.AddWithValue("@password", login.Password);

                var reader = command.ExecuteReader();

                if(!reader.Read())
                {
                    throw new UnauthorizedAccessException("Wrong password or user name");
                }

                student.FirstName = (string)reader.GetSqlString(0);
                student.Index = login.Index;

                return student;
            }





        }
    }
}
