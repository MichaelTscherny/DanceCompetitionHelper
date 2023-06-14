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
        | Gänserndorf Cup | Sch. Kombi BSP (LW, TG, CC, JI)        | 001        | Ko         | Sch      | 1        | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Sch. Sta D                             | 002        | Sta        | Sch      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Sch. Sta C                             | 003        | Sta        | Sch      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Sch. Sta B                             | 004        | Sta        | Sch      | 1        | B     | 999999999             | 10                    |
        #                                                            
        | Gänserndorf Cup | Sch. La D                              | 005        | La         | Sch      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Sch. La C                              | 006        | La         | Sch      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Sch. La B                              | 007        | La         | Sch      | 1        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jun. I + II Kombi BSP (LW, TG, CC, JI) | 008        | Ko         | Jun      | 2        | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. I Sta D                           | 009        | Sta        | Jun      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. I Sta C                           | 010        | Sta        | Jun      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. I Sta B                           | 011        | Sta        | Jun      | 1        | B     | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. II Sta D                          | 012        | Sta        | Jun      | 2        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. II Sta C                          | 013        | Sta        | Jun      | 2        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. II Sta B                          | 014        | Sta        | Jun      | 2        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jun. I La D                            | 015        | La         | Jun      | 1        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. I La C                            | 016        | La         | Jun      | 1        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. I La B                            | 017        | La         | Jun      | 1        | B     | 999999999             | 10                    |
        | Gänserndorf Cup | Jun. II La D                           | 018        | La         | Jun      | 2        | D     | 1000                  | 10                    |
        | Gänserndorf Cup | Jun. II La C                           | 019        | La         | Jun      | 2        | C     | 1800                  | 10                    |
        | Gänserndorf Cup | Jun. II La B                           | 020        | La         | Jun      | 2        | B     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jug. Sta D                             | 021        | Sta        | Jug      |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Jug. Sta C                             | 022        | Sta        | Jug      |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Jug. Sta B                             | 023        | Sta        | Jug      |          | B     | 1000                  | 10                    |
        | Gänserndorf Cup | Jug. Sta A                             | 024        | Sta        | Jug      |          | A     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Jug. La D                              | 025        | La         | Jug      |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Jug. La C                              | 026        | La         | Jug      |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Jug. La B                              | 027        | La         | Jug      |          | B     | 1000                  | 10                    |
        | Gänserndorf Cup | Jug. La A                              | 028        | La         | Jug      |          | A     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Allg.Kl. Sta D                         | 029        | Sta        | Allg     |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta C                         | 030        | Sta        | Allg     |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta B                         | 031        | Sta        | Allg     |          | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta A                         | 032        | Sta        | Allg     |          | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. Sta S                         | 033        | Sta        | Allg     |          | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Allg.Kl. La D                          | 034        | La         | Allg     |          | D     | 900                   | 10                    |
        | Gänserndorf Cup | Allg.Kl. La C                          | 035        | La         | Allg     |          | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La B                          | 036        | La         | Allg     |          | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La A                          | 037        | La         | Allg     |          | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Allg.Kl. La S                          | 038        | La         | Allg     |          | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. I Sta D                           | 039        | Sta        | Sen      | 1        | D     | 900                   | 10                    |
        | Gänserndorf Cup | Sen. I Sta C                           | 040        | Sta        | Sen      | 1        | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta B                           | 041        | Sta        | Sen      | 1        | B     | 1300                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta A                           | 042        | Sta        | Sen      | 1        | A     | 1600                  | 10                    |
        | Gänserndorf Cup | Sen. I Sta S                           | 043        | Sta        | Sen      | 1        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. II Sta D                          | 044        | Sta        | Sen      | 2        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta C                          | 045        | Sta        | Sen      | 2        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta B                          | 046        | Sta        | Sen      | 2        | B     | 2000                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta A                          | 047        | Sta        | Sen      | 2        | A     | 2600                  | 10                    |
        | Gänserndorf Cup | Sen. II Sta S                          | 048        | Sta        | Sen      | 2        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. III Sta D                         | 049        | Sta        | Sen      | 3        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta C                         | 050        | Sta        | Sen      | 3        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta B                         | 051        | Sta        | Sen      | 3        | B     | 2000                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta A                         | 052        | Sta        | Sen      | 3        | A     | 2600                  | 10                    |
        | Gänserndorf Cup | Sen. III Sta S                         | 053        | Sta        | Sen      | 3        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. I La D                            | 054        | La         | Sen      | 1        | D     | 900                   | 10                    |
        | Gänserndorf Cup | Sen. I La C                            | 055        | La         | Sen      | 1        | C     | 1500                  | 10                    |
        | Gänserndorf Cup | Sen. I La B                            | 056        | La         | Sen      | 1        | B     | 1800                  | 10                    |
        | Gänserndorf Cup | Sen. I La S                            | 057        | La         | Sen      | 1        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. II La D                           | 058        | La         | Sen      | 2        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. II La C                           | 059        | La         | Sen      | 2        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. II La B                           | 060        | La         | Sen      | 2        | B     | 2800                  | 10                    |
        | Gänserndorf Cup | Sen. II La S                           | 061        | La         | Sen      | 2        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Sen. III La D                          | 062        | La         | Sen      | 3        | D     | 1400                  | 10                    |
        | Gänserndorf Cup | Sen. III La C                          | 063        | La         | Sen      | 3        | C     | 2400                  | 10                    |
        | Gänserndorf Cup | Sen. III La B                          | 064        | La         | Sen      | 3        | B     | 2800                  | 10                    |
        | Gänserndorf Cup | Sen. III La S                          | 065        | La         | Sen      | 3        | S     | 999999999             | 10                    |
        #
        | Gänserndorf Cup | Single o.Startbuch (SA, CC, JI)        | 066        | La         | Offen    |          | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Single o.Startbuch (LW, TA, WW)        | 067        | Sta        | Offen    |          | Bsp   | 999999999             | 10                    |
        | Gänserndorf Cup | Single m.Startbuch (SA, CC, JI)        | 068        | La         | Offen    |          | D     | 999999999             | 10                    |
        | Gänserndorf Cup | Single m.Startbuch (LW, TA, WW)        | 069        | Sta        | Offen    |          | D     | 999999999             | 10                    |
