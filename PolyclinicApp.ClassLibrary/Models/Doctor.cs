using System;
using System.Collections.Generic;

namespace PolyclinicApp.ClassLibrary;

public partial class Doctor
{
    public string DoctorId { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string DoctorName { get; set; } = null!;

    public decimal Fees { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
