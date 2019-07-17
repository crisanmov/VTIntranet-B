document.addEventListener('DOMContentLoaded', function () {

    let json = JSON.parse(notices.replace(/&quot;/g, '"'));
    let events = setEventsCalendar(json);

    showCalendar(events);
    setDialogNotice(json);
    setNoticeClick();

    function getNotice(idNotice) {
        //console.log('Entra a consultar noticia');
        //console.log(idNotice);

        $.ajax({
            url: 'GetNotice',
            dataType: 'json',
            data: JSON.stringify({ idNotice: idNotice }),
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            async: true,
            processData: false,
            cache: false,
            success: function (data) {

                console.log(data);
                let title = data['Title'];
                let description = data['Description'];
                let date = data['StartDateNotice'];
                let date2 = data['EndDateNotice'];
                date = date.substring(0, date.length - 14);
                date2 = date2.substring(0, date2.length - 14);
                /*date = date.substring(6, date.length - 2);
                date2 = date2.substring(6, date2.length - 2);
                date = Number(date);
                date2 = Number(date2);*/

                //let startDate = new Date(date).toISOString().slice(0, 10).replace('T', ' ');
                //let endtDate = new Date(date2).toISOString().slice(0, 10).replace('T', ' ');

                $('#myModal2').modal('show');
                $('#titleNotice').text(title);
                $('#descriptionNotice').text(description);
                $('#startNotice').text(date);
                $('#endNotice').text(date2);
            },
            error: function (e) {
                //console.log(e);
            }
        });
    }

    function setEventsCalendar(json) {

        let noticeObj = [];

        //Parse Json Dates start and end notice
        for (let key in json) {

            let date = json[key].StartDateNotice;
            let date2 = json[key].EndDateNotice;
            date = date.substring(0, date.length - 14);
            date2 = date2.substring(0, date2.length - 14);
            date = date.match(/\d+/g);
            date2 = date2.match(/\d+/g);
            date = date[2] + "-" + date[1] + "-" + date[0];
            date2 = date2[2] + "-" + date2[1] + "-" + date2[0];

            //date2 = date2.substring(6, date2.length - 4);
            //let startDate = new Date(date).toISOString().slice(0, 10).replace('T', ' ');
            //console.log(date);
            //console.log(date2);

            json[key].StartDateNotice = date;
            json[key].EndDateNotice = date2;

        }

        //create json events for set calendar
        for (let key in json) {

            let str = {};
            let id;
            let title;
            let start;
            let end;

            id = json[key].IdNotice;
            title = json[key].Title;
            start = json[key].StartDateNotice;
            end = json[key].EndDateNotice;

            if (start == end) {
                //console.log(title);
                //console.log("fechas iguales");
                str.id = id.toString();
                //str.resourceId = 'a',
                str.title = title;
                str.start = start;
                //str.url = 'http://google.com/';
                noticeObj.push(str);

            } else {
                str.id = id.toString();
                str.title = title;
                str.start = start;
                str.end = end;

                noticeObj.push(str);
            }
        }

        return noticeObj;
    }

    function showCalendar(events) {

        var calendarEl = document.getElementById('calendar');

        var calendar = new FullCalendar.Calendar(calendarEl, {
            plugins: ['interaction', 'dayGrid'],
            header: {
                left: 'prevYear,prev,next,nextYear today',
                center: 'title',
                right: 'dayGridMonth,dayGridWeek,dayGridDay'
            },
            //defaultDate: '2019-03-12',
            navLinks: true, // can click day/week names to navigate views
            editable: false,
            eventLimit: true, // allow "more" link when too many events
            events: events,
            eventClick: function (event, jsEvent, view) {
                //console.log(event.id);
                //console.log(event.title);
            }






        });
        calendar.render();
        calendar.setOption('locale', 'es');
        calendar.on('dateClick', function (info) {
            console.log('clicked on ' + info.dateStr);

        });
    }

    function setDialogNotice(json) {
        //console.log('entra a setDialogNotice');
        //console.log(json);
        let div_notices = document.querySelector('#news');

        for (let i = 0; i < json.length; i++) {

            let startDate = json[i].StartDateNotice;
            //console.log(startDate);
            let tmp = startDate.split("-");
            //console.log(tmp);

            //let date = new Date(tmp[2], tmp[1], tmp[0]);
            let date = new Date(json[i].StartDateNotice);
            //console.log(date);
            let months = ['ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DEC'];
            let month = months[date.getMonth()];
            //console.log(month);
            //console.log(date);

            let div_notice = document.createElement('DIV');
            let div_date = document.createElement('DIV');
            let div_title = document.createElement('DIV');
            let notice = document.createElement('a');
            let p = document.createElement('p');

            p.innerHTML = month;
            div_notice.setAttribute('class', 'novedades');
            div_date.setAttribute('id', json[i].IdNotice);
            div_date.setAttribute('class', 'borrar');
            div_date.innerHTML = tmp[2];
            div_date.append(p);

            notice.setAttribute('class', 'newDetails');
            notice.setAttribute('href', json[i].IdNotice);
            notice.innerHTML = json[i].Title;

            div_title.setAttribute('class', 'anuncios');

            div_title.append(notice);
            div_notice.append(div_date);
            div_notice.append(div_title);

            //console.log(div_notice);
            div_notices.appendChild(div_notice);

        }

    }

    function setNoticeClick() {
        let notices = document.querySelectorAll('.newDetails');
        //console.log(notices);

        for (let i = 0; i < notices.length; i++) {
            $(notices[i]).click(function (e) {
                e.preventDefault();

                let idNew = $(this).attr('href');
                getNotice(idNew);
            });
        }

    }

});

