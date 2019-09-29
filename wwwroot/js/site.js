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
                    .build();
            start();
        })
        .catch(function(err) {
            $('#errorDialog').modal();
        });
}

function setStatus(status) {
    document.getElementById('connectionStatus').innerText = status;
}