// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('a.needConfirmation,input.needConfirmation').on("click",(e) => {
    var x = confirm("Esti sigur de actiunea ta?");
    if (!x) {
        e.preventDefault();
        e.stopPropagation();
    }
} )