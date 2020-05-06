using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using cw_5.Model;
using Microsoft.AspNetCore.Mvc;

namespace cw_5.Controllers
{/*
    [Route("api/students")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        [HttpGet]

        public string GetStudent()
        {
            List<Student> students = new List<Student>();

            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16578;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = "SELECT s.FirstName AS FirstName, s.LastName AS LastName, s.BirthDate AS BirthDate, st.Name AS Name, e.Semester AS Semester" +
                                  " FROM Enrollment AS e" +
                                  " INNER JOIN Student AS s" +
                                  " ON e.IdEnrollment = s.IdEnrollment" +
                                  " INNER JOIN Studies st" +
                                  " ON e.IdStudy = st.IdStudy";

                client.Open();
                var dataRead = com.ExecuteReader();
                //ArrayList<Student> students = new ArrayList<Student>();

                while (dataRead.Read())
                {
                    var student = new Student();
                    student.FirstName = dataRead["FirstName"].ToString();
                    student.LasteName = dataRead["LastName"].ToString();
                    student.BirthDate = dataRead["BirthDate"].ToString();
                    student.StudyName = dataRead["Name"].ToString();
                    student.Semester = Convert.ToInt32(dataRead["Semester"].ToString());

                    students.Add(student);
                }

                StringBuilder sb = new StringBuilder();

                foreach (Student s in students)
                {
                    sb.Append("name:" + s.FirstName +
                              " last name:" + s.LasteName +
                              " birth date: " + s.BirthDate +
                              " study name:" + s.StudyName +
                              " semester:" + s.Semester);
                    sb.AppendLine();
                }
                dataRead.Close();
                return sb.ToString();

            }

        }

        //[HttpGet("{id}")]

        //public IActionResult GetStudents(int id)
        //{
        //    if (id == 1)
        //        return Ok("kowalski");
        //    else if (id == 2)
        //        return Ok("malczewski");
        //    return NotFound("missing student");

        //}
        [HttpPost]

        public IActionResult CreateStudent(Student student)
        {
            // student.IndexNumber = $"s{new Random().Next(1, 1000)}";
            return Ok(student);
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteStudent(int id)
        {
            return Ok($"Student with {id} index has been removed");
        }

        [HttpPut("{id}")]

        public IActionResult InserStudent(int id)
        {
            return Ok($"Student with {id} idenx has been added");
        }

        [HttpGet("{index}")]
        public IActionResult GetSemesterByIndex(string index)

        {
            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16578;Integrated Security=TRUE"))

                try
                {
                    using (var command = new SqlCommand())
                    {
                        command.Connection = client;
                        command.CommandText = "SELECT e.Semester AS Semester " +
                            "FROM Enrollment AS e " +
                            "INNER JOIN Student AS s " +
                            "ON e.IdEnrollment = s.IdEnrollment " +
                            "INNER JOIN Studies st " +
                            "ON e.IdStudy = st.IdStudy " +
                            "WHERE s.IndexNumber = @index;";
                        command.Parameters.Add("@index", System.Data.SqlDbType.VarChar, 20).Value = index;

                        client.Open();
                        var dataReader = command.ExecuteReader();
                        string semester;

                        while (dataReader.Read())
                        {
                            semester = dataReader["Semester"].ToString();
                            return Ok($"Student with index = {index} is on: " + semester + " semester");
                        }
                        return NotFound($"There is no Student with index = {index}");
                    }
                }
                catch (InvalidOperationException)
                {
                    return NotFound($"There is no Student with index = {index}");
                }
        }
    }
    */
}