using Microsoft.Data.Sqlite;
using Avaliacao3BimLp3.Database;
using Avaliacao3BimLp3.Repositories;
using Avaliacao3BimLp3.Models;

var databaseConfig = new DatabaseConfig();
var databaseSetup = new DatabaseSetup(databaseConfig);


var modelName = args[0];
var modelAction = args[1];

if (modelName == "Student")
{
    var studentRepository = new StudentRepository(databaseConfig);

    switch(modelAction)
    {   

        case "New" :
        {   
            var registration = args[2];
            var name = args[3];
            var city = args[4];
            
            var student = new Student(registration, name, city);

            if(studentRepository.ExistsByRegistration(registration)){
                Console.WriteLine($"Estudante com Registro {student.Registration} já existe.");
            }

            else {
                studentRepository.Save(student);
                Console.WriteLine($"Estudante {student.Name} cadastrado com sucesso!");
            }
            break;
        }

        case "Delete":
        {
            var registration = args[2];

            if(studentRepository.ExistsByRegistration(registration))
            {   
                studentRepository.Delete(registration);
                Console.WriteLine($"Estudante {registration} removido com sucesso.");
            }

            else Console.WriteLine($"Estudante {registration} não encontrado.");

            break;
        }

        case "MarkAsFormed":
        {   
            var registration = args[2];

            if(studentRepository.ExistsByRegistration(registration))
            {   
                studentRepository.MarkAsFormed(registration);
                Console.WriteLine($"Estudante {registration} definido como formado.");
            }

            else Console.WriteLine($"Estudante {registration} não encontrado.");

            break;
        }

        case "List" : 
        {
            if(studentRepository.GetAll().Any())
            {
                Console.WriteLine("Lista de Estudantes");
            
                foreach (var student in studentRepository.GetAll())
                {   
                    var formed = studentRepository.IfFormed(student.Former);

                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, {formed}.");
                }
            }
            else Console.WriteLine("Nenhum estudante cadastrado.");

            break;
        }

        case "ListFormed":
        {
            if(studentRepository.GetAllFormed().Any())
            {
                Console.WriteLine("Lista de Estudantes");
            
                foreach (var student in studentRepository.GetAllFormed())
                {   
                    var formed = studentRepository.IfFormed(student.Former);
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, {formed}.");
                }
            }
            else Console.WriteLine("Nenhum estudante cadastrado.");

            break;
            
        }

        case "ListByCity":
        {   
            var cityName = args[2];
            if(studentRepository.GetAllStudentByCity(cityName).Any())
            {   
                Console.WriteLine("Lista de Estudantes");
    
                foreach (var student in studentRepository.GetAllStudentByCity(cityName))
                {   
                    var formed = studentRepository.IfFormed(student.Former);
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, {formed}.");
                }
            }
            else Console.WriteLine("Nenhum estudante cadastrado.");

            break;
            
        }

        case "ListByCities":
        {   
            var cities = new string[args.Length - 2];

            for(int i = 2; i < args.Length; i++)
            {
                cities[i-2] = args[i];
            }

             if(studentRepository.GetAllByCities(cities).Any())
            {   
                Console.WriteLine("Lista de Estudantes");
    
                foreach (var student in studentRepository.GetAllByCities(cities))
                {   
                    var formed = studentRepository.IfFormed(student.Former);
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, {formed}.");
                }
            }
            else Console.WriteLine("Nenhum estudante cadastrado.");

            break;
        }

        case "Report":
        {
            if(args[2] == "CountByCities")
            {
                Console.WriteLine("Número de Estudantes");
                if(studentRepository.CountByCities().Any())
                {
                    foreach (var student in studentRepository.CountByCities())
                    {
                        Console.WriteLine($"{student.AttributeName}, {student.StudentNumber}");       
                    }
                }
                else Console.WriteLine($"Nenhum estudante cadastrado");
            }

            if(args[2] == "CountByFormed")
            {   
                Console.WriteLine("Número de Estudantes");
                if(studentRepository.CountByFormed().Any())
                {
                    foreach (var student in studentRepository.CountByFormed())
                    {
                        var formed = student.AttributeName == "1" ? "Formados" : "Não formados";
                        Console.WriteLine($"{formed}, {student.StudentNumber}");
               
                    }
                }

                else Console.WriteLine($"Nenhum estudante cadastrado");      
            }

            break;
        }

        default : {
            Console.WriteLine("Comando inválido");
            break;
        }
    }
}
else Console.WriteLine("Comando inválido");