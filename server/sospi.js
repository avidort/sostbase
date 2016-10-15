const express = require('express');
const bodyParser = require('body-parser');
const sqlite3 = require('sqlite3').verbose();

const app = express();

const setup = {
    appPort: 1386,
    dbName: 'wildDbName.db',
    accessKey: 'hardcodedKey'
};

app.use(bodyParser.json());
app.use(bodyParser.urlencoded());

app.get('/', function(req, res) {
    let db = new sqlite3.Database(setup.dbName);
    db.all("SELECT * FROM `namedb`", function (err, rows) {
        db.close();
        res.send(JSON.stringify(rows));
    });
});

app.post('/', function(req, res) {
    if (req.body.key === setup.accessKey) {
        if (req.body.data) {
            let db = new sqlite3.Database(setup.dbName);
            db.run(`INSERT INTO namedb (name) VALUES ('${req.body.data}')`);
            db.close();
            res.send(201);
        }
        else res.send(400);
    }
    else res.send(401);
});

var server = app.listen(setup.appPort, function() {
    console.log("sospi v2 started on port %s", setup.appPort);
});
