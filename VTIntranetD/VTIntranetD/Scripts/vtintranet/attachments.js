let deptos = JSON.parse(t.replace(/&quot;/g, '"'));
let tagClabe = '';

//show area or deptos in view
showArea(deptos);
tagClabe = getUrlParam();

$('.attachment').attr('id', tagClabe);

function editAttachment(tagClabe, idAttachment) {
    console.log(tagClabe, idAttachment);
}

function deleteAttachment(idAttach, idDepto, fileName) {
    $.ajax({
        url: '/Home/DeleteAttach/',
        data: JSON.stringify({ "idAttach": idAttach, "idDepto": idDepto, "fileName": fileName }),
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json; charset=utf-8'
    })
    .done(function (value) {
        if (value === 'successfully') {
            alert("El Archivo se Borro Exitosamente.");
            location.reload();
        }
    });
}

function getAttachmentsArea(idParent) {

    $.ajax({
        url: '/Home/GetAttachArea/',
        data: { idParent: idParent },
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json;',
        asyn: 'true',
        processData: 'false',
        cache: 'false',
        success: function (data) {

            if ($("#attachments tbody tr").html()) {
                $("#attachments").html("<tr><th>File ID</th><th>Nombre</th><th>Departamento</th><th>Ver</th><th></th></tr>");
            }

            let json = JSON.parse(data);
            let table = document.querySelector('#attachments');

            for (let i = 0; i < json.length; i++) {

                let tr = document.createElement('TR');
                let td_id = document.createElement('TD');
                let td_file = document.createElement('TD');
                let td_tag = document.createElement('TD');
                let td_show = document.createElement('TD');
                //let td_edit = document.createElement('TD');
                let td_del = document.createElement('TD');
                let a_show = document.createElement('a');
                //let a_edit = document.createElement('a');
                let a_del = document.createElement('a');
                //let span_edit = document.createElement('span');
                let span_del = document.createElement('span');

                //show file
                a_show.innerHTML = '<i class="fas fa-eye" style="color:#000"></i>';
                a_show.setAttribute('id', json[i]['IdAttachment']);
                a_show.setAttribute('href', '/UploadedFiles/attachments/' + json[i]['AttachmentName']);
                a_show.setAttribute('target', '_blank');
                a_show.setAttribute('class', 'fileShow');
                //edit file
                //a_edit.innerHTML = '<i class="fas fa-edit"></i>';
                //span_edit.textContent = json[i]['AttachmentName'];
                //span_edit.setAttribute('class', 'span_title');
                //span_edit.style.display = 'none';

                //a_edit.setAttribute('href', '');
                //a_edit.setAttribute('id', json[i]['IdAttachment']);
                //a_edit.setAttribute('class', 'fileEdit');
                //delete file
                a_del.innerHTML = '<i class="fas fa-trash-alt"></i>';
                a_del.setAttribute('href', json[i]['IdAttachment']);
                a_del.setAttribute('name', json[i]['AttachmentName']);
                a_del.setAttribute('id', json[i]['IdDepto']);
                a_del.setAttribute('class',"deleteFile");
                span_del.style.display = 'none';
                span_del.textContent = json[i]['IdDepto'];

                td_id.innerHTML = json[i]['IdAttachment'];
                td_file.innerHTML = json[i]['AttachmentName'];
                td_tag.innerHTML = json[i]['DeptoName'];
                td_show.appendChild(a_show);
                //td_edit.appendChild(a_edit);
                //td_edit.append(span_edit);
                td_del.appendChild(a_del);
                td_del.append(span_del);

                tr.append(td_id);
                tr.append(td_file);
                tr.append(td_tag);
                tr.append(td_show);
                //tr.append(td_edit);
                tr.append(td_del);

                table.append(tr);

            }
        },
        error: function (e) {

        }
    });
}

