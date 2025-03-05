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
    }
}
