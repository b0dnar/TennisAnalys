$(document).ready(function () {
    $('input').keydown(function (e) {
        if (e.keyCode === 13) {
            alert("Message!!!");
            // можете делать все что угодно со значением текстового поля console.log($(this).val());
        }
    });
});