$('#title').on('focus', function (e) {
    $(this).attr("autocomplete", "off");
});

$('#oneDateChecked').change(function () {

    if (($('#startDateNews').val() != "") || ($('#endDateNews').val() != "")) {
        //$('#startDateNews').val('');
        $('#endDateNews').val('');
    }

    if ($(this).prop('checked')) {
        $('#endDateNews').prop('disabled', true);
    } else {
        $('#endDateNews').prop('disabled', false);
    }
});

$('#btnClose').click(function (e) {
    $modal = $('#myModal');
    $modal.find('form')[0].reset();

    $("p").remove();

});

$('#btnSaveNotice').click(function (e) {
    // console.log("entra save notice");
    e.preventDefault();

    if ($('#msgValidate').length) {
        $("p").remove();
    }

    let title = $('#title').val();
    let description = $('#description').val();
    let startDateNews = $('#startDateNews').val();
    let endDateNews = $('#endDateNews').val();
    let isEvent = false;

    //checked just oney day
    if ($('#oneDateChecked').is(":checked")) {

        validateControl(title, description, startDateNews);

        //console.log("entra is checked");
        //dateNow
        let dateNow = Date(Date.now());
        //console.log("Today: " + dateNow.toString());
        //dateSelect
        let startDate = $j("#startDateNews").val();
        //console.log("day select: " + startDate);
        let dateSelect = new Date(startDate).toISOString().slice(0, 19).replace('T', ' ');
        //console.log("day select: " + dateSelect);

        if (Date.parse(dateSelect) < Date.parse(dateNow)) {
            //alert("Seleccionaste un dia no valido   ");
            $('#validation').append("<p id='msgValidate' style='color: red; font-size: 15px'>La fecha no puede ser menor a la actual</p>");

            return;
        }

        console.log("Listo para enviar");
        console.log("title: " + title);
        console.log("description: " + description);
        console.log("dateSelect: " + dateSelect);

        let notice = {
            title: title,
            description: description,
            startDate: dateSelect,
            endDate: dateSelect,
            fileName: "",
            path: "",
            isEvent: isEvent
        };

        saveNotice(notice);

    } else {
        //validations
        validateControl(title, description, startDateNews, endDateNews);

        //dateNow
        let dateNow = Date(Date.now());
        console.log("Today: " + dateNow.toString());
        //dateStartDate
        let startDate = $j("#startDateNews").val();
        let dateSelect1 = new Date(startDate).toISOString().slice(0, 19).replace('T', ' ');
        console.log("day startDate: " + dateSelect1);
        //dateEndDate
        let endDate = $j("#endDateNews").val();
        console.log("endDateNews: " + endDate);
        let dateSelect2 = new Date(endDate).toISOString().slice(0, 19).replace('T', ' ');
        console.log("day endDate: " + dateSelect2);
        let isEvent = false;

        if (Date.parse(dateSelect1) < Date.parse(dateNow)) {
            //alert("Seleccionaste un dia no valido   ");
            $('#validation').append("<p id='msgValidate' style='color: red; font-size: 15px'>La fecha inicial no puede ser menor a la actual</p>");

            return;
        }

        if ((Date.parse(dateSelect2) < Date.parse(dateSelect1)) || (Date.parse(dateSelect2) == Date.parse(dateSelect1))) {
            //alert("Seleccionaste un dia no valido   ");
            $('#validation').append("<p id='msgValidate' style='color: red; font-size: 15px'>La fecha final no puede ser menor a la inicial</p>");

            return;
        }

        console.log("Listo para enviar");
        console.log("title: " + title);
        console.log("description: " + description);
        console.log("dateSelect1: " + dateSelect1);
        console.log("dateSelect2: " + dateSelect2);

        let notice = {
            title: title,
            description: description,
            startDate: dateSelect1,
            endDate: dateSelect2,
            isEvent: isEvent
        };

        saveNotice(notice);
    }


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

function saveNotice(notice) {
    console.log("entra a guardar");
    console.log(notice);

    console.log("####################");

    $.ajax({
        url: 'SaveNotice',
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ notice: { title: notice['title'], description: notice['description'], startDateNotice: notice['startDate'], endDateNotice: notice['endDate'], fileName: notice['fileName'], path: notice['path'], isEvent: notice['isEvent'] } }),
        //data: JSON.stringify(notice),
        async: true,
        processData: false,
        cache: false,
        success: function (data) {
            if (data === 'successfully') {
                console.log("Noticia creada");
                $modal = $('#myModal');
                $modal.find('form')[0].reset();
                $('#myModal').modal('hide');
                location.reload();
            }
        },
        error: function (xhr) {
            alert('error');
        }
    })

}

