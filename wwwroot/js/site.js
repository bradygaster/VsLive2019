var connection;

async function start()
{
    try {
        await connection.start();
        setStatus('Connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
}

async function login()
{
    var username = document.getElementById('usernameTextbox').value;
    console.log(username);

    axios
        .get('/authenticate?username=' + username)
        .then(function (response) {
            connection = new signalR.HubConnectionBuilder()
                    .withUrl('/hubs/hello', {
                        // ---------------------------------------
                        // this is how you auth SignalR via JWT!!!
                        // ---------------------------------------
                        accessTokenFactory: () => response.data
                        // ---------------------------------------
                    })
                    // -----------------------------------------------------
                    // this is how you tell SignalR to reconnect when it can
                    // -----------------------------------------------------
                    .withAutomaticReconnect()
                    // -----------------------------------------------------
                    .build();

            connection.on('userLoggedIn', (username) => addUserToList(username));

            // -----------------------------------------------------
            // this is how you tell users SignalR is reconnecting 
            // -----------------------------------------------------
            connection.onreconnecting((error) => {const li = document.createElement("li");
                setStatus('Reconnecting');
            });

            // -----------------------------------------------------
            // this is how you tell users SignalR has reconnected
            // -----------------------------------------------------
            connection.onreconnected((connectionId) => {
                setStatus('Connected');
            });

            start();
        })
        .catch(function(err) {
            $('#errorDialog').modal();
        });
}

function addUserToList(username) {
    console.log(username);
}

function setStatus(status) {
    document.getElementById('connectionStatus').innerText = status;
}