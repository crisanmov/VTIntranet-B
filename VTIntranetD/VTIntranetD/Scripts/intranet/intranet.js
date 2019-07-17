var UploadTags = {};

function generateTagList() {
    $.ajax({
        method: "GET"
   , url: "/intranet/generateTagList"
   , datatype: 'json'
    })
   .done(function (data, textStatus, jqXHR) {
       var items = data.data;
       $("#tags").empty();
       $.each(items, function (key, value) {

           $("#tags").append("<div> <input type='checkbox' class='etiquetas' id='" + value.tagName + '_' + value.idTag + "' value='" + value.idTag + "' name='" + value.tagName + "'>"
               + "<label for='" + value.tagName + '_' + value.idTag + "'> " + value.tagName + "</label></div> ");

           $("#" + value.tagName + '_' + value.idTag).click(function () {
               if ($("#" + value.tagName + '_' + value.idTag).is(':checked')) {
                   $("#" + value.tagName + '_' + value.idTag).attr("checked", "checked")
               }
               else {
                   $("#" + value.tagName + '_' + value.idTag).attr("checked", false)
               }
           });
       });

   })
   .fail(function (jqXHR, textStatus, errorThrown) {
       alert("error");
   });
}

function getChangeTagsList(idAttachment,name) {

    $.ajax({
        method: "GET"
   , url: "/intranet/generateTagList"
   , datatype: 'json'
    })
   .done(function (data, textStatus, jqXHR) {
       var items = data.data;

       $("#tagFileName").empty();
       $("#tagFileName").append(
                            '<div class="col-lg-12">' +
                                '<input type="hidden" value="' + idAttachment + '" name="idAttachment" id="hdfidAttachment" />' +
                                '<div class="col-lg-3" style="text-align: right;"><label>File name:</label></div>' +
                                '<div class="col-lg-9"><span id="fileName">' + name + '</span></div>' +
                            '</div>'
                        );
       $("#tagsChange").empty();
       $.each(items, function (key, value) {

           $("#tagsChange").append("<div> <input type='checkbox' class='etiquetasChange' id='ch_" + value.idTag + "' value='" + value.idTag + "' name='" + value.tagName + "'>"
               + "<label for='ch_" + value.idTag + "'> " + value.tagName + "</label></div> ");

           $("#ch_" + value.idTag).click(function () {
               if ($("#ch_" + value.idTag).is(':checked')) {
                   $("#ch_" + value.idTag).attr("checked", "checked")
               }
               else {
                   $("#ch_" + value.idTag).attr("checked", false)
               }
           });
       });

       if (idAttachment != 0) {
           getTagsByidAttachment(idAttachment);
       }

   })
   .fail(function (jqXHR, textStatus, errorThrown) {
       alert("error");
   });
}

function getTagsByidAttachment(idAttachment) {

    $.ajax({
        method: "GET"
            , url: "/intranet/getTagsByidAttachment"
            , data: { idAttachment: idAttachment }
            , datatype: 'json'
    })
            .done(function (data, textStatus, jqXHR) {
                var items = data.data;
                var etiquetasChange = $(".etiquetasChange");

                $(etiquetasChange).each(function () {
                    var input = $(this).prop("id");
                    $.each(items, function (key, value) {
                        var valor = "ch_" + value.value;
                        if (input == valor && value.attachmentTagActive == true) {
                            $('#' + input).attr('checked', true);
                            $('#' + input).val(value.value);
                        }
                    });
                });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                alert("error");
            });
}


function saveChangeTags() {

    var attachentTags = {};
    var tagsChange = $(".etiquetasChange");

    $.each(tagsChange, function (key, valor) {
        var changeTag = new Object();

        changeTag.idAttachment = $("#hdfidAttachment").val();
        changeTag.idTag = valor.value;
        changeTag.attachmentTagsActive = valor.checked;
        attachentTags[key] = changeTag;
    });
        $.ajax({
            url: "/intranet/attachmenTagsSave",
            type: 'POST',
            datatype: 'json',
            data: {
                idAttachment :$("#hdfidAttachment").val(),
                attachtags: attachentTags
            },
            success: function (response) {
                if (response.success === true) {

                    $("#infoModal").notify("The information has been added correctly",
                        {
                            position: "top right",
                            className: "success",
                            hideDuration: 500
                        });
                    Search();
                }
                else {
                    $("#infoModal").notify("Problems at captured the information:" + response.message,
                        {
                            position: "top right",
                            className: "error",
                            hideDuration: 2000
                        });
                }
            }
        });
}


