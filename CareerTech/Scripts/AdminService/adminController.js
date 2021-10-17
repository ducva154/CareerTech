$(document).ready(function () {
    $('#filterCandidate').click(function () {
        loadData();
    });
});
//Load Data function
function loadData() {
    var FilterViewModel = {
        Address: $('#Address').val(),
        Career: $('#Career').val(),
        Gender: $('#Gender').val()
    };
    var model = JSON.stringify(FilterViewModel);
    $.ajax({
        url: "https://localhost:44376/api/getcandidate",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        data: model,
        dataType: "json",
        success: function (response) {
            var html = '';
            $.each(response, function (index, item) {
                html += '<tr>';
                html += '<td>' + item.Name + '</td>';
                html += '<td>' + item.Age + '</td>';
                html += '<td>' + item.Address + '</td>';
                html += '<td>' + item.Address + '</td>';
                html += '<td>' + item.Address + '</td>';
                html += '<td>' + item.Address + '</td>';
                html += '<td>' + item.Address + '</td>';
                html += '<td>' + item.Address + '</td>';
                html += '</tr>';
            });
            $('#candidate-infor').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}