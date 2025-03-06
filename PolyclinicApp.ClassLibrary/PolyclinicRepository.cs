using PolyclinicApp.ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyclinicApp.ClassLibrary
{
    public class PolyclinicRepository
    {
        PolyclinicDbContext context;

        public PolyclinicRepository(PolyclinicDbContext context)
        {
            this.context = context;
        }

        public Patient GetPatientDetails(string patientId)
        {
            var patient = new Patient();
            patient = context.Patients.Where(c => c.PatientId.Equals(patientId)).FirstOrDefault();
            return patient;
        }

        public bool AddNewPatientDetails(Patient patientObj)
        {
            bool status = false;
            try
            {
                //context.Patients.Add(patientObj);
                context.Add<Patient>(patientObj);
                int rowsAffected = context.SaveChanges();
                Console.WriteLine("Rows Affected: " + rowsAffected);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                throw;
            }
            return status;
        }

        public bool UpdatePatientAge(string patientId, int age)
        {
            bool status = false;

            var patient = context.Patients.Where(c => c.PatientId.Equals(patientId)).FirstOrDefault();
            if(patient != null)
            {
                patient.Age = (Byte)(age);
                int rowsAffected = context.SaveChanges();
                Console.WriteLine($"Number of rows affected: {rowsAffected}");
                status = true;
            }
            return status;
        }

    }
}
