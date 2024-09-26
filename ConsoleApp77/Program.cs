using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp77
{
    public interface IRepository
    {
        void Add(Person person);
        void Update(Person person);
        void Delete(int id);
        List<Person> GetAll();
        Person Get(int id);
    }

    public class Person
    {
        private static int _personCount = 0;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public Person()
        {
            _personCount++;
        }

        public Person(int id, string name, string surname, int age) : this()
        {
            Id = id;
            Name = name;
            Surname = surname;
            Age = age;
        }
    }
    public class PersonRepo : IRepository
    {
        public const string CONNECTION_STRING = "Data Source=.;Initial Catalog=PersoonDb;Integrated Security=True;Encrypt=False";
        List<Person> person = new List<Person>();
        public void Add(Person person)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "insert into Persoon values(@Id,@PersoonName,@PersoonSurname,@Age)";
                    com.Parameters.Add(new SqlParameter("@Id", person.Id));
                    com.Parameters.Add(new SqlParameter("@PersoonName", person.Name));
                    com.Parameters.Add(new SqlParameter("@PersoonSurname", person.Surname));
                    com.Parameters.Add(new SqlParameter("@Age", person.Age));

                    com.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "Delete from Person where id = @Id";

                    com.ExecuteNonQuery();
                }
            }
        }

        public Person Get(int id)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                Person person1 = new Person();

                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "Select * from Persoon where id = @Id";

                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            person1.Id = int.Parse(reader["@Id"].ToString());
                            person1.Name = reader["@Name"].ToString();
                            person1.Surname = reader["@Surname"].ToString();
                            person1.Age = int.Parse(reader["@Age"].ToString());
                        }
                    }
                }
                return person1;
            }
        }

        public List<Person> GetAll()
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                List<Person> list = new List<Person>();
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "Select * from Persoon";

                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Person person = new Person();
                            person.Id = int.Parse(reader["@Id"].ToString());
                            person.Name = reader["@Name"].ToString();
                            person.Surname = reader["@Surname"].ToString();
                            person.Age = int.Parse(reader["@Age"].ToString());
                            list.Add(person);
                        }
                    }
                }
                return list;
            }
        }

        public void Update(Person person)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "Update Persoon set Id = @Id,Name = @PersoonName,Surname = @PersoonSurname,Age = @Age ";

                    com.Parameters.Add(new SqlParameter("@Id", person.Id));
                    com.Parameters.Add(new SqlParameter("@PersoonName", person.Name));
                    com.Parameters.Add(new SqlParameter("@PersoonSurname", person.Surname));
                    com.Parameters.Add(new SqlParameter("@Age", person.Age));

                    com.ExecuteNonQuery();
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("1.AddPerson || 2.DeletePerson || 3.GetPersonById || 4.GetAllPeople || 5.UpdatePerson || 0.Exit");
                Person person = null;
                IRepository repo = new PersonRepo();
                Console.WriteLine("Enter which one you want: ");

                string optionStr = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(optionStr))
                {
                    Console.WriteLine("Incorect option");
                    continue;
                }

                int n;

                if (!int.TryParse(optionStr, out n))
                {
                    Console.WriteLine("Incorect option");
                    continue;
                }

                if (n == 0)
                {
                    Console.WriteLine("You go out");
                    break;
                }

                switch (n)
                {
                    case 1:
                        person = new Person();
                        Console.Write("Input name: ");
                        string name = Console.ReadLine();
                        person.Name = name;
                        Console.WriteLine("Input surname: ");
                        string surname = Console.ReadLine();
                        person.Surname = surname;
                        Console.WriteLine("Input age: ");
                        int age = int.Parse(Console.ReadLine());
                        person.Age = age;
                        repo.Add(person);
                        break;

                    case 2:
                        Console.WriteLine("Enter person id: ");
                        int id = int.Parse(Console.ReadLine());
                        repo.Delete(id);
                        break;

                    case 3:
                        Console.WriteLine("Enter person id: ");
                        int id1 = int.Parse(Console.ReadLine());
                        repo.Get(id1);
                        Console.WriteLine($"Id: {person.Id} | Name: {person.Name} | Surname: {person.Surname} | Age: {person.Age}");
                        break;

                    case 4:
                        repo.GetAll().ForEach(x => Console.WriteLine($"Id: {person.Id} | Name: {person.Name} | Surname: {person.Surname} | Age: {person.Age}"));
                        break;

                    case 5:
                        Console.WriteLine("Enter personId: ");
                        int personId = int.Parse(Console.ReadLine());
                        person.Id = personId;



                        repo.Update(person);
                        break;
                }
            }
        }
    }
}
