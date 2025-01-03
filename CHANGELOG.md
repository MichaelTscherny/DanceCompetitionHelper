# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.7.x] ????-??-??

### Added

- PdfGenerator: creates Pdfs using [MigraDoc](https://docs.pdfsharp.net/)
    - Multiple Starters
    - Possible Promotions
- Multiple Starters: add view "grouped by dependent classes"
- PdfViewModelWrapper model/view: to get "consistent pdf download button" 
    - mandatory parameters - page format, orientation, etc.
    - optional select for Competition Classes

### Changed

- Selects/Drop Downs with Competition Classes: OrigId first
- Move all "Delete" buttons to the "Edit" view to availd "deleted by ooopps!.."
- Add several (https://xkcd.com/1070/) "Ignore" buttons instead of "Delete"

### Removed

- none


## [0.6.x] 2024-12-20

### Added

- Automapper
- ControllerBase.GetDefaultRequestHandler() for default requets handling

### Changed

- links/buttons rework/replaced with icons 
- *Controller: rework to use GetDefaultRequestHandler()
- Test: rework to use automapper
- #20 DanceCompetitionHelper: reworked to be more like a Repository-Pattern

### Removed

- #20 SpecFlow (R.I.P.) - changed to req'n'roll


## [0.5.x] 2023-02-23

### Added

- #10 Participants Overview
- Competition Class
    - #14 Promotion checks
    - Multiple starters

### Changed

- update bootstrap & bootstrap icons

### Removed

- none


## [0.1.x] 2022-12-28

### Added

- first draft for DanceCompetitionHelper.Database 
- first draft for DanceCompetitionHelper.Web
- first draft for DanceCompetitionHelper.Database.Test
- first draft for DanceCompetitionHelper.Test - NUnit and SpecFlow

### Changed

- none

### Removed

- none

