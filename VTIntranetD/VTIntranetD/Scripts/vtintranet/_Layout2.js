﻿if (array === "null") {
    //console.log("No se puede cargar Menu Principal");
    //alert("No se puede cargar Menu Principal"); 
    document.querySelector('#mValidate').style.display = "flex";
    //return;
}

let json = JSON.parse(array.replace(/&quot;/g, '"'));
let menuInfo = {};
let brands = [];

brands = getBrands(json);
brands = findDuplicateValues(brands);
menuInfo = getBrandDepto(json, brands);
createMenu(menuInfo);

jQuery(document).ready(function ($) {

    var tags = $('.tags');
    var contenedor = $("#container-right").height();

    $("#container-left").css("height", contenedor);

    $('#uploadFile').click(function () {
        $("#FormControlTag").children('option').not(':first').remove();
        populateListTag();
    });

    $("#FormControlArea").change(function () {

        let selectedArea = $(this).children("option:selected").text();
        let valArea = $(this).children("option:selected").val();
        let contenedorAreas = document.createElement('div');
        let borrarArea = document.createElement('div')

        borrarArea.innerHTML = ('x');
        borrarArea.setAttribute('class', 'borrar-area');
        contenedorAreas.setAttribute('class', 'contenedor-area');
        borrarArea.setAttribute('id', valArea);
        $('#areas-listas').append(contenedorAreas);
        $(contenedorAreas).append(selectedArea);
        $(contenedorAreas).append(borrarArea);
    });

    $("#FormControlArea").click(function () {
        let des = $(this).children("option:selected");
        des.attr('disabled', 'disabled');

    });

    $('#FormControlTag').change(function (e) {

        let brand = ($(this).val());
        populateListDepto(brand);
        $('.contenedor-area').remove();
        $("#FormControlDepto").children('option').not(':first').remove();
        $("#FormControlArea").children('option').not(':first').remove();

    });

    $('#FormControlDepto').change(function (e) {
        
        let idDepto = ($(this).val());
        populateListArea(idDepto);
        $('.contenedor-area').remove();
        $("#FormControlArea").children('option').not(':first').remove();
        $("#FormControlArea").children('option').removeAttr('disabled');

    });

    $(document).on('click', '.borrar-area', function () {

        let val1 = $(this).attr('id');
        $('#FormControlArea option[value="' + val1 + '"]').removeAttr('disabled');
        $(this).parent('div').remove();
        $('#FormControlArea').prop('selectedIndex', 0);

    });

    $('#btnSavePdf').click(function (e) {
        e.preventDefault();
    
        //save same file for all tags
        /*let cboxAlltags = document.querySelector('#cboxAllTags');
        if (cboxAlltags.checked) {

            //validate 


            let obj = {
                title: $('#titlePdf').val()
            }

            saveManualAllTags('Home/SaveAllTagsFile', obj)
            .then(data => console.log(data))
            .catch(error => console.log(error))
        }*/

        //validate

        //let title = $('#titlePdf').val();
        //let fileClabe = title + "-" + $('#FormControlTag option:selected').attr('value');
        let files = $('#filePdf').prop('files');

        for (let i = 0; i < $('.borrar-area').length; i++) {

            let idDepto = $('.borrar-area')[i].id;
            let idTag = $('#FormControlTag option:selected').attr('id');
            let idParent = $('#FormControlDepto option:selected').attr('value');
            
            for (let j = 0; j < files.length; j++) {

                let formData = new FormData();
                formData.append('idTag', idTag);
                formData.append('idParent', idParent);
                formData.append('idDepto', idDepto);
                formData.append('attachmentPost', files[j]);

                saveManual(formData);
            }
        }

        

        /*for (let i = 0; i < $('.borrar-area').length; i++) {

            //save all file formData
            for (let i = 0; i < files.length; i++) {
                console.log(files[i]);

                let idDepto = $('.borrar-area')[i].id
                let formData = new FormData();

                //formData.append('title', title)
                formData.append('idTag', idTag);
                formData.append('idParent', idParent);
                formData.append('idDepto', idDepto);
                //formData.append('fileClabe', fileClabe);

                let file = files[i];
                formData.append('attachmentPost', file[i]);
                saveManual(formData);
            }
        }*/
    });

    /*$('#cboxAllTags').change(function (e) {
        if (this.checked) {
            let divSelects = document.querySelector('#selectsForFile');
            divSelects.style.display = "none";
        }

        if (!this.checked) {
            let divSelects = document.querySelector('#selectsForFile');
            divSelects.style.display = "block";
        }

    });*/
});