function addUpload() {
    var count = $("#tfiles tbody > tr").length;
    var id = count + 1;
    $("#tfiles").append(
    '<tr row_id=' + id + '><td><div class="input-group">'
                          + '<label class="input-group-btn">'
                                + '<span class="btn btn-default">'
                                 + '<span style="font-size:9pt;">Select File</span>'
                                 + '<input type="file" class="fileUpload" name="fileUpload" id="fileUpload_' + id + '" style="display: none;" />'
                              + '</span>'
                           + '</label>'
                           + '<input id="file_' + id + '" type="text" class="form-control filelabel" value="No file selected" readonly />'
                       + '</div></td>'
                       +'<td>'
                                       + ' <a id="delete" title="Delete file" class="btn btn-default btn_delete">x</a>'
                                   +'</td>'
                       + '</tr>');


    $(function () {

        // We can attach the `fileselect` event to all file inputs on the page
        $(document).on('change', ':file', function () {
            var input = $(this),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            input.trigger('fileselect', [numFiles, label]);
        });

        // We can watch for our custom `fileselect` event like this
        $(document).ready(function () {
            $(':file').on('fileselect', function (event, numFiles, label) {

                var input = $(this).parents('.input-group').find(':text'),
                    log = numFiles > 1 ? numFiles + ' files selected' : label;

                if (input.length) {
                    input.val(log);
                } else {
                    if (log) alert(log);
                }

            });
        });

    });

    $(".btn_delete").on('click', function (event) {
        var tbl_row = $(this).closest('tr');
        var id = tbl_row.attr('row_id');
        tbl_row.remove();
    });
}