function getAttachmentsDepto(tagClabe) {

    $.ajax({
        url: '/Home/GetAttachDepto/',
        data: { tagClabe: tagClabe },
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json;',
        asyn: 'true',
        processData: 'false',
        cache: 'false',
        success: function (data) {
            let json = JSON.parse(data);
            let table = document.querySelector('#attachments');

            for (let i = 0; i < json.length; i++) {

                let tr = document.createElement('TR');
                let td_id = document.createElement('TD');
                let td_file = document.createElement('TD');
                let td_tag = document.createElement('TD');
                let td_show = document.createElement('TD');
                //let td_edit = document.createElement('TD');
                let td_del = document.createElement('TD');
                let a_show = document.createElement('a');
                //let a_edit = document.createElement('a');
                let a_del = document.createElement('a');
                //let span_edit = document.createElement('span');
                let span_del = document.createElement('span');

                //show file
                a_show.innerHTML = '<i class="fas fa-eye" style="color:#000"></i>';
                a_show.setAttribute('id', json[i]['IdAttachment']);
                a_show.setAttribute('href', '/UploadedFiles/attachments/' + json[i]['AttachmentName']);
                a_show.setAttribute('target', '_blank');
                a_show.setAttribute('class', 'fileShow');
                //edit file
                /*a_edit.innerHTML = '<i class="fas fa-edit"></i>';
                span_edit.textContent = json[i]['AttachmentName'];
                span_edit.setAttribute('class', 'span_title');
                span_edit.style.display = 'none';

                a_edit.setAttribute('href', '');
                a_edit.setAttribute('id', json[i]['IdAttachment']);
                a_edit.setAttribute('class', 'fileEdit');*/
                //delete file
                a_del.innerHTML = '<i class="fas fa-trash-alt"></i>';
                a_del.setAttribute('href', json[i]['IdAttachment']);
                a_del.setAttribute('name', json[i]['AttachmentName']);
                a_del.setAttribute('id', json[i]['IdDepto']);
                a_del.setAttribute('class',"deleteFile");
                span_del.style.display = 'none';
                span_del.textContent = json[i]['idDepto'];

                td_id.innerHTML = json[i]['IdAttachment'];
                td_file.innerHTML = json[i]['AttachmentName'];
                td_tag.innerHTML = json[i]['DeptoName'];
                td_show.appendChild(a_show);
                //td_edit.appendChild(a_edit);
                //td_edit.append(span_edit);
                td_del.appendChild(a_del);
                td_del.append(span_del);

                tr.append(td_id);
                tr.append(td_file);
                tr.append(td_tag);
                tr.append(td_show);
                //tr.append(td_edit);
                tr.append(td_del);
                table.append(tr);
            }
        },
        error: function (e) {

        }
    });
}

function getUrlParam() {

    let url_string = window.location.href;
    let url = new URL(url_string);
    let tagClabe = url.searchParams.get('tag');

    return tagClabe;
}

function showArea(deptos) {

    for (let i = 0; i < deptos.length; i++) {

        let brandDeptos = document.querySelector('#brand-deptos');
        let deptoName = deptos[i].Name;
        let div = document.createElement('DIV');

        div.setAttribute('class', 'area');
        div.setAttribute('id', deptos[i].IdDepto);
        div.append(deptoName);
        brandDeptos.appendChild(div);
    }
    getAttachmentsDepto(getUrlParam());
}

function setEventClickDeptos() {

    let areas = document.querySelectorAll('.area');

    for (let i = 0; i<areas.length; i++) {
        $(areas[i]).click(function () {
            let idParent = $(this).attr('id');
            getAttachmentsArea(idParent);
        });
    }
}

function setEventEditFile() {

    let files = document.querySelectorAll('.fileEdit');
    let span_title = document.querySelectorAll('.span_title');

    for (let i = 0; i < files.length; i++) {
        $(files[i]).click(function (e) {

            e.preventDefault();           
            document.querySelector('#titleModalManual').innerHTML = 'Editar Manual';
            $('#uploadFileModal').modal('show');
            let fileName = $(span_title[i]).html();
            document.querySelector('#titlePdf').setAttribute('value', fileName);
            document.querySelector('#FormControlTag').setAttribute('value', span_title[i]);
            populateListTag();

        });
    }
}

$(document).ready(function () {

    $('body').find('*').each(function() {
        var html= $(this).html().replace(/&amp;/g, "&");
        $(this).html(html);
    });
    //set event click show file
    setEventEditFile();
    setEventClickDeptos();

    $('.deleteFile').click(function (e) {

        e.preventDefault();
        let idAttach = $(this).attr('href');
        let idDepto = $(this).attr('id');
        let fileName = $(this).attr('name');
        //alert("Borrar File: " + idAttach + idDepto);
        $('.cd-popup').addClass('is-visible');

        //close popup
        $('.cd-popup').on('click', function (event) {
            if ($(event.target).is('.cd-popup-close') || $(event.target).is('.cd-popup')) {
                event.preventDefault();
                $(this).removeClass('is-visible');
            }
        });

        //close popup when clicking the esc keyboard button
        $(document).keyup(function (event) {
            if (event.which == '27') {
                $('.cd-popup').removeClass('is-visible');
            }
        });

        $('.del').click(function (e) {
            e.preventDefault();
            let res = $(this)[0].textContent;
            if (res === "Si") {
                $('.cd-popup').removeClass('is-visible');
                deleteAttachment(idAttach, idDepto, fileName);
            }
        });
    });

});

