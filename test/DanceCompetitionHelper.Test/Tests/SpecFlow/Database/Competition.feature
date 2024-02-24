Feature: Competition

A short summary of the feature

Scenario: Single Competition
    Given following DanceComp-DB "Dach_db_comp_01"
    And following Competitions in "Dach_db_comp_01"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
    Then following Competitions exists in "Dach_db_comp_01"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
    
Scenario: Multiple Competition
    Given following DanceComp-DB "Dach_db_comp_02"
    And following Competitions in "Dach_db_comp_02"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv        | ÖTSV-02          | Just another test |
    Then following Competitions exists in "Dach_db_comp_02"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv        | ÖTSV-02          | Just another test |
    