function createMenu(menuInfo) {

    //console.log(menuInfo);
    let nav_home = document.querySelector('#nav_home');
    let subMenu;

    let tags_default = ['AD', 'GS', 'IN-C', 'IN-H', 'VTECH', 'VT', 'WP'];
    let tags = [];

    for (field in menuInfo) {
        let clabe = menuInfo[field][0].TagClabe;
        tags.push(clabe);
    }

    //console.log(tags);

    if (tags_default.length != tags.length) {

        for (let i = 0; i < tags.length; i++) {
            let t = tags[i];
            for (let j = 0; j < tags_default.length; j++) {

                if (tags_default[j] === t) {
                    tags_default.splice(j, 1);
                }
            }
        }

        //console.log(tags_default);
        //console.log(menuInfo);


        //GET TAGS FOR CONSULT
        //let result = getTagname(tags_default);
        //console.log(result);

        for (field in menuInfo) {

            let li = document.createElement('li');
            let a = document.createElement('a');
            let clabe = menuInfo[field][0].TagClabe;

            a.style.color = 'white';
            a.innerHTML = field;
            a.setAttribute('class', 'tags');
            a.setAttribute('value', clabe);
            a.setAttribute('href', '/Home/Attachment/?tag=' + clabe);
            li.append(a);
            nav_home.append(li);

            let item = menuInfo[field];
            subMenu = createSubMenu(field, item);
            //console.log(subMenu);
            li.append(subMenu);

        }

    } else {

        for (field in menuInfo) {

            let li = document.createElement('li');
            let a = document.createElement('a');
            let clabe = menuInfo[field][0].tagClabe;

            a.style.color = 'white';
            a.innerHTML = field;
            a.setAttribute('class', 'tags');
            a.setAttribute('value', clabe);
            li.append(a);
            nav_home.append(li);

            let item = menuInfo[field];
            subMenu = createSubMenu(field, item);
            //console.log(subMenu);
            li.append(subMenu);

        }
    }
}

function createSubMenu(field, item) {
    //console.log(field);
    //console.log(item);

    let submenu = document.createElement('div');
    submenu.setAttribute('id', field);
    let ul = document.createElement('ul');

    for (let i = 0; i < item.length; i++) {
        let li = document.createElement('li');
        let a = document.createElement('a');
        a.setAttribute('href', '/Home/Attachment/?tag=' + item[i].TagClabe);
        a.setAttribute('id', item[i].idDepto);
        a.setAttribute('class', 'submenuDepto');
        a.innerHTML = item[i].deptoName;
        li.appendChild(a);
        //li.innerHTML = item[i].deptoName;
        //li.setAttribute('id', item[i].idDepto);
        ul.appendChild(li);
    }

    submenu.appendChild(ul);
    submenu.style.display = 'none';

    return submenu;
}

function getTagname(tags_default) {
  
    $.ajax({
        url: '/Home/GetTagName/',
        type: 'GET',
        dataType: 'json',
        data: { Tags: JSON.stringify(tags_default) }
    })
}

function populateListArea(idDepto) {
    
    $.ajax({
        url: '/Home/GetAreas/?idDepto=' + idDepto,
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        asyn: 'true',
        processData: 'false',
        cache: 'false',
        success: function (response) {

            if (response.success) {
                let json = JSON.parse(response.data);
                let selectArea = document.querySelector('#FormControlArea');

                for (let i = 0; i < json.length; i++) {
                    let option = document.createElement('option');
                    option.innerHTML = json[i].Name;
                    option.setAttribute('value', json[i].Id);
                    selectArea.append(option);
                }
            } else {
                alert(response.msgError);
                $('#uploadFileModal').modal('hide');
                $modal = $('#uploadFileModal');
                $modal.find('form')[0].reset();
                $('#files').val('');
                $('#list').empty();
            }
            
        },
        error: function (e) {

        }

    });
}

function populateListDepto(brand) {

    $.ajax({
        url: '/Home/GetDeptos/?brand=' + brand,
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        asyn: 'true',
        processData: 'false',
        cache: 'false',
        success: function (response) {

            if (response.success) {

                let json = JSON.parse(response.data);
                let selectDepto = document.querySelector('#FormControlDepto');

                for (let i = 0; i < json.length; i++) {
                    let option = document.createElement('option');
                    option.innerHTML = json[i].Name;
                    option.setAttribute('value', json[i].IdDepto);
                    selectDepto.append(option);
                }

            } else {
                alert(response.msgError);
                $('#uploadFileModal').modal('hide');
                $modal = $('#uploadFileModal');
                $modal.find('form')[0].reset();
                $('#files').val('');
                $('#list').empty();
            }
            
        },
        error: function (e) {

        }
    });
}

function populateListTag() {

    $.ajax({
        url: '/Home/GetTags/',
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        asyn: 'true',
        processData: 'false',
        cache: 'false',
        success: function (response) {
            if (response.success) {
                //console.log(response);
                let json = JSON.parse(response.data);
                let selectTag = document.querySelector('#FormControlTag');

                //selectTag.innerHTML = "";

                for (let i = 0; i < json.length; i++) {
                    let option = document.createElement('option');
                    option.innerHTML = json[i].tagName;
                    option.setAttribute('value', json[i].clabe);
                    option.setAttribute('id', json[i].idTag);
                    selectTag.append(option);
                }

                $('#uploadFileModal').modal('show');
            } else {
                alert(response.msgError);
                return;
            }
        },
        error: function (e) {

        }
    });

}

function saveManual(fd) {

    if (document.querySelector('#titleModalManual').textContent === 'Editar Manual') {
        alert('Entra a Editar');

    } else {
        //alert("1");
        
        $.ajax({
            url: "/Home/SaveFilePdf/",
            data: fd,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (response) {

                console.log(response);
                if (response.success) {
                    //alert(response.msgError);
                    window.location.reload();
                    $('#myModal4').modal('hide');
                    $modal = $('#myModal4');
                    $modal.find('form')[0].reset();
                    $('#files').val('');
                    $('#list').empty();
                } else {
                    alert(response.msgError);
                }
            }
        });
    }


}

/*function saveManualAllTags(url, data) {
    return fetch(url, {
        method: 'POST',
        body: JSON.stringify(data),
        headers: new Headers({
            'Content-Type': 'application/json'
        }),
    })
    .then(response => response.json())
}*/