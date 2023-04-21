import { ClassDiagram, selectedObject, reloadListIntoHtmlSelect } from './ClassDiagram.js';
import { CodeGenHTTPClient } from './HTTPClient/CodeGenHTTPClient.js';
import { NodeProperty } from './NodeProperty.js';
import { NodeMethod } from './NodeMethod.js';
import { NodeMethodParameter } from './NodeMethodParameter.js';


var classDiagram = null;

export function init(divID){
    classDiagram = new ClassDiagram(divID);
    classDiagram.render();
}

function nodeSelectedInfo()
{
    if(selectedObject == null)
    {
        console.warn('Node not selected');
        showMessage('Please select node', false, 'alert-dark');
        return false;
    }
    return true;
}


//HOOKS 
document.getElementById("LoadLanguages").onclick = function(){
    const xhr = new CodeGenHTTPClient();
    const url = document.getElementById("GeneratorUrl").value + 'LanguageManagement/GetLanguages';
    xhr.send(url, '', 'GET', function(response) {
      var responseObj = JSON.parse(response);
      document.getElementById("languageSelector").innerHTML = '';
      for(var i = 0; i < responseObj.length; i++){
        var option = document.createElement("option");
        option.text = responseObj[i];
        option.value = responseObj[i];
        document.getElementById("languageSelector").add(option);
    }
    if(responseObj.length > 0)
        document.getElementById("GenerateCode").disabled = false;
	});
    
}

document.getElementById("Import").onclick = function(){
    let input = document.createElement("input");
    input.type = "file";
    input.accept = "application/json";
    input.style.display = "none";
    document.body.appendChild(input);
    
    input.onchange = function() {
        let file = input.files[0];
        let reader = new FileReader();
        
        reader.onload = function(event) {
        let json = event.target.result;
        let data = JSON.parse(json);

        // Render the diagram with the imported data
        classDiagram.import(data.nodedata, data.linkdata);
        };
        
        reader.readAsText(file);
        document.body.removeChild(input);
    };
    
    input.click();
}

document.getElementById("Export").onclick = function(){
    let json = classDiagram.export();
    var fileToSave = new Blob([json], {
        type: 'application/json'
    });
    saveAs(fileToSave, "codeDiagram.json");
}

document.getElementById("ExportSVG").onclick = function(){
    var svg = classDiagram.diagram.makeSvg({ scale: 1, background: "white" });
    var svgstr = new XMLSerializer().serializeToString(svg);
    var blob = new Blob([svgstr], { type: "image/svg+xml" });
    downloadSVGCallback(blob);
}

function downloadSVGCallback(blob) {
    var url = window.URL.createObjectURL(blob);
    var filename = "mySVGFile.svg";

    var a = document.createElement("a");
    a.style = "display: none";
    a.href = url;
    a.download = filename;

    // IE 11
    if (window.navigator.msSaveBlob !== undefined) {
      window.navigator.msSaveBlob(blob, filename);
      return;
    }

    document.body.appendChild(a);
    requestAnimationFrame(() => {
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    });
  }

document.getElementById("GenerateCode").onclick = function(){
    console.log("EXPORTING JSON");
    var diagramJson = classDiagram.getJson();

    var languageSelector = document.getElementById('languageSelector');
    var selectedLanguageId = languageSelector.options[languageSelector.selectedIndex].value;

    var body = {
            language: selectedLanguageId,
            classSpecification: diagramJson
          };
    //console.log(diagramJson);
    var url = document.getElementById("GeneratorUrl").value + 'CodeGenerator/GenerateClasses';
    const xhr = new CodeGenHTTPClient();

    xhr.send(url, body, 'POST', function(response) {
      var responseObj = JSON.parse(response);
      const zip = new JSZip();
      
      for (let i = 0; i < responseObj.length; i++) {
        zip.file(responseObj[i].fileName, responseObj[i].code);
      }

      zip.generateAsync({ type: "blob" }).then(function (content) {
        saveAs(content, "generatedCode.zip");
      });
	});

};

document.getElementById("AddProperty").onclick = function(){
    if(nodeSelectedInfo())
    {
        var propertyName = document.getElementById('propertyName').value;
        if(propertyName == ''){
            showMessage('Property name is null or empty', true);
            return;
        }
        var propertyDefault = document.getElementById('propertyDefault').value;
        var propertyVisibility = document.getElementById('flexRadioPropertyPrivate').checked ?
                                    'private' : 'public';
        var propertyDataTypeSelector = document.getElementById('propertyDataType');
        var propertyDataType = propertyDataTypeSelector.options[propertyDataTypeSelector.selectedIndex].value;
        if (selectedObject.properties.some(e => e.name === propertyName)) {
            showMessage('This node already contains property with that name', true, 'alert-warning');
            return;
        }
        classDiagram.addNodeProperty(selectedObject.key, 
            new NodeProperty(propertyName,propertyDataType,propertyVisibility,propertyDefault));
        reloadListIntoHtmlSelect('selectedNodePropertyList', selectedObject.properties);
        console.log('Added property')
        showMessage('Property added', false, 'alert-success');
    }else{
        //showMessage('Please select node', false, 'alert-dark');
    }
    
};

document.getElementById("RemoveProperty").onclick = function(){
    if(nodeSelectedInfo())
    {
        var propertyNameListSelector = document.getElementById('selectedNodePropertyList');
        var propertyId = propertyNameListSelector.options[propertyNameListSelector.selectedIndex].value;
        classDiagram.removeNodeProperty(selectedObject.key, parseInt(propertyId));
        reloadListIntoHtmlSelect('selectedNodePropertyList', selectedObject.properties);
        console.log('Removed property');
        showMessage('Property removed', false, 'alert-success');
    }else{
        //showMessage('Please select node', false, 'alert-dark');
    }
    
};