function deleteUpload() {

    var tbl_row = $(this).closest('tr');
    var id = tbl_row.attr('row_id');
    tbl_row.remove();
}

 function uploapFiles() {
    event.preventDefault();
    var files = new Array();
    var input = $('.fileUpload');
    for (var i = 0; i < input.length; i++) {
        $.each(input[i].files, function (key, valor) {
            files.push(valor);
        });
    }
    
    var data = new FormData();

    // Add the uploaded image content to the form data collection 
    if (files.length > 0) {
        //data.append('UploadedImage', files);
        $.each(files, function (key, valor) {
            data.append('UploadedImage', files[key]);
        });

    }
    //var tags = $(".etiquetas")
    var myJSON = "";

    myJSON = getTags($(".etiquetas"));

    myJSON = "[" + myJSON + "]";

    if ($(".etiquetas").is(":checked") == false) {
        $("#info").notify(
                            "You should select a tag type to load the file.",
                            {
                                position: "top right",
                                className: "warn",
                                hideDuration: 500
                            });
    }

    else {

        // Make Ajax request with the contentType = false, and procesDate = false 
        var ajaxRequest = $.ajax({
            type: 'POST',
            url: '/intranet/AttachFilePropertieAjax?attachtags=' + myJSON + '',
            contentType: false,
            processData: false,
            data: data
        });
        ajaxRequest.done(function (xhr, textStatus) {
            if (xhr.success === false) {
                $("#info").notify(
                    xhr.message,
                    {
                        position: "top right",
                        className: "warn",
                        hideDuration: 1500
                    });
            }
            else {
                $("#info").notify(
                    "The file has been loaded correctly.",
                    {
                        position: "top right",
                        className: "success",
                        hideDuration: 500
                    });

                $('.etiquetas').attr("checked", false);
                $('input:checkbox').removeAttr('checked');
                $('.fileUpload').val('');
                $(".filelabel").val("No file selected");
                generateTagList();
                getAttachments();
                $("#fileUpload_1").empty();
                $("#filesName ul").empty();
            }

        });
        ajaxRequest.fail(function (jqXHR, textStatus, errorThrown) {

            if (jqXHR.status == 500) {
                myObj = JSON.parse(this.responseText);
                $("#info").notify(
                    myObj.title,
                    {
                        position: "top right",
                        className: "warn",
                        hideDuration: 1500
                    });
            }
        });
    }

 }

 function Search() {
     var AttachmentID = $("#idAttachment").val()
     var fileName = $("#attachmentName").val()
     var UploaddateStart = $("#attachmentDateStart").val();
     var UploaddateEnd = $("#attachmentDateEnd").val();
     var myJSON = "";
     var count = $(".etiquetas").length;

     if (UploaddateStart == "" && UploaddateEnd != "") {
         $("#attachmentDateStart").addClass("required")
         $(".required").css({
             "border-color": "red"
         });
     }
     if (UploaddateStart != "" && UploaddateEnd == "") {
         $("#attachmentDateEnd").addClass("required")
         $(".required").css({
             "border-color":"red"
         });
     }

     $.each($(".etiquetas"), function (key, valor) {
         var tag = new Object();

         if (valor.checked == true) {

             var cadena = count - 1 == key ? "{ idTag: " + valor.value + " }" : "{ idTag: " + valor.value + "},";
             myJSON += cadena;
         }
     });

     myJSON = "[" + myJSON + "]";

    $.ajax({
    method: "POST"
   , url: "/intranet/searchFiles?idAttachment=" + AttachmentID + "&attachmentName=" + fileName + "&attchtags=" + myJSON + "&uploadDateStart=" + UploaddateStart + "&uploadDateEnd=" + UploaddateEnd
   , datatype: 'json'
     })
   .done(function (data, textStatus, jqXHR) {
       var items = data.data;
       //console.log(items)
       $("#keysAttach tbody").empty();
       $.each(items, function (key, value) {
           var id = key + 1;
           $("#keysAttach tbody").append("<tr>"
               + '<td><a title="Edit file" id="attachModal_' + id + '" class="attachIcon"><span class="glyphicon glyphicon-edit"></span></a></td>'
               + "<td>" + value.idAttachment + "</td>"
               + "<td>" + value.attachmentName + "</td>"
               + "<td>" + value.attachmentTagsName + "</td>"
               + "<td>" + value.attachmentDateLastChange + "</td>"
               + "<td> <a class='fileUrl' id='file" + value.idAttachment + "' title='See file'><span class='glyphicon glyphicon-eye-open'></span></a></td>"
               + "</tr>");

           $("#file"+value.idAttachment).click(function () {
               viewAttachment(value.idAttachment, value.attachmentUrl)
           });

           $("#attachModal_" + id).click(function () {
               UpdateAttachmentTags(value.idAttachment, value.attachmentName);
               

           });
           $(".attachIcon").css("cursor", "pointer");
           $(".fileUrl").css("cursor", "pointer");
       });

   })
   .fail(function (jqXHR, textStatus, errorThrown) {
       alert("error");
   });
 }

 function UpdateAttachmentTags(idAttachment,name) {
     $.ajax({
         url: "/intranet/UpdateAttachmentTags",
         type: 'POST',
         datatype: 'json',
         data: {
             idAttachment: idAttachment
         },
         success: function (response) {
             if (response.success === true) {
                 getChangeTagsList(idAttachment, name);
                 $('#myModal').modal('show');
             }
             else {
                 if (response.data == null && response.message == "You do not have permission to perform this action") {
                     window.location.href = '/Account/NoAccess';
                 } else {
                     $("#info").notify("Problems at captured the information:" + response.message,
                           {
                               position: "top right",
                               className: "error",
                               hideDuration: 2000
                           });
                 }
             }
         }
     });
 }

function getAttachments() {
    $.ajax({
        method: "GET"
   , url: "/intranet/getAttachments"
   , datatype: 'json'
    })
   .done(function (data, textStatus, jqXHR) {
       var items = data.data;
       $("#keysAttach tbody").empty();
       if (items != null) {
           $.each(items, function (key, value) {
               var id = key + 1;
               $("#keysAttach tbody").append("<tr>"
                   + "<td>" + value.idAttachment + "</td>"
                   + "<td>" + value.attachmentName + "</td>"
                   + "<td>" + value.attachmentTagsName + "</td>"
                   + "<td>" + value.attachmentDateLastChange + "</td>"
                   + "</tr>");
           });
       }

   })
   .fail(function (jqXHR, textStatus, errorThrown) {
       alert("error");
   });
}

