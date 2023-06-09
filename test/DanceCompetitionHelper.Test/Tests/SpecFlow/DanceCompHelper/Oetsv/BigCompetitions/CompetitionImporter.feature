Feature: DanceCompHelper - CompetitionImporter - Big Competitions

A short summary of the feature

Scenario: Import 01 - Competition only
    Given following DanceCompetitionHelper "DanceCompHelper"
    And following data are imported by DanceCompetitionHelper "DanceCompHelper"
        | Organization | OrgCompetitionId | CompetitionFile                                                | ParticipantsFile                                                |
        | Oetsv        | 1547             | TestData\\Importer\\Oetsv\\CompetitionImport03_Competition.csv | TestData\\Importer\\Oetsv\\CompetitionImport03_Participants.csv |
    Then following Competitions exists in DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | Organization | OrgCompetitionId | CompetitionInfo                                      | CompetitionDate |
        | Gänserndorf Cup | Oetsv        | 1547             | Bewertungsturnier Stadthalle Gänserndorf Gänserndorf | 2023-6-24       |
    And following Classes exists in Competitions of DanceCompetitionHelper "DanceCompHelper"
        | CompetitionName | CompetitionClassName                   | OrgClassId | Discipline | AgeClass | AgeGroup | Class | MinPointsForPromotion | MinStartsForPromotion |
        | Gänserndorf Cup | Sch. Kombi BSP (LW, TG, CC, JI)        | 1          | Ko         | Sch      | 1        | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Sch. Sta D                             | 2          | Sta        | Sch      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Sch. Sta C                             | 3          | Sta        | Sch      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Sch. Sta B                             | 4          | Sta        | Sch      | 1        | B     | 999999999             | 10                    |
        # 
        | Gänserndorf Cup | Sch. La D                              | 5          | La         | Sch      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Sch. La C                              | 6          | La         | Sch      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Sch. La B                              | 7          | La         | Sch      | 1        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jun. I + II Kombi BSP (LW, TG, CC, JI) | 8          | Ko         | Jun      | 2        | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. I Sta D                           | 9          | Sta        | Jun      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. I Sta C                           | 10         | Sta        | Jun      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. I Sta B                           | 11         | Sta        | Jun      | 1        | B     | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. II Sta D                          | 12         | Sta        | Jun      | 2        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. II Sta C                          | 13         | Sta        | Jun      | 2        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. II Sta B                          | 14         | Sta        | Jun      | 2        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jun. I La D                            | 15         | La         | Jun      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. I La C                            | 16         | La         | Jun      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. I La B                            | 17         | La         | Jun      | 1        | B     | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. II La D                           | 18         | La         | Jun      | 2        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. II La C                           | 19         | La         | Jun      | 2        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. II La B                           | 20         | La         | Jun      | 2        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jug. Sta D                             | 21         | Sta        | Jug      |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Jug. Sta C                             | 22         | Sta        | Jug      |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Jug. Sta B                             | 23         | Sta        | Jug      |          | B     | 1000                  | 10                    |
        | Gänserndorf Cup | Jug. Sta A                             | 24         | Sta        | Jug      |          | A     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jug. La D                              | 25         | La         | Jug      |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Jug. La C                              | 26         | La         | Jug      |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Jug. La B                              | 27         | La         | Jug      |          | B     | 1000                  | 10                    |
        | Gänserndorf Cup | Jug. La A                              | 28         | La         | Jug      |          | A     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Allg.Kl. Sta D                         | 29         | Sta        | Allg     |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta C                         | 30         | Sta        | Allg     |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta B                         | 31         | Sta        | Allg     |          | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta A                         | 32         | Sta        | Allg     |          | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta S                         | 33         | Sta        | Allg     |          | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Allg.Kl. La D                          | 34         | La         | Allg     |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Allg.Kl. La C                          | 35         | La         | Allg     |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La B                          | 36         | La         | Allg     |          | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La A                          | 37         | La         | Allg     |          | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La S                          | 38         | La         | Allg     |          | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. I Sta D                           | 39         | Sta        | Sen      | 1        | D     | 900                   | 10                    |
        | Gänserndorf Cup | Sen. I Sta C                           | 40         | Sta        | Sen      | 1        | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta B                           | 41         | Sta        | Sen      | 1        | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta A                           | 42         | Sta        | Sen      | 1        | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta S                           | 43         | Sta        | Sen      | 1        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. II Sta D                          | 44         | Sta        | Sen      | 2        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta C                          | 45         | Sta        | Sen      | 2        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta B                          | 46         | Sta        | Sen      | 2        | B     | 2000                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta A                          | 47         | Sta        | Sen      | 2        | A     | 2600                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta S                          | 48         | Sta        | Sen      | 2        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. III Sta D                         | 49         | Sta        | Sen      | 3        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta C                         | 50         | Sta        | Sen      | 3        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta B                         | 51         | Sta        | Sen      | 3        | B     | 2000                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta A                         | 52         | Sta        | Sen      | 3        | A     | 2600                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta S                         | 53         | Sta        | Sen      | 3        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. I La D                            | 54         | La         | Sen      | 1        | D     | 900                   | 10                    |
        | Gänserndorf Cup | Sen. I La C                            | 55         | La         | Sen      | 1        | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Sen. I La B                            | 56         | La         | Sen      | 1        | B     | 1800                  | 10                    |
        | Gänserndorf Cup | Sen. I La S                            | 57         | La         | Sen      | 1        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. II La D                           | 58         | La         | Sen      | 2        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. II La C                           | 59         | La         | Sen      | 2        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. II La B                           | 60         | La         | Sen      | 2        | B     | 2800                  | 10                    |
        | Gänserndorf Cup | Sen. II La S                           | 61         | La         | Sen      | 2        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. III La D                          | 62         | La         | Sen      | 3        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. III La C                          | 63         | La         | Sen      | 3        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. III La B                          | 64         | La         | Sen      | 3        | B     | 2800                  | 10                    |
        | Gänserndorf Cup | Sen. III La S                          | 65         | La         | Sen      | 3        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Single o.Startbuch (SA, CC, JI)        | 66         | La         | Offen    |          | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Single o.Startbuch (LW, TA, WW)        | 67         | Sta        | Offen    |          | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Single m.Startbuch (SA, CC, JI)        | 68         | La         | Offen    |          | D     | 999999999             | 10                    |
        | Gänserndorf Cup | Single m.Startbuch (LW, TA, WW)        | 69         | Sta        | Offen    |          | D     | 999999999             | 10                    |