document.getElementById("ShowProperties").onclick = function(){
    if(nodeSelectedInfo())
    {
        var properties = classDiagram.getData(selectedObject.key).properties;
        //alert(JSON.stringify(properties));
        console.log(properties);
    }
    else{
        //showMessage('Please select node', false);
    }
};

document.getElementById("AddNode").onclick = function(){

    var className = document.getElementById('className').value;
    if(className == ''){
        showMessage('Class name is null or empty', true);
        return;
    }

    if (classDiagram.getModel().nodeDataArray.some(e => e.name === className)) {
        showMessage('This diagram already contains node with that name', true, 'alert-warning');
        return;
    }

    classDiagram.addNode(className);
  
};

document.getElementById("AddLink").onclick = function(){
    var classFromListSelector = document.getElementById('classesFrom');
    var classFrom = classFromListSelector.options[classFromListSelector.selectedIndex].value;
    var relationListSelector = document.getElementById('relations');
    var relation = relationListSelector.options[relationListSelector.selectedIndex].value;
    var classToListSelector = document.getElementById('classesTo');
    var classTo = classToListSelector.options[classToListSelector.selectedIndex].value;
    classDiagram.addLink(parseInt(classFrom)+1, parseInt(classTo)+1, relation);
};

document.getElementById("AddMethod").onclick = function(){
    if(nodeSelectedInfo())
    {
        var methodName = document.getElementById('methodName').value;
        if(methodName == ''){
            showMessage('Method name is null or empty', true);
            return;
        }
        var methodVisibility = document.getElementById('flexRadioMethodPrivate').checked ?
                                    'private' : 'public';
        var methodReturnTypeSelector = document.getElementById('methodReturnType');
        var methodReturnType = methodReturnTypeSelector.options[methodReturnTypeSelector.selectedIndex].value;
        
        
        // Get parameters from the form
        var parameters = [];
        var parameterNames = document.getElementsByName("parameterName");
        var parameterTypes = document.getElementsByName("parameterType");
        //console.log(parameterTypes);
        //var parameterDefaults = document.getElementsByName("parameterDefault");
        for (var i = 0; i < parameterNames.length; i++) {
            var name = parameterNames[i].value;
            var type = parameterTypes[i].options[parameterTypes[i].selectedIndex].value;
            //var defaultValue = parameterDefaults[i].value;
            if(name == null || name === ""){
                showMessage('Parameter name is null or empty', true, 'alert-warning');
                return;
            }
            parameters.push(new NodeMethodParameter(name, type));
        }
        
        /*if (selectedObject.methods.some(e => e.name === methodName && e.parameters)) {
            showMessage('This node already contains method with that name', true, 'alert-warning');
            return;
        }*/
        
        // Add the method to the class object
        classDiagram.addNodeMethod(selectedObject.key, 
            new NodeMethod(methodName,methodReturnType,methodVisibility,parameters));
        console.log(selectedObject);
        reloadListIntoHtmlSelect('selectedNodeMethodList', selectedObject.methods);
        console.log('Added method');
        showMessage('Method added', false, 'alert-success');
    }else{
        //showMessage('Please select node', false, 'alert-dark');
    }
};

document.getElementById("RemoveMethod").onclick = function(){
    if(nodeSelectedInfo())
    {
        var methodListSelector = document.getElementById('selectedNodeMethodList');
        var methodListSelectorId = methodListSelector.options[methodListSelector.selectedIndex].value;
        classDiagram.removeNodeMethod(selectedObject.key, parseInt(methodListSelectorId));
        reloadListIntoHtmlSelect('selectedNodeMethodList', selectedObject.methods);
        console.log('Removed method');
        showMessage('Method removed', false, 'alert-success');
    }else{
        //showMessage('Please select node', false, 'alert-dark');
    }
    
};




document.getElementById("ToggleLinking").onclick = function(){
    toggleHideDisplay('linkingManagement');
};

document.getElementById("ToggleProperty").onclick = function(){
    toggleHideDisplay('properyManagement');
};

document.getElementById("ToggleMethod").onclick = function(){
    toggleHideDisplay('methodManagement');
};

document.getElementById("ToggleNode").onclick = function(){
    toggleHideDisplay('nodeManagement');
};


const toggleStrings = ['linkingManagement', 'properyManagement', 'methodManagement', 'nodeManagement'];

function toggleHideDisplay(elementId){
    for (let i = 0; i < toggleStrings.length; i++) {
        if(toggleStrings[i] != elementId){
            var element = document.getElementById(toggleStrings[i]);
            element.style.display = "none";
        }
    }

    if(elementId != null){
        var element = document.getElementById(elementId);
        if (element.style.display === "none") {
            element.style.display = "block";
          } else {
            element.style.display = "none";
          }
    }
   
}


function showMessage(errorMessage, logError, type = 'alert-danger'){

    if(logError){
        console.error(errorMessage);
    }
    var message = document.createElement("div");
    message.innerHTML = '<div class="alert ' + type + ' alert-dismissible fade show " role="alert">' +
                            errorMessage +
                        '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close">' +
                        '</button></div>';

    var errorContainer = document.getElementById("error-container-div");
    errorContainer.appendChild(message);
}




//INIT
toggleHideDisplay(null);
init('diagramDiv');