function getAttachmentsSearch() {
    $.ajax({
        method: "GET"
   , url: "/intranet/getAttachments"
   , datatype: 'json'
    })
   .done(function (data, textStatus, jqXHR) {
       var items = data.data;
       $("#demo").empty();
       //console.log(items)
       $("#demo").append(
         '<table class="table table-bordered table-striped" id="keysAttach">' +
             '<thead>' +
                 '<tr>' +
                     '<th></th>'+
                     '<th>File id</th>' +
                     '<th>File Name</th>' +
                     '<th>Tags</th>' +
                     '<th>Upload Date</th>' +
                     '<th>Action</th>' +
                 '</tr>' +
             '</thead>' +
             '<tbody style="font-size: 10px;"></tbody>' +
         '</table>');

       $("#keysAttach tbody").empty();


       $.each(items, function (key, value) {
           var id = key + 1;
           $("#keysAttach tbody").append("<tr>"
               + '<td><a title="Edit file" id="attachModal_' + id + '" class="attachIcon"><span class="glyphicon glyphicon-edit"></span></a></td>'
               + "<td>" + value.idAttachment + "</td>"
               + "<td>" + value.attachmentName + "</td>"
               + "<td>" + value.attachmentTagsName + "</td>"
               + "<td>" + value.attachmentDateLastChange + "</td>"
               + "<td> <a class='fileUrl' id='file" + value.idAttachment + "' title='See file'><span class='glyphicon glyphicon-eye-open'></span></a></td>"
               + "</tr>");
           $("#file" + value.idAttachment).click(function () {
               viewAttachment(value.idAttachment, value.attachmentUrl)
           });
           $("#attachModal_" + id).click(function () {
               UpdateAttachmentTags(value.idAttachment, value.attachmentName);              
           });
           $(".attachIcon").css("cursor", "pointer");
           $(".fileUrl").css("cursor", "pointer");
       });

   })
   .fail(function (jqXHR, textStatus, errorThrown) {
       alert("error");
   });
}



function viewAttachment(id,Url) {
    $.ajax({
        method: "GET"
        , url: "/intranet/GenerateFile"
        , datatype: 'json'
        , data: { idAttachment: id }
    })
     .done(function (data, textStatus, jqXHR) {
                                    var items = data.data;
                                    var $iframe = $("#openFile");
                                    if (items == true) {
                                        $("#txtFile").css("display", "none");
                                        
                                            $iframe[0].src =  Url
                                       
                                    } else {
                                        $("#txtFile").css("display", "block");
                                        $iframe[0].src = "";
                                    }
                                    $("#modalDocs").modal('show');

                                })
       .fail(function (jqXHR, textStatus, errorThrown) {
             alert("error");
      });
}

function getTags(tags) {
    var myJSON = "";
    var count = tags.length;

    $.each(tags, function (key, valor) {
        var tag = new Object();
        tag.idAttachmentTag = 0;
        tag.idAttachment = 0;
        tag.idTag = valor.value;
        tag.attachmentTagsActive = valor.checked;

        UploadTags[key] = tag;

        var cadena = count - 1 == key ? "{ idAttachment: 0, idTag: " + valor.value + ", attachmentTagsActive: " + valor.checked + " }" : "{ idAttachment: 0, idTag: " + valor.value + ", attachmentTagsActive: " + valor.checked + " },";
        myJSON += cadena;
    });

    return myJSON;
}


//window.onload = function () {
//    document.getElementById('upload').onsubmit = function () {
//        var formdata = new FormData(); //FormData object
//        var fileInput = document.getElementById('fileUpload');
//        //Iterating through each files selected in fileInput
//        for (i = 0; i < fileInput.files.length; i++) {
//            //Appending each file to FormData object
//            formdata.append(fileInput.files[i].name, fileInput.files[i]);
//        }
//        var tags = $(".etiquetas")
//        $.each(tags, function (key, valor) {
//            var tag = new Object();

//            tag.idAttachmentTag = 0;
//            tag.idAttachment = 0;
//            tag.idTag = valor.value;
//            tag.attachmentTagsActive = valor.checked;
//            UploadTags[key] = tag;

//        });
//        //Creating an XMLHttpRequest and sending
//        var xhr = new XMLHttpRequest();
//        xhr.open('POST', '/intranet/AttachFilePropertieAjax');


//        xhr.send(formdata, UploadTags);
//        xhr.onreadystatechange = function () {
//            if (xhr.readyState == 4 && xhr.status == 200) {
//                alert(xhr.responseText);
//            }
//        }
//        return false;
//    }
//}