//const axios = require('axios');

async function login()
{
    var username = document.getElementById('usernameTextbox').value;
    console.log(username);

    axios.get('/authenticate?username=' + username)
      .then(function (response) {
        console.log(response.data);
      });
}