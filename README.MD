# Scrum Bot
<img src="bot_logo.png" width="200" height="200">

## Setup

The bot uses [PostgreSQL](https://www.postgresql.org/).

Set your user secret for the database connection string:
```
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=<ip>;Database=<dbname>;Username=<username>;Password=<pass>"
```
Find your bots Token under Bot><your_application>Build-A-Bot>Token [here](https://discord.com/developers/applications/), and set the token with:
```
dotnet user-secrets set "ConnectionStrings:DiscordClientSecret" "<token>"
```
Set your google api specific connection strings:
```
dotnet user-secrets set "GoogleApi:ApiKey" "<apikey>"
dotnet user-secrets set "GoogleApi:CalendarId" "<public_google_calendar_id" 
```

## Building the database
Change to the root directory of the application `ScrumBot` and enter the commands:
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Discord Usage Message

**_How to use the @Scrum bot_**

**Help**

- _prefix_ - `?`.
- _help_ - `?help`, or help for a specific command `?help <commandname>`.

**Commands**

_Sprints_
- `?beginsprint <sprintname>?` - Begins a new sprint with the given name, and sets it to the active sprint
- `?endsprint`- Ends the currently active sprint.

_Tasks_

- `?begintask <taskname>` - Begins a new task with the given name under the currently active sprint. Creates a new role and assigns it to 
  the person that began the task.
- `?jointask (@mention)<role>` - Joins the task with the specified role (meaning you get the role assigned).
- `?endtask (@mention)<role>` - Ends the task with the given role, and measures the time between it was started and when it was ended.
- `?tasks` - Gets a list of all the tasks under the current sprint, and shows whether they are in progress or if they are finished.

**Code**
- The code can be found at https://github.com/adamnegaard/ScrumBot, feel free to add any features.
