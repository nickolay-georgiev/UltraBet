function initHub() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/index")
        .build();

    connection.on("getMatchesInNext24Hours", function (data) {
        let data = data;
    });

    connection.on("getMatchById", function (data) {
        let data = data;
    });

    connection.start()
        .then(() => {         
        })
        .catch(err => console.log(err.toString()));
}
initHub()