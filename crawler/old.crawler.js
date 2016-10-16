// Unused script, originally made for testing ages ago

const sqlite3 = require('sqlite3').verbose();

let names = [];

let LineByLineReader = require('line-by-line'),
    lr = new LineByLineReader('sus.txt');

lr.on('line', function(line) {
    names.push(line);
});

lr.on('end', function() {
    let db = new sqlite3.Database('file.db');
    let stmt = db.prepare("INSERT INTO namedb (name) VALUES (?)");
    let count = 0;
    for (let i = 0; i < names.length; i++) {
        stmt.run(names[i]);
        count++;
        console.log('appending %s', names[i]);
    }
    stmt.finalize();
    console.log('job done (%s out of %s)', count, names.length);
});