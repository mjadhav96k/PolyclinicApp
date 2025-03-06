using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public int CancelAppointment(int appointmentNo)
        {
            int rowsAffected = 0;
            try
            {
                context.Appointments.Remove(context.Appointments.Find(appointmentNo));
                rowsAffected = context.SaveChanges();
            }
            catch (Exception)
            {
                rowsAffected = -1;
                throw;
            }
            return rowsAffected;
        }

        public List<ListOfAppointments> FetchAllAppointments(string doctorId, DateTime date)
        {
            List<ListOfAppointments> appointments = new List<ListOfAppointments>();
            try
            {
                SqlParameter prmDoctorId = new SqlParameter("@DoctorID", doctorId);
                SqlParameter prmDateOfAppointment = new SqlParameter("@DateofAppointment", date);
                appointments = context.ListOfAppointments
                    .FromSqlRaw("SELECT * FROM dbo.ufn_FetchAllAppointments(@DoctorID,@DateofAppointment)", prmDoctorId, prmDateOfAppointment)
                    .ToList();
            }
            catch (Exception)
            {
                appointments = null;
                throw;
            }
            return appointments;
        } 

    }
}