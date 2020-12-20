$(document).ready(() => {
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/tests")
    .configureLogging(signalR.LogLevel.Information)
    .build();

console.log("IN");


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
        console.log(id);
        console.log(success);
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


