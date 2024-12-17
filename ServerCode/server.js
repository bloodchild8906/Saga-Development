const io = require('socket.io')(8123, {
  pingInterval: 30005,
  pingTimeout: 5000,
  upgradeTimeout: 3000,
  allowUpgrades: true,
  cookie: false,
  serveClient: true
});



// App Code starts here

console.log('Starting Socket.IO demo server');

io.on('connection', (socket) => {
	console.log('[' + (new Date()).toUTCString() + '] game connecting');
	
    socket.on('KnockKnock', (data) => {
		console.log('[' + (new Date()).toUTCString() + '] game knocking... Answering "Who\'s there?"...');
        socket.emit('WhosThere');
    });

    socket.on('ItsMe', async (data) => {
		console.log('[' + (new Date()).toUTCString() + '] received game introduction. Welcoming the guest...');
        socket.emit('Welcome', 'Hi customer using unity' + data.version + ', this is the backend microservice. Thanks for buying our asset. (No data is stored on our server)');
        socket.emit('TechData', {
			podName: 'Local Test-Server',
			timestamp: (new Date()).toUTCString()
		});
    });
	
	socket.on('SendNumbers', async (data) => {
		console.log('[' + (new Date()).toUTCString() + '] Client is asking for random number array');
		socket.emit('RandomNumbers', [ Math.ceil((Math.random() * 100)), Math.ceil((Math.random() * 100)), Math.ceil((Math.random() * 100)) ]);
	});
	
	socket.on('Goodbye', async (data) => {
		console.log('[' + (new Date()).toUTCString() + '] Client said "' + data + '" - The server will now disconnect the client.');
		socket.disconnect(true);
	});

	socket.on('disconnect', (data) => {
		console.log('[' + (new Date()).toUTCString() + '] Bye, client ' + socket.id);
	});


	
	socket.on('PING', async (data) => {
		console.log('[' + (new Date()).toUTCString() + '] incoming PING #' + data + ' from ' + socket.id + ' answering PONG with some jitter...');
		await sleep(Math.random() * 2000);
        socket.emit('PONG', data);
    });
	
});
