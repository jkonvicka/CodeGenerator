import { Converts } from './Converts.js';
import { Node } from './Node.js';
import { NodeProperty } from "./NodeProperty.js";
import { NodeMethod} from "./NodeMethod.js";
import { NodeMethodParameter } from "./NodeMethodParameter.js";

var $ = go.GraphObject.make;


let diagram = null;


export var selectedObject = null;


var nodedata = [];

var linkdata = [];

// HTML MANIPULATOR FUNCTIONS
function loadListIntoHtmlSelect(id, list){
    var select = document.getElementById(id);
    //console.log(selectedObject);
    var i = 0;
    for (const _option of list) {
        //console.log(i + ' ' + _property.name);
        var option = document.createElement("option");
        option.text = _option.toString();
        option.value = i;
        select.add(option);      
        i++;
    }
}

function unloadListIntoHtmlSelect(id){
    var select = document.getElementById(id);
    select.innerHTML = '';
}

export function reloadListIntoHtmlSelect(id, list){
    unloadListIntoHtmlSelect(id);
    loadListIntoHtmlSelect(id, list);
}


export class ClassDiagram {
    constructor(divID) {
        this.diagram = null;
        this.divID = divID;
    }

    import(_nodedata, _linkdata){
        //nodes
        for(let i = 0; i < _nodedata.length; i++){
            this.addNode(_nodedata[i].name);
            for(let j = 0; j < _nodedata[i].properties.length; j++){
                let prop = new NodeProperty(_nodedata[i].properties[j].name, 
                    _nodedata[i].properties[j].type, 
                    _nodedata[i].properties[j].visibility, 
                    _nodedata[i].properties[j].default_value);
                this.addNodeProperty(_nodedata[i].key, prop);
            }
            for(let j = 0; j < _nodedata[i].methods.length; j++){
                let method = new NodeMethod(_nodedata[i].methods[j].name, 
                    _nodedata[i].methods[j].type, 
                    _nodedata[i].methods[j].visibility, 
                    _nodedata[i].methods[j].parameters.map(x=>new NodeMethodParameter(x.name, x.type)));
                this.addNodeMethod(_nodedata[i].key, method);
            }
        }
        //links
        for(let i = 0; i < _linkdata.length; i++){
            this.addLink(_linkdata[i].from, _linkdata[i].to, _linkdata[i].relationship, true);
        }
    }

    exportNodeData(){
        let nodedata =  this.diagram.model.nodeDataArray;
        return nodedata;
    }

    exportLinkData(){
        let linkdata = this.diagram.model.linkDataArray;
        return linkdata;
    }

    addLink(from, to, relationship, importing = false){
        this.diagram.model.startTransaction("add link");
        var link = {from, to, relationship};
        console.log('Adding link');
        console.log(link);
        this.diagram.model.addLinkData(link);
        
        
        // add property
        if(!importing){
            if(relationship === 'aggregation' || relationship === 'composition')
            {
                let prop = new NodeProperty(
                                    '_' + this.getData(to).name.toLowerCase() + 'Array', 
                                    this.getData(to).name + '[]', 
                                    'private', 
                                    '');
                this.addNodeProperty(from, prop)
            }else if(relationship === 'association')
            {
                let prop = new NodeProperty(
                                    this.getData(to).name.toLowerCase(),
                                    this.getData(to).name,
                                    'private',
                                    '');
                this.addNodeProperty(from, prop)
            }
        }

        this.diagram.model.commitTransaction("add link");
    }

    addNode(name){
        this.diagram.model.startTransaction("add data");
        var node = new Node(this.diagram.model.nodeDataArray.length + 1, name);
        console.log('Adding node :');
        console.log(node);
        this.diagram.model.addNodeData(node);
            // ... maybe modify other properties and/or other data objects
        this.diagram.model.commitTransaction("add data");
    }

    addNodeProperty(key, property){
        var data = this.diagram.model.findNodeDataForKey(key);
        if (data) {
            this.diagram.model.startTransaction("modified property");
            this.diagram.model.insertArrayItem(data.properties, -1, property);
            // ... maybe modify other properties and/or other data objects
            this.diagram.model.commitTransaction("modified property");
        }
    }

    removeNodeProperty(key, id){
        var data = this.diagram.model.findNodeDataForKey(key);
        if (data) {
            this.diagram.model.startTransaction("removed property");
            console.log(id);
            this.diagram.model.removeArrayItem(data.properties, id);
            // ... maybe modify other properties and/or other data objects
            this.diagram.model.commitTransaction("removed property");
        }
    }

    addNodeMethod(key, method){
        var data = this.diagram.model.findNodeDataForKey(key);
        if (data) {
            this.diagram.model.startTransaction("modified method");
            this.diagram.model.insertArrayItem(data.methods, -1, method);
            // ... maybe modify other properties and/or other data objects
            this.diagram.model.commitTransaction("modified method");
        }
    }