/*----------------------Change JQuery Version----------------------*/
$j = jQuery.noConflict();

$j(function () {
    $j('#datetimepicker1').datetimepicker();
});

$j("#datetimepicker1").on("dp.change", function (e) {

    $j("#datetimepicker1").hide();
    $j("#datetimepicker1").show();
});

$j(function () {
    $j('#startDateNews').datetimepicker({
        showClose: true,
        showClear: true,
        format: 'YYYY-MM-DD',

        tooltips: {
            selecTime: 'Selecciona la hora',
            clear: 'Limpiar Selección',
            close: 'Cerrar Ventana',

        }

    });
});

$j(function () {
    $j('#endDateNews').datetimepicker({
        showClose: true,
        showClear: true,
        format: 'YYYY-MM-DD',

        tooltips: {
            selecTime: 'Selecciona la hora',
            clear: 'Limpiar Selección',
            close: 'Cerrar Ventana',

        }
    });
});

$j("#startDateNews").on("dp.change", function (e) {

    $j('#endDateNews').data("DateTimePicker").minDate(e.date);
    $(this).attr("autocomplete", "off");

});

$j("#endDateNews").on("dp.change", function (e) {

    $j('#startDateNews').data("DateTimePicker").maxDate(e.date);
    $(this).attr("autocomplete", "off");
});