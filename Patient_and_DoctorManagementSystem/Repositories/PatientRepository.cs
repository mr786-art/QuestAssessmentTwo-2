using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Patient_and_DoctorManagementSystem.Models;

namespace Patient_and_DoctorManagementSystem.Repositories
{
    public class PatientRepository : IRepository<Patient>
    {
        private readonly string _connectionString; 

        public PatientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public GenericResponse<Patient> Create(Patient patient)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Patients (Name, Age, Gender, MedicalCondition) OUTPUT INSERTED.Id VALUES (@Name, @Age, @Gender, @MedicalCondition)", connection);
                command.Parameters.AddWithValue("@Name", patient.Name);
                command.Parameters.AddWithValue("@Age", patient.Age);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@MedicalCondition", patient.MedicalCondition);

                patient.Id = (int)command.ExecuteNonQuery(); 
            }

            return new GenericResponse<Patient> { Data = patient, Success = true, Message = "Patient added successfully." };
        }

        public GenericResponse<List<Patient>> Read()
        {
            var patients = new List<Patient>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Patients", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Age = (int)reader["Age"],
                            Gender = reader["Gender"].ToString(),
                            MedicalCondition = reader["MedicalCondition"].ToString()
                        });
                    }
                }
            }

            return new GenericResponse<List<Patient>> { Data = patients, Success = true };
        }

        public GenericResponse<Patient> Update(Patient patient)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open(); 
                var command = new SqlCommand("UPDATE Patients SET Name = @Name, Age = @Age, Gender = @Gender, MedicalCondition = @MedicalCondition WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", patient.Id);
                command.Parameters.AddWithValue("@Name", patient.Name);
                command.Parameters.AddWithValue("@Age", patient.Age);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@MedicalCondition", patient.MedicalCondition);

                command.ExecuteNonQuery(); 
            }

            return new GenericResponse<Patient> { Data = patient, Success = true, Message = "Patient updated successfully." };
        }

        public GenericResponse<bool> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open(); 
                var command = new SqlCommand("DELETE FROM Patients WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery(); 
                return new GenericResponse<bool> { Data = rowsAffected > 0, Success = true, Message = "Patient deleted successfully." };
            }
        }
    }
}
