function getBrands(array) {
    
    let brands = [];
    for (let i = 0; i < array.length; i++) {
        brands.push(array[i].TagName);
    }

    return brands;
}

function findDuplicateValues(array) {

    let object = {};
    let result = [];
    //found number coincidences
    array.forEach(function (item) {
        if (!object[item])
            object[item] = 0;
        object[item] += 1;
    });

    //create array of results coincidences
    for (let prop in object) {
        if (object[prop] >= 1) {
            result.push(prop);
        }
    }

    return result;
}

function getBrandDepto(brands_raw, brands) {
    /*console.log("--------");
    console.log(brands_raw);
    console.log(brands);*/
    let object = {};
    for (let i = 0; i < brands.length; i++) {
        let deptos = [];
        let ban = brands[i];
        for (let j = 0; j < brands_raw.length; j++) {
            let brand = brands_raw[j].TagName;
            if (brand === ban) {
                //build elements for json
                let depto = { idTag: brands_raw[j].IdTag, TagClabe: brands_raw[j].TagClabe, idDepto: brands_raw[j].IdDepto, deptoName: brands_raw[j].DeptoName, active: brands_raw[j].Active };
                deptos.push(depto);
            }
        }
        object[ban] = deptos;
        deptos = [];
    }

    return object;
}