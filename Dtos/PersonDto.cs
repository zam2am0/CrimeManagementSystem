using System;

namespace CrimeManagementSystem.Dtos;

public class PersonDto
{
    public string Type { get; set; }  // "Suspect", "Victim", "Witness"
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
}
