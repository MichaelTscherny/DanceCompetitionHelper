# DanceCompetitionHelper

Helper for Dance Competiton Organizer and Volunteers

## Purpose of this project

I am a amateur dancer for competitive ballroom dancing in Austria.
Sometimes I do volunteer at competions sites as IT support
for calculating the results of the compuetions (scrutineer).
During my work - over the last 10 years - I was facing
some major "issues" where organizer and volunteers always struggle
with: 

* winners ^(1)^ and "promoted couples" ^(2)^ dancing in a higher 
* Time-Table of a competition

This projects aim is to privide an *"easy" to use software* where
roganizer and volunteers are able to get these information out
of lists of staring participants.

- ^(1)^ in austria it's possible that the winner is allowed to dance
  in the next/higher competitive classifications
- ^(2)^ cuple accumulated enough points to compete in the next/higher 
  competitive classifications

## Technical base

- [C# dotNet core 6.0](https://dotnet.microsoft.com/en-us/) - Win & Linux (x64 each)
- [ASP.NET Core-Web SDK](https://learn.microsoft.com/de-de/aspnet/core/razor-pages/web-sdk?view=aspnetcore-7.0) for HTTP interfaces
- [Entity Framework 7](https://learn.microsoft.com/en-us/ef/core/) - [SQLite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
- [MigraDoc](http://www.pdfsharp.net/migradocoverview.ashx?AspxAutoDetectCookieSupport=1) for PDFs

## Contribute

If you like to help on this project, please check 
[CONTRIBUTING](CONTRIBUTING.md), create an issue and a branch.

## Thanks to

following sources helped to get this project "on track":

- [Österreichicher TanzSport Verband](https://www.tanzsportverband.at/)
- [USA Dance](https://usadance.org/)
- [WDSF](https://www.worlddancesport.org/)

## License

See the [LICENSE](LICENSE.md) file for license rights and limitations (MIT).
