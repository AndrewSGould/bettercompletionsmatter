# TAVIS

A TrueAchievement companion application for enhanced stat analysis and community events.

[![build and test](https://github.com/AndrewSGould/tavis/actions/workflows/main.yml/badge.svg)](https://github.com/AndrewSGould/tavis/actions/workflows/main.yml)


- Running locally:
- Install VS Code
- Install Git
- Clone the repo
- Get the .NET Core SDK
- Get the .NET Runtime
- Get the ASP Runtime
- Download Postgres (I use [dbForge](https://www.devart.com/dbforge/postgresql/))
- Download Postman
- Get invited to Postman workspace?  
- Update your `appsettings.Development.json` file to point to your local Postgres instance
- Path to `~/TavisApi/` and run `dotnet build`
- Path to `~/TavisApi.Tests` and run `dotnet test`
- Path back to `~/TavisApi/` and run `dotnet run`!

## Better Completions Matter (BCM) 

Yearly Achievement Hunting Contest Management Solution

### Requirements

Each requirement will be tagged with [v(x)] to denote at which point the feature is planned for. For a rough idea:

- [v1] Targets minimal useful functionality
- [v2] Targets extending that functionality to being easier to use
- [v3] Targets features to allow an end user to manage the yearly contest tasks
- [v4] Targets features to support the monthly bonuses and end user management

#### Home Page

- Reflect the first page on the sheet [v1-v3]
  - Rules overview [v1]
  - Timer for when the period ends on all pages [v2]
  - Links to tools [v2]
  - Links to Monthly Bonuses [v2]
  - Link to recaps [v3]
  - Discord link [v3]
  - Link to previous spreadsheets [v3]

~~#### RGSC Requirements~~

~~- Pull random game per users collection based on the below criteria [v1]~~
  ~~- Game must be over 1.2 ratio [v1]~~
  ~~- The highest time estimate is 100+ [v1]~~
  ~~- Ignore games marked Not for Contest. [v1]~~
  ~~- Ignore games with unobtainables [v1]~~
  ~~- Ignore games with server closures [v1]~~
  ~~- Ignore games marked No Longer Owned [v1]~~
  ~~- Only platforms Xbox 360, Xbox One, Series S/X, Windows [v1]~~

  ~~- 50 viable games minimum or it doesn't draw/returns error. [v1]~~
    ~~- return number of games needed ex 46/50? [v2]~~

  ~~- if a game has 0 completions, don't draw it [v1]~~

#### Completed Games Requirements

~~- Scan user for all completed games for the month [v1]~~
- What happens when a DLC/TU comes out? Scrape every Game Collection. compare achievement count? [v1/2]
- If a game is under 1.2 ratio, track it but ignore it [v1]

#### Page for each Player

- View RGSC game [v2]

#### Admin page

- Ability to add game to bonus category per person [v4]
- Reroll for user [v3]
- Think about how May Genre Tower would work [v4]
- Add/remove players [v1]

#### Page for each individual game

- See who all has done it in the competition [v4]
- Link it through to TA [v4]
- Game works for 'x' bonuses [v4]

#### UHH Matchup Ideas

~~Overall goal is to generate a list of 'compatability' for every player.~~
~~Player 1 has a xx% compatability with Player 2, a xx% compatability with Player 3, etc~~

~~- If a game is in both collections, add a point~~
~~- After adding points for every matchup, find the highest~~

#### Design Notes

- TA ID should be primary key for Player table?
- Write Integration tests that ping TA to ensure scraping is working (run monthly?)
- Diagnostics table that tracks processing time
  - Include 'Last Updated' field
- Change Api to versioning approach (v1/profile/.., v1/collection/.., v1/game/.., v1/period/.., v1/scraper/..)
- What other games to parse? Period Summary? Game pages?
- Thread and throttle TA hits to prevent unintentional DoS (Akka? ProtoActor?)
  - What time frame can I spread this out to while still being quick? 4hrs? Ideally sooner
- In the Player table (or separate table?) track what contests the player is in
- Try scraping Period Summary - https://www.trueachievements.com/gamer/{gamertag}/periods/2022/05
- Ability to schedule syncs

#### Roles

- Super Admin (god user)
- Admin (user who can do larger TA syncs)
- BcmAdmin, RaidBossAdmin (administrative duties per contest)
- Member

### Sync Manager

- Show how many Players will be sync'd
- Estimate how long the Sync will take (estimated)
  - Calculate this based off of Sync history. Average of sync time / average of players synced
