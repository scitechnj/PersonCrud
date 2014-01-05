$(function() {
    $(".delete").click(function() {
        var response = confirm("Are you sure you want to delete?");
        if (response != true) {
            return false;
        }
    });
});