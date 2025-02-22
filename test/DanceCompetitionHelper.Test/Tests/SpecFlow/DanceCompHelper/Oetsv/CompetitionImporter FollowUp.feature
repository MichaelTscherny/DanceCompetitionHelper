Feature: DanceCompHelper - CompetitionImporter - Basics - FollowUp

A short summary of the feature

Scenario: Import 01
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                | ParticipantsFile                                                | FindFollowUpClasses |
        | Oetsv        | 6451             | TestData\\Importer\\Oetsv\\CompetitionImport01_Competition.csv | TestData\\Importer\\Oetsv\\CompetitionImport01_Participants.csv | true                |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | Organization | OrgCompetitionId | CompetitionInfo                        | CompetitionDate |
        | Hans-Rueff-Gedächtnispokal | Oetsv        | 6451             | Bewertungsturnier HdB Floridsdorf Wien | 2013-05-22      |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion | PointsForFirst | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta D       | Allg.Kl. Sta C               | 01         | Sta        | Allg     | 0        | D     | 900                   | 10                    | 100            | 1                 | 0                  |                        |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta C       | Allg.Kl. Sta B               | 02         | Sta        | Allg     | 0        | C     | 1500                  | 10                    | 100            | 1                 | 1                  | Allg.Kl. Sta D (01)    |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. La B        | Allg.Kl. La A                | 03         | La         | Allg     | 0        | B     | 1300                  | 10                    | 100            | 0                 | 0                  |                        |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta B       |                              | 04         | Sta        | Allg     | 0        | B     | 1300                  | 10                    | 100            | 0                 | 1                  | Allg.Kl. Sta C (02)    |
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. La A        |                              | 05         | La         | Allg     | 0        | A     | 1600                  | 10                    | 100            | 0                 | 0                  |                        |
    And following Competition Venues exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | Name       | Comment                             |
        | Hans-Rueff-Gedächtnispokal | Main Floor | Created by OetsvCompetitionImporter |
    And following Participants exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            | CompetitionClassName | StartNumber | NamePartA   | NamePartB   | PossiblePromotionAInfo               |
        #                                                                                  
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta D       | 1           | John Doe    | Jane Doe    | [A] 594/13 + 300/3 = 894/16 -> False |
        #                                                                               
        | Hans-Rueff-Gedächtnispokal | Allg.Kl. Sta C       | 2           | John Summer | Jane Summer | [A] 123/4 + 200/2 = 323/6 -> False   |
    And none multiple starts exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName            |
        | Hans-Rueff-Gedächtnispokal |

Scenario: Import 02
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                | ParticipantsFile                                                | FindFollowUpClasses |
        | Oetsv        | 2142             | TestData\\Importer\\Oetsv\\CompetitionImport02_Competition.csv | TestData\\Importer\\Oetsv\\CompetitionImport02_Participants.csv | true                |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | Organization | OrgCompetitionId | CompetitionInfo                  | CompetitionDate |
        | Small Tournament | Oetsv        | 2142             | Landesmeisterschaft Unknown Wien | 2013-05-30      |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | CompetitionClassName | FollowUpCompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion | PointsForFirst | CountParticipants | ExtraPartByWinning | ExtraPartByWinningInfo | ExtraPartByPromotion | ExtraPartByPromotionInfo |
        | Small Tournament | Allg.Kl. Sta D       | Allg.Kl. Sta C               | 01         | Sta        | Allg     | 0        | D     | 900                   | 10                    | 150            | 2                 | 0                  |                        | 0                    |                          |
        | Small Tournament | Allg.Kl. Sta C       | Allg.Kl. Sta B               | 02         | Sta        | Allg     | 0        | C     | 1500                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta D (01)    | 1                    | #102                     |
        | Small Tournament | Allg.Kl. Sta B       | Allg.Kl. Sta A               | 03         | Sta        | Allg     | 0        | B     | 1300                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta C (02)    | 0                    |                          |
        | Small Tournament | Allg.Kl. Sta A       | Allg.Kl. Sta S               | 04         | Sta        | Allg     | 0        | A     | 1600                  | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta B (03)    | 0                    |                          |
        | Small Tournament | Allg.Kl. Sta S       |                              | 05         | Sta        | Allg     | 0        | S     | 999999999             | 10                    | 150            | 2                 | 1                  | Allg.Kl. Sta A (04)    | 0                    |                          |
    And following Competition Venues exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName  | Name       | Comment                             |
        | Small Tournament | Main Floor | Created by OetsvCompetitionImporter |
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


# #1609 -> test import and follow up
# Jun. I La D (014) 	Jun. I La C (015)
# Jun. I La C (015) 	Jun. II La B (019) 
#
