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
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub |
        #                                                                                                              
        | Test-Comp-01    | Sen 1 Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        |
        | Test-Comp-01    | Sen 1 Sta D          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | Test-Club-02 | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta D          | 4           | Dancer 03-A | 5          | Dancer 03-B | 6          | Test-Club-02 | 11        |
        #
        | Test-Comp-01    | Sen 1 Sta C          | 5           | Dancer 04-A | 7          | Dancer 04-B | 8          | Test-Club-01 | 10        |
        | Test-Comp-01    | Sen 1 Sta C          | 6           | Dancer 05-A | 9          | Dancer 05-B | 10         | Test-Club-02 | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta C          | 7           | Dancer 06-A | 11         | Dancer 06-B | 12         | Test-Club-03 | 12        |
        | Test-Comp-01    | Sen 2 Sta C          | 8           | Dancer 07-A | 13         | Dancer 07-B | 14         | Test-Club-04 | 13        |
    Then none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName |
        | Test-Comp-01    |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-01    | Sen 1 Sta D          | 2                 | 0                  |                        | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta D          | 1                 | 0                  |                        | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 1 Sta C          | 2                 | 1                  | Sen 1 Sta D (Class-01) | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta C          | 2                 | 1                  | Sen 2 Sta D (Class-02) | 0                    |                          | 0                  |
    
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
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub |
        #
        | Test-Comp-01    | Sen 1 Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        |
        | Test-Comp-01    | Sen 1 Sta D          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | Test-Club-02 | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta D          | 3           | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        |
        | Test-Comp-01    | Sen 2 Sta D          | 4           | Dancer 03-A | 5          | Dancer 03-B | 6          | Test-Club-02 | 11        |
        #
        | Test-Comp-01    | Sen 1 Sta C          | 5           | Dancer 04-A | 7          | Dancer 04-B | 8          | Test-Club-01 | 10        |
        | Test-Comp-01    | Sen 1 Sta C          | 6           | Dancer 05-A | 9          | Dancer 05-B | 10         | Test-Club-02 | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta C          | 7           | Dancer 06-A | 11         | Dancer 06-B | 12         | Test-Club-03 | 12        |
        | Test-Comp-01    | Sen 2 Sta C          | 8           | Dancer 07-A | 13         | Dancer 07-B | 14         | Test-Club-04 | 13        |
    Then following multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub | CompetitionClassName | StartNumber |
        | Test-Comp-01    | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        | Sen 1 Sta D          | 1           |
        | Test-Comp-01    | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        | Sen 2 Sta D          | 3           |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-01    | Sen 1 Sta D          | 2                 | 0                  |                        | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta D          | 2                 | 0                  |                        | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 1 Sta C          | 2                 | 1                  | Sen 1 Sta D (Class-01) | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta C          | 2                 | 1                  | Sen 2 Sta D (Class-02) | 0                    |                          | 0                  |

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
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub |
        #
        | Test-Comp-01    | Sen 1 Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        |
        | Test-Comp-01    | Sen 1 Sta D          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | Test-Club-02 | 11        |
        #
        | Test-Comp-01    | Sen 2 Sta D          | 3           | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        |
        | Test-Comp-01    | Sen 2 Sta D          | 4           | Dancer 03-A | 5          | Dancer 03-B | 6          | Test-Club-02 | 11        |
        #
        | Test-Comp-01    | Sen 1 Sta C          | 5           | Dancer 04-A | 7          | Dancer 04-B | 8          | Test-Club-01 | 10        |
        | Test-Comp-01    | Sen 1 Sta C          | 6           | Dancer 05-A | 9          | Dancer 05-B | 10         | Test-Club-02 | 11        |
        | Test-Comp-01    | Sen 1 Sta C          | 9           | Dancer 08-A | 15         | Dancer 08-B | 16         | Test-Club-05 | 14        |
        #
        | Test-Comp-01    | Sen 2 Sta C          | 7           | Dancer 06-A | 11         | Dancer 06-B | 12         | Test-Club-03 | 12        |
        | Test-Comp-01    | Sen 2 Sta C          | 8           | Dancer 07-A | 13         | Dancer 07-B | 14         | Test-Club-04 | 13        |
        | Test-Comp-01    | Sen 2 Sta C          | 9           | Dancer 08-A | 15         | Dancer 08-B | 16         | Test-Club-05 | 14        |
    Then following multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub | CompetitionClassName | StartNumber |
        # Sen x Sta D
        | Test-Comp-01    | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        | Sen 1 Sta D          | 1           |
        | Test-Comp-01    | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        | Sen 2 Sta D          | 3           |
        # Sen x Sta C
        | Test-Comp-01    | Dancer 08-A | 15         | Dancer 08-B | 16         | Test-Club-05 | 14        | Sen 1 Sta C          | 9           |
        | Test-Comp-01    | Dancer 08-A | 15         | Dancer 08-B | 16         | Test-Club-05 | 14        | Sen 2 Sta C          | 9           |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-01    | Sen 1 Sta D          | 2                 | 0                  |                        | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta D          | 2                 | 0                  |                        | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 1 Sta C          | 3                 | 1                  | Sen 1 Sta D (Class-01) | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta C          | 3                 | 1                  | Sen 2 Sta D (Class-02) | 0                    |                          | 0                  |
