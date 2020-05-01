const http = require("http");

const requestListener = function (req, res) {
  console.log("request got" + req);
  let body = req.method;
  if (req.method === "POST") {
    req.on("data", (chunk) => {
      console.log("have data.. " + chunk);
      body += chunk.toString(); // convert Buffer to string
    });
    req.on("end", () => {
      console.log(body);
      res.end("ok");
    });
  }
  res.writeHead(200);
  res.end(req + "   this is crazy" + body);
};

const server = http.createServer(requestListener);
server.listen(8088);
