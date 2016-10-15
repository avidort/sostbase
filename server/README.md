# server (sospi)
The server (written in `Node.js`) is a RESTful API serving JSON-formatted strings with the entire SQLite database contents. Additionally, it handles requests from third party sources to push new entries into the database.

## Requests
The API handles `GET HTTP requests` as content requests and prints them in JSON, represented by `id` and `name` each.

Pushing new entries into the database is possible via `POST HTTP requests` formatted in JSON and containing both access key and data desired to be added. The access key used for security is hardcoded within `sospi.js` and needs to be set prior to starting the server. An ideal POST request would appear like `{"key":"myAccessKey","data":"My new entry"}`

### Predictable Status Codes
* **200** - OK - Expect a JSON-formatted string in response
* **401** - Unauthorized - Provided access key is invalid
* **400** - Bad Request - Data is missing from your request
* **201** - Created - Successfully pushed a new entry to database
* **500** - Internal Server Error - Something went wrong and wasn't caught
