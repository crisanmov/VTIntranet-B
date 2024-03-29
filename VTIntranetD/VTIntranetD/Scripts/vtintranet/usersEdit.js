﻿'use strict'

var jsonTags = JSON.parse(tags.replace(/&quot;/g, '"'));
var jsonUserPermissions = JSON.parse(permsUser.replace(/&quot;/g, '"'));

//console.log(jsonTags);

var tagObject = {};
var userPerObject = {};

var brand = [];
var brand2 = [];
//Array for save states to each modal areas
var deptos = [];
var stateAreaChecks = 0;

//all Tags
brand = getBrands(jsonTags);
brand = findDuplicateValues(brand);
tagObject = getBrandDepto(jsonTags, brand);

//tags profile user in db
brand2 = getBrands(jsonUserPermissions);
brand2 = findDuplicateValues(brand2);
userPerObject = getBrandDepto(jsonUserPermissions, brand2);

//draw matrix
drawMatTag(tagObject, userPerObject);


$(document).ready(function () {

    $('.btn_control').on('click', function (e) {
        e.preventDefault();
        let btn = $(this);
        let btn_state = btn[0].innerHTML;

        if (btn_state === 'OFF') {
            console.log("OFF->ON");
            let area = btn.next()[0].innerHTML;
            let idTag = btn[0].value;
            let idDepto = btn.next()[0].id;

            console.log(btn);
            console.log(area);
            console.log(idTag);
            console.log(idDepto);
            //get Areas from Database
            getAreas(idTag, idDepto, area, btn);
        } else {
            btn_disable(btn);
            popDepto(btn.next()[0].innerHTML);
        }
    });

    $('.add').on('click', function (e) {
        console.log(".add");
        if ($('#area_content').childElementCount != 0) {
            document.querySelector('#area_content').innerHTML = '';
        }
        let p = $(this)[0].id;
        $('#selectAreas').modal('show');
        console.log(p);
        setInfoModal(p);
    });

    $('.close').click(function () {
        clear_modal();
    });

    $('#chunche').click(function (e) {
        e.preventDefault();
        console.log(deptos);
    });

    $('#saveUser').click(function (e) {
        e.preventDefault();

        //data personal user
        let name = $('#name').val();
        let lastP = $('#lastNameP').val();
        let lastM = $('#lastNameM').val();
        //data laboral user
        let profileN = $('#profileN').val();
        let skype = $('#skype').val();
        let userActive = $('#userActive').val();
        //data account
        let username = $('#username').val();
        let rolName = $('#rolName').val();
        let pass1 = $('#password').val();
        let pass2 = $('#password2').val();

        //save user
        let user = {
            name: name,
            lastNameP: lastP,
            lastNameM: lastM,
            skype: skype,
            userActive: userActive,
            username: username,
            password: pass1,
            password2: pass2,
        };

        validateForm(user, profileN, rolName);

    });

});

function btn_enable(btn) {

    btn.removeClass('btn_off');
    btn.parent().removeClass('disable');
    btn[0].innerHTML = 'ON';
    btn.addClass('btn_on');
    btn.parent().addClass('enable');
    btn.prev()[0].style.display = 'block';

    $('#selectAreas').modal('show')
}

function btn_disable(btn) {

    btn.prev()[0].style.display = 'none';
    btn.removeClass('btn_on');
    btn.parent().removeClass('enable');
    btn[0].innerHTML = 'OFF';
    btn.addClass('btn_off');
    btn.parent().addClass('disable');

    clear_modal();
}

function clear_modal() {

    $("#area_content").html("");
    $("#area").html("");
}

function drawMatTag(object_tag_all, object_tag_user) {
    
    for (let field in object_tag_all) {

        let tagNameG = field;
        if (/\s/.test(tagNameG)) { tagNameG = tagNameG.replace(/\s+/g, ''); }

        for (let field2 in object_tag_user) {

            let tagNameU = field2;
            if (/\s/.test(tagNameU)) { tagNameU = tagNameU.replace(/\s+/g, ''); }

            if (tagNameG == tagNameU) {
                for (let i = 0; i < object_tag_all[field].length; i++) {
                    for (let j = 0; j < object_tag_user[field2].length; j++) {
                        if (object_tag_all[field][i].idDepto == object_tag_user[field2][j].idDepto) {
                            object_tag_all[field][i].active = 1;
                        }
                    }
                }
            }
        }
    }

    drawMatrixEnableUser(object_tag_all);
   
}

