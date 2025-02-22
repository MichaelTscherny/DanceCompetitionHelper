Feature: DanceCompHelper - CompetitionImporter - Big Competitions

A short summary of the feature

Scenario: Import 01 - Competition only
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                | ParticipantsFile                                                | FindFollowUpClasses |
        | Oetsv        | 1547             | TestData\\Importer\\Oetsv\\CompetitionImport03_Competition.csv | TestData\\Importer\\Oetsv\\CompetitionImport03_Participants.csv | True                |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo                                      | CompetitionDate |
        | Gänserndorf Cup | Oetsv        | 1547             | Bewertungsturnier Stadthalle Gänserndorf Gänserndorf | 2023-6-24       |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName                   | FollowUpCompetitionClassName | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion |
        | Gänserndorf Cup | Sch. Kombi BSP (LW, TG, CC, JI)        |                              | 01         | Ko         | Sch      | 1        | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Sch. Sta D                             | Sch. Sta C                   | 02         | Sta        | Sch      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Sch. Sta C                             |                              | 03         | Sta        | Sch      | 1        | C     | 999999999             | 10                    |
        #                                                            
        | Gänserndorf Cup | Sch. La D                              | Sch. La C                    | 05        | La         | Sch      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Sch. La C                              |                              | 06        | La         | Sch      | 1        | C     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jun. I + II Kombi BSP (LW, TG, CC, JI) |                              | 08        | Ko         | Jun      | 2        | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. I Sta D                           | Jun. I Sta C                 | 09        | Sta        | Jun      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. I Sta C                           | Jun. I Sta B                 | 10        | Sta        | Jun      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. I Sta B                           |                              | 11        | Sta        | Jun      | 1        | B     | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. II Sta D                          | Jun. II Sta C                | 12        | Sta        | Jun      | 2        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. II Sta C                          | Jun. II Sta B                | 13        | Sta        | Jun      | 2        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. II Sta B                          |                              | 14        | Sta        | Jun      | 2        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jun. I La D                            | Jun. I La C                  | 15        | La         | Jun      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. I La C                            | Jun. I La B                  | 16        | La         | Jun      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. I La B                            |                              | 17        | La         | Jun      | 1        | B     | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. II La D                           | Jun. II La C                 | 18        | La         | Jun      | 2        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. II La C                           | Jun. II La B                 | 19        | La         | Jun      | 2        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. II La B                           |                              | 20        | La         | Jun      | 2        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jug. Sta D                             | Jug. Sta C                   | 21        | Sta        | Jug      |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Jug. Sta C                             | Jug. Sta B                   | 22        | Sta        | Jug      |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Jug. Sta B                             | Jug. Sta A                   | 23        | Sta        | Jug      |          | B     | 1000                  | 10                    |
        | Gänserndorf Cup | Jug. Sta A                             |                              | 24        | Sta        | Jug      |          | A     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jug. La D                              | Jug. La C                    | 25        | La         | Jug      |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Jug. La C                              | Jug. La B                    | 26        | La         | Jug      |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Jug. La B                              | Jug. La A                    | 27        | La         | Jug      |          | B     | 1000                  | 10                    |
        | Gänserndorf Cup | Jug. La A                              |                              | 28        | La         | Jug      |          | A     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Allg.Kl. Sta D                         | Allg.Kl. Sta C               | 29        | Sta        | Allg     |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta C                         | Allg.Kl. Sta B               | 30        | Sta        | Allg     |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta B                         | Allg.Kl. Sta A               | 31        | Sta        | Allg     |          | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta A                         | Allg.Kl. Sta S               | 32        | Sta        | Allg     |          | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta S                         |                              | 33        | Sta        | Allg     |          | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Allg.Kl. La D                          | Allg.Kl. La C                | 34        | La         | Allg     |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Allg.Kl. La C                          | Allg.Kl. La B                | 35        | La         | Allg     |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La B                          | Allg.Kl. La A                | 36        | La         | Allg     |          | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La A                          | Allg.Kl. La S                | 37        | La         | Allg     |          | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La S                          |                              | 38        | La         | Allg     |          | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. I Sta D                           | Sen. I Sta C                 | 39        | Sta        | Sen      | 1        | D     | 900                   | 10                    |
        | Gänserndorf Cup | Sen. I Sta C                           | Sen. I Sta B                 | 40        | Sta        | Sen      | 1        | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta B                           | Sen. I Sta A                 | 41        | Sta        | Sen      | 1        | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta A                           | Sen. I Sta S                 | 42        | Sta        | Sen      | 1        | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta S                           |                              | 43        | Sta        | Sen      | 1        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. II Sta D                          | Sen. II Sta C                | 44        | Sta        | Sen      | 2        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta C                          | Sen. II Sta B                | 45        | Sta        | Sen      | 2        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta B                          | Sen. II Sta A                | 46        | Sta        | Sen      | 2        | B     | 2000                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta A                          | Sen. II Sta S                | 47        | Sta        | Sen      | 2        | A     | 2600                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta S                          |                              | 48        | Sta        | Sen      | 2        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. III Sta D                         | Sen. III Sta C               | 49        | Sta        | Sen      | 3        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta C                         | Sen. III Sta B               | 50        | Sta        | Sen      | 3        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta B                         | Sen. III Sta A               | 51        | Sta        | Sen      | 3        | B     | 2000                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta A                         | Sen. III Sta S               | 52        | Sta        | Sen      | 3        | A     | 2600                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta S                         |                              | 53        | Sta        | Sen      | 3        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. I La D                            | Sen. I La C                  | 54        | La         | Sen      | 1        | D     | 900                   | 10                    |
        | Gänserndorf Cup | Sen. I La C                            | Sen. I La B                  | 55        | La         | Sen      | 1        | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Sen. I La B                            | Sen. I La S                  | 56        | La         | Sen      | 1        | B     | 1800                  | 10                    |
        | Gänserndorf Cup | Sen. I La S                            |                              | 57        | La         | Sen      | 1        | S     | 999999999             | 10                    |
        #                                                            
        | Gänserndorf Cup | Sen. II La D                           | Sen. II La C                 | 58        | La         | Sen      | 2        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. II La C                           | Sen. II La B                 | 59        | La         | Sen      | 2        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. II La B                           | Sen. II La S                 | 60        | La         | Sen      | 2        | B     | 2800                  | 10                    |
        | Gänserndorf Cup | Sen. II La S                           |                              | 61        | La         | Sen      | 2        | S     | 999999999             | 10                    |
        #                                                            
        | Gänserndorf Cup | Sen. III La D                          | Sen. III La C                | 62        | La         | Sen      | 3        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. III La C                          | Sen. III La B                | 63        | La         | Sen      | 3        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. III La B                          | Sen. III La S                | 64        | La         | Sen      | 3        | B     | 2800                  | 10                    |
        | Gänserndorf Cup | Sen. III La S                          |                              | 65        | La         | Sen      | 3        | S     | 999999999             | 10                    |
        #                                                            
        | Gänserndorf Cup | Single o.Startbuch (SA, CC, JI)        |                              | 66        | La         | Offen    |          | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Single o.Startbuch (LW, TA, WW)        |                              | 67        | Sta        | Offen    |          | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Single m.Startbuch (SA, CC, JI)        |                              | 68        | La         | Offen    |          | D     | 999999999             | 10                    |
        | Gänserndorf Cup | Single m.Startbuch (LW, TA, WW)        |                              | 69        | Sta        | Offen    |          | D     | 999999999             | 10                    |
    And following Competition Venues exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | Name       | Comment                             |
        | Gänserndorf Cup | Main Floor | Created by OetsvCompetitionImporter |
