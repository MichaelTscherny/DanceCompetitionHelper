Feature: DanceCompHelper - CompetitionImporter - Basics - No FollowUp

A short summary of the feature

Scenario: Import 01
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                | ParticipantsFile                                                | FindFollowUpClasses |
        | Oetsv        | 6451             | TestData\\Importer\\Oetsv\\CompetitionImport01_Competition.csv | TestData\\Importer\\Oetsv\\CompetitionImport01_Participants.csv | False               |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | Organization | OrgCompetitionId | CompetitionInfo                        | CompetitionDate |
        | Hans-Rueff-Gedächtnispokal | Oetsv        | 6451             | Bewertungsturnier HdB Floridsdorf Wien | 2013-05-22      |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion | PointsForFirst | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta D       |                              | 001        | Sta        | Allg     | 0        | D     | 900                   | 10                    | 100            | 1                 | 0                  |                        |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta C       |                              | 002        | Sta        | Allg     | 0        | C     | 1500                  | 10                    | 100            | 1                 | 0                  |                        |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. La B        |                              | 003        | La         | Allg     | 0        | B     | 1300                  | 10                    | 100            | 0                 | 0                  |                        |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta B       |                              | 004        | Sta        | Allg     | 0        | B     | 1300                  | 10                    | 100            | 0                 | 0                  |                        |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. La A        |                              | 005        | La         | Allg     | 0        | A     | 1600                  | 10                    | 100            | 0                 | 0                  |                        |
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
    And following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                | ParticipantsFile                                                | FindFollowUpClasses |
        | Oetsv        | 2142             | TestData\\Importer\\Oetsv\\CompetitionImport02_Competition.csv | TestData\\Importer\\Oetsv\\CompetitionImport02_Participants.csv | False               |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | Organization | OrgCompetitionId | CompetitionInfo                  | CompetitionDate |
        | Small Tournament | Oetsv        | 2142             | Landesmeisterschaft Unknown Wien | 2013-05-30      |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion | PointsForFirst | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo |
        | Small Tournament | Allg.Kl. Sta D       |                              | 001        | Sta        | Allg     | 0        | D     | 900                   | 10                    | 150            | 2                 | 0                  |                        |
        | Small Tournament | Allg.Kl. Sta C       |                              | 002        | Sta        | Allg     | 0        | C     | 1500                  | 10                    | 150            | 2                 | 0                  |                        |
        | Small Tournament | Allg.Kl. Sta B       |                              | 003        | Sta        | Allg     | 0        | B     | 1300                  | 10                    | 150            | 2                 | 0                  |                        |
        | Small Tournament | Allg.Kl. Sta A       |                              | 004        | Sta        | Allg     | 0        | A     | 1600                  | 10                    | 150            | 2                 | 0                  |                        |
        | Small Tournament | Allg.Kl. Sta S       |                              | 005        | Sta        | Allg     | 0        | S     | 999999999             | 10                    | 150            | 2                 | 0                  |                        |
    And following Participants exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | StartNumber | NamePartA   | NamePartB   | PossiblePromotionAInfo                 |
        #                                                                                  
        | Small Tournament | Allg.Kl. Sta D       | 101         | Dancer-A 01 | Dancer-B 01 | [A] 152/4 + 150/1 = 302/5 -> False     |
        | Small Tournament | Allg.Kl. Sta D       | 102         | Dancer-A 02 | Dancer-B 02 | [A] 500/11 + 150/1 = 650/12 -> False   |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta C       | 201         | Dancer-A 03 | Dancer-B 03 | [A] 333/3 + 150/1 = 483/4 -> False     |
        | Small Tournament | Allg.Kl. Sta C       | 202         | Dancer-A 04 | Dancer-B 04 | [A] 666/6 + 150/1 = 816/7 -> False     |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta B       | 301         | Dancer-A 05 | Dancer-B 05 | [A] 123/4 + 150/1 = 273/5 -> False     |
        | Small Tournament | Allg.Kl. Sta B       | 302         | Dancer-A 06 | Dancer-B 06 | [A] 567/8 + 150/1 = 717/9 -> False     |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta A       | 401         | Dancer-A 07 | Dancer-B 07 | [A] 555/10 + 150/1 = 705/11 -> False   |
        | Small Tournament | Allg.Kl. Sta A       | 402         | Dancer-A 08 | Dancer-B 08 | [A] 666/11 + 150/1 = 816/12 -> False   |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta S       | 501         | Dancer-A 09 | Dancer-B 09 | [A] 1268/36 + 150/1 = 1418/37 -> False |
        | Small Tournament | Allg.Kl. Sta S       | 502         | Dancer-A 10 | Dancer-B 10 | [A] 500/11 + 150/1 = 650/12 -> False   |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  |
        | Small Tournament |
