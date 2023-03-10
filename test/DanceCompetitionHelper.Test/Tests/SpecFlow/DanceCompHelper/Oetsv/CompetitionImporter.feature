Feature: DanceCompHelper - CompetitionImporter - Basics

A short summary of the feature

Scenario: Import 01
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following data is imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                | ParticipantsFile                                                |
        | Oetsv        | 6451             | TestData\\Importer\\Oetsv\\CompetitionImport01_Competition.csv | TestData\\Importer\\Oetsv\\CompetitionImport01_Participants.csv |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | Organization | OrgCompetitionId | CompetitionInfo                        | CompetitionDate |
        | Hans-Rueff-Gedächtnispokal | Oetsv        | 6451             | Bewertungsturnier HdB Floridsdorf Wien | 2013-05-22      |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | CompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion | PointsForFirst | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta D       | 1          | Sta        | Allg     | 0        | D     | 900                   | 10                    | 100            | 1                 | 0                  |                        |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta C       | 2          | Sta        | Allg     | 0        | C     | 1500                  | 10                    | 100            | 1                 | 1                  | Allg.Kl. Sta D (1)     |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. La B        | 3          | La         | Allg     | 0        | B     | 1300                  | 10                    | 100            | 0                 | 0                  |                        |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta B       | 4          | Sta        | Allg     | 0        | B     | 1300                  | 10                    | 100            | 0                 | 1                  | Allg.Kl. Sta C (2)     |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. La A        | 5          | La         | Allg     | 0        | A     | 1600                  | 10                    | 100            | 0                 | 1                  | Allg.Kl. La B (3)      |
    And following Participants exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | CompetitionClassName | StartNumber | NamePartA   | NamePartB   | PossiblePromotionAInfo               |
        #                                                                                  
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta D       | 1           | John Doe    | Jane Doe    | [A] 594/13 + 100/1 = 694/14 -> False |
        #                                                                               
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta C       | 2           | John Summer | Jane Summer | [A] 123/4 + 100/1 = 223/5 -> False   |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            |
        | Hans-Rueff-Gedächtnispokal |

Scenario: Import 02
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following data is imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                | ParticipantsFile                                                |
        | Oetsv        | 2142             | TestData\\Importer\\Oetsv\\CompetitionImport02_Competition.csv | TestData\\Importer\\Oetsv\\CompetitionImport02_Participants.csv |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | Organization | OrgCompetitionId | CompetitionInfo                  | CompetitionDate |
        | Small Tournament | Oetsv        | 2142             | Landesmeisterschaft Unknown Wien | 2013-05-30      |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion | PointsForFirst | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo |
        | Small Tournament | Allg.Kl. Sta D       | 1          | Sta        | Allg     | 0        | D     | 900                   | 10                    | 150            | 2                 | 0                  |                        |
        | Small Tournament | Allg.Kl. Sta C       | 2          | Sta        | Allg     | 0        | C     | 1500                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta D (1)     |
        | Small Tournament | Allg.Kl. Sta B       | 3          | Sta        | Allg     | 0        | B     | 1300                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta C (2)     |
        | Small Tournament | Allg.Kl. Sta A       | 4          | Sta        | Allg     | 0        | A     | 1600                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta B (3)     |
        | Small Tournament | Allg.Kl. Sta S       | 5          | Sta        | Allg     | 0        | S     | 999999999             | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta A (4)     |
    And following Participants exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | StartNumber | NamePartA   | NamePartB   | PossiblePromotionAInfo             |
        #                                                                                  
        | Small Tournament | Allg.Kl. Sta D       | 101         | Dancer-A 01 | Dancer-B 01 | [A] 152/4 + 150/1 = 302/5 -> False |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta C       | 201         | Dancer-A 03 | Dancer-B 03 | [A] 333/3 + 150/1 = 483/4 -> False |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  |
        | Small Tournament |

