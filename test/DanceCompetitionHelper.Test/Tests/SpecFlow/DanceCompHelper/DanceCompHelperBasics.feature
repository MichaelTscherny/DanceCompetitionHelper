Feature: DanceCompHelperBasics

A short summary of the feature

Scenario: Simple Counts
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Adjudicator Panels in "DanceCompHelper-db"
        | CompetitionName | Name     | Comment        |
        | Test-Comp-01    | Panel 01 | Just one Panel |
    And following Adjudicators in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name              | Comment |
        | Test-Comp-01    | Panel 01             | 01-01        | Adjudicator 01-01 | 01-01   |
        | Test-Comp-01    | Panel 01             | 01-02        | Adjudicator 01-02 | 01-02   |
        | Test-Comp-01    | Panel 01             | 01-03        | Adjudicator 01-03 | 01-03   |
        | Test-Comp-01    | Panel 01             | 01-04        | Adjudicator 01-04 | 01-04   |
        | Test-Comp-01    | Panel 01             | 01-05        | Adjudicator 01-05 | 01-05   |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst |
        | Test-Comp-01    | Panel 01             | 1       | Clas-01    | Allg. Sta D          | STA        | Allg     | 0        | D     | 10                    | 900                   | 100            |
    And following Participants in "DanceCompHelper-db"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub |
        | Test-Comp-01    | Allg. Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        |
        | Test-Comp-01    | Allg. Sta D          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | Test-Club-01 | 10        |
    Then following Counts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CountClasses | CountParticipants |
        | Test-Comp-01    | 1            | 2                 |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-01    | Allg. Sta D          | 2                 | 0                  |                        | 0                    |                          | 0                  |
    And following Participants exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | StartNumber | MultipleStarts | NamePartA   | PossiblePromotionA | PossiblePromotionAInfo           | NamePartB   |
        | Test-Comp-01    | Allg. Sta D          | 1           | false          | Dancer 01-A | false              | [A] 0/0 + 100/1 = 100/1 -> False | Dancer 01-B |
        | Test-Comp-01    | Allg. Sta D          | 2           | false          | Dancer 02-A | false              | [A] 0/0 + 100/1 = 100/1 -> False | Dancer 02-B |
    
