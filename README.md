
# Hermes

DiscordBot that uses mainly SlashCommands for personal use

The Bots functionality is going to increase from time to time as need is seen. The Initial Push is over, now the Commands have to be implemented


## Authors

- [@xAfterLife](https://www.github.com/xAfterLife)


## Badges

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/) ![Language](https://img.shields.io/github/languages/top/xAfterLife/Hermes) ![Code Size](https://img.shields.io/github/languages/code-size/xAfterLife/Hermes) [![CodeQL](https://github.com/xAfterLife/Hermes/actions/workflows/codeql.yml/badge.svg)](https://github.com/xAfterLife/Hermes/actions/workflows/codeql.yml)



## Contributing

If you found an issue or would like to submit an improvement to this project, please submit an issue using the issue tab above. If you would like to submit a PR with a fix, refrence the issue you created


## <a name="appsettings.json">appsettings.json</a>

To run this project, you will need to add the following appsettings.json

```json
{
  "DiscordToken": "YOUR TOKEN",
  "SpotifyId": "YOUR ID",
  "SpotifySecret": "YOUR SECRET",
  "testGuild": "GuildId"
}
```


## Features

- Random Reddit Pictures
- YoMama Jokes
- DM-Messages as reply


## Installation

- clone this project
- create your [appsettings.json](#appsettings.json)
- Compile & Run the project

    
## License

[MIT](https://choosealicense.com/licenses/mit/)


## Tech Stack

* .Net 6.0 
* Discord.Net 
* Microsoft.Extensions.Configuration
* Microsoft.Extensions.DependencyInjection
* (Probably EF.CORE or MongoDB in the Future)
