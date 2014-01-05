using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonCrud.Entites;

namespace PersonCrud.Data
{
    public class PersonCrudDb
    {
        private readonly string _connectionString;

        public PersonCrudDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        private T GetFromDb<T>(string commandText, Func<SqlDataReader, T> func)
        {
            return GetFromDb(commandText, null, func);
        }

        private T GetFromDb<T>(string commandText, Dictionary<string, object> parameters,
            Func<SqlDataReader, T> func)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }
                connection.Open();
                var reader = command.ExecuteReader();
                return func(reader);
            }
        }

        private Person GetFromReader(SqlDataReader reader)
        {
           
            var person = new Person();
            person.Id = (int)reader["Id"];
            person.FirstName = (string)reader["FirstName"];
            person.LastName = (string)reader["LastName"];
            person.Age = (int)reader["Age"];
            return person;
        }

        public IEnumerable<Person> Get()
        {
            return GetFromDb("SELECT * FROM Person", r =>
                {
                    var list = new List<Person>();
                    while (r.Read())
                    {
                        list.Add(GetFromReader(r));
                    }

                    return list;
                });
            #region old
            //var list = new List<Person>();

            //using (var connection = new SqlConnection(_connectionString))
            //using (var command = connection.CreateCommand())
            //{
            //    command.CommandText = "SELECT * FROM Person";
            //    connection.Open();
            //    using (var reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            var person = new Person();
            //            person.Id = (int)reader["Id"];
            //            person.FirstName = (string)reader["FirstName"];
            //            person.LastName = (string)reader["LastName"];
            //            person.Age = (int)reader["Age"];
            //            list.Add(person);
            //        }
            //    }
            //}

            //return list;

            #endregion
        }

        public Person GetById(int id)
        {
            return GetFromDb("SELECT * FROM Person WHERE Id = @id",
                             new Dictionary<string, object>
                                 {
                                     {"@id", id}
                                 },
                             reader =>
                                 {
                                     reader.Read();
                                     return GetFromReader(reader);
                                 });
        }

        public void Add(Person person)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Person (FirstName, LastName, Age) VALUES "
                                      + "(@firstName, @lastName, @age); SELECT @@Identity";
                command.Parameters.AddWithValue("@firstName", person.FirstName);
                command.Parameters.AddWithValue("@lastName", person.LastName);
                command.Parameters.AddWithValue("@age", person.Age);
                connection.Open();
                person.Id = (int)(decimal)command.ExecuteScalar();
            }
        }

        public void Edit(Person person)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Person SET FirstName = @firstName, LastName = @lastName, " +
                    "Age = @age WHERE Id = @id";
                command.Parameters.AddWithValue("@firstName", person.FirstName);
                command.Parameters.AddWithValue("@lastName", person.LastName);
                command.Parameters.AddWithValue("@age", person.Age);
                command.Parameters.AddWithValue("@id", person.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Person WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
