using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PolyclinicApp.ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public decimal CalculateDoctorFees(string doctorId, DateTime date)
        {
            decimal fee = -1;
            try
            {
                //Query Syntax
                //fee = (from f 
                //       in context.Doctors 
                //       select PolyclinicDbContext.ufn_CalculateDoctorFees(doctorId, date))
                //       .FirstOrDefault();
                //Method Syntax
                fee = context.Appointments
                    .Select(d => PolyclinicDbContext.ufn_CalculateDoctorFees(doctorId, date))
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return fee;
        }

        public int GetDoctorAppointment(string patienId, string doctorId, DateTime dateOfAppointment, out int appointmentNo)
        {
            int returnResult = 0;
            appointmentNo = 0;
            int noOfRowsAffected = 0;

            SqlParameter prmPatientId = new SqlParameter("@PatientID", patienId);
            SqlParameter prmDoctorId = new SqlParameter("@DoctorID", doctorId);
            SqlParameter prmDateOfAppointment = new SqlParameter("@DateOfAppointment", dateOfAppointment);
            
            SqlParameter prmappointmentNo = new SqlParameter("@AppointmentNo", System.Data.SqlDbType.Int);
            prmappointmentNo.Direction = System.Data.ParameterDirection.Output;

            SqlParameter prmResult = new SqlParameter("@ReturnResult", System.Data.SqlDbType.Int);
            prmResult.Direction = System.Data.ParameterDirection.Output;

            try
            {
                noOfRowsAffected = context.Database.ExecuteSqlRaw("EXEC @ReturnResult = dbo.usp_GetDoctorAppointment @PatientID, @DoctorID, @DateOfAppointment, @AppointmentNo OUTPUT", prmResult, prmPatientId, prmDoctorId, prmDateOfAppointment, prmappointmentNo);
                returnResult = Convert.ToInt32(prmResult.Value);
                appointmentNo = Convert.ToInt32(prmappointmentNo.Value);
            }
            catch (Exception)
            {
                appointmentNo = 0;
                noOfRowsAffected = -1;
                returnResult = -99;
                throw;
            }
            return returnResult;
        }



    }
}