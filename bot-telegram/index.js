const fs = require('fs');
const request = require("request");
const schedule = require('node-schedule');
const TelegramBot = require('node-telegram-bot-api');

const config = {
    telegramToken: 'yourTelegramBotToken',
    apiUrl: 'yourApiUrl',
    cronReload: '0 0 * * *', // Everyday @ Midnight
    cronMotd: '0 8 * * *' // Everyday @ 8am
};

let namelist = {
    list: [],

    random: function() {
        return this.list[Math.floor(Math.random()*this.list.length)].name;
    },

    load: function() {
        return request(config.apiUrl, (error, response, body) => this.list = JSON.parse(body));
    }
};

let subscribers = [];
let bot = new TelegramBot(config.telegramToken, {polling: true});

(function() {
    let t;
    try {
        fs.readFileSync('subscribers', 'utf8').split(',').map(c => {
            t = parseInt(c);
            if(!isNaN(t)) subscribers.push(t);
        });
    }
    catch (err) {
        console.log('[sostbot] subscribers not found');
    }
    namelist.load();
    console.log('[sostbot] started v1 (%s subscribers)', subscribers.length);
}());

bot.onText(/\/subscribe/, function(msg, match) {
    let user = msg.from.id;
    let status = subscribers.includes(user);
    if (!status) subscribers.push(user);
    else subscribers.remove(user);
    fs.writeFile('subscribers', subscribers.join(), err => {
        if (err) throw err;
    });
    console.log('[subscriber] %s %s', user, (!status ? 'added' : 'removed'));
    bot.sendMessage(user, (!status ? 'You are now a subscriber!' : 'You are no longer a subscriber!'));
});

bot.on('message', function(msg) {
    bot.sendMessage(msg.chat.id, namelist.random());
});

schedule.scheduleJob(config.cronReload, function() {
    namelist.load();
});

schedule.scheduleJob(config.cronMotd, function() {
    let name = namelist.random();
    console.log('[cron] Sending to all subscribers: %s', name);
    for (let i = 0; i < subscribers.length; i++) {
        bot.sendMessage(subscribers[i], name);
    }
});

Array.prototype.remove = function() {
    var what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};
