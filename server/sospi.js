var http = require('http');
var sqlite3 = require('sqlite3').verbose();

var names;
var interval = 1000;
var port = 1386;

if(!isNaN(process.argv[2])) interval = process.argv[2];
if(!isNaN(process.argv[3])) port = process.argv[3];

function refresh() {
	var db = new sqlite3.Database('musa.db');
	db.all("SELECT `name` FROM `namedb`", function (err, rows) {
		names = rows;
		db.close();
	});
}

var server = http.createServer(function (request, response) {
	if(request.url.startsWith('/push')) {
		// TODO: parse & handle push requests
	}
	else response.end(JSON.stringify(names));
});

server.listen(port, function() {
	setInterval(refresh, interval);
    console.log("SoS[T]API v1.0 started on port %s, interval %s", port, interval);
});
