using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patient_and_DoctorManagementSystem.Models;
using System.Data.SqlClient;

namespace Patient_and_DoctorManagementSystem.Repositories
{
    public class DoctorRepository : IRepository<Doctor>
    {
        private readonly string _connectionString; // Connection string to the database

        public DoctorRepository(string connectionString)
        {
            _connectionString = connectionString; // Initialize the connection string
        }

        public GenericResponse<Doctor> Create(Doctor doctor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open(); // Open the database connection
                var command = new SqlCommand("INSERT INTO Doctors (Name, Specialization, PatientId) OUTPUT INSERTED.Id VALUES (@Name, @Specialization, @PatientId)", connection);
                command.Parameters.AddWithValue("@Name", doctor.Name);
                command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                command.Parameters.AddWithValue("@PatientId", (object)doctor.PatientId ?? DBNull.Value); // Handle nullable PatientId

                doctor.Id = (int)command.ExecuteScalar(); // Execute the command and get the new doctor's ID
            }

            return new GenericResponse<Doctor> { Data = doctor, Success = true, Message = "Doctor added successfully." };
        }

        public GenericResponse<List<Doctor>> Read()
        {
            var doctors = new List<Doctor>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open(); // Open the database connection
                var command = new SqlCommand("SELECT * FROM Doctors", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.Add(new Doctor
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Specialization = reader["Specialization"].ToString(),
                            PatientId = reader["PatientId"] as int? // Handle nullable PatientId
                        });
                    }
                }
            }

            return new GenericResponse<List<Doctor>> { Data = doctors, Success = true };
        }

        public GenericResponse<Doctor> Update(Doctor doctor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open(); // Open the database connection
                var command = new SqlCommand("UPDATE Doctors SET Name = @Name, Specialization = @Specialization WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", doctor.Id);
                command.Parameters.AddWithValue("@Name", doctor.Name);
                command.Parameters.AddWithValue("@Specialization", doctor.Specialization);

                command.ExecuteNonQuery(); // Execute the update command
            }

            return new GenericResponse<Doctor> { Data = doctor, Success = true, Message = "Doctor updated successfully." };
        }

        public GenericResponse<bool> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open(); // Open the database connection
                var command = new SqlCommand("DELETE FROM Doctors WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery(); // Execute the delete command
                return new GenericResponse<bool> { Data = rowsAffected > 0, Success = true, Message = "Doctor deleted successfully." };
            }
        }
    }
}
