namespace Avaliacao3BimLp3.Models;

class Student
{
    public string Registration { get; set; }
    public string Name {get; set;}
    public string City { get; set; }
    public bool Former { get; set; }

    public Student(){ }

    public Student(string registration, string name, string city)
    {
        Registration = registration;
        Name = name;
        City = city;
        Former = false;
    }
}