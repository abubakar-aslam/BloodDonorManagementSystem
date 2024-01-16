
            // Get references to the canvas elements
        var ctx1 = document.getElementById('pieChart1').getContext('2d');
        var ctx2 = document.getElementById('pieChart2').getContext('2d');

        // Define the data for the first pie chart
        if (@Html.Raw(Json.Serialize(testdonorCount)) !== null) {

                var records1 = @Html.Raw(Json.Serialize(testdonorCount));

        console.log("Serialized Donor Count Data: ", records1);

        var labels1 = Object.keys(records1);
        var dataValues1 = Object.values(records1);

        var data1 = {
            labels: labels1,
        datasets: [{
            data: dataValues1,
        backgroundColor: ['red', 'blue', 'green', 'yellow', 'black', 'pink', 'grey', 'orange'],
                    }]
                };

        var pieChart1 = new Chart(ctx1, {
            type: 'pie',
        data: data1,
                });

            }
        // Define the data for the second pie chart

        if (@Html.Raw(Json.Serialize(testdonorCount)) !== null) {

                var records2 = @Html.Raw(Json.Serialize(inventoryCount));

        console.log("Serialized Inventory Count Data: ", records2);
        var labels2 = Object.keys(records2);
        var dataValues2 = Object.values(records2);

        var data2 = {
            labels: labels2,
        datasets: [{
            data: dataValues2,
        backgroundColor: ['red', 'blue', 'green', 'yellow', 'black', 'pink', 'grey', 'orange'],
                    }]
                };

        var pieChart2 = new Chart(ctx2, {
            type: 'pie',
        data: data2,
                });
            }