function drawMatrixEnableUser(object_tag_all) {

    let index = 0;

    for (let field in object_tag_all) {

        let table = document.querySelector('#mtags');
        let row = field;
        let idTag = object_tag_all[field][0].idTag;

        if (/\s/.test(row)) {
            row = row.replace(/\s+/g, '');
        }

        let tr = document.createElement('tr');
        tr.setAttribute('id', row);
        let th = document.createElement('th');
        th.innerHTML = field;
        tr.append(th);
        table.append(tr);

        let array = object_tag_all[field];
        let idRow = table.rows[index].getAttribute('id');

        for (let i = 0; i < array.length; i++) {
            if (array[i].active == 1) {
                console.log(array[i]);

                let tr_tag = document.querySelector('#' + idRow);
                let depto = array[i].deptoName;
                let td = document.createElement('td');
                let btn = document.createElement('button');
                let p = document.createElement('p');
                let add = document.createElement('i');

                add.style.display = 'block';
                add.setAttribute('class', 'fas fa-clipboard-list add');
                td.setAttribute('class', 'enable');
                btn.setAttribute('value', idTag);
                btn.setAttribute('class', 'btn_control btn_on');
                btn.innerHTML = 'ON';
                p.setAttribute('id', array[i].idDepto);
                p.innerHTML = depto;

                td.append(add);
                td.append(btn);
                td.append(p);
                tr_tag.append(td);

            } else {

                let tr_tag = document.querySelector('#' + idRow);
                let depto = array[i].deptoName;
                let td = document.createElement('td');
                let btn = document.createElement('button');
                let p = document.createElement('p');
                let add = document.createElement('i');

                add.style.display = 'none';
                add.setAttribute('class', 'fas fa-clipboard-list add');
                td.setAttribute('class', 'disable');
                btn.setAttribute('value', idTag);
                btn.setAttribute('class', 'btn_control btn_off');
                btn.innerHTML = 'OFF';
                p.setAttribute('id', array[i].idDepto);
                p.innerHTML = depto;

                td.append(add);
                td.append(btn);
                td.append(p);
                tr_tag.append(td);
            }
        }

        index++;
    }

}

function getAreas(idTag, idDepto, area, btn) {

    return $.ajax({
        url: '/Users/GetAreas/',
        type: 'GET',
        dataType: 'json',
        data: { idDepto: idDepto },
        success: function (response) {

            console.log(response);
            if (response.success) {
                const depto = new Depto(idTag, area, response.data);
                let index = deptos.length;
                console.log(index);
                deptos.push(depto);

                //set id in icon with class add for to able search areas with checbox
                btn.prev()[0].id = index;
                let keys = Object.keys(depto.getAreas());

                if ($('#area_content').childElementCount != 0) {
                    document.querySelector('#area_content').innerHTML = '';
                }

                for (let i = 0; i < keys.length; i++) {

                    let tmp = depto.getArea(i);
                    setModal(tmp.IdDepto, tmp.Name, tmp.State, area, index);
                }

                btn_enable(btn);

            } else {
                alert(response.msgError);
            }
        }
    });
}

function getPermissions(deptos) {

    let tags = [];
    let idTag = "";

    //get tags
    for (let i = 0; i < deptos.length; i++) {
        idTag = deptos[i].idDepto;
        let idDepto = deptos[i].areas[0].IdParent;
        let idParent = 0;
        let depto = { idTag: idTag, idParent: idParent, idDepto: idDepto };
        tags.push(depto);

    }

    //get deptos
    for (let j = 0; j < deptos.length; j++) {
        let idTagD = deptos[j].idDepto;
        let keys = Object.keys(deptos[j].areas);

        for (let k = 0; k < keys.length; k++) {
            if (deptos[j].areas[k].State) {
                let idParentD = deptos[j].areas[k].IdParent;
                let idDeptoD = deptos[j].areas[k].IdDepto;
                let area = { idTag: idTagD, idParent: idParentD, idDepto: idDeptoD };
                tags.push(area);

            }
        }
    }
    return tags;
}

