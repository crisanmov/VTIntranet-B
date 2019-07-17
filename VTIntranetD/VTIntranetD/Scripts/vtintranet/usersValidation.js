{
    let nombre;
    let apellidoP;
    let apellidoM;
    let skypeName;
    let skype;
    let userName;
    let pass;
    let pass2;

    let lbNombre;
    let lbApellidoP;
    let lbApellidoM;
    let lbSkypeName;
    let lbSkype;
    let lbUserName;
    let lbPass;
    let lbPass2;

    let enviaFormulario;


    let init = function () {

        nombre = document.getElementById('name');
        apellidoP = document.getElementById('lastNameP');
        apellidoM = document.getElementById('lastNameM');
        skypeName = document.getElementById('profileN');
        skype = document.getElementById('skype');
        userName = document.getElementById('username');
        pass = document.getElementById('password');
        pass2 = document.getElementById('password2');


        lbNombre = document.getElementById('SpanName');
        lbApellidoP = document.getElementById('SpanAP');
        lbApellidoM = document.getElementById('SpanAM');
        lbSkypeName = document.getElementById('spanSkypeName');
        lbSkype = document.getElementById('spanSkype');
        lbUserName = document.getElementById('spanUser');
        lbPass = document.getElementById('spanPass');
        lbPass2 = document.getElementById('spanPass2');

        nombre.addEventListener("blur", validarNombre);
        apellidoP.addEventListener("blur", validarApellidoP);
        apellidoM.addEventListener("blur", validarApellidoM);
        skypeName.addEventListener("blur", validarSkypeName);
        skype.addEventListener("blur", validarSkype);
        userName.addEventListener("blur", validarUserName);
        pass.addEventListener("blur", validarPass);
        pass2.addEventListener("blur", validarPass2);


        enviaFormulario = document.getElementById("saveUser");

        enviaFormulario.addEventListener("click", function (event) {
            event.preventDefault();
            validar();
        });
    };
    let validarNombre = function () {
        let valorNombre = nombre.value;
        let erNombre = new RegExp("^[a-zA-Z ]*$");

        if (valorNombre === "") {
            lbNombre.innerHTML = "El nombre no puede ir vacío";
            return false;
        } else if (valorNombre.length == 1) {
            lbNombre.innerHTML = "el nombre es muy corto";
            return false;
        } else if (!erNombre.test(valorNombre)) {
            lbNombre.innerHTML = "El nombre no puede tener numeros";
            return false;
        } else {
            lbNombre.innerHTML = "";
            return true;
        }
    };
    let validarApellidoP = function () {
        let valorApellidoP = apellidoP.value;
        let erApellidoP = new RegExp("^[a-zA-Z ]*$");

        if (valorApellidoP === "") {
            lbApellidoP.innerHTML = "El apellido paterno no puede ir vacío";
            return false;
        } else if (valorApellidoP.length == 1) {
            lbApellidoP.innerHTML = "el apellido paterno es muy corto";
            return false;
        } else if (!erApellidoP.test(valorApellidoP)) {
            lbApellidoP.innerHTML = "El apellido paterno no puede tener numeros";
            return false;
        } else {
            lbApellidoP.innerHTML = "";
            return true;
        }
    };
    let validarApellidoM = function () {
        let valorApellidoM = apellidoM.value;
        let erApellidoM = new RegExp("^[a-zA-Z ]*$");

        if (valorApellidoM === "") {
            lbApellidoM.innerHTML = "El apellido materno no puede ir vacío";
            return false;
        } else if (valorApellidoM.length == 1) {
            lbApellidoM.innerHTML = "el apellido materno es muy corto";
            return false;
        } else if (!erApellidoM.test(valorApellidoM)) {
            lbApellidoM.innerHTML = "El apellido materno no puede tener numeros";
            return false;
        } else {
            lbApellidoM.innerHTML = "";
            return true;
        }
    };
    let validarSkypeName = function () {
        let valorSkype = skypeName.value;
        let erSkype = new RegExp("^[a-zA-Z ]*$");

        if (valorSkype === "") {
            lbSkypeName.innerHTML = "El nombre de perfil no puede ir vacío";
            return false;
        } else if (valorSkype.length == 1) {
            lbSkypeName.innerHTML = "el nombre de perfil es muy corto";
            return false;
        } else if (!erSkype.test(valorSkype)) {
            lbSkypeName.innerHTML = "El nombre de perfil no puede tener numeros";
            return false;
        } else {
            lbSkypeName.innerHTML = "";
            return true;
        }
    };
    let validarSkype = function () {
        let valorSkype1 = skype.value;
        let erSkype1 = new RegExp("^[a-zA-Z0-9.]*$");

        if (valorSkype1 === "") {
            lbSkype.innerHTML = "El skype no puede ir vacío";
            return false;
        } else if (valorSkype1.length == 1) {
            lbSkype.innerHTML = "el skype es muy corto";
            return false;
        } else if (!erSkype1.test(valorSkype1)) {
            lbSkype.innerHTML = "El skype no puede tener espacios en blanco";
            return false;
        } else {
            lbSkype.innerHTML = "";
            return true;
        }
    };
    let validarUserName = function () {
        let valorUser = userName.value;
        let erUserName = new RegExp("^[a-zA-Z0-9]*$");

        if (valorUser === "") {
            lbUserName.innerHTML = "El nombre de usuario no puede ir vacío";
            return false;
        } else if (valorUser.length < 5) {
            lbUserName.innerHTML = "el nombre de usuario es muy corto";
            return false;
        } else if (!erUserName.test(valorUser)) {
            lbUserName.innerHTML = "El nombre de usuario no puede espacios en blanco";
            return false;
        } else {
            lbUserName.innerHTML = "";
            return true;
        }
    };
    let validarPass = function () {
        let valorPass = pass.value;
        let erPass = new RegExp("^[a-zA-Z0-9]*$");

        if (valorPass === "") {
            lbPass.innerHTML = "La contraseña no puede ir vacío";
            return false;
        } else if (valorPass.length < 5) {
            lbPass.innerHTML = "La contraseña es muy corta";
            return false;
        } else if (!erPass.test(valorPass)) {
            lbPass.innerHTML = "La contraseña no puede espacios en blanco";
            return false;
        } else {
            lbPass.innerHTML = "";
            return true;
        }
    };
    let validarPass2 = function () {
        let valorPass2 = pass2.value;
        let valorpass1 = pass.value;

        if (valorPass2 != valorpass1) {
            lbPass2.innerHTML = "Las contraseñas no coinciden";
            return false;
        } else {
            lbPass2.innerHTML = "";
            return true;
        }
    };

    let validar = function () {
        if (!validarNombre()) {
            nombre.focus();
        } else if (!validarApellidoP()) {
            apellidoP.focus();
        } else if (!validarApellidoM()) {
            apellidoM.focus();
        } else if (!validarSkypeName()) {
            skypeName.focus();
        } else if (!validarUserName()) {
            userName.focus();
        } else if (!validarPass()) {
            pass.focus();
        } else if (!validarPass2()) {
            pass2.focus();
        } else {
            alert("Se enviaría el formulario correctamente");
        }
    };
    window.onload = init;
}