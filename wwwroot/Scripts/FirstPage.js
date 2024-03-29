// document.addEventListener("DOMContentLoaded", function () {
//     verificarEnvioDeFormularios();
// });



// function verificarEnvioDeFormularios() {
//     var forms = document.getElementsByTagName("form");
//     for (var i = 0; i < forms.length; i++) {
//         forms[i].addEventListener("submit", function (event) {
//             var div = document.getElementById("private-key-block");
//             if (div.innerHTML.length > 100) {
//                 window.location.href = "/encrypt/generateKeyPair/99";
//             }
//         });
//     }
// }