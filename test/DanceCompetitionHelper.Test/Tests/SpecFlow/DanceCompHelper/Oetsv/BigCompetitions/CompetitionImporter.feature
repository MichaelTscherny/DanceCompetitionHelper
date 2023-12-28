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
        | Gänserndorf Cup | Sch. Kombi BSP (LW, TG, CC, JI)        |                              | 001        | Ko         | Sch      | 1        | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Sch. Sta D                             | Sch. Sta C                   | 002        | Sta        | Sch      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Sch. Sta C                             |                              | 003        | Sta        | Sch      | 1        | C     | 1800                  | 10                    |
        #                                                            
        | Gänserndorf Cup | Sch. La D                              | Sch. La C                    | 005        | La         | Sch      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Sch. La C                              |                              | 006        | La         | Sch      | 1        | C     | 1800                  | 10                    |
        #
        | Gänserndorf Cup | Jun. I + II Kombi BSP (LW, TG, CC, JI) |                              | 008        | Ko         | Jun      | 2        | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. I Sta D                           | Jun. I Sta C                 | 009        | Sta        | Jun      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. I Sta C                           | Jun. I Sta B                 | 010        | Sta        | Jun      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. I Sta B                           |                              | 011        | Sta        | Jun      | 1        | B     | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. II Sta D                          | Jun. II Sta C                | 012        | Sta        | Jun      | 2        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. II Sta C                          | Jun. II Sta B                | 013        | Sta        | Jun      | 2        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. II Sta B                          |                              | 014        | Sta        | Jun      | 2        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jun. I La D                            | Jun. I La C                  | 015        | La         | Jun      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. I La C                            | Jun. I La B                  | 016        | La         | Jun      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. I La B                            |                              | 017        | La         | Jun      | 1        | B     | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. II La D                           | Jun. II La C                 | 018        | La         | Jun      | 2        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. II La C                           | Jun. II La B                 | 019        | La         | Jun      | 2        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. II La B                           |                              | 020        | La         | Jun      | 2        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jug. Sta D                             | Jug. Sta C                   | 021        | Sta        | Jug      |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Jug. Sta C                             | Jug. Sta B                   | 022        | Sta        | Jug      |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Jug. Sta B                             | Jug. Sta A                   | 023        | Sta        | Jug      |          | B     | 1000                  | 10                    |
        | Gänserndorf Cup | Jug. Sta A                             |                              | 024        | Sta        | Jug      |          | A     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jug. La D                              | Jug. La C                    | 025        | La         | Jug      |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Jug. La C                              | Jug. La B                    | 026        | La         | Jug      |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Jug. La B                              | Jug. La A                    | 027        | La         | Jug      |          | B     | 1000                  | 10                    |
        | Gänserndorf Cup | Jug. La A                              |                              | 028        | La         | Jug      |          | A     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Allg.Kl. Sta D                         | Allg.Kl. Sta C               | 029        | Sta        | Allg     |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta C                         | Allg.Kl. Sta B               | 030        | Sta        | Allg     |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta B                         | Allg.Kl. Sta A               | 031        | Sta        | Allg     |          | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta A                         | Allg.Kl. Sta S               | 032        | Sta        | Allg     |          | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta S                         |                              | 033        | Sta        | Allg     |          | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Allg.Kl. La D                          | Allg.Kl. La C                | 034        | La         | Allg     |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Allg.Kl. La C                          | Allg.Kl. La B                | 035        | La         | Allg     |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La B                          | Allg.Kl. La A                | 036        | La         | Allg     |          | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La A                          | Allg.Kl. La S                | 037        | La         | Allg     |          | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La S                          |                              | 038        | La         | Allg     |          | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. I Sta D                           | Sen. I Sta C                 | 039        | Sta        | Sen      | 1        | D     | 900                   | 10                    |
        | Gänserndorf Cup | Sen. I Sta C                           | Sen. I Sta B                 | 040        | Sta        | Sen      | 1        | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta B                           | Sen. I Sta A                 | 041        | Sta        | Sen      | 1        | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta A                           | Sen. I Sta S                 | 042        | Sta        | Sen      | 1        | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta S                           |                              | 043        | Sta        | Sen      | 1        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. II Sta D                          | Sen. II Sta C                | 044        | Sta        | Sen      | 2        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta C                          | Sen. II Sta B                | 045        | Sta        | Sen      | 2        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta B                          | Sen. II Sta A                | 046        | Sta        | Sen      | 2        | B     | 2000                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta A                          | Sen. II Sta S                | 047        | Sta        | Sen      | 2        | A     | 2600                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta S                          |                              | 048        | Sta        | Sen      | 2        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. III Sta D                         | Sen. III Sta C               | 049        | Sta        | Sen      | 3        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta C                         | Sen. III Sta B               | 050        | Sta        | Sen      | 3        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta B                         | Sen. III Sta A               | 051        | Sta        | Sen      | 3        | B     | 2000                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta A                         | Sen. III Sta S               | 052        | Sta        | Sen      | 3        | A     | 2600                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta S                         |                              | 053        | Sta        | Sen      | 3        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. I La D                            | Sen. I La C                  | 054        | La         | Sen      | 1        | D     | 900                   | 10                    |
        | Gänserndorf Cup | Sen. I La C                            | Sen. I La B                  | 055        | La         | Sen      | 1        | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Sen. I La B                            | Sen. I La S                  | 056        | La         | Sen      | 1        | B     | 1800                  | 10                    |
        | Gänserndorf Cup | Sen. I La S                            |                              | 057        | La         | Sen      | 1        | S     | 999999999             | 10                    |
        #                                                            
        | Gänserndorf Cup | Sen. II La D                           | Sen. II La C                 | 058        | La         | Sen      | 2        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. II La C                           | Sen. II La B                 | 059        | La         | Sen      | 2        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. II La B                           | Sen. II La S                 | 060        | La         | Sen      | 2        | B     | 2800                  | 10                    |
        | Gänserndorf Cup | Sen. II La S                           |                              | 061        | La         | Sen      | 2        | S     | 999999999             | 10                    |
        #                                                            
        | Gänserndorf Cup | Sen. III La D                          | Sen. III La C                | 062        | La         | Sen      | 3        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. III La C                          | Sen. III La B                | 063        | La         | Sen      | 3        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. III La B                          | Sen. III La S                | 064        | La         | Sen      | 3        | B     | 2800                  | 10                    |
        | Gänserndorf Cup | Sen. III La S                          |                              | 065        | La         | Sen      | 3        | S     | 999999999             | 10                    |
        #                                                            
        | Gänserndorf Cup | Single o.Startbuch (SA, CC, JI)        |                              | 066        | La         | Offen    |          | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Single o.Startbuch (LW, TA, WW)        |                              | 067        | Sta        | Offen    |          | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Single m.Startbuch (SA, CC, JI)        |                              | 068        | La         | Offen    |          | D     | 999999999             | 10                    |
        | Gänserndorf Cup | Single m.Startbuch (LW, TA, WW)        |                              | 069        | Sta        | Offen    |          | D     | 999999999             | 10                    |
