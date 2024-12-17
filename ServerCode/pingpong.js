const io = require('socket.io')(8124, { //8124 is the local port we are binding the pingpong server to
    pingInterval: 30005,
    pingTimeout: 5000,
    upgradeTimeout: 3000,
    allowUpgrades: true,
    cookie: false,
    serveClient: true,
    allowEIO3: false,
    cors: {
        origin: "*"
    }
});


function sleep(ms) {
    return new Promise((resolve) => {
        setTimeout(resolve, ms);
    });
}


console.log('Starting Socket.IO pingpong server');

io.on('connection', (socket) => {
    var cnt = 0;
    console.log('[' + (new Date()).toUTCString() + '] unity connecting with SocketID ' + socket.id);

    socket.on('PING', async (data) => {
        cnt++;
        console.log('[' + (new Date()).toUTCString() + '] incoming PING #' + data + ' answering PONG with some jitter...');
        await sleep(Math.random() * 2000);
        socket.emit('PONG', data);
    });

    socket.on('disconnect', (reason) => {
        console.log('[' + (new Date()).toUTCString() + '] ' + socket.id + ' disconnected after ' + cnt + ' pings. Reason: ' + reason);
    });

});

