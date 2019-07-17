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
        //alert('push');
        $('#uploadFileModal').modal('show');
        //console.log(brands);
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
        //document.getElementById('FormControlDepto').innerHTML = "";
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

        //return;
        e.preventDefault();
        //validate form


        let title = $('#titlePdf').val();
        let fileClabe = title + "-" + $('#FormControlTag option:selected').attr('value');

        let files = $('#filePdf').prop('files');

        let idTag = $('#FormControlTag option:selected').attr('id');
        let idParent = $('#FormControlDepto option:selected').attr('value');
        //let fileClabe = brand_clabe + '-' + depto_clabe;

        for (let i = 0; i < $('.borrar-area').length; i++) {

            let idDepto = $('.borrar-area')[i].id
            let formData = new FormData();

            formData.append('title', title)
            formData.append('idTag', idTag);
            formData.append('idParent', idParent);
            formData.append('idDepto', idDepto);
            formData.append('fileClabe', fileClabe);

            //save all file formData
            for (let i = 0; i < files.length; i++) {
                let file = files[i];
                formData.append('attachmentPost', file);
            }

            //save Attachment
            saveManual(formData);
        }
    });



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

        let result = getTagname(tags_default);
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
    //console.log(tags_default);
    //console.log({ Tags: tags_default });

    $.ajax({
        url: '/Home/GetTagName/',
        type: 'GET',
        dataType: 'json',
        data: { Tags: JSON.stringify(tags_default) }
    })
}

function populateListArea(idDepto) {
    //console.log(idDepto);

    $.ajax({
        url: '/Home/GetAreas/?idDepto=' + idDepto,
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        asyn: 'true',
        processData: 'false',
        cache: 'false',
        success: function (data) {

            //clearSelect();
            let json = JSON.parse(data);
            let selectArea = document.querySelector('#FormControlArea');

            for (let i = 0; i < json.length; i++) {
                let option = document.createElement('option');
                option.innerHTML = json[i].Name;
                option.setAttribute('value', json[i].Id);
                selectArea.append(option);
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
        success: function (data) {
            //clearSelect();
            let json = JSON.parse(data);
            let selectDepto = document.querySelector('#FormControlDepto');

            for (let i = 0; i < json.length; i++) {
                let option = document.createElement('option');
                option.innerHTML = json[i].Name;
                option.setAttribute('value', json[i].IdDepto);
                selectDepto.append(option);
            }
        },
        error: function (e) {

        }
    });
}

function populateListTag() {
    //alert('populateListTag');
    $.ajax({
        url: '/Home/GetTags/',
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        asyn: 'true',
        processData: 'false',
        cache: 'false',
        success: function (data) {
            let json = JSON.parse(data);
            let selectTag = document.querySelector('#FormControlTag');

            for (let i = 0; i < json.length; i++) {
                let option = document.createElement('option');
                option.innerHTML = json[i].tagName;
                option.setAttribute('value', json[i].clabe);
                option.setAttribute('id', json[i].idTag);
                selectTag.append(option);
            }
        },
        error: function (e) {

        }
    });

}

function saveManual(fd) {

    if (document.querySelector('#titleModalManual').textContent === 'Editar Manual') {
        alert('Vamos a Editar');

    } else {
        $.ajax({
            url: "SaveFilePdf",
            data: fd,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (response) {
                //console.log(response);
                if (response === "successfully") {

                    window.location.reload();
                    $('#myModal4').modal('hide');
                    $modal = $('#myModal4');
                    $modal.find('form')[0].reset();
                    $('#files').val('');
                    $('#list').empty();
                }
            }
        });
    }


}