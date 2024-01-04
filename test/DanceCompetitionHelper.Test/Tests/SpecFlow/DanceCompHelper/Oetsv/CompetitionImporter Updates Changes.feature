Feature: DanceCompHelper - CompetitionImporter - Updates Changes

A short summary of the feature

Scenario: Update 03 - Changed Points and Starts
    Given following DanceCompetitionHelper "DanceCompHelper"
    # IMPORT 01
    And following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                                | ParticipantsFile                                                                | FindFollowUpClasses |
        | Oetsv        | 6451             | TestData\\Importer\\Oetsv\\Updates\\03\\CompetitionImport03_01_Competition.csv | TestData\\Importer\\Oetsv\\Updates\\03\\CompetitionImport03_01_Participants.csv | true                |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | Organization | OrgCompetitionId | CompetitionInfo                  | CompetitionDate |
        | Small Tournament | Oetsv        | 2142             | Landesmeisterschaft Unknown Wien | 2013-05-30      |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion | PointsForFirst | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo | ExtraPartByPromotion | ExtraPartByPromotionInfo |
        | Small Tournament | Allg.Kl. Sta D       | Allg.Kl. Sta C               | 001        | Sta        | Allg     | 0        | D     | 900                   | 10                    | 150            | 2                 | 0                  |                        | 0                    |                          |
        | Small Tournament | Allg.Kl. Sta C       | Allg.Kl. Sta B               | 002        | Sta        | Allg     | 0        | C     | 1500                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta D (001)   | 1                    | #102                     |
        | Small Tournament | Allg.Kl. Sta B       | Allg.Kl. Sta A               | 003        | Sta        | Allg     | 0        | B     | 1300                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta C (002)   | 0                    |                          |
        | Small Tournament | Allg.Kl. Sta A       | Allg.Kl. Sta S               | 004        | Sta        | Allg     | 0        | A     | 1600                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta B (003)   | 0                    |                          |
        | Small Tournament | Allg.Kl. Sta S       |                              | 005        | Sta        | Allg     | 0        | S     | 999999999             | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta A (004)   | 0                    |                          |
    And following Participants exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | StartNumber | NamePartA   | NamePartB   | PossiblePromotionA | PossiblePromotionAInfo                 |
        #                                                                                  
        | Small Tournament | Allg.Kl. Sta D       | 101         | Dancer-A 01 | Dancer-B 01 | False              | [A] 152/4 + 750/5 = 902/9 -> False     |
        | Small Tournament | Allg.Kl. Sta D       | 102         | Dancer-A 02 | Dancer-B 02 | True               | [A] 500/11 + 450/3 = 950/14 -> True    |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta C       | 201         | Dancer-A 03 | Dancer-B 03 | False              | [A] 333/3 + 600/4 = 933/7 -> False     |
        | Small Tournament | Allg.Kl. Sta C       | 202         | Dancer-A 04 | Dancer-B 04 | False              | [A] 666/6 + 600/4 = 1266/10 -> False   |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta B       | 301         | Dancer-A 05 | Dancer-B 05 | False              | [A] 123/4 + 450/3 = 573/7 -> False     |
        | Small Tournament | Allg.Kl. Sta B       | 302         | Dancer-A 06 | Dancer-B 06 | False              | [A] 567/8 + 450/3 = 1017/11 -> False   |
        #
        | Small Tournament | Allg.Kl. Sta A       | 401         | Dancer-A 07 | Dancer-B 07 | False              | [A] 555/10 + 300/2 = 855/12 -> False   |
        | Small Tournament | Allg.Kl. Sta A       | 402         | Dancer-A 08 | Dancer-B 08 | False              | [A] 666/11 + 300/2 = 966/13 -> False   |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta S       | 501         | Dancer-A 09 | Dancer-B 09 | False              | [A] 1268/36 + 150/1 = 1418/37 -> False |
        | Small Tournament | Allg.Kl. Sta S       | 502         | Dancer-A 10 | Dancer-B 10 | False              | [A] 500/11 + 150/1 = 650/12 -> False   |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  |
        | Small Tournament |
    # IMPORT 02
    Given following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                                | ParticipantsFile                                                                | FindFollowUpClasses |
        | Oetsv        | 6451             | TestData\\Importer\\Oetsv\\Updates\\03\\CompetitionImport03_02_Competition.csv | TestData\\Importer\\Oetsv\\Updates\\03\\CompetitionImport03_02_Participants.csv | false               |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | Organization | OrgCompetitionId | CompetitionInfo                  | CompetitionDate |
        | Small Tournament | Oetsv        | 2142             | Landesmeisterschaft Unknown Wien | 2013-05-30      |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion | PointsForFirst | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo | ExtraPartByPromotion | ExtraPartByPromotionInfo |
        | Small Tournament | Allg.Kl. Sta D       | Allg.Kl. Sta C               | 001        | Sta        | Allg     | 0        | D     | 900                   | 10                    | 150            | 2                 | 0                  |                        | 0                    |                          |
        | Small Tournament | Allg.Kl. Sta C       | Allg.Kl. Sta B               | 002        | Sta        | Allg     | 0        | C     | 1500                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta D (001)   | 2                    | #101, #102               |
        | Small Tournament | Allg.Kl. Sta B       | Allg.Kl. Sta A               | 003        | Sta        | Allg     | 0        | B     | 1300                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta C (002)   | 0                    |                          |
        | Small Tournament | Allg.Kl. Sta A       | Allg.Kl. Sta S               | 004        | Sta        | Allg     | 0        | A     | 1600                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta B (003)   | 1                    | #302                     |
        | Small Tournament | Allg.Kl. Sta S       |                              | 005        | Sta        | Allg     | 0        | S     | 999999999             | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta A (004)   | 0                    |                          |
    And following Participants exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | StartNumber | NamePartA   | NamePartB   | PossiblePromotionA | PossiblePromotionAInfo                 |
        #                                                                                  
        | Small Tournament | Allg.Kl. Sta D       | 101         | Dancer-A 01 | Dancer-B 01 | True               | [A] 300/5 + 750/5 = 1050/10 -> True    |
        | Small Tournament | Allg.Kl. Sta D       | 102         | Dancer-A 02 | Dancer-B 02 | True               | [A] 600/12 + 300/2 = 900/14 -> True    |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta C       | 201         | Dancer-A 03 | Dancer-B 03 | False              | [A] 444/4 + 600/4 = 1044/8 -> False    |
        | Small Tournament | Allg.Kl. Sta C       | 202         | Dancer-A 04 | Dancer-B 04 | False              | [A] 777/7 + 600/4 = 1377/11 -> False   |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta B       | 301         | Dancer-A 05 | Dancer-B 05 | False              | [A] 456/4 + 450/3 = 906/7 -> False     |
        | Small Tournament | Allg.Kl. Sta B       | 302         | Dancer-A 06 | Dancer-B 06 | True               | [A] 890/8 + 450/3 = 1340/11 -> True    |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta A       | 401         | Dancer-A 07 | Dancer-B 07 | False              | [A] 666/12 + 300/2 = 966/14 -> False   |
        | Small Tournament | Allg.Kl. Sta A       | 402         | Dancer-A 08 | Dancer-B 08 | False              | [A] 777/14 + 300/2 = 1077/16 -> False  |
        #                                                                               
        | Small Tournament | Allg.Kl. Sta S       | 501         | Dancer-A 09 | Dancer-B 09 | False              | [A] 2379/37 + 150/1 = 2529/38 -> False |
        | Small Tournament | Allg.Kl. Sta S       | 502         | Dancer-A 10 | Dancer-B 10 | False              | [A] 600/12 + 150/1 = 750/13 -> False   |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  |
        | Small Tournament |