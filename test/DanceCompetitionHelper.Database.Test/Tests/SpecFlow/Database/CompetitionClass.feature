Feature: Competition Class

A short summary of the feature

Scenario: Single Competition Class
    Given following DanceComp-DB "Dach_db_compClass_01"
    And following Competitions in "Dach_db_compClass_01"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Competition Classes in "Dach_db_compClass_01"
        | CompetitionName | Version | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | 1       | Clas-01    | STA        | Allg     | 0        | D     | 10                    | 900                   |
    Then following Competitions exists in "Dach_db_compClass_01"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Competition Classes exists in "Dach_db_compClass_01"
        | CompetitionName | Version | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | 1       | Clas-01    | STA        | Allg     | 0        | D     | 10                    | 900                   |
    
Scenario: Multiple Competition Classes
    Given following DanceComp-DB "Dach_db_compClass_02"
    And following Competitions in "Dach_db_compClass_02"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv         | ÖTSV-02          | Just another test |
    And following Competition Classes in "Dach_db_compClass_02"
        | CompetitionName | Version | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | 1       | Clas-01    | STA        | Allg     | 0        | D     | 10                    | 900                   |
        | Test-Comp-01    | 1       | Clas-02    | STA        | Allg     | 0        | C     | 10                    | 1800                  |
    And following Competition Classes in "Dach_db_compClass_02"
        | CompetitionName | Version | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-02    | 1       | Clas-01    | STA        | Allg     | 0        | A     | 10                    | 1800                  |
        | Test-Comp-02    | 1       | Clas-02    | STA        | Allg     | 0        | S     | 10                    | 9999                  |
    Then following Competitions exists in "Dach_db_compClass_02"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv         | ÖTSV-02          | Just another test |
    And following Competition Classes exists in "Dach_db_compClass_02"
        | CompetitionName | Version | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-01    | 1       | Clas-01    | STA        | Allg     | 0        | D     | 10                    | 900                   |
        | Test-Comp-01    | 1       | Clas-02    | STA        | Allg     | 0        | C     | 10                    | 1800                  |
    And following Competition Classes exists in "Dach_db_compClass_02"
        | CompetitionName | Version | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion |
        | Test-Comp-02    | 1       | Clas-01    | STA        | Allg     | 0        | A     | 10                    | 1800                  |
        | Test-Comp-02    | 1       | Clas-02    | STA        | Allg     | 0        | S     | 10                    | 9999                  |
    
