using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        if (await dbContext.Patients.AnyAsync()) return; // csak egyszer t�lts�k fel

        var random = new Random();
        DateTime RandomDate() => DateTime.UtcNow.AddDays(-random.Next(0, 60)).AddMinutes(random.Next(0, 1440));

        var patients = new List<Patient>
        {
            new() { Id = Guid.NewGuid(), Name = "Kov�cs B�la", Address = "Budapest, Bart�k B�la �t 12.", HealthInsuranceNumber = "123-456-789", Complaints = "Fejf�j�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Nagy Anna", Address = "Debrecen, Piac utca 45.", HealthInsuranceNumber = "234-567-890", Complaints = "H�tf�j�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Szab� G�bor", Address = "Szeged, Tisza Lajos k�r�t 8.", HealthInsuranceNumber = "345-678-901", Complaints = "L�z �s k�h�g�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "T�th Eszter", Address = "P�cs, R�k�czi �t 3.", HealthInsuranceNumber = "456-789-012", Complaints = "Gyomorf�jdalom", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Varga D�niel", Address = "Gy�r, Szent Istv�n �t 22.", HealthInsuranceNumber = "567-890-123", Complaints = "F�lz�g�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Kiss J�lia", Address = "Miskolc, Pet�fi utca 9.", HealthInsuranceNumber = "678-901-234", Complaints = "Sz�d�l�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Farkas L�szl�", Address = "Ny�regyh�za, Kossuth t�r 1.", HealthInsuranceNumber = "789-012-345", Complaints = "Magas v�rnyom�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Balogh Zs�fia", Address = "Sz�kesfeh�rv�r, Budai �t 17.", HealthInsuranceNumber = "890-123-456", Complaints = "Allergia", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Moln�r P�ter", Address = "Eger, Dob� t�r 5.", HealthInsuranceNumber = "901-234-567", Complaints = "L�gszomj", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Horv�th Katalin", Address = "Kecskem�t, R�k�czi �t 10.", HealthInsuranceNumber = "012-345-678", Complaints = "�z�leti f�jdalom", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Bogn�r �d�m", Address = "Zalaegerszeg, Ady Endre utca 4.", HealthInsuranceNumber = "111-222-333", Complaints = "Hasmen�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Ol�h Emese", Address = "Sopron, V�rker�let 20.", HealthInsuranceNumber = "222-333-444", Complaints = "Torokf�j�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Lakatos Zolt�n", Address = "B�k�scsaba, Andr�ssy �t 6.", HealthInsuranceNumber = "333-444-555", Complaints = "Mellkasi f�jdalom", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Pint�r No�mi", Address = "Tatab�nya, Szent Borb�la �t 11.", HealthInsuranceNumber = "444-555-666", Complaints = "�lmatlans�g", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Jakab Tam�s", Address = "Szolnok, Kossuth Lajos utca 13.", HealthInsuranceNumber = "555-666-777", Complaints = "F�radts�g", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Simon R�ka", Address = "Veszpr�m, �v�ros t�r 2.", HealthInsuranceNumber = "666-777-888", Complaints = "L�t�szavar", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Guly�s Andr�s", Address = "Esztergom, Mindszenty t�r 7.", HealthInsuranceNumber = "777-888-999", Complaints = "Sz�vdobog�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "B�r� N�ra", Address = "Salg�tarj�n, R�k�czi �t 15.", HealthInsuranceNumber = "888-999-000", Complaints = "Hidegr�z�s", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Feh�r Levente", Address = "Kaposv�r, F� utca 19.", HealthInsuranceNumber = "999-000-111", Complaints = "H�nyinger", CreatedAt = RandomDate() },
            new() { Id = Guid.NewGuid(), Name = "Sz�cs D�ra", Address = "Szeksz�rd, B�la kir�ly t�r 3.", HealthInsuranceNumber = "000-111-222", Complaints = "L�bf�jdalom", CreatedAt = RandomDate() }
        };

        dbContext.Patients.AddRange(patients);
        await dbContext.SaveChangesAsync();
    }
}
