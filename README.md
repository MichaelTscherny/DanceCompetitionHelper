# DanceCompetitionHelper

Helper for Dance Competiton Organizers and Volunteers

## Purpose of this project

I am a amateur dancer for competitive ballroom dancing in Austria.
Sometimes I do volunteer at competions sites as IT support
for calculating the results of the compuetions (scrutineer).
During my work - over the last 10 years - I was facing
some major "issues" where organizer and volunteers always struggle
with: 

* (class) winners [^1]</sup> and "promoted couples" [^2] dancing in a higher 
* Time-Table of a competition

This projects aim is to privide an *"easy" to use software* where
roganizer and volunteers are able to get these information out
of lists of staring participants.

[^1]: in austria it's possible that the winner is allowed to dance
  in the next/higher competitive classifications
[^2]: cuple accumulated enough points to compete in the next/higher 
  competitive classifications

### Competition organization in Austria

The [Österreichicher TanzSport Verband (ÖTSV)](https://www.tanzsportverband.at/) 
is "the head" origanization of competitive ballroom dancesport in Austria and
member of [WDSF](https://www.worlddancesport.org/). In Austria its common 
that a club request
a competition with a various number of classes (age, classification, etc.).
If accepted, couples who are member of ÖTSV (by joining a club which is 
member of ÖTSV) are able to *register via ÖTSV* to participate at this 
competition. When the registration is closed - in common 10 days before
the competition - the club who is in charge of the competition needs 
to create at least a time-schedule for the participants. This is done 
based on the registration counts by *starting class*. Because the count 
of couples differ at each competition (based on possible classes, 
location, etc.) it is always a *different story*.

In other countries it is common that the time-schedule is needed as acceptence criteria
for a competition registration. 


## Technical base

- [C# dotNet core 6.0](https://dotnet.microsoft.com/en-us/) - Win & Linux (x64 each)
- [ASP.NET Core-Web SDK](https://learn.microsoft.com/de-de/aspnet/core/razor-pages/web-sdk?view=aspnetcore-7.0) for HTTP interfaces
- [Entity Framework 7](https://learn.microsoft.com/en-us/ef/core/) - [SQLite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
- [MigraDoc](http://www.pdfsharp.net/migradocoverview.ashx?AspxAutoDetectCookieSupport=1) for PDFs
- [NUnit](https://www.nuget.org/packages/NUnit)
- [SpecFlow](https://www.nuget.org/packages/SpecFlow/)

### Folder struct

    - docs  -> documentation for this project
    - src   -> code of software projects
    - test  -> test of software projects

## EU General Data Protection Regulation (GDPR)

Some info about this software according to [EU-GDPR](https://eur-lex.europa.eu/legal-content/EN/ALL/?uri=celex%3A32016R0679).
This software is supposted to work with folloing information.

### Participants List/Start numbers

* Full Name (First-, Middle- Last-Name) or parts of it for the participants lists
* Name (or Parts) of club or origanization the participant is member of
* Nationality of participants and club/organizations ("available" via addresses)
* Address/Location of the competition

### Time Schedule

* Nationality of club/organizations ("available" via addresses)
* Address/Location of the competition

### Please note

It is not designed to work with adresses of participants at all. But it does not check
if the imported or entered data contains parts of addresses or other personal data.

## Contribute

If you like to help on this project, please check 
[CONTRIBUTING](CONTRIBUTING.md), create an issue and a branch.

## Thanks to

following sources helped to get this project "on track":

- [Österreichicher TanzSport Verband (ÖTSV)](https://www.tanzsportverband.at/): Docu, API
- [WDSF](https://www.worlddancesport.org/): Docu
- [USA Dance](https://usadance.org/): Docu

## License

See the [LICENSE](LICENSE.md) file for license rights and limitations (MIT).
