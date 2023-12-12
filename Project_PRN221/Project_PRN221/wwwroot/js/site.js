// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/SignalrServer").build();
    connection.start();
  
    connection.on("LoadReceiveDocument", function () {
        LoadProdData1();
    })

    function LoadProdData1() {
        var tr = '';
        location.reload();
        //$.ajax({
        //    url: '/Documents/ReceiveDocument',
        //    method: 'GET',
        //    success: (result) => {
        //        document.open();
        //        document.write(result);
        //        document.close();
        //        console.log(result);
        //    },
        //    error: (error) => {
        //        console.log(result)
        //        console.log(error)
        //    }
        //});
    }
})