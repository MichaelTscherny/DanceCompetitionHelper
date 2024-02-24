Feature: Configuration

A short summary of the feature

Scenario: Mandatory Basic Configuration
	Given following DanceCompetitionHelper "DanceCompHelper"
	Then following Configuration Values exists in DanceCompetitionHelper "DanceCompHelper"
		| Organization | CompetitionName | CompetitionClassName | CompetitionVenueName | Key                     | Value |
		|              |                 |                      |                      | MaxCouplesPerHeat       | 6     |
		|              |                 |                      |                      | MinCooldownTimePerRound | 10:00 |
		|              |                 |                      |                      | MinChangeClothesTime    | 15:00 |
		| Oetsv        |                 |                      |                      | MinTimePerDance         | 1:30  |