function popDepto(name) {

    let tmp = [];
    for (let i = 0; i < deptos.length; i++) {
        if (deptos[i].getDeptoName() != name) {
            tmp.push(deptos[i]);
        }
    }

    deptos = "";
    deptos = tmp;
}

function saveUser(user, nameProfile, deptosD, rolName) {

    //var token = $('input[name=__RequestVerificationToken]').val();
    //console.log(token);

    $.ajax({
        url: "Create",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ user: user, nameProfile: nameProfile, deptosD: deptosD, rolName: rolName }),
        success: function (response) {
            console.log(response);
            if (response.success) {
                alert(response.msg);
                $('#__AjaxAntiForgeryForm')[0].reset();
                window.location.reload();
            } else {
                alert(response.msgError);
            }
        },
        error: function (e) {
            alert(e);
        }

    });

}

function setInfoModal(index) {

    console.log(index);

    let depto = deptos[index];
    let area = depto.getDeptoName();
    let keys = Object.keys(depto.getAreas());

    for (let i = 0; i < keys.length; i++) {
        let tmp = depto.getArea(i);

        console.log(tmp);
        setModal(tmp.IdDepto, tmp.Name, tmp.State, area, index);
    }

}

function setModal(idDepto, name, state, area, index) {

    let div = document.createElement('div');
    let input = document.createElement('input');
    let lbl = document.createElement('label');
    let span = document.createElement('span');

    div.setAttribute('class', 'form-check');
    input.setAttribute('type', 'checkbox');
    input.setAttribute('onclick', 'setStateCheck(' + index + ', ' + '"' + name + '"' + ')');
    input.setAttribute('value', 'false');

    if (state) {
        input.setAttribute('checked', state);
    } else {
        input.setAttribute('unchecked', true);
    }

    input.setAttribute('id', idDepto);
    lbl.setAttribute('class', 'form-check-label');
    lbl.innerHTML = name;
    span.setAttribute('class', 'in');
    span.innerHTML = index;
    span.style.display = 'none';
    div.append(input);
    div.append(lbl);

    document.querySelector('#area').innerHTML = 'Departamento: ' + area;
    document.querySelector('#area_content').append(div);
    document.querySelector('#area').append(span);
}

function setStateCheck(index, name) {
    console.log(index + " " + name);
    
    let indexArea = deptos[index].getIndexArea(name);

    if (!deptos[index].getStateArea(indexArea)) {
        deptos[index].setStateArea(indexArea, true);
        stateAreaChecks = 1;
    } else {
        deptos[index].setStateArea(indexArea, false);
        stateAreaChecks = 0;
    }

}

function validateForm(user, profileN, rolName) {

    let res1 = validateBlankSpaces(user);
    let res2 = validatePass(user);
    let res3;

    if (validateBlank(profileN) && validateBlank(rolName)) {
        res3 = true;
    } else {
        res3 = false;
    }

    if (profileN == "seleccion" || rolName == "seleccion") {
        alert("Falta un campo de SELECCIONAR");
        return false;
    }

    if ((res1 && res2) && res3) {
        //prepare array rows databases
        let deptosD = getPermissions(deptos);

        if (deptosD != "" && stateAreaChecks != 0) {
            saveUser(user, profileN, deptosD, rolName);
        } else {
            alert("Debes habilitar al menos un material de acceso.");
        }
    } else {
        alert("ERROR AL ENVIAR EL FORMULARIO");
    }
}

function validateBlank(field) {

    if (field == "") {
        alert("El Formulario NO debe contener campos vacios.");
        return false;
    }

    return true;
}

function validateBlankSpaces(user) {

    for (field in user) {

        if (user[field] == "") {
            alert("El Formulario NO debe contener campos vacios.");
            return false;
        }
    }

    return true;
}

function validatePass(user) {

    if (user.password.length < 8 || user.password2.length < 8) {
        alert("La contraseña debe tener una longitud de 8 caracteres");
        return false;
    }

    if (user.password == user.password2) {
        return true;
    } else {
        alert("Error: Las contraseñas NO coinciden.");
        return false;
    }


}