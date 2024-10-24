using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patient_and_DoctorManagementSystem.Repositories;

namespace Patient_and_DoctorManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=MUDASIR-DELL\SQLEXPRESS01;Database=QuestDb;Integrated Security=true"; 
            var patientRepo = new PatientRepository(connectionString); 
            var doctorRepo = new DoctorRepository(connectionString); 

            //  Adding a new patient
            var newPatient = new Patient { Name = "Mudasir", Age = 23, Gender = "Male", MedicalCondition = "Viral Fever" };
            var response = patientRepo.Create(newPatient); 
            Console.WriteLine(response.Message);

            //Adding a new doctor
            var newDoctor = new Doctor { Name = "Dr. Doom", Specialization = "General Surgen" };
            var doctorResponse = doctorRepo.Create(newDoctor);
            Console.WriteLine(doctorResponse.Message); 

            //Reading all patients
            var patientsResponse = patientRepo.Read(); 
            foreach (var patient in patientsResponse.Data)
            {
                Console.WriteLine($"Patient: {patient.Name}, Age: {patient.Age}, Condition: {patient.MedicalCondition}");
            }

            //Reading all doctors
            var doctorsResponse = doctorRepo.Read();
            foreach (var doctor in doctorsResponse.Data)
            {
                Console.WriteLine($"Doctor: {doctor.Name}, Specialization: {doctor.Specialization}");
            }

            //Updating a patient
            newPatient.MedicalCondition = "Recovered";
            var updateResponse = patientRepo.Update(newPatient);
            Console.WriteLine(updateResponse.Message);

            //Deleting a patient
            var deleteResponse = patientRepo.Delete(newPatient.Id);
            Console.WriteLine(deleteResponse.Message);
        }
    }
}
