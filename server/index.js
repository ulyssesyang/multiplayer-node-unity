var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');

console.log("server started...");

var players = [];

io.on('connection', function(socket) {

	var thisCliendId = shortid.generate();
	players.push(thisCliendId);

	console.log("client connected and broadcast spawn:",thisCliendId);
	socket.broadcast.emit('spawn', {id: thisCliendId});

	players.forEach(function(playerId) {
		if (playerId == thisCliendId)
			return
		
		socket.emit('spawn',playerId);
		console.log('sending spawn to new player for id:',playerId);
	})

	socket.on('move', function(data) {
		data.id = thisCliendId;
		console.log('client moved...', JSON.stringify(data));
		socket.broadcast.emit("move", data);
	});

	socket.on('disconnect', function(argument) {
		console.log("client disconnected...");
		players.splice(players.indexOf(thisCliendId),1);
	});

});