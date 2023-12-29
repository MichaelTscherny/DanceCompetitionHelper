Feature: Competition Class

A short summary of the feature

Scenario: Single Competition Class
    Given following DanceComp-DB "Dach_db_compClass_01"
    And following Competitions in "Dach_db_compClass_01"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
    And following Adjudicator Panels in "Dach_db_compClass_01"
        | CompetitionName | Name     | Comment        |
        | Test-Comp-01    | Panel 01 | Just one Panel |
    And following Adjudicators in "Dach_db_compClass_01"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name              | Comment |
        | Test-Comp-01    | Panel 01             | 01-01        | Adjudicator 01-01 | 01-01   |
        | Test-Comp-01    | Panel 01             | 01-02        | Adjudicator 01-02 | 01-02   |
        | Test-Comp-01    | Panel 01             | 01-03        | Adjudicator 01-03 | 01-03   |
        | Test-Comp-01    | Panel 01             | 01-04        | Adjudicator 01-04 | 01-04   |
        | Test-Comp-01    | Panel 01             | 01-05        | Adjudicator 01-05 | 01-05   |
    And following Competition Classes in "Dach_db_compClass_01"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg. Sta D          | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
    # #######
    Then following Competitions exists in "Dach_db_compClass_01"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
    And following Competition Classes exists in "Dach_db_compClass_01"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg. Sta D          | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
    
Scenario: Multiple Competition Classes
    Given following DanceComp-DB "Dach_db_compClass_02"
    And following Competitions in "Dach_db_compClass_02"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv        | ÖTSV-02          | Just another test |
    And following Adjudicator Panels in "Dach_db_compClass_02"
        | CompetitionName | Name     | Comment            |
        | Test-Comp-01    | Panel 01 | Just one Panel     |
        | Test-Comp-02    | Panel 02 | Just another Panel |
    And following Adjudicators in "Dach_db_compClass_02"
        | CompetitionName | AdjudicatorPanelName | Abbreviation | Name              | Comment |
        # Test-Comp-01
        | Test-Comp-01    | Panel 01             | 01-01        | Adjudicator 01-01 | 01-01   |
        | Test-Comp-01    | Panel 01             | 01-02        | Adjudicator 01-02 | 01-02   |
        | Test-Comp-01    | Panel 01             | 01-03        | Adjudicator 01-03 | 01-03   |
        | Test-Comp-01    | Panel 01             | 01-04        | Adjudicator 01-04 | 01-04   |
        | Test-Comp-01    | Panel 01             | 01-05        | Adjudicator 01-05 | 01-05   |
        # Test-Comp-02
        | Test-Comp-02    | Panel 02             | 02-01        | Adjudicator 02-01 | 02-01   |
        | Test-Comp-02    | Panel 02             | 02-02        | Adjudicator 02-02 | 02-02   |
        | Test-Comp-02    | Panel 02             | 02-03        | Adjudicator 02-03 | 02-03   |
        | Test-Comp-02    | Panel 02             | 02-04        | Adjudicator 02-04 | 02-04   |
        | Test-Comp-02    | Panel 02             | 02-05        | Adjudicator 02-05 | 02-05   |
    And following Competition Classes in "Dach_db_compClass_02"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg. Sta D          | Allg. Sta C                  | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg. Sta C          |                              | Sta        | Allg     | 0        | C     | 10                    | 1800                  | 100            | 10            |
    And following Competition Classes in "Dach_db_compClass_02"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-02    | Panel 02             | 1       | Class-01   | Allg. Sta A          | Allg. Sta S                  | Sta        | Allg     | 0        | A     | 10                    | 1800                  | 100            | 10            |
        | Test-Comp-02    | Panel 02             | 1       | Class-02   | Allg. Sta S          |                              | Sta        | Allg     | 0        | S     | 10                    | 9999                  | 100            | 10            |
    # #######
    Then following Competitions exists in "Dach_db_compClass_02"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv        | ÖTSV-02          | Just another test |
    And following Competition Classes exists in "Dach_db_compClass_02"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg. Sta D          | Allg. Sta C                  | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            | 10            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg. Sta C          |                              | Sta        | Allg     | 0        | C     | 10                    | 1800                  | 100            | 10            |
    And following Competition Classes exists in "Dach_db_compClass_02"
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst | PointsForLast |
        | Test-Comp-02    | Panel 02             | 1       | Class-01   | Allg. Sta A          | Allg. Sta S                  | Sta        | Allg     | 0        | A     | 10                    | 1800                  | 100            | 10            |
        | Test-Comp-02    | Panel 02             | 1       | Class-02   | Allg. Sta S          |                              | Sta        | Allg     | 0        | S     | 10                    | 9999                  | 100            | 10            |
    
