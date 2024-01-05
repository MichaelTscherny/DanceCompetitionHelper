Feature: Configuration for Oetsv

A short summary of the feature

Scenario: Mandatory Basic Configuration 
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
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
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg Sta D           | Allg Sta C                   | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg Sta C           |                              | Sta        | Allg     | 0        | C     | 10                    | 1200                  | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-03   | Allg La D            | Allg La C                    | La         | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-04   | Allg La C            |                              | La         | Allg     | 0        | C     | 10                    | 1200                  | 100            |
    And following Competition Venues in "DanceCompHelper-db"
        | CompetitionName | Name     | Comment    |
        | Test-Comp-01    | Venue-01 | Main Floor |
    # Basic configuration
    Then following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        |              |                 |                      |                      | MinTimePerDance         |       |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinTimePerDance         | 1:30  |

Scenario: With Organization Configuration
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
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
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg Sta D           | Allg Sta C                   | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg Sta C           |                              | Sta        | Allg     | 0        | C     | 10                    | 1200                  | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-03   | Allg La D            | Allg La C                    | La         | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-04   | Allg La C            |                              | La         | Allg     | 0        | C     | 10                    | 1200                  | 100            |
    And following Competition Venues in "DanceCompHelper-db"
        | CompetitionName | Name     | Comment    |
        | Test-Comp-01    | Venue-01 | Main Floor |
    And following Configuration Values in "DanceCompHelper-db"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        |                 |                      |                      | MaxCouplesPerHeat       | 333   |
    # Basic configuration
    Then following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        |              |                 |                      |                      | MinTimePerDance         |       |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        |                 |                      |                      | MaxCouplesPerHeat       | 333   |
        | Oetsv        |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      |                      | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinTimePerDance         | 1:30  |

Scenario: With Competition Configuration
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
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
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg Sta D           | Allg Sta C                   | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg Sta C           |                              | Sta        | Allg     | 0        | C     | 10                    | 1200                  | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-03   | Allg La D            | Allg La C                    | La         | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-04   | Allg La C            |                              | La         | Allg     | 0        | C     | 10                    | 1200                  | 100            |
    And following Competition Venues in "DanceCompHelper-db"
        | CompetitionName | Name     | Comment    |
        | Test-Comp-01    | Venue-01 | Main Floor |
    And following Configuration Values in "DanceCompHelper-db"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      |                      | MaxCouplesPerHeat       | 333   |
    # Basic configuration
    Then following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        |              |                 |                      |                      | MinTimePerDance         |       |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      |                      | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinTimePerDance         | 1:30  |
        

Scenario: With Competition Class Configuration
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
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
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg Sta D           | Allg Sta C                   | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg Sta C           |                              | Sta        | Allg     | 0        | C     | 10                    | 1200                  | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-03   | Allg La D            | Allg La C                    | La         | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-04   | Allg La C            |                              | La         | Allg     | 0        | C     | 10                    | 1200                  | 100            |
    And following Competition Venues in "DanceCompHelper-db"
        | CompetitionName | Name     | Comment    |
        | Test-Comp-01    | Venue-01 | Main Floor |
    And following Configuration Values in "DanceCompHelper-db"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key               | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MaxCouplesPerHeat | 333   |
    # Basic configuration
    Then following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        |              |                 |                      |                      | MinTimePerDance         |       |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinTimePerDance         | 1:30  |

Scenario: With Competition Venue Configuration
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
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
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg Sta D           | Allg Sta C                   | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg Sta C           |                              | Sta        | Allg     | 0        | C     | 10                    | 1200                  | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-03   | Allg La D            | Allg La C                    | La         | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-04   | Allg La C            |                              | La         | Allg     | 0        | C     | 10                    | 1200                  | 100            |
    And following Competition Venues in "DanceCompHelper-db"
        | CompetitionName | Name     | Comment    |
        | Test-Comp-01    | Venue-01 | Main Floor |
    And following Configuration Values in "DanceCompHelper-db"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key               | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MaxCouplesPerHeat | 333   |
    # Basic configuration
    Then following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        |              |                 |                      |                      | MinTimePerDance         |       |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinTimePerDance         | 1:30  |

Scenario: With Competition Venue Only Configuration
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following Competitions in "DanceCompHelper-db"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo |
        | Test-Comp-01    | Oetsv        | ÖTSV-01          | Just a test     |
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
        | CompetitionName | AdjudicatorPanelName | Version | OrgClassId | CompetitionClassName | FollowUpCompetitionClassName | Discipline | AgeClass | AgeGroup | Class | MinStartsForPromotion | MinPointsForPromotion | PointsForFirst |
        | Test-Comp-01    | Panel 01             | 1       | Class-01   | Allg Sta D           | Allg Sta C                   | Sta        | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-02   | Allg Sta C           |                              | Sta        | Allg     | 0        | C     | 10                    | 1200                  | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-03   | Allg La D            | Allg La C                    | La         | Allg     | 0        | D     | 10                    | 900                   | 100            |
        | Test-Comp-01    | Panel 01             | 1       | Class-04   | Allg La C            |                              | La         | Allg     | 0        | C     | 10                    | 1200                  | 100            |
    And following Competition Venues in "DanceCompHelper-db"
        | CompetitionName | Name     | Comment    |
        | Test-Comp-01    | Venue-01 | Main Floor |
    And following Configuration Values in "DanceCompHelper-db"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key               | Value |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MaxCouplesPerHeat | 333   |
    # Basic configuration
    Then following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        |              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        |              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        |              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        |              |                 |                      |                      | MinTimePerDance         |       |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        |                 |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        |                 |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    |                      |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MaxCouplesPerHeat       | 6     |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           |                      | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    |                      | Venue-01             | MinTimePerDance         | 1:30  |
    And following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
        | Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MaxCouplesPerHeat       | 333   |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinCooldownTimePerRound | 10:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinChangeClothesTime    | 15:00 |
        | Oetsv        | Test-Comp-01    | Allg Sta D           | Venue-01             | MinTimePerDance         | 1:30  |