    removeNodeMethod(key, id){
        var data = this.diagram.model.findNodeDataForKey(key);
        if (data) {
            this.diagram.model.startTransaction("removed method");
            this.diagram.model.removeArrayItem(data.methods, id);
            // ... maybe modify other properties and/or other data objects
            this.diagram.model.commitTransaction("removed method");
        }
    }

    getData(key){
        var data = this.diagram.model.findNodeDataForKey(key);
        return data;
    }

    getCodeGenSpecification(namespace, includes){
        var nodeData =  this.diagram.model.nodeDataArray;
        var linkData = this.diagram.model.linkDataArray;
        
        const classSpecifications = [];
        nodeData.forEach(function(node) {
            const classSpecification = {
                Name: node.name,
                NameSpace: namespace,
                AccessOperator: 1,
                Includes: includes,
                BaseClasses: [],
                Properties: [],
                Methods: []
              };
            // Parse properties
            node.properties.forEach(property => {
                const parsedProperty = {
                AccessOperator: property.visibility === "public" ? 0 : 1,
                GenerateGetter: true,
                GenerateSetter: true,
                Name: property.name,
                DataType: { Key: property.type },
                DefaultValue: property.default || ""
                };

                classSpecification.Properties.push(parsedProperty);
            });

            // Parse methods
            node.methods.forEach(method => {
                const parsedMethod = {
                Name: method.name,
                ReturnType: { Key: method.type },
                AccessOperator: method.visibility === "public" ? 0 : 1,
                Arguments: method.parameters.map(parameter => {
                    return {
                    Name: parameter.name,
                    DataType: { Key: parameter.type },
                    DefaultValue: parameter.default || ""
                    };
                })
                };

                classSpecification.Methods.push(parsedMethod);
            });

            // Parse properties
            var generalizationLinks = linkData.filter(obj => obj.from === node.key && 
                                                        obj.relationship === 'generalization');
            generalizationLinks.forEach(link => {
                const baseClass = {
                    Name: nodeData.filter(obj => obj.key === link.to)[0].name || ""
                };

                classSpecification.BaseClasses.push(baseClass);
            });

            classSpecifications.push(classSpecification);
          });
        return classSpecifications;
    }

    getModel(){
        return this.diagram.model;
    }

