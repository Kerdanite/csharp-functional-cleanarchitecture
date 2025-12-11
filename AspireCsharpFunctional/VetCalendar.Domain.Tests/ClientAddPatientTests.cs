using VetCalendar.Domain.Customers;

namespace VetCalendar.Domain.Tests
{
    public class ClientAddPatientTests
    {
        [Fact]
        public void AddPatient_ValidData_AddsPatientToClientAndReturnsSuccess()
        {
            var clientResult = Client.Create(
                firstName:   "John",
                lastName:    "Doe",
                email:       "john.doe@example.com",
                phoneNumber: "+33601020304");

            Assert.True(clientResult.IsSuccess); 

            var client = clientResult.Value;

            var expectedName     = "Misty";
            var expectedSpecies  = "Cat";
            var expectedBirthDate = new DateOnly(2020, 5, 12);

            var result = client.AddPatient(
                name: expectedName,
                species: expectedSpecies,
                birthDate: expectedBirthDate);

            Assert.True(result.IsSuccess);

            var patient = result.Value;

            Assert.Equal(expectedName, patient.Name);
            Assert.Equal(expectedSpecies, patient.Species);
            Assert.Equal(expectedBirthDate, patient.BirthDate);

            Assert.Single(client.Patients);

            var patientInCollection = Assert.Single(client.Patients);

            Assert.Same(patient, patientInCollection);
        }

        [Theory]
        [InlineData("", DomainErrors.Patient.NameIsRequired)]
        [InlineData(" ", DomainErrors.Patient.NameIsRequired)]
        [InlineData(null, DomainErrors.Patient.NameIsRequired)]
        [InlineData(@"this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
", 
            DomainErrors.Patient.NameTooLong)]
        public void AddPatient_InvalidName_ReturnsFailure(string name, string error)
        {
            var clientResult = Client.Create(
                firstName: "John",
                lastName: "Doe",
                email: "john.doe@example.com",
                phoneNumber: "+33601020304");

            var client = clientResult.Value;

            var result = client.AddPatient(
                name: name,
                species: "Cat",
                birthDate: new DateOnly(2020, 1, 1));

            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);

            Assert.Empty(client.Patients);
        }
        
        
        [Theory]
        [InlineData("", DomainErrors.Patient.SpeciesIsRequired)]
        [InlineData(" ", DomainErrors.Patient.SpeciesIsRequired)]
        [InlineData(null, DomainErrors.Patient.SpeciesIsRequired)]
        [InlineData(@"this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
this is a really really too long name this is a really really too long name this is a really really too long name
", 
            DomainErrors.Patient.SpeciesTooLong)]
        public void AddPatient_SpeciesInvalid_ReturnsFailure(string species, string error)
        {
            var clientResult = Client.Create(
                firstName: "John",
                lastName: "Doe",
                email: "john.doe@example.com",
                phoneNumber: "+33601020304");

            var client = clientResult.Value;

            var result = client.AddPatient(
                name: "Misty",
                species: species,
                birthDate: new DateOnly(2020, 1, 1));

            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);

            Assert.Empty(client.Patients);
        }


    }
}