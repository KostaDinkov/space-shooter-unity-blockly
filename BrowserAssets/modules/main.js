

import InitCustomBlocks from './custom_blocks.js';
import SetTheme from './theme.js';


InitCustomBlocks();
SetTheme();

let workspace;

function SetToolbox(toolbox) {

  
  let blocklyDiv = document.getElementById("blocklyDiv");
  blocklyDiv.innerHTML = "";
  let toolboxJson = JSON.parse(toolbox);
  
  workspace = Blockly.inject('blocklyDiv', {
    toolbox: toolboxJson,
    scrollbars: true,
    renderer: "zelos",
    theme: "kiberlab",
    collapse: false,
    trashcan: false,
    zoom:
    {
      controls: true,
      wheel: true,
      startScale: 1.0,
      maxScale: 3,
      minScale: 0.3,
      scaleSpeed: 1.2,
      pinch: false
    },
    grid:
    {
      spacing: 50,
      length: 10,
      colour: '#49F5ED55',
      snap: true
    },
  });
  workspace.toolbox_.flyout_.autoClose = false;
  workspace.addChangeListener(Blockly.Events.disableOrphans);
}
function getCode() {
  return Blockly.CSharp.workspaceToCode(workspace);
}

function saveWorkspace(){
  let workspaceXML = Blockly.Xml.workspaceToDom(workspace)
  
  let workspaceString = Blockly.Xml.domToText(workspaceXML);
  console.log(workspaceString)
  return workspaceString;
}

function loadLastWorkspace(workspaceString){
  
  if(workspaceString ===""){
    workspaceString = '<xml xmlns="https://developers.google.com/blockly/xml"></xml>'
  }
  const doc = Blockly.Xml.textToDom(workspaceString)

  console.log(doc)
  Blockly.Xml.clearWorkspaceAndLoadFromXml(doc, workspace);
}

function getBlocksCount(){
  return workspace.getAllBlocks().length;
}

window.getCode = getCode;
window.setToolbox = SetToolbox;
window.saveWorkspace = saveWorkspace;
window.loadLastWorkspace = loadLastWorkspace;
window.getBlocksCount = getBlocksCount;
