# tymbot
Telegram bot to allow users to get their time and the time of their friends. 

I have already created this Telegram bot **@kaleurbot** using this source code.
Feel free to add this bot to your telegram groups.


## Running the application
- You need to have **docker** (https://docs.docker.com/engine/install/) and **docker-compose** (https://docs.docker.com/compose/install/) installed.

- Create your own bot using the Telegram bot **@BotFather** and get the **API access token** of your created bot.

- Add .env file and follow the instructions in the sample.env file. Set the access token you obtained from BotFather and set the database password to any password you want.

- At the project root directory, run **docker-compose build --no-cache**

- At the project root directory, run **docker-compose up -d**
