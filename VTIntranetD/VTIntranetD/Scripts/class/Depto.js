'use strict'

class Depto {

    constructor(idDepto, deptoName, areas) {
        this.idDepto = idDepto;
        this.deptoName = deptoName;
        this.areas = {};

        for (let i = 0; i < areas.length; i++) {
            this.areas[i] = areas[i];
        }
    }

    getIdDepto() {
        return this.idDepto;
    }

    getDeptoName() {
        return this.deptoName;
    }

    getAreas() {
        return this.areas;
    }

    getArea(index) {
        return this.areas[index];
    }

    getIndexArea(name) {
        let areas = this.areas;
        let index = 0;
        let keys = Object.keys(areas);

        for (let i = 0; i < keys.length; i++) {
            if (areas[i].Name == name) {
                index = i;
            }
        }
        return index;
    }

    getStateArea(index) {
        return this.areas[index].State;
    }

    setStateArea(index, state) {
        this.areas[index].State = state;
    }
}