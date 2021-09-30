// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#shortenedURL').hide();

    $('#urlShortenBtn').on('click', function (e) {
        $('#shortenedURL').hide();
        e.preventDefault();
        var urlString = $('#urlInput').val();
        $.ajax({
            url: "/Home/PostUrl",
            type: "POST",
            data: { url: urlString },
            success: function (result) {
                $('#shortenedURL').text(result);
                $('#shortenedURL').show();
            }
        })
    });
})



