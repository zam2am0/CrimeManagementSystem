using System;

namespace CrimeManagementSystem.Models;

public class Person
{
    public string Id{ get; set; }
    public string Type { get; set; } // "Suspect", "Victim"
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
}
