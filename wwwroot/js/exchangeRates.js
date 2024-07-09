const connection = new signalR.HubConnectionBuilder().withUrl("/kurlarHub").build();

connection.on("ReceiveKur", (CentralBankExchangeRate) => {
    console.log(`Received CentralBankExchangeRate: ${CentralBankExchangeRate}`);

    var list = document.createElement("li");
    list.textContent = `${CentralBankExchangeRate}`;
    document.getElementById("list").appendChild(list);
});

connection.start().then(function () {
    console.log("SignalR connected.");

}).catch(function (err) {
    return console.error(err.toString());
});