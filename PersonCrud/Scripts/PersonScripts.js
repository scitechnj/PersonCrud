$(function() {
    $("table").on('click', '.delete', function() {
        var response = confirm("Are you sure you want to delete?");
        if (response != true) {
            return;
        }

        var id = $(this).data("personId");
        $.post('/Person/Delete', { id: id }, function(data) {
            if (data.status) {
                $('#row-' + id).remove();
            }
        });
    });

    $('#addnewbtn').click(function() {
        $.post('/person/create', {
                firstname: $('#addfirstname').val(),
                lastname: $('#addlastname').val(),
                age: $('#addage').val()
            }, function(data) {
                $('table').append(data);
            });
    });
});