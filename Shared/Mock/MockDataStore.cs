using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Models;

namespace Shared.Mock;

//Mock data - test git CI updates
public static class MockDataStore
{
    private static readonly Random _random = new();
    public static List<PatientDto> Patients { get; } = new()
    {
        new() { Id = Guid.NewGuid(), Name = "Kovács Béla", Address = "Budapest, Bartók Béla út 12.", HealthInsuranceNumber = "123-456-789", Complaints = "Fejfájás", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Nagy Anna", Address = "Debrecen, Piac utca 45.", HealthInsuranceNumber = "234-567-890", Complaints = "Hátfájás", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Szabó Gábor", Address = "Szeged, Tisza Lajos körút 8.", HealthInsuranceNumber = "345-678-901", Complaints = "Láz és köhögés", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Tóth Eszter", Address = "Pécs, Rákóczi út 3.", HealthInsuranceNumber = "456-789-012", Complaints = "Gyomorfájdalom", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Varga Dániel", Address = "Győr, Szent István út 22.", HealthInsuranceNumber = "567-890-123", Complaints = "Fülzúgás", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Kiss Júlia", Address = "Miskolc, Petőfi utca 9.", HealthInsuranceNumber = "678-901-234", Complaints = "Szédülés", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Farkas László", Address = "Nyíregyháza, Kossuth tér 1.", HealthInsuranceNumber = "789-012-345", Complaints = "Magas vérnyomás", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Balogh Zsófia", Address = "Székesfehérvár, Budai út 17.", HealthInsuranceNumber = "890-123-456", Complaints = "Allergia", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Molnár Péter", Address = "Eger, Dobó tér 5.", HealthInsuranceNumber = "901-234-567", Complaints = "Légszomj", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Horváth Katalin", Address = "Kecskemét, Rákóczi út 10.", HealthInsuranceNumber = "012-345-678", Complaints = "Ízületi fájdalom", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Bognár Ádám", Address = "Zalaegerszeg, Ady Endre utca 4.", HealthInsuranceNumber = "111-222-333", Complaints = "Hasmenés", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Oláh Emese", Address = "Sopron, Várkerület 20.", HealthInsuranceNumber = "222-333-444", Complaints = "Torokfájás", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Lakatos Zoltán", Address = "Békéscsaba, Andrássy út 6.", HealthInsuranceNumber = "333-444-555", Complaints = "Mellkasi fájdalom", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Pintér Noémi", Address = "Tatabánya, Szent Borbála út 11.", HealthInsuranceNumber = "444-555-666", Complaints = "Álmatlanság", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Jakab Tamás", Address = "Szolnok, Kossuth Lajos utca 13.", HealthInsuranceNumber = "555-666-777", Complaints = "Fáradtság", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Simon Réka", Address = "Veszprém, Óváros tér 2.", HealthInsuranceNumber = "666-777-888", Complaints = "Látászavar", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Gulyás András", Address = "Esztergom, Mindszenty tér 7.", HealthInsuranceNumber = "777-888-999", Complaints = "Szívdobogás", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Bíró Nóra", Address = "Salgótarján, Rákóczi út 15.", HealthInsuranceNumber = "888-999-000", Complaints = "Hidegrázás", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Fehér Levente", Address = "Kaposvár, Fő utca 19.", HealthInsuranceNumber = "999-000-111", Complaints = "Hányinger", CreatedAt = RandomDate() },
        new() { Id = Guid.NewGuid(), Name = "Szűcs Dóra", Address = "Szekszárd, Béla király tér 3.", HealthInsuranceNumber = "000-111-222", Complaints = "Lábfájdalom", CreatedAt = RandomDate() }   
    };
    private static DateTime RandomDate()
    {
        var daysAgo = _random.Next(0, 60);
        return DateTime.Now.AddDays(-daysAgo).AddMinutes(_random.Next(0, 1440));
    }
}
