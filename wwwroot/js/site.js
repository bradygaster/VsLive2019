var connection;

async function start()
{
    try {
        await connection.start();
        console.log("connected");
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
                        accessTokenFactory: () => response.data
                    })
                    .build();
            start();
        });
}