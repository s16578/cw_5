using cw_5.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw_5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        public IActionResult EnrollStudentPromotions(EnrollStudentPromotions promotion)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16578;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                command.CommandText = "EXEC PromoteStudents @Studies = @studies, @Semester = @semester";
                command.Parameters.AddWithValue("studies", promotion.Studies);
                command.Parameters.AddWithValue("semester", promotion.Semester);

                var dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    var errorMessage = dataReader["ErrorMessage"];
                    dataReader.Close();
                    transaction.Rollback();
                    return BadRequest(errorMessage);
                }
                dataReader.Close();
            }
            return new OkObjectResult(new { message = "201 OK, Students have been promoted", currentDate = DateTime.Now });
        }

        public IActionResult EnrollStudents(EnrollStudentRequests newStudent)
        {
            int idEnrollment;

            using (var connection = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16578;Integrated Security=True"))
            using (var command = new SqlCommand())
            {

                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "SELECT * FROM Student WHERE IndexNumber = @index";
                    command.Parameters.AddWithValue("index", newStudent.Index);
                    //command.Transaction = transaction;

                    var dataReader = command.ExecuteReader();


                    if (dataReader.Read())
                    {
                        dataReader.Close();
                        transaction.Rollback();
                        return BadRequest("Student with " + newStudent.Index + " index exists");
                    }
                    dataReader.Close();

                    command.CommandText = "SELECT IdStudy FROM Studies WHERE name = @name";
                    command.Parameters.AddWithValue("name", newStudent.Studies);
                    dataReader = command.ExecuteReader();

                    if (!dataReader.Read())
                    {
                        dataReader.Close();
                        transaction.Rollback();
                        return BadRequest("Studies does not exist");
                    }
                    int idStudy = (int)dataReader["IdStudy"];
                    dataReader.Close();

                    command.CommandText = "SELECT (MAX(idEnrollment)+1) AS IdEnrollment FROM Enrollment";
                    dataReader = command.ExecuteReader();

                    dataReader.Read();
                    idEnrollment = (int)dataReader["IdEnrollment"];

                    dataReader.Close();

                    command.CommandText = "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate)" +
                                          "VALUES (@IdEnrollment, 1, @IdStudies, CURRENT_TIMESTAMP)";
                    command.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                    command.Parameters.AddWithValue("IdStudies", idStudy);

                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment)" +
                                          "VALUES (@index, @firstname, @lastname, @birthDate, @IdEnrollment)";
                    command.Parameters.AddWithValue("firstname", newStudent.FirstName);
                    command.Parameters.AddWithValue("lastname", newStudent.LastName);
                    command.Parameters.AddWithValue("birthDate", newStudent.BirthDate);

                    command.ExecuteNonQuery();

                    transaction.Commit();

                }
                catch (SqlException exc)
                {

                    transaction.Rollback();
                    return BadRequest("Something bad happened with database" + exc);
                }

                return new OkObjectResult(new { message = "201 OK, student added with IdEnrollment: " + idEnrollment, currentDate = DateTime.Now });
            }
        }
    }
}
