Feature: DanceCompHelper - Multiple Starts

A short summary of the feature

Scenario: None
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | 1       | Class-01   | Sen 1 Sta D          | Sta        | Sen      | 1        | D     | 10                    | 900                   |
        | Test-Comp-01    | 1       | Class-02   | Sen 2 Sta D          | Sta        | Sen      | 2        | D     | 10                    | 1200                  |
        | Test-Comp-01    | 1       | Class-03   | Sen 1 Sta C          | Sta        | Sen      | 1        | C     | 10                    | 900                   |
        | Test-Comp-01    | 1       | Class-04   | Sen 2 Sta C          | Sta        | Sen      | 2        | C     | 10                    | 1200                  |
    And following Participants in "DanceCompHelper-db"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | OrgIdClub |
        #
        | Test-Comp-01    | Sen 1 Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        |
        | Test-Comp-01    | Sen 1 Sta D          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta D          | 4           | Dancer 03-A | 5          | Dancer 03-B | 6          | 11        |
        #
        | Test-Comp-01    | Sen 1 Sta C          | 5           | Dancer 04-A | 7          | Dancer 04-B | 8          | 10        |
        | Test-Comp-01    | Sen 1 Sta C          | 6           | Dancer 05-A | 9          | Dancer 05-B | 10         | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta C          | 7           | Dancer 06-A | 11         | Dancer 06-B | 12         | 12        |
        | Test-Comp-01    | Sen 2 Sta C          | 8           | Dancer 07-A | 13         | Dancer 07-B | 14         | 13        |
    Then none multiple starts exists in Competition "Test-Comp-01" of DanceCompetitionHelper "DanceCompHelper"

    
Scenario: One in Sen Sta D
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | 1       | Class-01   | Sen 1 Sta D          | Sta        | Sen      | 1        | D     | 10                    | 900                   |
        | Test-Comp-01    | 1       | Class-02   | Sen 2 Sta D          | Sta        | Sen      | 2        | D     | 10                    | 1200                  |
        | Test-Comp-01    | 1       | Class-03   | Sen 1 Sta C          | Sta        | Sen      | 1        | C     | 10                    | 900                   |
        | Test-Comp-01    | 1       | Class-04   | Sen 2 Sta C          | Sta        | Sen      | 2        | C     | 10                    | 1200                  |
    And following Participants in "DanceCompHelper-db"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | OrgIdClub |
        #
        | Test-Comp-01    | Sen 1 Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        |
        | Test-Comp-01    | Sen 1 Sta D          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta D          | 3           | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        |
        | Test-Comp-01    | Sen 2 Sta D          | 4           | Dancer 03-A | 5          | Dancer 03-B | 6          | 11        |
        #
        | Test-Comp-01    | Sen 1 Sta C          | 5           | Dancer 04-A | 7          | Dancer 04-B | 8          | 10        |
        | Test-Comp-01    | Sen 1 Sta C          | 6           | Dancer 05-A | 9          | Dancer 05-B | 10         | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta C          | 7           | Dancer 06-A | 11         | Dancer 06-B | 12         | 12        |
        | Test-Comp-01    | Sen 2 Sta C          | 8           | Dancer 07-A | 13         | Dancer 07-B | 14         | 13        |
    Then following multiple starts exists in Competition "Test-Comp-01" of DanceCompetitionHelper "DanceCompHelper"
        | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | OrgIdClub | CompetitionClassName | StartNumber |
        | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        | Sen 1 Sta D          | 1           |
        | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        | Sen 2 Sta D          | 3           |

Scenario: One in Sen Sta D and C
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | 1       | Class-01   | Sen 1 Sta D          | Sta        | Sen      | 1        | D     | 10                    | 900                   |
        | Test-Comp-01    | 1       | Class-02   | Sen 2 Sta D          | Sta        | Sen      | 2        | D     | 10                    | 1200                  |
        | Test-Comp-01    | 1       | Class-03   | Sen 1 Sta C          | Sta        | Sen      | 1        | C     | 10                    | 900                   |
        | Test-Comp-01    | 1       | Class-04   | Sen 2 Sta C          | Sta        | Sen      | 2        | C     | 10                    | 1200                  |
    And following Participants in "DanceCompHelper-db"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | OrgIdClub |
        #
        | Test-Comp-01    | Sen 1 Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        |
        | Test-Comp-01    | Sen 1 Sta D          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta D          | 3           | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        |
        | Test-Comp-01    | Sen 2 Sta D          | 4           | Dancer 03-A | 5          | Dancer 03-B | 6          | 11        |
        #
        | Test-Comp-01    | Sen 1 Sta C          | 5           | Dancer 04-A | 7          | Dancer 04-B | 8          | 10        |
        | Test-Comp-01    | Sen 1 Sta C          | 6           | Dancer 05-A | 9          | Dancer 05-B | 10         | 11        |
        | Test-Comp-01    | Sen 1 Sta C          | 9           | Dancer 08-A | 15         | Dancer 08-B | 16         | 14        |
        #
        | Test-Comp-01    | Sen 2 Sta C          | 7           | Dancer 06-A | 11         | Dancer 06-B | 12         | 12        |
        | Test-Comp-01    | Sen 2 Sta C          | 8           | Dancer 07-A | 13         | Dancer 07-B | 14         | 13        |
        | Test-Comp-01    | Sen 2 Sta C          | 9           | Dancer 08-A | 15         | Dancer 08-B | 16         | 14        |
    Then following multiple starts exists in Competition "Test-Comp-01" of DanceCompetitionHelper "DanceCompHelper"
        | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | OrgIdClub | CompetitionClassName | StartNumber |
        # Sen x Sta D
        | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        | Sen 1 Sta D          | 1           |
        | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        | Sen 2 Sta D          | 3           |
        # Sen x Sta C
        | Dancer 08-A | 15         | Dancer 08-B | 16         | 14        | Sen 1 Sta C          | 9           |
        | Dancer 08-A | 15         | Dancer 08-B | 16         | 14        | Sen 2 Sta C          | 9           |
