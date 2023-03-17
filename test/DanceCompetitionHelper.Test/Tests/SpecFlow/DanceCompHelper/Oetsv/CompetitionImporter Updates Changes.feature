Feature: DanceCompHelper - CompetitionImporter - Updates Changes

A short summary of the feature

Scenario: Update 03 - Changed Points and Starts
    Given following DanceCompetitionHelper "DanceCompHelper"
    # IMPORT 01
    And following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                                | ParticipantsFile                                                                |
        | Oetsv        | 6451             | TestData\\Importer\\Oetsv\\Updates\\03\\CompetitionImport03_01_Competition.csv | TestData\\Importer\\Oetsv\\Updates\\03\\CompetitionImport03_01_Participants.csv |
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
    # IMPORT 02
    Given following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                                | ParticipantsFile                                                                |
        | Oetsv        | 6451             | TestData\\Importer\\Oetsv\\Updates\\03\\CompetitionImport03_02_Competition.csv | TestData\\Importer\\Oetsv\\Updates\\03\\CompetitionImport03_02_Participants.csv |
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
        | CompetitionName  | CompetitionClassName | StartNumber | NamePartA   | NamePartB   | PossiblePromotionAInfo                 |
        #                                                                                  
        | Small Tournament | Allg.Kl. Sta D       | 101         | Dancer-A 01 | Dancer-B 01 | [A] 300/5 + 150/1 = 450/6 -> False     |
        | Small Tournament | Allg.Kl. Sta D       | 102         | Dancer-A 02 | Dancer-B 02 | [A] 600/12 + 150/1 = 750/13 -> False   |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta C       | 201         | Dancer-A 03 | Dancer-B 03 | [A] 444/4 + 150/1 = 594/5 -> False     |
        | Small Tournament | Allg.Kl. Sta C       | 202         | Dancer-A 04 | Dancer-B 04 | [A] 777/7 + 150/1 = 927/8 -> False     |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta B       | 301         | Dancer-A 05 | Dancer-B 05 | [A] 456/4 + 150/1 = 606/5 -> False     |
        | Small Tournament | Allg.Kl. Sta B       | 302         | Dancer-A 06 | Dancer-B 06 | [A] 890/8 + 150/1 = 1040/9 -> False    |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta A       | 401         | Dancer-A 07 | Dancer-B 07 | [A] 666/12 + 150/1 = 816/13 -> False   |
        | Small Tournament | Allg.Kl. Sta A       | 402         | Dancer-A 08 | Dancer-B 08 | [A] 777/14 + 150/1 = 927/15 -> False   |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta S       | 501         | Dancer-A 09 | Dancer-B 09 | [A] 2379/37 + 150/1 = 2529/38 -> False |
        | Small Tournament | Allg.Kl. Sta S       | 502         | Dancer-A 10 | Dancer-B 10 | [A] 600/12 + 150/1 = 750/13 -> False   |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  |
        | Small Tournament |