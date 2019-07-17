$(document).ready(function () {

    let json = JSON.parse(events.replace(/&quot;/g, '"'));
    createCardEvent(json);

});

function createCardEvent(json) {

    let gallery = document.querySelector('#gallery_container');

    for (let i = 0; i < json.length; i++) {

        let idAlbum = json[i].IdEvent;
        let divContainer = document.createElement('DIV');

        divContainer.style.maxWidth = '95%';
        divContainer.style.margin = '0 auto 30px';

        let divImg = document.createElement('DIV');
        divImg.setAttribute('class', 'cont-img');
        divImg.style.display = 'grid';
        divImg.style.width = '100%';
        divImg.style.gridTemplateColumns = 'auto auto';

        let span = document.createElement('span');
        span.setAttribute('class', 'album');
        span.style.display = 'none';
        span.textContent = idAlbum;
        divImg.append(span);

        let promise = getPortrait(idAlbum);
        promise.then(function (value) {
            for (let i = 0; i < value.length; i++) {
                let src = "/UploadedFiles/events/";
                src += value[i].FileName;
                //console.log(data[i].FileName);
                let img = document.createElement('img');
                let asd = document.createElement('div');
                img.setAttribute("class", "img-album");
                img.src = src;
                divImg.append(asd);
                asd.append(img);
            }
        });

        divContainer.append(divImg);

        let divCard = document.createElement('DIV');
        divCard.setAttribute('class', 'card-body');
        divCard.style.width = '100%';
        divCard.style.marginLeft = '2%';

        let h5 = document.createElement('h5');
        h5.setAttribute('class', 'card-title');
        h5.innerHTML = json[i].Title;

        let p = document.createElement('p');
        p.setAttribute('class', 'card-text');
        p.innerHTML = json[i].Description;

        let a = document.createElement('a');
        a.setAttribute('class', 'btn btn-primary');
        a.setAttribute('href', '/Gallery/Album?idEvent=' + json[i].IdEvent);
        a.innerHTML = 'Ver Álbum';

        divCard.append(h5);
        divCard.append(p);
        divCard.append(a);
        divContainer.append(divCard);
        gallery.append(divContainer);

    }

}

async function getPortrait(idAlbum) {

    const result = $.ajax({
        url: 'getPortrait/?idAlbum=' + idAlbum,
        dataType: 'json',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
    });

    return result;
}