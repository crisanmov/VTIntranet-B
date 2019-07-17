var idAlb = 0;

$('#events').click(function (e) {
    e.preventDefault();
    $('#myModal3').modal('show');
});

$('#titleEvt').attr("autocomplete", "off");

$('#btnSaveEvt').click(function (e) {
    console.log("Entra a guardar evento");
    e.preventDefault();

    if ($('#msgValidate').length) {
        $("p").remove();
    }

    //info event
    let title = $('#titleEvt').val();
    let url = $('#urlEvt').val();
    let description = $('#desEvt').val();


    validateControl(title, url, description);
    let fr = $('#fileEvt').prop('files')[0];
    console.log(fr);

    if (fr === undefined) {
        $('#validation').append("<p id='msgValidate' style='color: red; font-size: 15px'>Debes seleccionar un archivo</p>");
        return;
    }

    let file = $('#fileEvt').prop('files')[0];

    let formData = new FormData();
    formData.append('filePost', file);
    formData.append('title', title);
    formData.append('url', url);
    formData.append('description', description);
    formData.append('isEvent', true);

    console.log(formData.getAll('filePost'));

    saveEvent(formData);

});

$('.addImage').click(function (e) {
    e.preventDefault();

    //alert();
    $('#myModal4').modal('show');
    idAlb = $(this).attr('id');

});

$('#btnSaveImg').click(function (e) {

    e.preventDefault();

    //let idAlbum = $('.addImage').attr("id");
    //let idAlbum = $(this).attr('id');
    console.log(idAlb);

    //return;


    let fd = new FormData();
    /*-------------------------------------------------------*/
    let files = $('#files').prop('files');
    console.log(files.length);

    for (let i = 0; i < files.length; i++) {
        let file = files[i];
        fd.append('filesPost', file);
    }

    fd.append('idAlbum', idAlb);

    saveAlbum(fd);

});

$('.close').click(function (e) {
    $modal = $('#myModal4');
    $modal.find('form')[0].reset();
    $('#files').val('');
    $('#list').empty();
});

function validateControl(...restArgs) {
    //console.log(restArgs.length);
    let blank = 0;

    for (let i = 0; i < restArgs.length; i++) {
        //validate blank fields
        blank = validateBlank(restArgs[i]);
    }

    if (blank === 1) {
        $('#validation').append("<p id='msgValidate' style='color: red; font-size: 15px'>El formulario no debe tener campos vacios</p>");

        return;
    }
}

function validateBlank(...restArgs) {
    for (let i = 0; i < restArgs.length; i++) {
        //console.log(restArgs[i].toString());
        if (restArgs[i] === '') {
            return 1;
        }
    }
}

function saveEvent(formData) {

    $.ajax({
        url: "SaveEvent",
        //data: formData ? formData : form.serialize(),
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        type: 'POST',
        success: function (response) {
            if (response.success) {
                $modal = $('#myModal3');
                $modal.find('form')[0].reset();
                $('#myModal3').modal('hide');
                location.reload();
            }
        }

    });
}

function saveAlbum(fd) {

    $.ajax({
        url: "SaveAlbum",
        //data: formData ? formData : form.serialize(),
        data: fd,
        cache: false,
        contentType: false,
        processData: false,
        type: 'POST',
        success: function (response) {
            console.log(response);
            if (response.success) {

                $('#myModal4').modal('hide');
                $modal = $('#myModal4');
                $modal.find('form')[0].reset();
                $('#files').val('');
                $('#list').empty();
            }
        }
    });
}

function handleFileSelect(evt) {
    var files = evt.target.files; // FileList object

    // Loop through the FileList and render image files as thumbnails.
    for (var i = 0, f; f = files[i]; i++) {

        // Only process image files.
        if (!f.type.match('image.*')) {
            continue;
        }

        var reader = new FileReader();

        // Closure to capture the file information.
        reader.onload = (function (theFile) {
            return function (e) {
                // Render thumbnail.
                var span = document.createElement('span');
                span.innerHTML = ['<img class="thumb" src="', e.target.result,
                    '" title="', escape(theFile.name), '"/>'].join('');
                document.getElementById('list').insertBefore(span, null);
            };
        })(f);

        // Read in the image file as a data URL.
        reader.readAsDataURL(f);
    }
}

document.getElementById('files').addEventListener('change', handleFileSelect, false);

$('#del-img').click(function () {

    var x = document.getElementById("padredetodo");
    if (x.className === "prueba") {
        x.className += " visible";
    } else {
        x.className = "prueba";
    }

});