    render() {
        this.diagram = $(go.Diagram, this.divID,
            {
                "undoManager.isEnabled": true,
                layout: $(go.TreeLayout,
                  { // this only lays out in trees nodes connected by "generalization" links
                    angle: 90,
                    path: go.TreeLayout.PathSource,  // links go from child to parent
                    setsPortSpot: false,  // keep Spot.AllSides for link connection spot
                    setsChildPortSpot: false,  // keep Spot.AllSides
                    // nodes not connected by "generalization" links are laid out horizontally
                    arrangement: go.TreeLayout.ArrangementHorizontal
                  })
            });

            this.diagram.model = new go.GraphLinksModel(
                {
                  copiesArrays: true,
                  copiesArrayObjects: true,
                  nodeDataArray: nodedata,
                  linkDataArray: linkdata
                });
            
                var methodTemplate =
                    $(go.Panel, "Horizontal",
                // method visibility/access
                    $(go.TextBlock,
                        { isMultiline: false, editable: false, width: 12 },
                        new go.Binding("text", "visibility", Converts.convertVisibility)),
                    // method name, underlined if scope=="class" to indicate static method
                    $(go.TextBlock,
                        { isMultiline: false, editable: true },
                        new go.Binding("text", "name").makeTwoWay(),
                        new go.Binding("isUnderline", "scope", s => s[0] === 'c')),
                    // method parameters
                    $(go.TextBlock, "()",
                        // this does not permit adding/editing/removing of parameters via inplace edits
                        new go.Binding("text", "parameters", function (parr) {
                        var s = "(";
                        for (var i = 0; i < parr.length; i++) {
                            var param = parr[i];
                            if (i > 0) s += ", ";
                            s += param.name + ": " + param.type;
                        }
                        return s + ")";
                        })),
                    // method return type, if any
                    $(go.TextBlock, "",
                        new go.Binding("text", "type", t => t ? ": " : "")),
                    $(go.TextBlock,
                        { isMultiline: false, editable: true },
                        new go.Binding("text", "type").makeTwoWay())
                    );
            
                var propertyTemplate =
                    $(go.Panel, "Horizontal",
                    // property visibility/access
                    $(go.TextBlock,
                        { isMultiline: false, editable: false, width: 12 },
                        new go.Binding("text", "visibility", Converts.convertVisibility)),
                    // property name, underlined if scope=="class" to indicate static property
                    $(go.TextBlock,
                        { isMultiline: false, editable: true },
                        new go.Binding("text", "name").makeTwoWay(),
                        new go.Binding("isUnderline", "scope", s => s[0] === 'c')),
                    // property type, if known
                    $(go.TextBlock, "",
                        new go.Binding("text", "type", t => t ? ": " : "")),
                    $(go.TextBlock,
                        { isMultiline: false, editable: true },
                        new go.Binding("text", "type").makeTwoWay()),
                    // property default value, if any
                    $(go.TextBlock,
                        { isMultiline: false, editable: false },
                        new go.Binding("text", "default", s => s ? " = " + s : ""))
                    );
            
            
                this.diagram.nodeTemplate =
                    $(go.Node, "Auto",
                    {
                        locationSpot: go.Spot.Center,
                        fromSpot: go.Spot.AllSides,
                        toSpot: go.Spot.AllSides
                    },
                    $(go.Shape, 
                        { 
                            fill: "lightyellow"
                        }),
                    $(go.Panel, "Table",
                        { defaultRowSeparatorStroke: "black" },
                        // header
                        $(go.TextBlock,
                        {
                            row: 0, columnSpan: 2, margin: 3, alignment: go.Spot.Center,
                            font: "bold 12pt sans-serif",
                            isMultiline: false, editable: true
                        },
                        new go.Binding("text", "name").makeTwoWay()),
                        // properties
                        $(go.TextBlock, "Properties",
                        { row: 1, font: "italic 10pt sans-serif" },
                        new go.Binding("visible", "visible", v => !v).ofObject("PROPERTIES")),
                        $(go.Panel, "Vertical", { name: "PROPERTIES" },
                        new go.Binding("itemArray", "properties"),
                        {
                            row: 1, margin: 3, stretch: go.GraphObject.Fill,
                            defaultAlignment: go.Spot.Left, background: "lightyellow",
                            itemTemplate: propertyTemplate
                        }
                        ),
                        $("PanelExpanderButton", "PROPERTIES",
                        { row: 1, column: 1, alignment: go.Spot.TopRight, visible: false },
                        new go.Binding("visible", "properties", arr => arr.length > 0)),
                        // methods
                        $(go.TextBlock, "Methods",
                        { row: 2, font: "italic 10pt sans-serif" },
                        new go.Binding("visible", "visible", v => !v).ofObject("METHODS")),
                        $(go.Panel, "Vertical", { name: "METHODS" },
                        new go.Binding("itemArray", "methods"),
                        {
                            row: 2, margin: 3, stretch: go.GraphObject.Fill,
                            defaultAlignment: go.Spot.Left, background: "lightyellow",
                            itemTemplate: methodTemplate
                        }
                        ),
                        $("PanelExpanderButton", "METHODS",
                        { row: 2, column: 1, alignment: go.Spot.TopRight, visible: false },
                        new go.Binding("visible", "methods", arr => arr.length > 0))
                    )
                    );
            
                    this.diagram.linkTemplate =
                    $(go.Link,
                        { routing: go.Link.Orthogonal },
                        new go.Binding("isLayoutPositioned", "relationship", Converts.convertIsTreeLink),
                        $(go.Shape),
                        $(go.Shape, { scale: 1.3, fill: "white" },
                          new go.Binding("fromArrow", "relationship", Converts.convertFromArrow)),
                        $(go.Shape, { scale: 1.3, fill: "white" },
                          new go.Binding("toArrow", "relationship", Converts.convertToArrow))
                      );

                    
                        this.setListener();
                        loadListIntoHtmlSelect('classesFrom', this.diagram.model.nodeDataArray);
                        loadListIntoHtmlSelect('classesTo', this.diagram.model.nodeDataArray);
                    }

    setListener()
    {
        this.diagram.addDiagramListener("ObjectSingleClicked",
        function(e) {
        var part = e.subject.part;
        if (!(part instanceof go.Link))
        {
            unloadListIntoHtmlSelect('selectedNodePropertyList');
            unloadListIntoHtmlSelect('selectedNodeMethodList');
            selectedObject = part.data;
            loadListIntoHtmlSelect('selectedNodePropertyList', selectedObject.properties);
            loadListIntoHtmlSelect('selectedNodeMethodList', selectedObject.methods);
            console.log('Selected object with key: ' + selectedObject.key);
            //console.log(selectedObject.methods);
        }
        else{
        }
        });

        this.diagram.addDiagramListener("BackgroundSingleClicked",
            function(e) 
            { 
                selectedObject = null;
                unloadListIntoHtmlSelect('selectedNodePropertyList');
                unloadListIntoHtmlSelect('selectedNodeMethodList');
                console.log('Deselected object');
            });

        this.diagram.addModelChangedListener(function(e) {
                // ignore unimportant Transaction events
                if (!e.isTransactionFinished) return;
                //var json = e.model.toIncrementalJson(e);
                //var data = e.model.toIncrementalData(e);
                //console.log(data);
                reloadListIntoHtmlSelect('classesFrom', e.model.nodeDataArray);
                reloadListIntoHtmlSelect('classesTo', e.model.nodeDataArray);
              });
        
    }
  }