Feature: DanceCompHelper - Multiple Starts - Double checks

A short summary of the feature

Scenario: None - 2 Comps with same starters
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test 01  |
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-02    | Oetsv         | ÖTSV-02          | Just a test 02  |
    And following Adjudicator Panels in "DanceCompHelper-db"
        | CompetitionName | Name        | Comment              |
        | Test-Comp-01    | Panel 01-01 | Just one Panel 01-01 |
    And following Adjudicator Panels in "DanceCompHelper-db"
        | CompetitionName | Name        | Comment              |
        | Test-Comp-02    | Panel 02-01 | Just one Panel 02-01 |
    And following Adjudicators in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name                 | Comment  |
        | Test-Comp-01    | Panel 01-01          | A-01         | Adjudicator 01-01-01 | 01-01-01 |
        | Test-Comp-01    | Panel 01-01          | B-01         | Adjudicator 01-01-02 | 01-01-02 |
        | Test-Comp-01    | Panel 01-01          | C-01         | Adjudicator 01-01-03 | 01-01-03 |
        | Test-Comp-01    | Panel 01-01          | D-01         | Adjudicator 01-01-04 | 01-01-04 |
        | Test-Comp-01    | Panel 01-01          | E-01         | Adjudicator 01-01-05 | 01-01-05 |
    And following Adjudicators in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name                 | Comment  |
        | Test-Comp-02    | Panel 02-01          | A-02         | Adjudicator 02-01-01 | 02-01-01 |
        | Test-Comp-02    | Panel 02-01          | B-02         | Adjudicator 02-01-02 | 02-01-02 |
        | Test-Comp-02    | Panel 02-01          | C-02         | Adjudicator 02-01-03 | 02-01-03 |
        | Test-Comp-02    | Panel 02-01          | D-02         | Adjudicator 02-01-04 | 02-01-04 |
        | Test-Comp-02    | Panel 02-01          | E-02         | Adjudicator 02-01-05 | 02-01-05 |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId  | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | Panel 01-01          | 1       | Class-01-01 | Sen 1 Sta D          | Sen 1 Sta C                  | Sta        | Sen      | 1        | D     | 10                    | 900                   |
        | Test-Comp-01    | Panel 01-01          | 1       | Class-01-02 | Sen 2 Sta D          | Sen 2 Sta C                  | Sta        | Sen      | 2        | D     | 10                    | 1200                  |
        | Test-Comp-01    | Panel 01-01          | 1       | Class-01-03 | Sen 1 Sta C          |                              | Sta        | Sen      | 1        | C     | 10                    | 900                   |
        | Test-Comp-01    | Panel 01-01          | 1       | Class-01-04 | Sen 2 Sta C          |                              | Sta        | Sen      | 2        | C     | 10                    | 1200                  |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId  | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-02    | Panel 02-01          | 1       | Class-02-01 | Sen 1 Sta D          | Sen 1 Sta C                  | Sta        | Sen      | 1        | D     | 10                    | 900                   |
        | Test-Comp-02    | Panel 02-01          | 1       | Class-02-02 | Sen 2 Sta D          | Sen 2 Sta C                  | Sta        | Sen      | 2        | D     | 10                    | 1200                  |
        | Test-Comp-02    | Panel 02-01          | 1       | Class-02-03 | Sen 1 Sta C          |                              | Sta        | Sen      | 1        | C     | 10                    | 900                   |
        | Test-Comp-02    | Panel 02-01          | 1       | Class-02-04 | Sen 2 Sta C          |                              | Sta        | Sen      | 2        | C     | 10                    | 1200                  |
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
    And following Participants in "DanceCompHelper-db"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub |
        #                                                                                                              
        | Test-Comp-02    | Sen 1 Sta D          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        |
        | Test-Comp-02    | Sen 1 Sta D          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | Test-Club-02 | 11        |
        #
        | Test-Comp-02    | Sen 2 Sta D          | 4           | Dancer 03-A | 5          | Dancer 03-B | 6          | Test-Club-02 | 11        |
        #
        | Test-Comp-02    | Sen 1 Sta C          | 5           | Dancer 04-A | 7          | Dancer 04-B | 8          | Test-Club-01 | 10        |
        | Test-Comp-02    | Sen 1 Sta C          | 6           | Dancer 05-A | 9          | Dancer 05-B | 10         | Test-Club-02 | 11        |
        #
        | Test-Comp-02    | Sen 2 Sta C          | 7           | Dancer 06-A | 11         | Dancer 06-B | 12         | Test-Club-03 | 12        |
        | Test-Comp-02    | Sen 2 Sta C          | 8           | Dancer 07-A | 13         | Dancer 07-B | 14         | Test-Club-04 | 13        |
    Then none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName |
        | Test-Comp-01    |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName |
        | Test-Comp-02    |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId  | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo    | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-01    | Sen 1 Sta D          | Sen 1 Sta C                  | Class-01-01 | 2                 | 0                  |                           | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta D          | Sen 2 Sta C                  | Class-01-02 | 1                 | 0                  |                           | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 1 Sta C          |                              | Class-01-03 | 2                 | 1                  | Sen 1 Sta D (Class-01-01) | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta C          |                              | Class-01-04 | 2                 | 1                  | Sen 2 Sta D (Class-01-02) | 0                    |                          | 0                  |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId  | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo    | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-02    | Sen 1 Sta D          | Sen 1 Sta C                  | Class-02-01 | 2                 | 0                  |                           | 0                    |                          | 0                  |
        | Test-Comp-02    | Sen 2 Sta D          | Sen 2 Sta C                  | Class-02-02 | 1                 | 0                  |                           | 0                    |                          | 0                  |
        | Test-Comp-02    | Sen 1 Sta C          |                              | Class-02-03 | 2                 | 1                  | Sen 1 Sta D (Class-02-01) | 0                    |                          | 0                  |
        | Test-Comp-02    | Sen 2 Sta C          |                              | Class-02-04 | 2                 | 1                  | Sen 2 Sta D (Class-02-02) | 0                    |                          | 0                  |
        
        
