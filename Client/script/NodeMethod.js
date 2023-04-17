
 //  { name: "deposit", parameters: [{ name: "amount", type: "Currency" }], visibility: "public" }
export class NodeMethod{
    constructor(name, type, visibility, parameters){
        this.name = name;
        this.type = type;//type
        this.visibility = visibility;
        this.parameters = parameters;
    }

    //function returns a string of the method
    toString(){
        return this.visibility + " " + this.name + "(" + this.parameters.map(element=> element.toString()).join(', ') + ")" + " : " + this.type;
    }
}