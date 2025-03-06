using System.Reflection;
using PolyclinicApp.ClassLibrary;
using PolyclinicApp.ClassLibrary.Models;

namespace PolyclinicApp.ConsoleApp
{
    public class Program
    {
        static PolyclinicDbContext context;
        static PolyclinicRepository repository;

        static Program()
        {
            context = new PolyclinicDbContext();
            repository = new PolyclinicRepository(context);
        }

        static void Main(string[] args)
        {
            //TestAddNewPatientDetails(); 
            //TestCalculateDoctorFees();  
            //TestCancelAppointment();  
            //TestFetchAllAppointments();
            //TestGetDoctorAppointmentUsingUSP();  
            //TestGetPatientDetails(); 
            //TestUpdatePatientAge(); 

        }

        #region TestAddNewPatientDetails
        public static void TestAddNewPatientDetails()
        {
            Patient patientObj = new Patient();

            patientObj.PatientId = "P113";
            patientObj.PatientName = "Joseph Kuruvila";
            patientObj.Age = 26;
            patientObj.Gender = "M";
            patientObj.ContactNumber = "7540432730";

            bool result = repository.AddNewPatientDetails(patientObj);
            if (result)
            {
                Console.WriteLine("New Patient Details added successfully and PatientId = " + patientObj.PatientId + ".");
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again!");
            }

        }
        #endregion

        #region TestCalculateDoctorFees
        public static void TestCalculateDoctorFees()
        {
        //    decimal result = repository.CalculateDoctorFees("D2", new DateTime(2023, 01, 02));

        //    if (result > 0)
        //    {
        //        Console.WriteLine("The fees of the doctor  is calculated successfully and the charge is Rs." + result + " only.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Something went wrong. Try again! Enter a valid DoctorId.");
        //    }
        }
        #endregion

        #region TestCancelAppointment
        public static void TestCancelAppointment()
        {
            int result = repository.CancelAppointment(9);

            if (result > 0)
            {
                Console.WriteLine("The appointment is cancelled successfully.");
            }

            else
            {
                Console.WriteLine("The appointmentNo is not available or some error occured during cancelling.");
            }
        }
        #endregion

        #region TestFetchAllAppointments
        public static void TestFetchAllAppointments()
        {
            var appointments = repository.FetchAllAppointments("D1", new DateTime(2025, 03, 20)); //2025-03-20
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("{0,-15}{1,-15}{2,-10}{3,-15}{4}", "DoctorName", "Specialization", "PatientId", "PatientName", "AppointmentNo");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            foreach (var meet in appointments)
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-10}{3,-15}{4}", meet.DoctorName, meet.Specialization, meet.PatientId, meet.PatientName, meet.AppointmentNo);
            }
        }
        #endregion

        #region TestGetDoctorAppointmentUsingUSP
        public static void TestGetDoctorAppointmentUsingUSP()
        {
            //int appointmentNo = 0;
            //int returnResult = repository.GetDoctorAppointment("P105", "D1", DateTime.Today, out appointmentNo);

            //if (returnResult > 0)
            //{
            //    Console.WriteLine("Appointment placed successfully and AppointmentNo = " + appointmentNo + ".");
            //}
            //else
            //{
            //    Console.WriteLine("Some error occurred. Try again!");
            //}
        }
        #endregion

        #region TestGetPatientDetails
        public static void TestGetPatientDetails()
        {
            var patientDetails = repository.GetPatientDetails("P104");
            if (patientDetails == null)
            {
                Console.WriteLine("Patient with ID 'P10445' not found.");
                return; // Exit the method early
            }
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("{0,-10}{1,-15}{2,-5}{3,-10}{4,-15}", "PatientID", "PatientName", "Age", "Gender", "ContactNumber");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("{0,-10}{1,-15}{2,-5}{3,-10}{4,-15}", patientDetails.PatientId, patientDetails.PatientName, patientDetails.Age, patientDetails.Gender, patientDetails.ContactNumber);
        }
        #endregion

        #region TestUpdatePatientAge
        public static void TestUpdatePatientAge()
        {
            bool result = repository.UpdatePatientAge("P113", 53);

            if (result)
            {
                Console.WriteLine("The age of the patient is updated successfully.");
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again!");
            }
        }
        #endregion

    }
}
