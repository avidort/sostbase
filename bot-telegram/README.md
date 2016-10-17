# bot-telegram
The telegram bot (written in `Node.js`) is hooked with Telegram's API, our own server API and other resources to allow users to fetch random entries from the shared database by messaging it (... on telegram). Additionally, users may issue a `/subscribe` command to sign up for daily MOTDs (default is everyday @ 8am), being random entries too!

###  Resources
* Telegram HTTP-based API - https://core.telegram.org/bots
* node-telegram-bot-api - https://github.com/yagop/node-telegram-bot-api
* node-schedule - https://github.com/node-schedule/node-schedule
* request - https://github.com/request/request
