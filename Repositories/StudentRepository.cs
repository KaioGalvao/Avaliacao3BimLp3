using Dapper;
using Avaliacao3BimLp3.Database;
using Avaliacao3BimLp3.Models;
using Microsoft.Data.Sqlite;
namespace Avaliacao3BimLp3.Repositories;

class StudentRepository
{
    private readonly DatabaseConfig _databaseConfig;

    public StudentRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

     public Student Save(Student student)
    {   
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        
        connection.Execute("INSERT INTO Students VALUES(@Registration, @Name, @City, @Former)", student);

        return student;
    }

    public void Delete(string id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Execute("DELETE FROM Students WHERE registration = @Registration;", new {Registration = id});

    }

    public void MarkAsFormed(string id)
    {   
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        connection.Execute(@"
            UPDATE Students 
            SET former = @Former
            WHERE registration = @Registration;
        ", new {Registration = id, Former = true});
    }

    public List<Student> GetAll()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        var students = connection.Query<Student>("SELECT * FROM Students").ToList();

        return students;
    }

    public List<Student> GetAllFormed()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        var students = connection.Query<Student>("SELECT * FROM Students WHERE former = @Former", new {Former = true}).ToList();

        return students;
    }

    public List<Student> GetAllStudentByCity(string city)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        
        var students = connection.Query<Student>("SELECT * FROM Students WHERE city LIKE @City", new{City = $"{city}%"}).ToList();

        return students;

    }

      public List<Student> GetAllByCities(string[] cities)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        
        
        var students = connection.Query<Student>("SELECT * FROM Students WHERE city IN @City", new{City = cities}).ToList();

        return students;

    }

     public List<CountStudentGroupByAttribute> CountByFormed()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        
        var students = connection.Query<CountStudentGroupByAttribute>("SELECT former as AttributeName, COUNT(former) as StudentNumber FROM Students GROUP BY former").ToList();

        return students;
    }

    public List<CountStudentGroupByAttribute> CountByCities()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        
        var students = connection.Query<CountStudentGroupByAttribute>("SELECT city as AttributeName, COUNT(city) as StudentNumber FROM Students GROUP BY city").ToList();

        return students;
    }

    public bool ExistsByRegistration(string id)
    {   
        using (var connection = new SqliteConnection(_databaseConfig.ConnectionString))
        {
            var result = connection.ExecuteScalar<Boolean>("SELECT count(registration) FROM Students WHERE registration = @Registration;", new{Registration = id});
            return result;
        }
    }

    public string IfFormed(bool former)
    {   
        var formed = Convert.ToString(former);

        if (formed == "True")
        {
            formed = "formado";
        }
        else formed = "não formado";

        return formed;
    }
}