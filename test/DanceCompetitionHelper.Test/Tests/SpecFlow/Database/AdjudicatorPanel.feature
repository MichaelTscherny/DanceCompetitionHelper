Feature: Adjudicator Panel

A short summary of the feature

Scenario: Single Adjudicator Panel
    Given following DanceComp-DB "Dach_db_adjPanel_01"
    And following Competitions in "Dach_db_adjPanel_01"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Adjudicator Panels in "Dach_db_adjPanel_01"
        | CompetitionName | Name     | Comment        |
        | Test-Comp-01    | Panel 01 | Just one Panel |
    Then following Competitions exists in "Dach_db_adjPanel_01"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test     |
    And following Adjudicator Panels exists in "Dach_db_adjPanel_01"
        | CompetitionName | Name     | Comment        |
        | Test-Comp-01    | Panel 01 | Just one Panel |
    
Scenario: Multiple Adjudicator Panels
    Given following DanceComp-DB "Dach_db_adjPanel_02"
    And following Competitions in "Dach_db_adjPanel_02"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv         | ÖTSV-02          | Just another test |
    And following Adjudicator Panels in "Dach_db_adjPanel_02"
        | CompetitionName | Name        | Comment              |
        | Test-Comp-01    | Panel 01-01 | Just one Panel       |
        | Test-Comp-01    | Panel 01-02 | Next Panel           |
        | Test-Comp-02    | Panel 02-01 | Panel of other class |
    Then following Competitions exists in "Dach_db_adjPanel_02"
        | CompetitionName | Origanization | OrgCompetitionId | CompetitionInfo   |
        | Test-Comp-01    | Oetsv         | ÖTSV-01          | Just a test       |
        | Test-Comp-02    | Oetsv         | ÖTSV-02          | Just another test |
    And following Adjudicator Panels exists in "Dach_db_adjPanel_02"
        | CompetitionName | Name        | Comment              |
        | Test-Comp-01    | Panel 01-01 | Just one Panel       |
        | Test-Comp-01    | Panel 01-02 | Next Panel           |
        | Test-Comp-02    | Panel 02-01 | Panel of other class |
    
