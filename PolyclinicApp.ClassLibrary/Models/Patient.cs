using System;
using System.Collections.Generic;

namespace PolyclinicApp.ClassLibrary;

public partial class Patient
{
    public string PatientId { get; set; } = null!;

    public string PatientName { get; set; } = null!;

    public byte Age { get; set; }

    public string Gender { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
