mergeInto(LibraryManager.library, {
	InitializeSIOVars: function () {
		window.UnitySocketIOInstances = [];
	},
	
	CreateSIOInstance: function (instanceName, targetAddress, enableReconnect) {
		var iName = UTF8ToString(instanceName);
		
		try {
			if (typeof window.UnitySocketIOInstances[iName] !== 'undefined' && window.UnitySocketIOInstances[iName] != null) {
				console.log("Cleaning up Socket.IO system for " + iName);
				window.UnitySocketIOInstances[iName].close();
				delete window.UnitySocketIOInstances[iName];
			}
		} catch(e) {
			console.warning('Exception while cleaning up SocketIO on ' + iName + ': ' + e);
		}
		
		console.log('Connecting SIO to ' + UTF8ToString(targetAddress));
		window.UnitySocketIOInstances[iName] = window.io(UTF8ToString(targetAddress), {
			transports: ['websocket'],
			autoConnect: false,
			reconnection: (enableReconnect == 1),
			reconnectionDelay: 1000,
			reconnectionDelayMax: 30000,
			timeout: 5000,
			upgrade: true,
			rememberUpgrade: true
		});
		
		window.UnitySocketIOInstances[iName].on('connect', function() {
			SendMessage(iName, 'UpdateSIOStatus', 1); //connected
			SendMessage(iName, 'UpdateSIOSocketID', window.UnitySocketIOInstances[iName].id);
		});
		
		window.UnitySocketIOInstances[iName].on('disconnect', function(reason) {
			SendMessage(iName, 'UpdateSIOStatus', 0); //disconnected
		});
		
		window.UnitySocketIOInstances[iName].on('reconnect', function(attemptNumber) {
			SendMessage(iName, 'UpdateSIOStatus', 1); //connected
			SendMessage(iName, 'UpdateSIOSocketID', window.UnitySocketIOInstances[iName].id);
		});
		
		window.UnitySocketIOInstances[iName].on('connect_timeout', function() {
			SendMessage(iName, 'UpdateSIOStatus', 2); //errored
			SendMessage(iName, 'SIOWarningRelay', 'Timeout on connection ' + iName);
		});
		
		window.UnitySocketIOInstances[iName].on('connect_error', function(error) {
			SendMessage(iName, 'UpdateSIOStatus', 2); //errored
			SendMessage(iName, 'SIOWarningRelay', 'Error on connection attempt for ' + iName + ': ' + error);
		});
		
		window.UnitySocketIOInstances[iName].on('reconnect_attempt', function() {
			window.UnitySocketIOInstances[iName].io.opts.transports = ['polling', 'websocket'];
			SendMessage(iName, 'SIOWarningRelay', 'Websocket failed for ' + iName + '. Trying to reconnect with polling enabled.');
		});
		window.UnitySocketIOInstances[iName].on('reconnect_error', function(error) {
			SendMessage(iName, 'UpdateSIOStatus', 2); //errored
			SendMessage(iName, 'SIOWarningRelay', 'Error on reconnection attempt for ' + iName + ': ' + error);
		});
		
		window.UnitySocketIOInstances[iName].on('reconnect_failed', function(error) {
			SendMessage(iName, 'UpdateSIOStatus', 2); //errored
			SendMessage(iName, 'SIOWarningRelay', 'Reconnect failed for ' + iName + ': Max. attempts exceeded.');
		});
	},
	
	ConnectSIOInstance: function (instanceName) {
		window.UnitySocketIOInstances[UTF8ToString(instanceName)].connect();
	},
	
	RegisterSIOEvent: function (instanceName, eventName) {
		var iName = UTF8ToString(instanceName);
		var eName = UTF8ToString(eventName);
		window.UnitySocketIOInstances[iName].on(eName, function (data) {
			SendMessage(iName, 'RaiseSIOEvent', JSON.stringify({
				eventName: eName,
				data: (typeof data == 'undefined' ? null : (typeof data == 'string' ? data : JSON.stringify(data)))
			}));
		});
	},
	
	UnregisterSIOEvent: function (instanceName, eventName) {
		var iName = UTF8ToString(instanceName);
		var eName = UTF8ToString(eventName);
		window.UnitySocketIOInstances[iName].off(eName);
	},
	
	SIOEmitNoData: function (instanceName, eventName) {
		window.UnitySocketIOInstances[UTF8ToString(instanceName)].emit(UTF8ToString(eventName));
	},
	
	SIOEmitWithData: function (instanceName, eventName, data, parseAsJSON) {
		var parsedData = "__ERROR__";
		if (parseAsJSON == 1) {
			parsedData = JSON.parse(UTF8ToString(data));
		}
		else 
		{
			parsedData = UTF8ToString(data)
		}
		window.UnitySocketIOInstances[UTF8ToString(instanceName)].emit(UTF8ToString(eventName), parsedData);
	}
});
