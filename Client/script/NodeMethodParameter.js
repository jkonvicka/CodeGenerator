

export class NodeMethodParameter{
    constructor(name, type){
        this.name = name;
        this.type = type;
    }

    toString(){
        return this.name + " : " + this.type;
    }
}