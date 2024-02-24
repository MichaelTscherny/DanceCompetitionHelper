Feature: Participants

A short summary of the feature

Scenario: Single Participant
    Given following DanceComp-DB "Dach_db_participants_01"
    And following Competitions in "Dach_db_participants_01"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
    And following Adjudicator Panels in "Dach_db_participants_01"
        | CompetitionName | Name     | Comment        |
        | Test-Comp-01    | Panel 01 | Just one Panel |
    And following Adjudicators in "Dach_db_participants_01"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name              | Comment |
        | Test-Comp-01    | Panel 01             | 01-01        | Adjudicator 01-01 | 01-01   |
        | Test-Comp-01    | Panel 01             | 01-02        | Adjudicator 01-02 | 01-02   |
        | Test-Comp-01    | Panel 01             | 01-03        | Adjudicator 01-03 | 01-03   |
        | Test-Comp-01    | Panel 01             | 01-04        | Adjudicator 01-04 | 01-04   |
        | Test-Comp-01    | Panel 01             | 01-05        | Adjudicator 01-05 | 01-05   |
    And following Competition Classes in "Dach_db_participants_01"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg. Sta D          | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
    And following Participants in "Dach_db_participants_01"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA        | OrgIdPartA | NamePartB       | OrgIdPartB | ClubName     | OrgIdClub |
        | Test-Comp-01    | Allg. Sta D          | 1           | Michael Tscherny | 1          | Margot Tscherny | 2          | Test-Club-01 | 3         |
    Then following Competitions exists in "Dach_db_participants_01"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
    And following Competition Classes exists in "Dach_db_participants_01"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg. Sta D          | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
    And following Participants exists in "Dach_db_participants_01"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA        | OrgIdPartA | NamePartB       | OrgIdPartB | ClubName     | OrgIdClub |
        | Test-Comp-01    | Allg. Sta D          | 1           | Michael Tscherny | 1          | Margot Tscherny | 2          | Test-Club-01 | 3         |
    
Scenario: Multiple Participants
    Given following DanceComp-DB "Dach_db_participants_02"
    And following Competitions in "Dach_db_participants_02"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv        | ÖTSV-02          | Just another test |
    # Test-Comp-01
    And following Adjudicator Panels in "Dach_db_participants_02"
        | CompetitionName | Name     | Comment        |
        | Test-Comp-01    | Panel 01 | Just one Panel |
    And following Adjudicators in "Dach_db_participants_02"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name              | Comment |
        | Test-Comp-01    | Panel 01             | 01-01        | Adjudicator 01-01 | 01-01   |
        | Test-Comp-01    | Panel 01             | 01-02        | Adjudicator 01-02 | 01-02   |
        | Test-Comp-01    | Panel 01             | 01-03        | Adjudicator 01-03 | 01-03   |
        | Test-Comp-01    | Panel 01             | 01-04        | Adjudicator 01-04 | 01-04   |
        | Test-Comp-01    | Panel 01             | 01-05        | Adjudicator 01-05 | 01-05   |
    And following Competition Classes in "Dach_db_participants_02"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg. Sta D          | Allg. Sta C                  | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg. Sta C          |                              | Sta        | Allg     | 0        | C     | 10                    | 1800                  | 100            | 10            |
    And following Participants in "Dach_db_participants_02"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA        | OrgIdPartA | NamePartB       | OrgIdPartB | ClubName     | OrgIdClub |
        | Test-Comp-01    | Allg. Sta D          | 1           | Michael Tscherny | 1          | Margot Tscherny | 2          | Test-Club-01 | 10        |
        | Test-Comp-01    | Allg. Sta D          | 2           | John Smith       | 3          | Jane Smith      | 4          | Test-Club-01 | 10        |
    # Test-Comp-02
    And following Adjudicator Panels in "Dach_db_participants_02"
        | CompetitionName | Name     | Comment        |
        | Test-Comp-02    | Panel 02 | Just one Panel |
    And following Adjudicators in "Dach_db_participants_02"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name              | Comment |
        | Test-Comp-02    | Panel 02             | 02-01        | Adjudicator 02-01 | 02-01   |
        | Test-Comp-02    | Panel 02             | 02-02        | Adjudicator 02-02 | 02-02   |
        | Test-Comp-02    | Panel 02             | 02-03        | Adjudicator 02-03 | 02-03   |
        | Test-Comp-02    | Panel 02             | 02-04        | Adjudicator 02-04 | 02-04   |
        | Test-Comp-02    | Panel 02             | 02-05        | Adjudicator 02-05 | 02-05   |
    And following Competition Classes in "Dach_db_participants_02"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-02    | Panel 02             | 1       | Class-01   | Allg. Sta A          | Allg. Sta S                  | Sta        | Allg     | 0        | A     | 10                    | 1800                  | 100            | 10            |
        | Test-Comp-02    | Panel 02             | 1       | Class-02   | Allg. Sta S          |                              | Sta        | Allg     | 0        | S     | 10                    | 9999                  | 100            | 10            |
    And following Participants in "Dach_db_participants_02"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub |
        | Test-Comp-02    | Allg. Sta A          | 10          | Dancer 01-A | 5          | Dancer 01-B | 6          | Test-Club-01 | 10        |
        | Test-Comp-02    | Allg. Sta S          | 11          | Dancer 02-A | 7          | Dancer 02-B | 8          | Test-Club-01 | 10        |
    # Comps
    Then following Competitions exists in "Dach_db_participants_02"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv        | ÖTSV-02          | Just another test |
    # Test-Comp-01
    And following Competition Classes exists in "Dach_db_participants_02"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg. Sta D          | Allg. Sta C                  | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg. Sta C          |                              | Sta        | Allg     | 0        | C     | 10                    | 1800                  | 100            | 10            |
    And following Participants exists in "Dach_db_participants_02"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA        | OrgIdPartA | NamePartB       | OrgIdPartB | ClubName     | OrgIdClub |
        | Test-Comp-01    | Allg. Sta D          | 1           | Michael Tscherny | 1          | Margot Tscherny | 2          | Test-Club-01 | 10        |
    # Test-Comp-02
    And following Competition Classes exists in "Dach_db_participants_02"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-02    | Panel 02             | 1       | Class-01   | Allg. Sta A          | Allg. Sta S                  | Sta        | Allg     | 0        | A     | 10                    | 1800                  | 100            | 10            |
        | Test-Comp-02    | Panel 02             | 1       | Class-02   | Allg. Sta S          |                              | Sta        | Allg     | 0        | S     | 10                    | 9999                  | 100            | 10            |
    And following Participants exists in "Dach_db_participants_02"
        | CompetitionName | CompetitionClassName | StartNumber | NamePartA   | OrgIdPartA | NamePartB   | OrgIdPartB | ClubName     | OrgIdClub |
        | Test-Comp-02    | Allg. Sta A          | 10          | Dancer 01-A | 5          | Dancer 01-B | 6          | Test-Club-01 | 10        |
        | Test-Comp-02    | Allg. Sta S          | 11          | Dancer 02-A | 7          | Dancer 02-B | 8          | Test-Club-01 | 10        |
    
