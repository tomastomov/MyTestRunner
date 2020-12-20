$(document).ready(() => {
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/tests")
    .configureLogging(signalR.LogLevel.Information)
    .build();

$('#runTestsBtn').click(() => {
    $('.notExecuted').hide();
    $('.passed').hide();
    $('.failed').hide();
    $('.spinners').show();

    fetch('/runtests', {
        method: 'GET',
        headers: {
            'Content-Type':'application/json'
        }
    })
})

async function start() {
    try{
        await connection.start();
        console.log("SignalR connected");
    } catch(err){
        console.log("Connection failed");
    }
}

start().then(() => {
    connection.on("SendMessage", (id, success) => {
        const prefix = `#${id}-`;

        $(prefix + 'spinner').hide();

        if(success) {
            $(prefix + 'passed').show();
        }
        else {
            $(prefix + 'failed').show();
        }
    })
})

})