Scenario: None - 2 Comps with (mixed) same starters
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test 01  |
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-02    | Oetsv        | ÖTSV-02          | Just a test 02  |
    And following Adjudicator Panels in "DanceCompHelper-db"
        | CompetitionName | Name        | Comment              |
        | Test-Comp-01    | Panel 01-01 | Just one Panel 01-01 |
    And following Adjudicator Panels in "DanceCompHelper-db"
        | CompetitionName | Name        | Comment              |
        | Test-Comp-02    | Panel 02-01 | Just one Panel 02-01 |
    And following Adjudicators in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name                 | Comment  |
        | Test-Comp-01    | Panel 01-01          | A-01         | Adjudicator 01-01-01 | 01-01-01 |
        | Test-Comp-01    | Panel 01-01          | B-01         | Adjudicator 01-01-02 | 01-01-02 |
        | Test-Comp-01    | Panel 01-01          | C-01         | Adjudicator 01-01-03 | 01-01-03 |
        | Test-Comp-01    | Panel 01-01          | D-01         | Adjudicator 01-01-04 | 01-01-04 |
        | Test-Comp-01    | Panel 01-01          | E-01         | Adjudicator 01-01-05 | 01-01-05 |
    And following Adjudicators in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name                 | Comment  |
        | Test-Comp-02    | Panel 02-01          | A-02         | Adjudicator 02-01-01 | 02-01-01 |
        | Test-Comp-02    | Panel 02-01          | B-02         | Adjudicator 02-01-02 | 02-01-02 |
        | Test-Comp-02    | Panel 02-01          | C-02         | Adjudicator 02-01-03 | 02-01-03 |
        | Test-Comp-02    | Panel 02-01          | D-02         | Adjudicator 02-01-04 | 02-01-04 |
        | Test-Comp-02    | Panel 02-01          | E-02         | Adjudicator 02-01-05 | 02-01-05 |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId  | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | Panel 01-01          | 1       | Class-01-01 | Sen 1 Sta D          | Sen 1 Sta C                  | Sta        | Sen      | 1        | D     | 10                    | 900                   |
        | Test-Comp-01    | Panel 01-01          | 1       | Class-01-02 | Sen 2 Sta D          | Sen 2 Sta C                  | Sta        | Sen      | 2        | D     | 10                    | 1200                  |
        | Test-Comp-01    | Panel 01-01          | 1       | Class-01-03 | Sen 1 Sta C          |                              | Sta        | Sen      | 1        | C     | 10                    | 900                   |
        | Test-Comp-01    | Panel 01-01          | 1       | Class-01-04 | Sen 2 Sta C          |                              | Sta        | Sen      | 2        | C     | 10                    | 1200                  |
    And following Competition Classes in "DanceCompHelper-db"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId  | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-02    | Panel 02-01          | 1       | Class-02-01 | Sen 1 Sta D          | Sen 1 Sta C                  | Sta        | Sen      | 1        | D     | 10                    | 900                   |
        | Test-Comp-02    | Panel 02-01          | 1       | Class-02-02 | Sen 2 Sta D          | Sen 2 Sta C                  | Sta        | Sen      | 2        | D     | 10                    | 1200                  |
        | Test-Comp-02    | Panel 02-01          | 1       | Class-02-03 | Sen 1 Sta C          |                              | Sta        | Sen      | 1        | C     | 10                    | 900                   |
        | Test-Comp-02    | Panel 02-01          | 1       | Class-02-04 | Sen 2 Sta C          |                              | Sta        | Sen      | 2        | C     | 10                    | 1200                  |
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
    And following Participants in "DanceCompHelper-db"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub |
        #                                                                                                              
        | Test-Comp-02    | Sen 1 Sta D          | 8           | Dancer 07-A | 13         | Dancer 07-B | 14         | Test-Club-04 | 13        |
        | Test-Comp-02    | Sen 1 Sta D          | 7           | Dancer 06-A | 11         | Dancer 06-B | 12         | Test-Club-03 | 12        |
        #                             
        | Test-Comp-02    | Sen 2 Sta D          | 6           | Dancer 05-A | 9          | Dancer 05-B | 10         | Test-Club-02 | 11        |
        #
        | Test-Comp-02    | Sen 1 Sta C          | 5           | Dancer 04-A | 7          | Dancer 04-B | 8          | Test-Club-01 | 10        |
        | Test-Comp-02    | Sen 1 Sta C          | 4           | Dancer 03-A | 5          | Dancer 03-B | 6          | Test-Club-02 | 11        |
        #
        | Test-Comp-02    | Sen 2 Sta C          | 2           | Dancer 02-A | 3          | Dancer 02-B | 4          | Test-Club-02 | 11        |
        | Test-Comp-02    | Sen 2 Sta C          | 1           | Dancer 01-A | 1          | Dancer 01-B | 2          | Test-Club-01 | 10        |
    Then none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName |
        | Test-Comp-01    |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName |
        | Test-Comp-02    |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId  | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo    | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-01    | Sen 1 Sta D          | Sen 1 Sta C                  | Class-01-01 | 2                 | 0                  |                           | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta D          | Sen 2 Sta C                  | Class-01-02 | 1                 | 0                  |                           | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 1 Sta C          |                              | Class-01-03 | 2                 | 1                  | Sen 1 Sta D (Class-01-01) | 0                    |                          | 0                  |
        | Test-Comp-01    | Sen 2 Sta C          |                              | Class-01-04 | 2                 | 1                  | Sen 2 Sta D (Class-01-02) | 0                    |                          | 0                  |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId  | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo    | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-02    | Sen 1 Sta D          | Sen 1 Sta C                  | Class-02-01 | 2                 | 0                  |                           | 0                    |                          | 0                  |
        | Test-Comp-02    | Sen 2 Sta D          | Sen 2 Sta C                  | Class-02-02 | 1                 | 0                  |                           | 0                    |                          | 0                  |
        | Test-Comp-02    | Sen 1 Sta C          |                              | Class-02-03 | 2                 | 1                  | Sen 1 Sta D (Class-02-01) | 0                    |                          | 0                  |
        | Test-Comp-02    | Sen 2 Sta C          |                              | Class-02-04 | 2                 | 1                  | Sen 2 Sta D (Class-02-02) | 0                    |                          | 0                  |
