Feature: DanceCompHelper - Promotion Tests - PartA Only - Multiple Starts

A short summary of the feature

Scenario: Multiple Start - None
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
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg Sta D           | Allg Sta C                   | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg Sta C           |                              | Sta        | Allg     | 0        | C     | 10                    | 1200                  | 100            | 10            |
        | Test-Comp-01    | Panel 01             | 1       | Class-03   | Allg La D            | Allg La C                    | La         | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
        | Test-Comp-01    | Panel 01             | 1       | Class-04   | Allg La C            |                              | La         | Allg     | 0        | C     | 10                    | 1200                  | 100            | 10            |
    And following Participants in "DanceCompHelper-db"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | ClubName     | OrgIdClub | OrgPointsPartA | OrgStartsPartA |
        #                                                                                   
        | Test-Comp-01    | Allg Sta D           | 10          | Dancer 10-A | 10         | Test-Club-10 | 1010      | 0              | 0              |
        #                                                                                   
        | Test-Comp-01    | Allg Sta C           | 11          | Dancer 11-A | 11         | Test-Club-11 | 1011      | 0              | 0              |
        #
        | Test-Comp-01    | Allg La D            | 20          | Dancer 20-A | 20         | Test-Club-20 | 1020      | 0              | 0              |
        | Test-Comp-01    | Allg La D            | 100         | Dancer 10-A | 10         | Test-Club-10 | 1010      | 0              | 0              |
        #
        | Test-Comp-01    | Allg La C            | 21          | Dancer 21-A | 21         | Test-Club-21 | 2021      | 0              | 0              |
    Then following multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | ClubName     | OrgIdClub |
        | Test-Comp-01    | Allg Sta D           | 10          | Dancer 10-A | 10         | Test-Club-10 | 1010      |
        | Test-Comp-01    | Allg La D            | 100         | Dancer 10-A | 10         | Test-Club-10 | 1010      |
    And following Participants exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | MultipleStarts | PossiblePromotionA | PossiblePromotionAInfo           |
        #                                                                                  
        | Test-Comp-01    | Allg Sta D           | 10          | Dancer 10-A | true           | false              | [A] 0/0 + 100/1 = 100/1 -> False |
        #                                                                               
        | Test-Comp-01    | Allg Sta C           | 11          | Dancer 11-A | false          | false              | [A] 0/0 + 100/1 = 100/1 -> False |
        #                                                        
        | Test-Comp-01    | Allg La D            | 20          | Dancer 20-A | false          | false              | [A] 0/0 + 100/1 = 100/1 -> False |
        | Test-Comp-01    | Allg La D            | 100         | Dancer 10-A | true           | false              | [A] 0/0 + 100/1 = 100/1 -> False |
        #                                                        
        | Test-Comp-01    | Allg La C            | 21          | Dancer 21-A | false          | false              | [A] 0/0 + 100/1 = 100/1 -> False |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo | ExtraPartByPromotion | ExtraPartByPromotionInfo | ExtraManualStarter |
        | Test-Comp-01    | Allg Sta D           | Allg Sta C                   | Class-01   | 1                 | 0                  |                        | 0                    |                          | 0                  |
        | Test-Comp-01    | Allg Sta C           |                              | Class-02   | 1                 | 1                  | Allg Sta D (Class-01)  | 0                    |                          | 0                  |
        | Test-Comp-01    | Allg La D            | Allg La C                    | Class-03   | 2                 | 0                  |                        | 0                    |                          | 0                  |
        | Test-Comp-01    | Allg La C            |                              | Class-04   | 1                 | 1                  | Allg La D (Class-03)   | 0                    |                          | 0                  |
