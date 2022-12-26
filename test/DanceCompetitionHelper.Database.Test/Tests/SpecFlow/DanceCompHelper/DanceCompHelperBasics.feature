Feature: DanceCompHelperBasics

A short summary of the feature

Scenario: Simple Counts
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | 1       | Clas-01    | Allg. Sta D          | STA        | Allg     | 0        | D     | 10                    | 900                   |
    And following Participants in "DanceCompHelper-db"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | OrgIdClub |
        | Test-Comp-01    | Allg. Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | 10        |
        | Test-Comp-01    | Allg. Sta D          | 2           | Dancer 02-B | 3          | Dancer 02-B | 4          | 10        |
    Then following DanceCompetitionHelper "DanceCompHelper" counts exists
        | CompetitionName | CountClasses | CountParticipants |
        | Test-Comp-01    | 1            | 2                 |
    
